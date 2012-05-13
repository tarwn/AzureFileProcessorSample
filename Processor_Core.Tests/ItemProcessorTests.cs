using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Processor_Core.Tests {
	[TestFixture]
	public class ItemProcessorTests {

		[Test]
		public void ProcessItem_LowerCaseData_IsUpperCased() {
			string data = "my test data";
			var rawItem = new FullItem() {
				File = new UTF8Encoding().GetBytes(data),
				FileName = "test.txt",
				Received = DateTime.Now.ToUniversalTime(),
				ResourceId = Guid.NewGuid()
			};
			var processor = new ItemProcessor();

			var finishedItem = processor.ProcessItem(rawItem);
			string result = new UTF8Encoding().GetString(finishedItem.File);

			Assert.AreEqual(data.ToUpper(), result);
		}

		[Test]
		public void ProcessItem_RawData_MarksItemAsProcessed() {
			string data = "my test data";
			var rawItem = new FullItem() {
				File = new UTF8Encoding().GetBytes(data),
				FileName = "test.txt",
				Received = DateTime.Now.ToUniversalTime(),
				ResourceId = Guid.NewGuid()
			};
			var processor = new ItemProcessor();

			var finishedItem = processor.ProcessItem(rawItem);

			Assert.IsTrue(finishedItem.IsProcessed);
		}

	}
}
