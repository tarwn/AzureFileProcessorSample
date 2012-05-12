using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Processor_Core {
	public class ItemStore {

		public void AddNewItem(RawItem item) {
			_rawBlob.Store(item);
			_queue.Enqueue(item as ItemBase);
			_table.Insert(item.ResourceId, item as ItemBase);
		}

		public IEnumerable<ItemBase> GetUnprocessedList() {
			// ?
			return new List<ItemBase>();
		}

		public IEnumerable<ItemBase> GetProcessedList() {
			// ?
			return new List<ItemBase>();
		}

		public RawItem RetrieveForProcessing() {
			var item = _queue.Dequeue() as ItemBase;
			if (item != null) {
				return _blob.Get(item.ResourceId);
			}
		}

		public void FinishProcessingItem(ProcessedItem item) {
			_finishedBlob.Store(item);
			_rawBlob.Remove(item.ResourceId);
			_table.Update(item.ResourceId, item as ItemBase);
		}
	}
}
