using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UserMaker;

namespace UserMakerWPF
{
	/// <summary>
	/// Interaction logic for adminForm.xaml
	/// </summary>
	public partial class adminForm : Window
	{

		private string firstName;
		private string lastName;
		private string domain;
		
		string samName;
		string upnName;
		private ProgressBar progressBar;
		public adminForm(string firstName, string lastName, string domain, ProgressBar progressBar)
		{
			InitializeComponent();

			this.firstName = firstName;
			this.lastName = lastName;
			this.domain = domain;
			this.progressBar = progressBar;

			adminPassword.KeyDown += new KeyEventHandler(adminPassword_KeyDown);
		}

		
		#region ADMIN LOGIN
			private async void adminBtnOKClick(object sender, EventArgs e)
			{
				// to stop the async process when certain conditions are met
				//string[] propertiesToLoad = { "targetAddress" };

				if (string.IsNullOrEmpty(usernameBox.Text))
				{
					label_username.Foreground = Brushes.Red;

					MessageBox.Show("Fill the required fields", "Error");
					return;
				}
				label_username.Foreground = Brushes.Red;

				if (string.IsNullOrEmpty(adminPassword.Password))
				{
					label_password.Foreground = Brushes.Red;
					MessageBox.Show("Fill the required fields", "Error");
					return;
				}
				label_password.Foreground = Brushes.Black;

				string userName = usernameBox.Text;
				string password = adminPassword.Password;

				if (string.IsNullOrEmpty(lastName))
				{
					upnName = firstName + "@" + domain;
					samName = firstName;
				}

				else
				{
					upnName = firstName + "." + lastName + "@" + domain;
					samName = firstName + "." + lastName;
				}

				this.Close();

				using (DirectoryEntry entry = new DirectoryEntry("LDAP://internal.detmold.com.au"))
				using (DirectorySearcher searcher = new DirectorySearcher(entry))
				{
					searcher.Filter = "(targetAddress=" + "SMTP:" + samName + "@detconnect.mail.onmicrosoft.com)";
					SearchResult routingResult = searcher.FindOne();

					if (routingResult == null)
					{
						MessageBox.Show("Mail box not found");

						bool mailboxCreated = false;


						while (!mailboxCreated)
						{

							try
							{
								mailboxCreated = await MailBoxConnect.GetMailboxInfo(userName, password, firstName, lastName, domain);

								progressBar.Visibility = Visibility.Visible;
								//progressLabel.Visible = true;


								if (mailboxCreated)
								{
									progressBar.Visibility = Visibility.Hidden;
									//progressLabel.Visible = false;

									MessageBox.Show("Mailbox creation completed or error occured.(ONLY FOR TESTING)");
								}

								if (!mailboxCreated)
								{
									await ShowProgressDuringDelay(10000, progressBar);

									await Task.Delay(10000);
								}
							}

							catch (Exception ex)
							{
								MessageBox.Show($"Try again. Unexpected error: {ex.Message}");

								progressBar.Visibility	 = Visibility.Hidden;
								//progressLabel.Visible= false;
								mailboxCreated = true;

							}

						}
					}
					else

					{
						MessageBox.Show("MailBox already exists.?? pls check");
					}

				}

				progressBar.Visibility = Visibility.Visible;

			}
		#endregion

		#region For progress bar 
		private async Task ShowProgressDuringDelay(int delayMilliseconds, ProgressBar progressBar)
		{
			int delayIncrement = 100; // Update interval in milliseconds
			int totalSteps = delayMilliseconds / delayIncrement;
			int currentStep = 0;

			progressBar.Maximum = totalSteps;
			progressBar.Value = 0;



			for (int i = 0; i < totalSteps; i++)
			{
				await Task.Delay(delayIncrement);
				currentStep++;
				progressBar.Value = currentStep;
			}

			progressBar.Value = progressBar.Maximum; // Ensure progress bar is full at the end
		}
		#endregion

		private void adminPassword_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				// Trigger the search button click event
				adminBtnOKClick(this, new RoutedEventArgs());

				// Prevent the beep sound on Enter key press
				e.Handled = true;
			}
		}

		private void button2_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
