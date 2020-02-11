# Pets

Mediatonic Server Engineering Coding Test by Hector Soria Villalva

The solution is composed out of 4 different projects:

- **PetAPI**: It contains the implementation of all the REST API endpoints for managing the Users and the Animals.

- **PetData**: It has the definition of the models and the connection with the DB.

- **PetTasks**: A simple BackgroundService to update the Hapinness and Hunger of every animal.

- **PetTests**: Unit test for all the previous modules.


## General information

There are users and animals.

All the animals need to belong to a user.

There are 3 kinds of animals although that is easely extensible:

- **Mammals**: with a multiplier of *1*
- **Reptiles**: with a multiplier of *2*
- **Birds**: with a multiplier of *3*

Their happiness decreases and its hunger increases every second by *1 \* multiplier*

You can Feed them or stroke them to mitigate its sadness or hunger under the endpoints:

- /api/animal/feed/{id}

- /api/animal/stroke/{id}

This way you can decrease and increase their hunger and hapinness by *10 \* multiplier*

The rest of the endponts for CRUD operations for animals and users are the expected in a RESTful application under:

- /api/animal/
- /api/user/

You can find the full description of the API under the endpoint: /index.html


## Requirements

- [.NET Core SDK 3.1.1 or greater](https://dotnet.microsoft.com/download)

- ADO.NET Entity Framework

        dotnet tool install --global dotnet-ef

- [MySQL](https://dev.mysql.com/downloads/installer/)


## Set up


#### Apply the migrations
Make sure you have a 'root' user with 'admin' password and all priviledges in the DB.

Inside PetAPI project run: 

    dotnet ef database update
  
#### Run server

Inside PetAPI project run: 

    dotnet run

Go to http://localhost:5000/

#### API Docs

http://localhost:5000/index.html

#### Run the tests

Inside PetTests run

    dotnet test

## TODOs

- [ ] Animal types as a table in the DB.
- [ ] Expand test further than only the happy flow.
- [ ] Implement Repository Pattern.
- [ ] Implement Request-Response Pattern.
- [ ] Use Async calls for DB operations.


## Usefull dev commands

    dotnet ef database drop
    dotnet ef --startup-project ../PetAPI/ migrations add Initial
    dotnet ef database update
        