using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Processor_Core.Storage.Azure {
	public class TableStore : ITableStore {

		string _tableName;

		public TableStore(string name) {
			_tableName = name;
		}

		public void Create(ItemBase item) {
			throw new NotImplementedException();
		}

		public void Update(ItemBase item) {
			throw new NotImplementedException();
		}

		public IEnumerable<ItemBase> GetUnprocessedItems() {
			throw new NotImplementedException();
		}

		public IEnumerable<ItemBase> GetProcessedItems() {
			throw new NotImplementedException();
		}
	}
}
