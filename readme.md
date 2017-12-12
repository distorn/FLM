## The Project

Football League Manager(FLM) is a demo project that includes Web API ASP.NET Core 2.x back-end with EF Core 2.x as ORM and SPA web client on AngularJS and Bootstrap. The main goal of this project is just to demonstrate my coding skills to potential employers, but of course, it would be nice if some of the strangers that came here found it useful for their educational or other own purposes.

## Subject Area

The subject area of the project is football(soccer). I chose this field because of a few reasons:
- This subject is more or less common and widely-known.
- It's easy to come up with various tasks in this field: from default CRUD actions and one-to-one, one-to-many, many-to-many relationships between entities, client and server validation of input data, etc., to more interesting from an algorithmic point of view(like applying game rules to the entities in C# code and database, calculating team points and position in the league table, generating plausible test data, etc.).
- And of course, because I personally like this sport.

## Architecture

The project has multitier architecture so the solution consists of several sub-projects:

**FLM.Model** : C# library project. Represents project's data model. Includes model entity and DTO classes plus some model extension tools.

**FLM.Model.User** : C# library project. Includes User and Roles interfaces and classes. Shares between Auth server, API, Data and Business layers.

**FLM.API** : Web API .NET Core project. Provides REST-ful web API to get\manipulate data through HTTP. Also, DI container is set up here and resolves interface contracts with certain services implementations.

**FLM.Auth.IdentityServer** : ASP.NET Core MVC project. UI Based on default ASP.NET MVC project with Individual User Accounts authentication mode. Identity Server 4 integrates here in order to be security token service endpoint. The goal of this project is to be an authentication server and have an ability to manage users and restrict access to some API methods.

**FLM.BL** : C# library project. This project represents the business layer of the application. Includes contracts and implementations of services that rules business logic, validates incoming data and pass it down to the Data Access Layer or retrieve data from repositories and return it back to the caller(e.g. API controller).

**FLM.DAL** : C# library project. Describes Data Access Layer contracts and provides interfaces of repositories and other data-related components.

**FLM.DAL.EFCore** : C# library project. Certain implementation of Data Access Layer that uses EF Core as ORM framework. Contains realization of generic EF repository and several inherited repositories related to specific Model parts. Also contains set of mapping configurations that describes how Entities from the Model should be mapped to relational database tables. And finally includes AutoMapper profile that defines how Entities should be converted to DTO(Data Transfer Objects).

**FLM.DAL.Mocks** : C# library project. Utility project that provides test DB Initializer that seeds the database with plausible test data. Here you can configure amounts of generated leagues, teams, players, automatically generate league schedules, simulate played matches and scores.

**FLM.Client.Angular** : SPA client of the application. Written in AngularJS using Bootstrap. This web client has a responsive grid layout, so all pages should look well both on a desktop and mobile devices.

## Databases

The project has 2 databases: one for storing all application data(teams, players, matches, etc.) and another used by the authentication server to store user credentials, roles, password hashes, etc. You can configure connection strings accordingly in FLM.API/appsettings.json and FLM.Auth.IdentityServer/appsettings.json

Databases will be created and seeded automatically after the first run of the application or you can do it manually in Visual Studio Package Manager Console running following commands:

For main database select **FLM.DAL.EFCore** project and run:
```
PM> update-database -startup FLM.API
```

For Identity Server database just select **FLM.Auth.IdentityServer** and run
```
PM> update-database
```

## Authentication

The application uses Identity Server 4 as a security token endpoint using OpenID Connect implicit flow. After you click Login in SPA client you will be redirected to Auth server where you will be able to enter credentials or register a new user.

There are a few pre-registered users that are automatically created during database seeding process. You can use the following credentials to test the application:

```
admin@test.com : admin
user1@test.com : user1
user2@test.com : user2
```

In fact, currently there is no difference in work with the application using anonymous access or registered accounts, but only user with Administrator role can edit data and see appropriate UI elements.

## Configure, Build and Run

First, you need to restore all NuGet packages for the solution.

For **FLM.Client.Angular** you also need to restore npm and bower js packages. File bower.json enumerates all external libraries that final SPA application uses.

Node modules listed in package.json are used by Gulp that automates front-end builds: it compiles LESS to CSS, combines all style files into a single minified css, compiles and minifies all js libraries and app sources, inject application configuration, etc. You can see the detailed configuration of all these tasks in gulpfile.js. To execute full front-end build process use Task Runner Explorer in Visual Studio and run full-build:dev or full-build:release task.

By default the project has the following setup:

```
API: http://localhost:5000
Client: http://localhost:5001
Auth Server: https://localhost:44335
```

If there is a need to change this, please consider to modify following configurational parts of the application:

FLM.Client.Angular/app.config.json : this config file contains urls of API and Auth servers that SPA client works with. After making changes in this file you also need to run front-end build: executing full-build or just build:config-dev/production task. Doing this Gulp converts settings from app.config.json to separate Angular module that contains the above-mentioned urls together with some other provided settings.

FLM.API/Startup.cs : Here inside ConfigureServices you can find the configuration of Authority server that we use to restrict access to some API methods.

FLM.Auth.IdentityServer/Config.cs : Contains configuration of allowed API resources and clients.