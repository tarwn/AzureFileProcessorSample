using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Processor_Core;
using Processor_Core.Storage.Azure;
using Processor_Core.Storage;
using Processor_WebRole.Models;

namespace Processor_WebRole.Controllers {
	public class HomeController : Controller {

		IStorageLocator _storageLocator;

		public HomeController() {
			_storageLocator = new StorageManager();
		}

		public ActionResult Index() {
			ViewBag.Message = "Welcome to ASP.NET MVC!";

			var store = new ItemStore(_storageLocator);
			var model = new StatusViewModel() {
				ProcessedItems = store.GetProcessedList(),
				UnprocessedItems = store.GetUnprocessedList()
			};

			return View(model);
		}

		[HttpPost]
		public ActionResult AddFile(HttpPostedFile file) {
			if (file != null && file.ContentLength > 0) {
				var item = new FullItem() {
					ResourceId = Guid.NewGuid(),
					Received = DateTime.Now.ToUniversalTime(),
					IsProcessed = false,
					FileName = file.FileName
				};
				item.ReadFileFromStream(file.InputStream);

				new ItemStore(_storageLocator).AddNewItem(item);
			}

			return View();
		}
	}
}
