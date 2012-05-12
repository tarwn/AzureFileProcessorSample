using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Processor_Core.Storage {
	public interface IStorageLocator {
		ITableStore GetTable(string name);
		IQueueStore GetQueue(string name);
		IBlobStore GetBlob(string name);
	}
}
