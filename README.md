# Good Reading

**GoodReading** is an online book web API that stores Customer, Order, Product and also you can create a new customer and place a new order.

It uses .Net Core 5.0, MongoDB, Mediator, FluentValidation, JWT, RedLock and Serilog.

It is dockerized so you can try it easily. Also, it has swagger documentation, and when you run the project you can reach it on **localhost:5000/swagger/index.html**

## Prerequities
   - ##### Docker(https://www.docker.com/get-started)
   - ##### [.NET 5](https://dotnet.microsoft.com/download/dotnet/5.0) installed 
   - ##### Mongo Db (via Docker)
   - ##### Redis (via Docker)

## Libraries that used

   - [MediatR](https://github.com/jbogard/MediatR) - For Processing Commands, Queries, Events
   - [RedLock](https://github.com/samcook/RedLock.net) - For Distributed Lock via Redis
   - [Serilog](https://github.com/serilog/serilog) - For Logging purposes
   - [FluentValidation](https://github.com/FluentValidation/FluentValidation) - For Validating Command and Query models

## Installation
 
To run the application, please do the followings
 * Clone the project
 * Open the terminal at project directory
 * Execute following command
 <pre><code>docker-compose up</code></pre>
 
After the command is executed, an application with four containers on docker will be launched.
These;
 * MongoDb[^1]
 * Mongo Express[^2]
 * Redis[^3]
 * GoodReading.Web.Api[^4]
 
 [^1]: By default, MongoDb access port is set to **27017** and port forwarding is enabled.</sup>
 [^2]: By default, Mongo Express ui is accessible at **http:8081** and port forwarding is enabled.</sup>
 [^3]: By default, Redis access port is set to **6379** and port forwarding is enabled.</sup>
 [^4]: By default, GoodReading.Web.Api is listening  **http:5000** and port forwarding is enabled.</sup>

 ## Tests
 
The application includes integration tests on its own.
These tests are located under **GoodReading.Web.Api.Integration.Tests**.

To run these tests;
 * A MongoDb instance
 * A Redis instance
 * Build the solution.
 * Open the terminal at **GoodReading.Web.Api.Integration.Tests\bin\Debug\net5.0** directory
 * Execute following command
 <pre><code>dotnet test GoodReading.Web.Api.Integration.Tests.dll</code></pre>
 * Or use Visual Studio's **Test Explorer** pane
