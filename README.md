#Web Api .Net Core 3.1

Basic Api with CRUD operations and filtering
using Entity Framework core DB


Before starting the application, please first adjust the connection string as appropriate for your needs. (appsettings.json)

On initial start Data Base should be created and initial seed on data should be performed.

You can address the API using postman or other client.

If you use Postman mind to set off SSL sertificate varification to off in settings.

I exported some operations in .json format. They could be found at: BasicApi/Postman requests export/ in the repository.

To make request at https://localhost:5001 you need to start the API with Kestrel.

Or you can find endpoints below.

GET
https://localhost:5001/api/cars/


POST
https://localhost:5001/api/cars/

{
    "brand": "Mazda",
    "yearOfProduction": "2019-02-01T21:28:56.321"
}

PUT
https://localhost:5001/api/cars

{
	"id": 4,
	"brand": "Kamaz 304",
	"yearOfProduction": "2014-02-01T21:28:56.321"
}

DELETE
https://localhost:5001/api/cars/1


View item
GET
https://localhost:5001/api/cars/2


//    Filtering, Sorting, Pagination

GET
https://localhost:5001/api/cars/filter?fromYear=2000&toYear=2002

https://localhost:5001/api/cars/sort

https://localhost:5001/api/cars/options?fromYear=2002&page=1&byBrand=0