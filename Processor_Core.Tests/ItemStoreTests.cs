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

		private FullItem GetSampleItem() {
			var id = Guid.NewGuid();
			var data = new UTF8Encoding().GetBytes("test data " + id.ToString());

			return new FullItem() {
				File = data,
				FileName = "test.txt",
				Received = DateTime.Now.ToUniversalTime(),
				ResourceId = id
			};
		}

		[Test]
		public void AddNewItem_StoresRawFileInBlob() {
			var mocks = new LocatorMocks();
			var store = new ItemStore(mocks.MockLocator.Object);
			var rawItem = GetSampleItem();

			store.AddNewItem(rawItem);

			mocks.MockRawBlobStore.Verify(mrb => mrb.Create(rawItem.ResourceId, rawItem.File), Times.Once());
		}

		[Test]
		public void AddNewItem_AddsItemToQueueForProcessing() {
			var mocks = new LocatorMocks();
			var store = new ItemStore(mocks.MockLocator.Object);
			var rawItem = GetSampleItem();

			store.AddNewItem(rawItem);

			mocks.MockQueueStore.Verify(mqs => mqs.Enqueue(Match.Create<ItemBase>(ib => ib.ResourceId == rawItem.ResourceId)), Times.Once());
		}

		[Test]
		public void AddNewItem_RecordsAdditionInTable() {
			var mocks = new LocatorMocks();
			var store = new ItemStore(mocks.MockLocator.Object);
			var rawItem = GetSampleItem();

			store.AddNewItem(rawItem);

			mocks.MockTableStore.Verify(mts => mts.Create(Match.Create<ItemBase>(ib => ib.ResourceId == rawItem.ResourceId)), Times.Once());
		}

		[Test]
		public void RetrieveForProcessing_ItemInQueue_ReturnsItemWithRawData() {
			var mocks = new LocatorMocks();
			var store = new ItemStore(mocks.MockLocator.Object);
			Guid id = Guid.NewGuid();
			byte[] expected = new UTF8Encoding().GetBytes("test data");
			mocks.MockQueueStore.Setup(q => q.Dequeue()).Returns(new ItemBase() { ResourceId = id });
			mocks.MockRawBlobStore.Setup(b => b.Retrieve(id)).Returns(expected);

			var actual = store.RetrieveForProcessing();

			Assert.AreEqual(expected, actual.File);
		}

		[Test]
		public void RetrieveForProcessing_ItemInQueue_DequeuesASingleItem() {
			var mocks = new LocatorMocks();
			var store = new ItemStore(mocks.MockLocator.Object);
			Guid id = Guid.NewGuid();
			byte[] expected = new UTF8Encoding().GetBytes("test data");
			mocks.MockQueueStore.Setup(q => q.Dequeue()).Returns(new ItemBase() { ResourceId = id });
			mocks.MockRawBlobStore.Setup(b => b.Retrieve(id)).Returns(expected);

			var actual = store.RetrieveForProcessing();

			mocks.MockRawBlobStore.Verify(b => b.Retrieve(id), Times.Once());
		}

		[Test]
		public void RetrieveForProcessing_NoItemInQueue_ReturnsNull() {
			var mocks = new LocatorMocks();
			var store = new ItemStore(mocks.MockLocator.Object);
			Guid id = Guid.NewGuid();
			byte[] expected = new UTF8Encoding().GetBytes("test data");
			mocks.MockQueueStore.Setup(q => q.Dequeue()).Returns<ItemBase>(null);

			var actual = store.RetrieveForProcessing();

			Assert.IsNull(actual);
		}

		[Test]
		public void RetrieveForProcessing_NoItemInQueue_DoesNotAttemptToRetrieveDataFroBlobStore() {
			var mocks = new LocatorMocks();
			var store = new ItemStore(mocks.MockLocator.Object);
			Guid id = Guid.NewGuid();
			byte[] expected = new UTF8Encoding().GetBytes("test data");
			mocks.MockQueueStore.Setup(q => q.Dequeue()).Returns<ItemBase>(null);

			var actual = store.RetrieveForProcessing();

			mocks.MockRawBlobStore.Verify(b => b.Retrieve(It.IsAny<Guid>()), Times.Never());
		}

		[Test]
		public void FinishProcessingItem_AddsItemToFinishedBlobStore() {
			var mocks = new LocatorMocks();
			var store = new ItemStore(mocks.MockLocator.Object);
			var rawItem = GetSampleItem();

			store.StoreFinishedItem(rawItem);

			mocks.MockFinishedBlobStore.Verify(b => b.Create(rawItem.ResourceId, rawItem.File), Times.Once());
		}

		[Test]
		public void FinishProcessingItem_RemovesRawItemFromBlobStore() {
			var mocks = new LocatorMocks();
			var store = new ItemStore(mocks.MockLocator.Object);
			var rawItem = GetSampleItem();

			store.StoreFinishedItem(rawItem);

			mocks.MockRawBlobStore.Verify(b => b.Delete(rawItem.ResourceId), Times.Once());
		}

		[Test]
		public void FinishProcessingItem_UpdatesTableStoreInformation() {
			var mocks = new LocatorMocks();
			var store = new ItemStore(mocks.MockLocator.Object);
			var rawItem = GetSampleItem();

			store.StoreFinishedItem(rawItem);

			mocks.MockTableStore.Verify(ts => ts.Update(Match.Create<ItemBase>(ib => ib.ResourceId == rawItem.ResourceId)), Times.Once());
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
