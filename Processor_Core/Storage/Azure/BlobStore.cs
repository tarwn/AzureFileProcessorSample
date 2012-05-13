using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

namespace Processor_Core.Storage.Azure {
	public class BlobStore : IBlobStore {
		private string _blobStoreName;
		private CloudStorageAccount _storageAccount;

		public BlobStore(CloudStorageAccount storageAccount, string blobStoreName) {
			_storageAccount = storageAccount;
			_blobStoreName = blobStoreName.ToLower();	// required by blob storage
		}

		private CloudBlobContainer GetBlobContainer() {
			CloudBlobClient blobClient = _storageAccount.CreateCloudBlobClient();
			CloudBlobContainer container = blobClient.GetContainerReference(_blobStoreName);
			container.CreateIfNotExist();
			return container;
		}

		public void Create(Guid resourceId, byte[] file) {
			var container = GetBlobContainer();
			CloudBlob blob = container.GetBlobReference(resourceId.ToString());
			blob.UploadByteArray(file);
		}

		public byte[] Retrieve(Guid resourceId) {
			var container = GetBlobContainer();
			CloudBlob blob = container.GetBlobReference(resourceId.ToString());
			return blob.DownloadByteArray();
		}

		public void Delete(Guid resourceId) {
			var container = GetBlobContainer();
			CloudBlob blob = container.GetBlobReference(resourceId.ToString());
			blob.DeleteIfExists();
		}
	}
}
