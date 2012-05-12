using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Processor_Core {
	public class FullItem : ItemBase {
		public byte[] File { get; set; }

		public FullItem() { }
		public FullItem(ItemBase item) : base(item) { }
	}
}