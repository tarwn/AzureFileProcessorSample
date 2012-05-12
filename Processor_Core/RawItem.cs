using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Processor_Core {
	public class RawItem : ItemBase {
		public byte[] File { get; set; }
		public override bool IsProcessed { get { return false; } }
	}
}