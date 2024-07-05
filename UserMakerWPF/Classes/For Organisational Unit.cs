using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserMakerWPF.Classes
{
	internal class For_Organisational_Unit
	{
		public string[] Load_OU()
		{
			// Specify the LDAP path of the root organizational unit
			string rootOUPath = "LDAP://OU=,OU=,DC=,DC=,DC=,DC=au";

			// Create a DirectoryEntry object for the root OU
			using (DirectoryEntry rootOU = new DirectoryEntry(rootOUPath))
			{
				// Create a DirectorySearcher object to search within the root OU
				using (DirectorySearcher searcher = new DirectorySearcher(rootOU))
				{
					// Filter to find only organizational units (OU)
					searcher.Filter = "(objectClass=organizationalUnit)";

					// Perform the search
					SearchResultCollection results = searcher.FindAll();

					string[] organizationalUnits = new string[results.Count];
					for (int i = 0; i < results.Count; i++)
					
					{
						DirectoryEntry entry = results[i].GetDirectoryEntry();
						//Exclude "OU=" prefix in the list
						//Helps to store the attribute value in the diorectory without any issue.
						string ouName = entry.Name.Substring(3);
						organizationalUnits[i] = ouName;
					}

					return organizationalUnits;
				}
			}
		}
	}
}
