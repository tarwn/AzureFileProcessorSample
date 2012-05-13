using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Processor_Core.Storage.Azure {
	public class TableStore : ITableStore {

		string _tableName;
		CloudStorageAccount _storageAccount;

		public TableStore(CloudStorageAccount storageAccount, string tableName) {
			_storageAccount = storageAccount;
			_tableName = tableName;
		}

		private TableServiceContext GetContext() {
			CloudTableClient tableClient = _storageAccount.CreateCloudTableClient();
			tableClient.CreateTableIfNotExist(_tableName);
			TableServiceContext serviceContext = tableClient.GetDataServiceContext();
			serviceContext.RetryPolicy = RetryPolicies.Retry(3, TimeSpan.FromSeconds(1));
			return serviceContext;
		}

		public void Create(ItemBase item) {
			TableServiceContext serviceContext = GetContext();
			serviceContext.AddObject(_tableName, item);
			serviceContext.SaveChangesWithRetries();
		}

		public void Update(ItemBase item) {
			TableServiceContext serviceContext = GetContext();
			serviceContext.UpdateObject(item);
			serviceContext.SaveChangesWithRetries();
		}

		public IEnumerable<ItemBase> GetUnprocessedItems() {
			TableServiceContext serviceContext = GetContext();
			return serviceContext.CreateQuery<ItemBase>(_tableName)
								 .Where(ib => !ib.IsProcessed)
								 .AsTableServiceQuery();
		}

		public IEnumerable<ItemBase> GetProcessedItems() {
			TableServiceContext serviceContext = GetContext();
			return serviceContext.CreateQuery<ItemBase>(_tableName)
								 .Where(ib => ib.IsProcessed)
								 .AsTableServiceQuery();
		}
	}
}
