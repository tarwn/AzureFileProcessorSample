using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Processor_Core.Storage {
	public interface ITableStore {
		void Create(ItemBase item);
		void Update(ItemBase item);
		IEnumerable<ItemBase> GetUnprocessedItems();
		IEnumerable<ItemBase> GetProcessedItems();
	}
}
