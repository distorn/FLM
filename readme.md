## The Project

Football League Manager(FLM) is a demo project that includes Web API ASP.NET Core 2.x back-end with EF Core 2.x as ORM and SPA web client on AngularJS and Bootstrap. The main goal of this project is just to demonstrate my coding skills to potential employers, but of course it would be nice if some of the strangers that came here found it useful for their educational or other own purposes.

## Subject Area

The subject area of the project is foolball(soccer). I chose this field because of a few reasons: 
- This subject is more or less common and widely-known.
- It's easy to come up with various tasks in this field: from default CRUD actions and one-to-one, one-to-many, many-to-many relationships between entities, client and server validation of input data, etc., to more interesting from an algorithmic point of view(like applying game rules to the entities in C# code and database, calculating team points and position in the league table, generating plausible test data, etc.).
- And of course because I personally like this sport.

## Architecture

The project has multitier architecture so the solution consists of several sub-projects:

**FLM.Model** : C# library project. Represents project's data model. Includes model entity and DTO classes plus some model extension tools.

**FLM.Model.User** : C# library project. Includes User and Roles interfaces and classes. Shares between Auth server, API, Data and Business layers.

**FLM.API** : Web API .NET Core project. Provides REST-ful web API to get\manipulate data through HTTP. Also DI container is set up here and resolves interface contracts with certain services implementations.

**FLM.Auth.IdentityServer** : ASP.NET Core MVC project. UI Based on default ASP.NET MVC project with Individual User Accounts authentication mode. Identity Server 4 integrates here in order to be security token service endpoint. Goal of this project is to be an authentication server and have an ability to manage users and restrict access to some API methods.

**FLM.BL** : C# library project. This project represents business layer of the application. Includes contracts and implementations of services that rules business logic, validates incoming data and pass it down to the Data Access Layer or retrieve data from repositories and return it back to the caller(e.g. API controller).

**FLM.DAL** : C# library project. Describes Data Access Layer contracts and provides interfaces of repositories and other data-related components.

**FLM.DAL.EFCore** : C# library project. Certain implementation of Data Access Layer that uses EF Core as ORM framework. Contains realization of generic EF repository and several inhereted repositories related to specific Model parts. Also contains set of mapping configurations that describes how Entities from the Model should be mapped to relational database tables. And finally includes AutoMapper profile that define how Entities should be converted to DTO(Data Transfer Objects).

**FLM.DAL.Mocks** : C# library project. Utility project that provides test DB Initializer that seeds the database with plausible test data. Here you can configure amount of generated leagues, teams, players, automatically generate league schedules, simulate played matches and scores.

**FLM.Client.Angular** : SPA client of the application. Written in AngularJS using Bootstrap. This web client has responsive grid layout, so all pages should look well both on desktop and mobile devices.







