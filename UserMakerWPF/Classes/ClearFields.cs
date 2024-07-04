using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace UserMakerWPF.Classes
{
	internal class ClearFields
	{
		public void ClearControls(TabControl tabControl)
		{
			foreach (TabItem tabItem in tabControl.Items)
			{
				if (tabItem.Content is Panel panel)
				{
					ClearPanelControls(panel);
				}
			}
		}
		private void ClearPanelControls(Panel panel)
		{
			foreach (UIElement control in panel.Children)
			{
				if (control is TextBox textBox)
				{
					textBox.Clear();
				}
				else if (control is ComboBox comboBox)
				{
					comboBox.SelectedIndex = -1;
					comboBox.Text = "";
				}
				else if (control is CheckBox checkBox)
				{
					checkBox.IsChecked = false;
				}
				else if (control is Panel nestedPanel)
				{
					ClearPanelControls(nestedPanel); // Recursively clear nested panels
				}
			}
		}
	}
}
