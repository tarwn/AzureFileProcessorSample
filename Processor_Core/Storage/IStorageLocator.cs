using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Processor_Core.Storage {
	public interface IStorageLocator {
		public ITableStore GetTable(string name);
		public IQueueStore GetQueue(string name);
		public IBlobStore GetBlob(string name);
	}
}
