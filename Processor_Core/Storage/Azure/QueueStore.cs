using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace Processor_Core.Storage.Azure {
	public class QueueStore : IQueueStore {
		private string _queueName;
		private CloudStorageAccount _storageAccount;

		public QueueStore(CloudStorageAccount storageAccount, string queueName) {
			_queueName = queueName;
			_storageAccount = storageAccount;
		}

		private CloudQueue GetQueue() {
			CloudQueueClient queueClient = _storageAccount.CreateCloudQueueClient();
			CloudQueue queue = queueClient.GetQueueReference(_queueName);
			queue.CreateIfNotExist();
			return queue;
		}

		public void Enqueue(ItemBase item) {

			CloudQueueMessage message;
			using(var stream = new MemoryStream()){
				new BinaryFormatter().Serialize(stream, item);
				stream.Flush();
				message = new CloudQueueMessage(stream.ToArray());
			}
			GetQueue().AddMessage(message);
		}

		public ItemBase Dequeue() {
			var queue = GetQueue();
			var message = queue.GetMessage();
			if (message == null)
				return null;

			queue.DeleteMessage(message);

			object result = null;
			using (var stream = new MemoryStream(message.AsBytes)) {
				result = new BinaryFormatter().Deserialize(stream);
			}
			return result as ItemBase;
		}
	}
}
