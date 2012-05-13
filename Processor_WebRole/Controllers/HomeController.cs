using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Processor_Core;
using Processor_Core.Storage.Azure;
using Processor_Core.Storage;
using Processor_WebRole.Models;
using System.Configuration;

namespace Processor_WebRole.Controllers {
	public class HomeController : Controller {

		IStorageLocator _storageLocator;

		public HomeController() {
			_storageLocator = new StorageManager(ConfigurationManager.AppSettings["Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString"]);
		}

		public ActionResult Index() {
			var store = new ItemStore(_storageLocator);
			var model = new StatusViewModel() {
				ProcessedItems = store.GetProcessedList(),
				UnprocessedItems = store.GetUnprocessedList()
			};

			ViewData["file"] = TempData["file"];

			return View(model);
		}

		[HttpPost]
		public ActionResult AddFile(HttpPostedFileBase file) {
			if (file != null && file.ContentLength > 0) {
				var item = new FullItem() {
					ResourceId = Guid.NewGuid(),
					Received = DateTime.Now.ToUniversalTime(),
					IsProcessed = false,
					FileName = file.FileName
				};
				item.ReadFileFromStream(file.InputStream);

				new ItemStore(_storageLocator).AddNewItem(item);
				TempData["file"] = file.FileName + " uploaded and queued for processing.";
			}
			else {
				TempData["file"] = "Processor ignores empty files, sorry.";
			}

			return RedirectToAction("Index");
		}

		[HttpGet]
		public ActionResult ProcessNextItem() {
			var store = new ItemStore(_storageLocator);
			var nextItem = store.RetrieveForProcessing();
			if (nextItem != null) {
				var finishedItem = new ItemProcessor().ProcessItem(nextItem);
				store.StoreFinishedItem(finishedItem);
			}

			return RedirectToAction("Index");
		}
	}
}
