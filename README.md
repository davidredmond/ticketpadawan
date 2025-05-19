# The RESTful API that is not yet quite a master!

TicketPadawan is a lightweight event ticketing system providing a free and open source alternative to manage your event and ticketing needs.

## Implementation

This project is developed using .NET Core 9, EF Core and only external library [Fast Endpoints](https://fast-endpoints.com).  

Database used is SQLLite for the case study purposes due to ease of spinning up an instance, however in a real world deployment the database choice would be MS SQL Server.

`Fast Endpoints` was used as a replacement for minimal API due to performance in addition to providing clearer project structure.

## Requirements
.NET Core 9

## Building & running
```console
dotnet build TP.RestfulAPI
dotnet ef database update --project TP.RestfulAPI
dotnet run --project TP.RestfulAPI
```

These will install any needed dependencies, build the project, build and update the database and run the project respectively.  You can then browse to http://localhost:5135/

## Documentation
API specification is available via Swagger, by browsing to the API root directory which will redirect to [Swagger](http://localhost:5135/).  The API can be imported into [Postman](https://www.postman.com/downloads/) using [this link](http://localhost:5135/swagger/v1/swagger.json)
