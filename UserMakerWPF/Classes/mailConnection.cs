using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Security;
using System.DirectoryServices.ActiveDirectory;
using PowerShell = System.Management.Automation.PowerShell;
using static System.Resources.ResXFileRef;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Page;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Spin;
using System.Drawing.Text;
using System.DirectoryServices;
using UserMakerWPF;
using System.Threading;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace UserMaker
{
	public class MailBoxConnect
	{
		//private ProgressBar pb = Application.OpenForms["detmoldApp"].Controls["progressBar_Mail"] as ProgressBar;



		public static async Task<bool> GetMailboxInfo(string userName, string password, string firstName, string lastName, string domain)
		{
			using (DirectoryEntry entry = new DirectoryEntry("LDAP://internal.detmold.com.au"))
			using (DirectorySearcher searcher = new DirectorySearcher(entry))
			{
				// Configuration variables
				string exchangeServer = "sgmail00.internal.detmold.com.au";
				string strUserPrincipalName, strSamAccountName;
				// Create a secure password

				SecureString securePassword = new SecureString();
				foreach (char c in password) //password is user credential password
				{
					securePassword.AppendChar(c);
				}

				if (string.IsNullOrEmpty(lastName))
				{
					strUserPrincipalName = firstName + "@" + domain;
					strSamAccountName = firstName;
				}

				else
				{
					strUserPrincipalName = firstName + "." + lastName + "@" + domain;
					strSamAccountName = firstName + "." + lastName;
				}



				PSCredential credential = new PSCredential($"{userName}", securePassword);

				// Create WSMan connection info
				Uri connectionUri = new Uri($"http://{exchangeServer}/PowerShell/");

				WSManConnectionInfo connectionInfo = new WSManConnectionInfo(connectionUri, "http://schemas.microsoft.com/powershell/Microsoft.Exchange", credential);


				connectionInfo.AuthenticationMechanism = AuthenticationMechanism.Kerberos;


				// Create runspace
				using (Runspace rs = RunspaceFactory.CreateRunspace(connectionInfo))

				{
					// Initialize DirectoryEntry and DirectorySearcher objects
					searcher.Filter = "(userPrincipalName=" + strUserPrincipalName + ")";
					SearchResult result = searcher.FindOne();
					await Task.Run(() => rs.Open());

					if (result != null)
					{
						// Create credential object


						try

						{
							MessageBox.Show("Connection Established");

							MessageBox.Show("Mailbox creation started");

							MessageBox.Show("User Found");

							PowerShell powershell = PowerShell.Create();
							powershell.Runspace = rs;

							powershell.AddCommand("Enable-RemoteMailbox")
									  .AddParameter("Identity", strUserPrincipalName)
									  .AddParameter("RemoteRoutingAddress", $"{strSamAccountName}@detconnect.mail.onmicrosoft.com");

							var results = await Task.Run(() => powershell.Invoke());

							if (powershell.HadErrors)
							{
								foreach (var errorRecord in powershell.Streams.Error)
								{
									MessageBox.Show($"Error: {errorRecord.ToString()}");
								}
								return false;
							}

							MessageBox.Show($"Mail for the user: {strSamAccountName} created succesfully");

							return true;


						}

						catch (OperationCanceledException)
						{
							MessageBox.Show("Operation was canceled.");

							return false;
						}

						catch (Exception ex)
						{

							MessageBox.Show($"IN CATCH Unexpected error: {ex.Message}");

							return true;
						}

						finally
						{
							rs.Close();
							rs.Dispose();

						}

					}

					else

					{
						MessageBox.Show($"User: {strSamAccountName.Replace(".", " ")} not found. Will try again in 30 seconds....(SET TO 10 SECONDS FOR TESTING)");
						return false;

					}

				}
			}

		}
	}
}

