using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;


namespace UserMakerWPF.Classes
{
	internal class SearchComboBox
	{
		public void SelectComboBoxItemContains(ComboBox comboBox, string searchMe)
		{
			// Loop through all items in the ComboBox
			for (int i = 0; i < comboBox.Items.Count; i++)
			{
				// Get the current item as a ComboBoxItem
				ComboBoxItem currentItem = comboBox.Items[i] as ComboBoxItem;

				if (currentItem != null)
				{
					// Check if the current item's content contains the target substring
					string content = currentItem.Content.ToString();
					if (content.Contains(searchMe, StringComparison.OrdinalIgnoreCase))
					{
						// Select the item if it contains the substring
						comboBox.SelectedIndex = i;
						return;
					}
				}
			}

			MessageBox.Show($"Item containing '{searchMe}' not found in the ComboBox.");
		}
	}
}
