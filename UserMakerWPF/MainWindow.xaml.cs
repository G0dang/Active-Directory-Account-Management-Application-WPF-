using System.DirectoryServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Controls.Primitives;
using MaterialSkin.Controls;
using System.DirectoryServices.ActiveDirectory;
using UserMakerWPF.Classes;
using System.Net.Http.Headers;
using System.Data;
using Newtonsoft.Json;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Net.Http;






//using System.Windows.Forms;


namespace UserMakerWPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private const string OUDN = "OU=Aayush Test,OU=Accounts,DC=internal,DC=detmold,DC=com,DC=au";
		private static readonly string apiBaseUrl = "https://detmoldgroupuat.haloitsm.com";

		private static string username = "Aayush Gurung";
		private static string password = "DetmoldGroupUAT2024!";
		private static string clientID = "2c45fe93-3daa-40b2-9533-e2fd19df7dc3";

		private string managerDistinguishedName;
		private DNFinder dnFinder;
		private string CompanyName;
		private int i;

		private SearchComboBox domainFinder;
		public MainWindow()
		{
			InitializeComponent(); 

			pb_RM.Visibility = Visibility.Hidden;
			pb_mail.Visibility = Visibility.Hidden;

			Loaded += MainWindow_Load;
			dnFinder = new DNFinder();    

			domainFinder = new SearchComboBox();

			ticketID_textbox.KeyDown += new KeyEventHandler(ticketID_textbox_KeyDown);
		}

		

		#region 1. MAIN FORM | METHOD/s HERE LOADs ON RUN
		private async void MainWindow_Load(object sender, EventArgs e)
		{
			#region 1.1. Calling Load_RM method and set up asynchronous Task for regional manager loading 
			//using try catch statement to show the progress bar when loading regional manager
			try
			{
				// Show the progress bar
				
				pb_RM.Visibility = Visibility.Visible;

				for (int i = 0; i <= 100; i= i+10)
				{
					pb_RM.Value = i;
					await Task.Delay(1000); // Adjust delay as needed
				}

				UserInformation[] userInformations = await LoadRegionalManagersAsync();
				UserInformation[] users = userInformations;

				if (users != null && users.Length > 0)
				{
					//sort the list in an alphabetical order

					//using LINQ method to order it alphabetically before binding it to the RMBox.
					users = users.OrderBy(u => u.DisplayName).ToArray();

					RMBox.DisplayMemberPath = "DisplayName";
					RMBox.SelectedValuePath = "DistinguishedName";
					RMBox.ItemsSource = users;
				}
				else
				{
					MessageBox.Show("Failed to retrieve regional managers data.");
				}
			}
			catch (Exception ex)
			{

				MessageBox.Show($"An error occurred: {ex.Message}");
			}
			finally
			{
				// Hide the progress bar when loading is complete
				pb_RM.Visibility = Visibility.Hidden;


			}
			#endregion
		}
		#endregion


		private async Task<UserInformation[]> LoadRegionalManagersAsync()
		{
			RegionalManager regionalManagerLoader = new RegionalManager();
			return await Task.Run(() => regionalManagerLoader.Load_RM());
		}


		private void RMBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (RMBox.SelectedItem != null)
			{
				UserInformation selectedUser = (UserInformation)RMBox.SelectedItem;
				string displayName = selectedUser.DisplayName;
				string distinguishedName = selectedUser.DistinguishedName;

				
			}
		}
		private void Next_Click(object sender, RoutedEventArgs e)
		{

		}

		private void fName_TextChanged(object sender, TextChangedEventArgs e)
		{

        }
		#region 6. CODE TO VERIFY IF THE USER IS IN THE DIRECTORY

		private void verifyUser_btnClick(object sender, RoutedEventArgs e)
		{
			// Determine user identity based on first name and last name
			string UPN, Pre2000;

			if (string.IsNullOrEmpty(lName.Text))
			{
				UPN = fName.Text + "@" + domainList.Text;
				Pre2000 = fName.Text;
			}
			else
			{
				UPN = fName.Text + "." + lName.Text + "@" + domainList.Text;
				Pre2000 = fName.Text + "." + lName.Text;
			}

			if (!string.IsNullOrEmpty(fName.Text))
			{
				// Initialize DirectoryEntry and DirectorySearcher objects
				using (DirectoryEntry entry = new DirectoryEntry("LDAP://internal.detmold.com.au"))
				using (DirectorySearcher searcher = new DirectorySearcher(entry))
				{
					// Set the filter to search for user with the provided UPN
					searcher.Filter = "(userPrincipalName=" + UPN + ")";
					SearchResult result = searcher.FindOne();
					if (result != null)
					{
						System.Windows.MessageBox.Show("UPN/Email already exists");
						return;
					}

					// Set the filter to search for user with the provided samaccount name
					searcher.Filter = "(sAMAccountName=" + Pre2000 + ")";
					result = searcher.FindOne();
					if (result != null)
					{
						System.Windows.MessageBox.Show("Username already Exists");
						return;
					}

					MessageBox.Show("Continue to create new user");
				}
			}
			else 
			{
				fnameLabel.Foreground = Brushes.Red;
				MessageBox.Show("Please fill the highlighted field before verifying", "Error");
			}

			
		}
		#endregion

		private void textBox_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		private void disclaimerSuffix_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{

		}

		#region FOR PASSWORD
		private void passwordCheckBox_Checked(object sender, RoutedEventArgs e)
		{
			// Set the desired length of the password
			int length = 15;
			; // Example length

			// Generate the password
			string password = CreatePassword(length);


			passwordBox.Password = password; //filling the password text box with the newly generated password

			string CreatePassword(int length) //random password generator
			{
				const string valid = "1234567890~!@#$%^&*()_-=+abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
				
				StringBuilder res = new StringBuilder(); // Changed StringBuilder declaration


				Random rnd = new Random();

				while (0 < length--)
				{
					res.Append(valid[rnd.Next(valid.Length)]);

				}

				return res.ToString(); ;
			}

			
		}
		

		private void passwordCheckBox_Unchecked(object sender, RoutedEventArgs e)
		{
			//clear passwordtextbox when the check box is unchecked
			
				// Clear the text of the passwordBox
				passwordBox.Password = string.Empty;
			
		}

		private void button_Click(object sender, RoutedEventArgs e)
		{
			Clipboard.SetText(passwordBox.Password);
		}

		#endregion

		private void lName_TextChanged(object sender, TextChangedEventArgs e)
		{

		}

		#region FOR NEXT BUTTON IN TAB 1
		private void nextBtn_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(fName.Text))
			{
				//highlight the text field
				fnameLabel.Foreground = Brushes.Red;
				MessageBox.Show("Please fill the highlighted field", "Error");
								
				return;
				// Exit the method to prevent further processing
			}
			
			fName.Foreground = Brushes.Black; //to reset the font color to black if it was highlighted previously.
			
			if (string.IsNullOrWhiteSpace(domainList.Text))
			{
				domainLabel.Foreground = Brushes.Red;
				System.Windows.MessageBox.Show("Please select a domain from the list", "Error");
				
				return;
			}

			domainLabel.Foreground = Brushes.Black;

			if (string.IsNullOrWhiteSpace(passwordBox.Password))
			{
				passwordLabel.Foreground = Brushes.Red;
				System.Windows.MessageBox.Show("Please create a unique password from the the password generater or create your unique own", "Error");
				
				return;
			}

			passwordLabel.Foreground = Brushes.Black;

			if (string.IsNullOrWhiteSpace(disclaimerSuffix.Text))
			{
				discLabel.Foreground = Brushes.OrangeRed;
				System.Windows.MessageBox.Show("Please select a Disclaimer Suffix for the user", "Error");
				return;
			}

			discLabel.Foreground = Brushes.Black;

			tabControl.SelectedItem = tab2;
		}
		#endregion

		private void textBox_TextChanged_1(object sender, TextChangedEventArgs e)
		{

		}

		private void pb_mail_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{

		}
		private void pb_RM_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{

		}

		private void button2_Click(object sender, RoutedEventArgs e)
		{

		}

		#region For searching user detials using TICKET SEARCH BUTTON
		private async void searchTicket_btnClick(object sender, RoutedEventArgs e)
		{
			{
				string ticketID = ticketID_textbox.Text;

				if (string.IsNullOrEmpty(ticketID))
				{
					MessageBox.Show("Enter the ticket ID", "Error");
					return;
				}

				string accessToken = await AuthenticateAsync(username, password, clientID);

				if (accessToken != null)
				{
					var ticket = await GetTicketAsync(ticketID, accessToken);


					if (ticket != null)
					{
						string ticketInfo = $"Ticket ID: {ticket.id}\nTicket Summary: {ticket.summary}\nDetails:\n";

						bool isNewUserRequest = false;


						var cityDict = new CityDictionary();
						string postcode = string.Empty;
						string city = string.Empty;

						#region for separating employee addressssss
						//compare the string of the name in the customfields. If the string matches, display and fill the values accordingly.
						foreach (var field in ticket.customfields)
						{
							if (field.name == "CFEmployeeAddress")
							{
								string address = field.display;
								//checking if the address in the ticket contains a city name as well as the post code
								city = cityDict.Cities.Keys.FirstOrDefault(c => address.Contains(c)) ?? ""; //c=>address.Contains(c) is used as a predicate

								if (!string.IsNullOrEmpty(city))
								{
									ticketInfo += $"City: {city}\n";
									cityBox.Text = city;

									// Find the postcode in the address
									if (cityDict.Cities.TryGetValue(city, out var postcodes))
									{
										postcode = postcodes.FirstOrDefault(pc => address.Contains(pc)) ?? "";

										if (!string.IsNullOrEmpty(postcode))
										{
											ticketInfo += $"Postcode: {postcode}\n";
											zipBox.Text = postcode;

											// Remove city and postcode from address
											address = address.Replace(city, "").Replace(postcode, "").Trim();
										}
									}
								}
								
								else
								
								{
									// Check for empty string key (where the address does not copntain city name but has post code)
									if (cityDict.Cities.TryGetValue("", out var postcodes))
									{
										postcode = postcodes.FirstOrDefault(pc => address.Contains(pc)) ?? "";

										if (!string.IsNullOrEmpty(postcode))
										{
											ticketInfo += $"Postcode: {postcode}\n";
											zipBox.Text = postcode;

											// Remove postcode from address
											address = address.Replace(postcode, "").Trim();
										}

										cityBox.Text = ""; // No specific city found
									}
									
									else
									{
										cityBox.Text = "";
										zipBox.Text = "";
									}
								}
								// Update the street address
								streetBox.Text = address.Trim();
							}
							#endregion

							#region 7.1. Populating textboxes with the information from the ticket
							//populating text boxes here by grabbing the details from the ticket
							if (field.name == "CFfirstName")
							{
								fName.Text = field.display.Trim();
							}
							if (field.name == "CFlastName")
							{
								lName.Text = field.display.Trim();
							}
							if (field.name == "CFJobTitle")
							{
								jTitle.Text = field.display.Trim();

							}
							if (field.name == "CFEmployeeCountry")
							{
								countryBox.Text = field.display.Trim();
								OUBox.Text = field.display.Trim();
							}
							if (field.name == "CFEmployeeState")
							{
								stateBox.Text = field.display.Trim();
							}
							#endregion

							#region 7.2. searching the DN of the manager name we got from the ticket.

							if (field.name == "CFEmployeeManager")
							{
								if (!string.IsNullOrEmpty(field.display))
								{
									RMBox.IsEnabled = false; //disable the reginal manager combobox.
									string managerName = field.display;

									// Find the distinguished name of the manager
									managerDistinguishedName = dnFinder.FindUserDistinguishedName(managerName);

									// Check if the distinguished name was found
									if (managerDistinguishedName != null)
									{
										// Set the display manager name in the box but the Dn will be supplied to update the user attribute
										tempBox.Text = managerName;
										ticketInfo += $"{field.name}: {managerDistinguishedName}\n";

									}
								}
								else
								{
									// Handle the case where the manager was not found
									MessageBox.Show("Manager not found. Select manager name from the list manually", "Note");
								}
							}

							#endregion

							#region 7.3. To use the company name for searching the domain 
							if (field.name == "CFEmployeeCompany")
							{

								CompanyName = field.display;
								ticketInfo += $"{field.name}: {field.display}\n";
								string compName = CompanyName;

								compName = compName.Replace(" ", "");

								domainFinder.SelectComboBoxItemContains(domainList, compName);
							}
							#endregion

							if (field.name == "CFEmployeeAddress")
							{
								ticketInfo += $"{field.name}: {field.display}\n";
								isNewUserRequest = true;
							}
						}

						if (!isNewUserRequest)
						{
							ticketInfo += "This ticket is not a new user request";
						}

						MessageBox.Show(ticketInfo, "Ticket Information");
					}
				}
			}
		}
		#endregion

		//search for tickets
		#region 15. Generate Token
		private static async Task<string> AuthenticateAsync(string username, string password, string clientID)
		{
			using (var client = new HttpClient { BaseAddress = new Uri(apiBaseUrl) })
			{
				var content = new FormUrlEncodedContent(new[]
				{
					new KeyValuePair<string, string>("grant_type", "password"),
					new KeyValuePair<string, string>("client_id", clientID),
					new KeyValuePair<string, string>("username", username),
					new KeyValuePair<string, string>("password", password),
					new KeyValuePair<string, string>("scope", "all")
				});

				var response = await client.PostAsync("/auth/token", content);

				if (response.IsSuccessStatusCode)
				{
					var jsonResponse = await response.Content.ReadAsStringAsync();
					var authResult = JsonConvert.DeserializeObject<AuthenticationRoot>(jsonResponse);
					return authResult.access_token;
				}

				else
				{
					MessageBox.Show($"Authentication failed with status code: {response.StatusCode}", "Error");
					return null;
				}
			}
		}
		#endregion

		#region 16. search for the ticket provided and return data
		private static async Task<Request> GetTicketAsync(string ticketID, string accessToken)
		{
			using (var client = new HttpClient { BaseAddress = new Uri(apiBaseUrl) })
			{
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
				var response = await client.GetAsync($"/api/tickets/{ticketID}");

				if (response.IsSuccessStatusCode)
				{
					var jsonResponse = await response.Content.ReadAsStringAsync();
					return JsonConvert.DeserializeObject<Request>(jsonResponse);
				}
				else
				{
					MessageBox.Show($"Failed to retrieve ticket with status code: {response.StatusCode}", "Error");
					return null;
				}
			}
		}
		#endregion

		#region 17. [AdminLogin btn click event] Passing values from the textboxes in the mainform to the admin form. adminForm requires values from mainform textfields.
		private void btnAdminLogin_Click(object sender, EventArgs e)
		{
			string firstName = fName.Text;
			string lastName = lName.Text;
			string domain = domainList.Text;

			//string password = passwordBox.Text;

			var existingAdminForm = Application.Current.Windows.OfType<adminForm>().FirstOrDefault();

			if (existingAdminForm != null)
			{
				existingAdminForm.Activate(); // Bring the existing form to the front
			}
			else
			{
				adminForm adminLoginForm = new adminForm(firstName, lastName, domain, pb_mail);
				adminLoginForm.Show();
			}


		}

		#endregion

		private void button4_Click(object sender, RoutedEventArgs e)
		{

		}

		private void btnCreateUser_Click(object sender, RoutedEventArgs e)
		{
			#region check empty field where input is required
			if (string.IsNullOrWhiteSpace(fName.Text))
			{
				//highlight the text field
				fName.Foreground = Brushes.Red;
				MessageBox.Show("Please fill the highlighted field", "Error");
				return;
				// Exit the method to prevent further processing
			}
			fName.Foreground = Brushes.Black;

			if (string.IsNullOrWhiteSpace(domainList.Text))
			{
				domainLabel.Foreground =Brushes.Red;
				MessageBox.Show("Please select a domain from the list", "Error");
				return;
			}
			domainLabel.Foreground = Brushes.Black;


			if (string.IsNullOrWhiteSpace(passwordBox.Password))
			{
				passwordLabel.Foreground = Brushes.Red;
				MessageBox.Show("Please create a unique password from the the password generater or create your unique own", "Error");
				return;
			}
			passwordLabel.Foreground = Brushes.Black;


			if (string.IsNullOrWhiteSpace(OUBox.Text))
			{
				orgUnitLabel.Foreground = Brushes.Red;
				MessageBox.Show("Please select a organisational unit from the list", "Error");
				return;
			}
			orgUnitLabel.Foreground = Brushes.Black;


			//if (string.IsNullOrWhiteSpace(RMBox.Text))
			//{
			//	reportingManager.ForeColor = Color.OrangeRed;
			//	MessageBox.Show("Please select a reporting manager from the list", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			//	return;
			//}
			//reportingManager.ForeColor = Color.Black;

			if (string.IsNullOrWhiteSpace(disclaimerSuffix.Text))
			{
				discLabel.Foreground = Brushes.Red;
				MessageBox.Show("Please select a Disclaimer Suffix for the user", "Error");
				return;
			}
			discLabel.Foreground = Brushes.Black;


			if (string.IsNullOrWhiteSpace(cCenter.Text))
			{
				ccLabel.Foreground = Brushes.Red;
				MessageBox.Show("Please select a Cost Center for the user", "Error");
				return;
			}
			ccLabel.Foreground = Brushes.Black;

			if (string.IsNullOrWhiteSpace(streetBox.Text) &&
				string.IsNullOrWhiteSpace(cityBox.Text) &&
				string.IsNullOrWhiteSpace(stateBox.Text) &&
				string.IsNullOrWhiteSpace(countryBox.Text) &&
				string.IsNullOrWhiteSpace(zipBox.Text))
			{
				userAddress.Foreground = Brushes.Red;
				MessageBox.Show("Please select an address", "Error");
				return;
			}

			userAddress.Foreground = Brushes.Black;
			#endregion

			#region 8.1. creating UPN,sAMAccount Name, directory path and so on....
			string UPN, Pre2000;
			if (string.IsNullOrEmpty(lName.Text))
			{
				UPN = fName.Text + "@" + domainList.Text;
				Pre2000 = fName.Text;
			}
			else
			{
				UPN = fName.Text + "." + lName.Text + "@" + domainList.Text;
				Pre2000 = fName.Text + "." + lName.Text;
			}
			using (DirectoryEntry entry = new DirectoryEntry("LDAP://internal.detmold.com.au"))
			using (DirectorySearcher searcher = new DirectorySearcher(entry))
			{
				// Set the filter to search for user with the provided UPN
				searcher.Filter = "(userPrincipalName=" + UPN + ")";
				SearchResult result = searcher.FindOne();
				if (result != null)
				{
					MessageBox.Show("User already Exists. Unable to create duplicate user");
					return;
				}

				// Set the filter to search for user with the provided pre-Windows 2000 account name
				searcher.Filter = "(sAMAccountName=" + Pre2000 + ")";
				result = searcher.FindOne();
				if (result != null)
				{
					MessageBox.Show("User already Exists. Unable to create duplicate user");
					return;
				}
			}
			#endregion

			#region 8.2. update the users attribute values in the active directory
			try
			{
				// Create a DirectoryEntry object for the OU
				DirectoryEntry ou = new DirectoryEntry("LDAP://" + OUDN);

				// Creating a new user object in the Organisational Unit (OU)
				DirectoryEntry newUser = ou.Children.Add("CN=" + fName.Text + " " + lName.Text, "user");


				// Set user attributes //Unknown user attributes 
				newUser.Properties["givenName"].Value = fName.Text;
				newUser.Properties["mail"].Value = UPN;
				newUser.Properties["userPrincipalName"].Value = UPN;
				newUser.Properties["samAccountName"].Value = Pre2000;


				// When a user does not have a last name. Do not update the attributes
				// ! inverts the function of the statement.
				if (!String.IsNullOrEmpty(lName.Text))
				{
					newUser.Properties["sn"].Value = lName.Text;
					newUser.Properties["displayName"].Value = lName.Text + " " + lName.Text;

				}

				//newUser.Properties["countryCode"].Value = cCode.Text; //getting error here because the attribute only accepts numeric values

				if (!String.IsNullOrEmpty(streetBox.Text))
				{
					newUser.Properties["streetAddress"].Value = streetBox.Text;
				}
				if (!String.IsNullOrEmpty(cityBox.Text))
				{
					newUser.Properties["l"].Value = cityBox.Text;
				}
				if (!String.IsNullOrEmpty(stateBox.Text))
				{
					newUser.Properties["st"].Value = stateBox.Text;
				}
				if (!String.IsNullOrEmpty(countryBox.Text))
				{
					newUser.Properties["co"].Value = countryBox.Text;
				}
				if (!String.IsNullOrEmpty(zipBox.Text))
				{
					newUser.Properties["postalCode"].Value = zipBox.Text;
				}

				if (!String.IsNullOrEmpty("disclaimerSuffix"))
				{
					newUser.Properties["scriptPath"].Value = $"Disclaimer.exe {disclaimerSuffix.Text}";
				}

				if (!String.IsNullOrEmpty("CompanyName"))
				{
					newUser.Properties["company"].Value = CompanyName;
				}
				
				
				newUser.Properties["ou"].Value = OUBox.Text;

				if (!String.IsNullOrEmpty("jTitle"))
				{
					newUser.Properties["title"].Value = jTitle.Text;
				}

				if (!String.IsNullOrEmpty(tempBox.Text))
				{
					newUser.Properties["manager"].Value = managerDistinguishedName;
				}

				else
				{
					#region 8.2.1. [REGIONAL MANAGER ATTRIBUTE] Previously selected regional manager from the list and updated the attribute 
					//but now this may not be need as we have loaded the manager name from the ticket and have created generated the DN name.
					//We have retrieved the DN of the manager from the reginal manager class.
					//update 1: regional manager will be supplied through the regional manager list selection if the ticket returns empty manager name

					if (RMBox.SelectedItem != null)
					{
						try {
							UserInformation selectedUser = (UserInformation)RMBox.SelectedItem;


							string distinguishedNameOfManager = selectedUser.DistinguishedName;

							newUser.Properties["manager"].Value = distinguishedNameOfManager;
							}

						catch (System.DirectoryServices.DirectoryServicesCOMException ex)
						{
							// Handle directory service errors
							MessageBox.Show($"Error setting manager: {ex.Message}");
						}
						catch (Exception ex)
						{
							MessageBox.Show($"Error in regional manager : {ex.Message}");
						}

						
					}
					else
					{
						MessageBox.Show("Manager DN is invalid or empty");
					}
					#endregion
				}
				//calling CountryCode Class in here. 

				CountryCode countryCodeUpdater = new CountryCode();
				countryCodeUpdater.UpdateCountryCodeInActiveDirectory(countryBox, newUser);




				// Save the user object to Active Directory

				newUser.CommitChanges();

				MessageBox.Show("User created successfully.", "Success");

			}
			catch (Exception ex)
			{
				MessageBox.Show("Error: " + ex.Message, "Error");
			}
			#endregion
		}

		private void button3_Click(object sender, RoutedEventArgs e)
		{

		}

		#region 13. Button click event for organisational units.

		private void ou_BtnClick(object sender, EventArgs e)
		{
			#region 13.1. DIRECTLY SEARCH FOR THE ORGANISATIONAL UNITS FROM THE DIRECTORY
			// Instantiate the class to retrieve organizational units
			For_Organisational_Unit extractOU = new For_Organisational_Unit();

			// Retrieve the organizational units
			string[] organizationalUnits = extractOU.Load_OU();

			// Populate the ComboBox with the retrieved organizational units
			if (organizationalUnits != null)
			{

				OUBox.Items.Clear();
				foreach (string ou in organizationalUnits)
				{
					OUBox.Items.Add(ou);
				}
			}
			else
			{
				MessageBox.Show("Failed to retrieve organizational units data.");
			}
			#endregion

		}
		#endregion
		

		private void btnClearForm_Click(object sender, RoutedEventArgs e)
		{
			ClearFields wipeAll = new ClearFields();

			wipeAll.ClearControls(tabControl);
			// clear button will also clear the highlights showing the missing field
			fnameLabel.Foreground = Brushes.Black;
			domainLabel.Foreground = Brushes.Black;
			passwordLabel.Foreground = Brushes.Black;
			orgUnitLabel.Foreground = Brushes.Black;
			discLabel.Foreground = Brushes.Black;
			ccLabel.Foreground = Brushes.Black;
			rmLabel.Foreground = Brushes.Black;

		}

		private void ticketID_textbox_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.Key == Key.Enter)
			{
				// Trigger the search button click event
				searchTicket_btnClick(this, new RoutedEventArgs());

				// Prevent the beep sound on Enter key press
				e.Handled = true;
			}
		}

		private void ticketID_textbox_TextChanged(object sender, TextChangedEventArgs e)
		{

		}
	}

	#region classes for ticket information retireval from the Halo API
	public class AuthenticationRoot
	{
		public string access_token { get; set; }
	}

	public class Customfield
	{
		public int id { get; set; }
		public string name { get; set; }
		public string label { get; set; }
		public object value { get; set; }
		public string display { get; set; }
	}

	public class Request
	{
		public string id { get; set; }
		public string summary { get; set; }
		public List<Customfield> customfields { get; set; }
	}
	#endregion


}