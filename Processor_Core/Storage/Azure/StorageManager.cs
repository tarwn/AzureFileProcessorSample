using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Processor_Core.Storage.Azure {
	public class StorageManager : IStorageLocator {
		string _connectionString;	//RoleEnvironment.GetConfigurationSettingValue("StorageConnectionString")

		public ITableStore GetTable(string tableName) {
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_connectionString);
			return new TableStore(storageAccount, tableName);
		}
		
		public IQueueStore GetQueue(string name) {
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_connectionString);
			return new QueueStore(storageAccount, name);

		}

		public IBlobStore GetBlob(string name) {
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_connectionString);
			return new BlobStore(storageAccount, name);
		}
	}
}

