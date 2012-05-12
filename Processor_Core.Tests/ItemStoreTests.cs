using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Processor_Core.Storage;
using Moq;

namespace Processor_Core.Tests {

	[TestFixture]
	public class ItemStoreTests {

		[Test]
		public void AddNewItem_StoresRawFileInBlob() {
			var mocks = new LocatorMocks();
			var store = new ItemStore(mocks.MockLocator.Object);
			var rawItem = new FullItem() {
				File = new byte[] { },
				FileName = "test.txt",
				Received = DateTime.Now.ToUniversalTime(),
				ResourceId = Guid.NewGuid()
			};

			store.AddNewItem(rawItem);

			mocks.MockRawBlobStore.Verify(mrb => mrb.Create(rawItem.ResourceId, rawItem.File), Times.Once());
		}

		[Test]
		public void AddNewItem_AddsItemToQueueForProcessing() {
			var mocks = new LocatorMocks();
			var store = new ItemStore(mocks.MockLocator.Object);
			var rawItem = new FullItem() {
				File = new byte[] { },
				FileName = "test.txt",
				Received = DateTime.Now.ToUniversalTime(),
				ResourceId = Guid.NewGuid()
			};

			store.AddNewItem(rawItem);

			mocks.MockQueueStore.Verify(mqs => mqs.Enqueue(Match.Create<ItemBase>(ib => ib.ResourceId == rawItem.ResourceId)), Times.Once());
		}

		[Test]
		public void AddNewItem_RecordsAdditionInTable() {
			var mocks = new LocatorMocks();
			var store = new ItemStore(mocks.MockLocator.Object);
			var rawItem = new FullItem() {
				File = new byte[] { },
				FileName = "test.txt",
				Received = DateTime.Now.ToUniversalTime(),
				ResourceId = Guid.NewGuid()
			};

			store.AddNewItem(rawItem);

			mocks.MockTableStore.Verify(mts => mts.Create(Match.Create<ItemBase>(ib => ib.ResourceId == rawItem.ResourceId)), Times.Once());
		}

	}

    public class LocatorMocks {
		public Mock<IStorageLocator> MockLocator { get; private set; }
		public Mock<IBlobStore> MockRawBlobStore { get; private set; }
		public Mock<IBlobStore> MockFinishedBlobStore { get; private set; }
		public Mock<IQueueStore> MockQueueStore { get; private set; }
		public Mock<ITableStore> MockTableStore { get; private set; }

		public LocatorMocks() {
			MockLocator = new Mock<IStorageLocator>();
			MockRawBlobStore = new Mock<IBlobStore>();
			MockLocator.Setup(ml => ml.GetBlob(ItemStore.RAW_BLOB_NAME)).Returns(MockRawBlobStore.Object);
			MockFinishedBlobStore = new Mock<IBlobStore>();
			MockLocator.Setup(ml => ml.GetBlob(ItemStore.FINISHED_BLOB_NAME)).Returns(MockFinishedBlobStore.Object);
			MockQueueStore = new Mock<IQueueStore>();
			MockLocator.Setup(ml => ml.GetQueue(ItemStore.QUEUE_NAME)).Returns(MockQueueStore.Object);
			MockTableStore = new Mock<ITableStore>();
			MockLocator.Setup(ml => ml.GetTable(ItemStore.TABLE_NAME)).Returns(MockTableStore.Object);
		}
	}
}
