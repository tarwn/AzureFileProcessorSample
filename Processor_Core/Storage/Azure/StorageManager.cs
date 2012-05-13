using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Configuration;

namespace Processor_Core.Storage.Azure {
	public class StorageManager : IStorageLocator {
		string _connectionString;	//RoleEnvironment.GetConfigurationSettingValue("StorageConnectionString")

		/// <summary>
		/// Instantiates the storage manager with the specified settings for the connection string setting
		/// </summary>
		/// <param name="connectionSettingName">Name of setting that holds the configuration, sourced from role config or local config</param>
		public StorageManager(string connectionSettingName) {
			_connectionString = GetConfigurationSetting(connectionSettingName);
		}

		public ITableStore GetTable(string tableName) {
			return new TableStore(GetStorageAccount(), tableName);
		}

		public IQueueStore GetQueue(string name) {
			return new QueueStore(GetStorageAccount(), name);
		}

		public IBlobStore GetBlob(string name) {
			return new BlobStore(GetStorageAccount(), name);
		}

		private CloudStorageAccount GetStorageAccount() {
			return CloudStorageAccount.Parse(_connectionString);
		}

		public static string GetConfigurationSetting(string name) {
			if (RoleEnvironment.IsAvailable)
				return RoleEnvironment.GetConfigurationSettingValue(name);
			else
				return ConfigurationManager.AppSettings[name];
		}
	}
}

