using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Processor_Core {
	public class ItemBase {
		public Guid ResourceId { get; set; }
		public string FileName { get; set; }
		public DateTime Received { get; set; }
		public bool IsProcessed { get; set; }

		public ItemBase() { }
		public ItemBase(ItemBase item) {
			CopyFrom(item);
		}

		public void CopyFrom(ItemBase item) {
				ResourceId = item.ResourceId;
				FileName = item.FileName;
				Received = item.Received;
				IsProcessed = item.IsProcessed;
		}

		public ItemBase AsSummary() {
			return new ItemBase() {
				ResourceId = ResourceId,
				FileName = FileName,
				Received = Received,
				IsProcessed = IsProcessed
			};
		}
	}
}