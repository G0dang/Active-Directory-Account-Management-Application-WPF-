using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace UserMakerWPF.Classes
{
	internal class DNFinder
	{
		public string FindUserDistinguishedName(string userName)
		{
			try
			{
				using (DirectorySearcher searcher = new DirectorySearcher())
				{
					searcher.Filter = $"(&(objectClass=user)(|(name={userName})(displayName={userName})))";
					searcher.PropertiesToLoad.Add("distinguishedName");

					SearchResult result = searcher.FindOne();

					if (result != null)
					{
						return result.Properties["distinguishedName"][0].ToString();
					}
					else
					{
						// User not found
						return null;
					}
				}
			}
			catch (Exception ex)
			{
				// Handle any exceptions that occur during the search
				MessageBox.Show($"An error occurred: {ex.Message}");
				return null;
			}
		}
	}
}
