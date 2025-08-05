
<div align="center">
  <picture>
    <img alt="Maanfee" src="SolutionItems/Contents/Logo.png">
  </picture>
  <h2 align="center">
    Maanfee Dashboard MudBlazor Template
  </h2>
  <p align="center">
    Open source solution template for Blazor Web-Assembly  built with MudBlazor
  </p>
  <div>
      <a href="https://github.com/Maanfee/Maanfee-Dashboard/blob/main/LICENSE" target="_blank">
        <img src="SolutionItems/Contents/license.svg" />
      </a>
      <a href="https://www.linkedin.com/in/mansour-farshidi-091a41185/" target="_blank">
        <img src="SolutionItems/Contents/linkedin.svg" />
      </a>    
 </div>
</div>

## Screenshots 
![Screenshots](SolutionItems/Screenshots/Dashboard.png)

## Prerequisites
- Supported .NET versions
  - [.NET 9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) 

## Development Enviroment
- [Microsoft Visual Studio 2022](https://visualstudio.microsoft.com/downloads/) 
- [.NET 9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) 

## Supported databases
- [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) 
- [SQLite](https://www.sqlite.org/index.html) 

## How to build solution 
### SQL Server Connection String Configuration

In the `appsettings.json` file of the `Maanfee.Dashboard.Server` and `Maanfee.Logging.Server` projects, set the `SQLServerConnection_DebugMode` connection string as follows:

```json
{
  "ConnectionStrings": {
    "SQLServerConnection_DebugMode": "Your_Connection_String_Here"
  }
}
```

### Configuration Details

#### Local SQL Server (Windows Authentication)
```text
Server=.;Database=YourDB;Trusted_Connection=True;
```

#### Remote SQL Server (SQL Authentication)
```text
Server=your_server_ip;Database=YourDB;User Id=username;Password=password;
```

#### Common Parameters
- `Server`: Server name/IP (`.` for local)
- `Database`: Database name
- `Trusted_Connection`: Windows authentication
- `User Id/Password`: SQL authentication
- `MultipleActiveResultSets`: Enable MARS feature

> Note: Always keep connection strings secure and never commit sensitive credentials to version control.
### Project startup Configuration 
Right click on the solution name and then select 
"properties->multiple startup projects" then set followings project 
as startup.

-`Maanfee.Dashboard.Server` 

-`Gateway.Api`

-`Maanfee.Logging.Server`

### Run solution
![image](SolutionItems/Screenshots/VisualStudio.png)
- Default login credentials
    - Username : Admin
    - Password : 123456
    
    > **Important**
As the Maanfee dashboard is under development, please make sure you delete your existing database (SQL Server, SQLite). Also install the latest version of Maanfee dashboard.

**Project History Releases**

- [01-10-08 : Init]()


 