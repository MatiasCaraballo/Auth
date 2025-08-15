This is an **Authentication Microservice** that uses the default tables ,schemas and endpoints created by **Microsoft Identity**.
It is designed to provide scalable authentication features for a microservices-based architecture.

## 🔐 Features  
- Endpoints from **Microsoft Identity**. 
- Logout
- RegisterUserRole

  New endpoints:
  `/RegisterUserRole`| POST   | Register user and assign role (admin or client)
  `/Logout` | POST   | Closes the current user session

 # Important! To run the APIRESTful : 
 Open the appsettings.Development.json and add 
 "ConnectionStrings": {
      "DefaultConnection": "Server=yourServer;Database=YourDatabaseName;Trusted_Connection=True;TrustServerCertificate=True;"
  }
  "SecretKey":"Insert your secret key there"
