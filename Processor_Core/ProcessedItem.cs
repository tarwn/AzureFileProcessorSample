using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Processor_Core {
	public class ProcessedItem : ItemBase {
		public override bool IsProcessed { get { return true; } }
	}
}
