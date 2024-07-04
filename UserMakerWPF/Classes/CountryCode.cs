
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.DirectoryServices;
using System.Windows.Controls;

namespace UserMakerWPF.Classes
{
	internal class CountryCode
	{

		public static readonly Dictionary<string, string> CountryCodes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
		{
			{"Australia","AU"},
			{"United States","US"},
			{"China","CN"},
			{"Hong Kong","HK"},
			{"India","IN"},
			{"Indonesia","ID"},
			{"Korea","KR"},
			{"Malayasia","MY"},
			{"United Arab Emirates","AE"},
			{"Netherlands","NL"},
			{"New Zealand","NZ"},
			{"Philippines","PH"},
			{"Singapore","SG"},
			{"South Africa","SG"},
			{"Vietnam","VN"},
			{"Japan","JP"}
		};


		// Method to convert country name to country code
		public string ConvertCountryNameToCode(string countryName)
		{
			if (CountryCodes.TryGetValue(countryName, out string countryCode))
			{
				return countryCode;
			}
			else
			{
				try
				{
					// Fallback to CultureInfo if the country name is not in the dictionary
					var region = new RegionInfo(countryName);
					return region.TwoLetterISORegionName;
				}
				catch (ArgumentException)
				{
					throw new ArgumentException($"Invalid country name: {countryName}");
				}
			}
		}

		// Method to update the 'c' attribute in Active Directory
		public void UpdateCountryCodeInActiveDirectory(TextBox countryBox, DirectoryEntry directoryEntry)
		{
			try
			{
				string countryName = countryBox.Text; // Extract country name from countryBox in the form
				string countryCode = ConvertCountryNameToCode(countryName);


				directoryEntry.Properties["c"].Value = countryCode;
				directoryEntry.CommitChanges();

			}
			catch (Exception ex)
			{
				MessageBox.Show($"An error occurred while updating Active Directory: {ex.Message}");
			}
		}
	}
}
