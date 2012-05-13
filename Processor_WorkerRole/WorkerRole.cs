using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.Diagnostics;
using Microsoft.WindowsAzure.ServiceRuntime;
using Microsoft.WindowsAzure.StorageClient;
using Processor_Core.Storage.Azure;
using Processor_Core;

namespace Processor_WorkerRole {
	public class WorkerRole : RoleEntryPoint {
		public override void Run() {
			while (true) {
				var storageLocator = new StorageManager("Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString");
				var store = new ItemStore(storageLocator);
				new ItemProcessor().ProcessNextItem(store);
				Thread.Sleep(1000);
				Trace.WriteLine("Working", "Information");
			}
		}

		public override bool OnStart() {
			// Set the maximum number of concurrent connections 
			ServicePointManager.DefaultConnectionLimit = 12;

			// For information on handling configuration changes
			// see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.

			return base.OnStart();
		}
	}
}
