using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Processor_Core.Storage;

namespace Processor_Core {
	public class ItemStore {

		public static string RAW_BLOB_NAME = "RawItems";
		public static string FINISHED_BLOB_NAME = "FinishedItems";
		public static string QUEUE_NAME = "ToBeProcessed";
		public static string TABLE_NAME = "Items";

		ITableStore _table;
		IBlobStore _rawBlob, _finishedBlob;
		IQueueStore _queue;

		public ItemStore(IStorageLocator storageLocator) {
			_table = storageLocator.GetTable(TABLE_NAME);
			_rawBlob = storageLocator.GetBlob(RAW_BLOB_NAME);
			_finishedBlob = storageLocator.GetBlob(FINISHED_BLOB_NAME);
			_queue = storageLocator.GetQueue(QUEUE_NAME);
		}

		public void AddNewItem(FullItem item) {
			_rawBlob.Create(item.ResourceId, item.File);
			_queue.Enqueue(item.AsSummary());
			_table.Create(item.AsSummary());
		}

		public IEnumerable<ItemBase> GetUnprocessedList() {
			return _table.GetUnprocessedItems();
		}

		public IEnumerable<ItemBase> GetProcessedList() {
			// ?
			return _table.GetProcessedItems();
		}

		public FullItem RetrieveForProcessing() {
			FullItem rawItem = null;

			var item = _queue.Dequeue();
			if (item != null) {
				rawItem = new FullItem(item);
				rawItem.File = _rawBlob.Retrieve(item.ResourceId);
			}
			return rawItem;
		}

		public void StoreFinishedItem(FullItem item) {
			_finishedBlob.Create(item.ResourceId, item.File);
			_rawBlob.Delete(item.ResourceId);
			_table.Update(item.AsSummary());
		}
	}
}

