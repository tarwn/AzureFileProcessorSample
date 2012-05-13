using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Processor_Core {

	[Serializable]
	public class ItemBase {
		public Guid ResourceId { get; set; }
		public string FileName { get; set; }
		public DateTime Received { get; set; }
		public bool IsProcessed { get; set; }

		public ItemBase() { }
		protected ItemBase(SerializationInfo info, StreamingContext ctx) {}
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