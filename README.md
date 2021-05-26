## Setup

Connecting to the database:
Make sure to set it to connect to a local database for debugging. To do this, go to Data Access Layer/CoilContext.cs and set the connection name to TEST. It should look like this.


```
private const string TEST = "TestDatabase";
private const string PROD = "name=JOBSHEETSPOCK";

public CoilContext()
    : base(TEST)
{
}
```

<<<<<<< HEAD
__Make sure you always change this back to PROD if you're opening a pull request for your branch into master!__

=======
>>>>>>> 20dc8097312474c3381ef757d096d52e648b3b0d
Assuming you're using Visual Studio 2019, open the solution file (ASC Coil Tracker Production.sln) and click the button with the green play button at the top that says IIS Express (browser used), which should build the project and show you the coil tracker on localhost. Having the solution configured for Debug or Release doesn't matter, as the connection string is set to only connect to the production database on publishing.

See Phi Ton (<pton@ascmt.com>) for any help with this process.
