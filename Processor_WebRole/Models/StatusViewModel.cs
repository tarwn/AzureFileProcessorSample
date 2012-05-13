using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Processor_Core;

namespace Processor_WebRole.Models {
	public class StatusViewModel {
		public IEnumerable<ItemBase> UnprocessedItems { get; set; }
		public IEnumerable<ItemBase> ProcessedItems { get; set; }
	}
}