# Active-Directory-Account-Management-Application-WPF-
Recreated in WPF 
> This project was done during my time at Detmold group.

<details>
 <summary>
The list of Microsoft .NET development stacks used for this project.
</summary>
  
|      |           |
|-----:|-----------|
|     1| .NET      |
|     2| C#        |
|     3| Powershell|  

</details>

  
<details>
<summary>The list of advanced C# programming techniques and libraries found in this project</summary>
  
|      |           |
|-----:|-----------|
|     1|   APIs    |
|     2| PowerShell runspaces and commands|
|     3| Asynchronous Tasks |
|     4| Classes|
|     5| RestSharp|
|     6| Dictionaries and Lists|
|     7| CSV Helper Library |
|     8| JSON.Net|
|     9| THreding Tasks |
  
</details>

Guide to use user creation windows application
  
# New User Creation Application

The purpose of this application is to create a new user in the Active Directory. Users can manually enter the user details or load them from the "New User Request" ticket via the Halo API. 

> 

## Steps to Create a New User
### Search or Enter Details:

- Search using Ticket ID, or manually enter/select the First Name and Domain.

- Click the "Verify" button to check if the user is already in the directory.

- If "User is not found," proceed to the next step using the "Next" button.

> Note: Make sure the contents with * are filled.

### Select Reporting Manager:

- The option to select a reporting manager from a dropdown list will be disabled if the manager's name was retrieved from the ticket.
- If you choose not to search the user information with the Ticket ID, you can manually select the manager's name.
- Click the "Load Org. Unit" button to generate a list of organizational units.

### Create User:

- After filling in all required details, click the "Create User" button to create the new user in the directory.

# Steps to create new user mail:
## Wait for Synchronisation
- After the user is created, the synchronisation process may happen instanlty or take about 2-3 minutes approximately.

## Open Mail creation window:
- Click on "Create Mail" button and a new window will open asking for admin credential (username and password).
- 
 > Before you login ensure that you have not clicked the "Clear" button as this will empty all the textboxes. The app uses the data from the FirstName, Last Name and Domain textfields.


## Log In and create mail:

- After logging in, the mail creation will automatically start. If you get "Error: Mailbox found". The user already has a mailbox.
- If "Error: User not found" appears, it means the user detials has not synchronised yet, and the app will try again to create the mailbox in thirty seconds.
- "Mailbox Created" message will appear after success.
  
