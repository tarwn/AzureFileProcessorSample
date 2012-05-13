using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace Processor_Core {
	public class FullItem : ItemBase {
		public byte[] File { get; set; }

		public FullItem() { }
		public FullItem(ItemBase item) : base(item) { }

		public void ReadFileFromStream(System.IO.Stream inputStream) {
			using (var reader = new BinaryReader(inputStream)) {
				File = reader.ReadBytes((int) inputStream.Length - 1);
			}
		}
	}
}