using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Processor_Core {
	public class ItemBase {
		public Guid ResourceId { get; set; }
		public string FileName { get; set; }
		public DateTime Received { get; set; }
		public virtual bool IsProcessed { get; }
	}
}
