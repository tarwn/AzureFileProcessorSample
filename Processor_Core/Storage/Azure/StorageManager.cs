using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Processor_Core.Storage.Azure {
	public class StorageManager {
		string _connectionString;	//RoleEnvironment.GetConfigurationSettingValue("StorageConnectionString")

		public ITableStore GetTable(string tableName) {
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_connectionString);
			CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
			tableClient.CreateTableIfNotExist(tableName);
			return new TableStore(tableName);
		}

	}
}

