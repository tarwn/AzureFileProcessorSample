using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Processor_Core {
	public class ItemProcessor {

		//purposefully slow processing method w/ less than ideal memory usage to simulate real work
		public FullItem ProcessItem(FullItem rawItem) {
			string originalData = new UTF8Encoding().GetString(rawItem.File);
			string[] dataRows = originalData.Split('\n');

			List<string> result = new List<string>();
			foreach (string s in dataRows) {
				string s2 = s;
				foreach (char c in s.ToCharArray()) {
					s2 = s2.Replace(c.ToString(), c.ToString().ToUpper());
				}
				result.Add(s2);
			}

			rawItem.MarkAsProcessed();
			rawItem.File = new UTF8Encoding().GetBytes(String.Join("\n", result));
			return rawItem;
		}

	}
}
