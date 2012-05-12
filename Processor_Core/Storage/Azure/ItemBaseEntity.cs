using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure.StorageClient;

namespace Processor_Core.Storage.Azure {
	public class ItemBaseEntity : TableServiceEntity 
	{
		public ItemBaseEntity(){}
		public ItemBaseEntity(ItemBase item) : base(item.ResourceId.ToString(), item.FileName)
		{
			ResourceId = item.ResourceId;
			FileName = item.FileName;
			Received = item.Received;
			IsProcessed = item.IsProcessed;
		}

		public Guid ResourceId { get; set; }
		public string FileName { get; set; }
		public DateTime Received { get; set; }
		public bool IsProcessed { get; set; }

	}
}
