## Setup

Connecting to the database:
You'll need a copy of the connection strings in a file WebDeploy.config, check with me for this.

Make sure to set it to connect to a local database for debugging. To do this, go to Data Access Layer/CoilContext.cs and set the connection name to TEST. It should look like this.


```
private const string TEST = "TestDatabase";
private const string PROD = "name=JOBSHEETSPOCK";

public CoilContext()
    : base(TEST)
{
}
```

__Make sure you always change this back to PROD if you're opening a pull request for your branch into master!__

Assuming you're using Visual Studio 2019, open the solution file (ASC Coil Tracker Production.sln) and click the button with the green play button at the top that says IIS Express (browser used), which should build the project and show you the coil tracker on localhost.

See Phi Ton (<pton@ascmt.com>) for any help with this process.

## Admin

Logging in with the admin account will give you the dropdown option `Add New User` when clicking on the account email in the top right on the navigation bar. Please email me for these credentials, along with the login for the database.

## Development

When updating this project:

1. Change TEST to PROD in DataAccessLayer/CoilContext.cs to ensure connection to the right database. Would be nice to set this up to do it automatically later if you're continuing development.
2. Update the patch notes on Views/Home/Login.cshtml.
3. From the master branch with these changes applied, go to Build/Publish ASC Coil Tracker Production and make sure you're using the correct profile (currently the one from Smarter ASP.NET's hosting), then publish the site.

This was the first project I ever worked on, so there may be some jank code/poor design decisions. I don't expect this to see any more development, though, so this code will lay among the ghosts and skeletons of the world.

