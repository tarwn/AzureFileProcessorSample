using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Processor_Core.Storage.Azure {
	public class StorageManager {
		string _connectionString;	//RoleEnvironment.GetConfigurationSettingValue("StorageConnectionString")

		public ITableStore GetTable(string name) {
			// Retrieve storage account from connection-string
			CloudStorageAccount storageAccount = CloudStorageAccount.Parse(_connectionString);

			// Create the table client
			CloudTableClient tableClient = storageAccount.CreateCloudTableClient();

			// Create the table if it doesn't exist
			string tableName = "people";
			tableClient.CreateTableIfNotExist(tableName);
		}

	}
}

