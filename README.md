# MongoDbQuickstart

This project contains the base of .NET Core MongoDb identity via JWT token supporting user roles.

Setup needed:
- provide valid mongo db connection to appsettings.json (Project.API) and the database name

To test:
- build solution and start Project API
- HTTP POST TO https://localhost:44302/api/account/register with sample data;
{ 
  "Email": "tests@test.com",
  "Password": "Test12345!"
}
If everything goes corrent you should get a 200 response with SignIn details (user id, email, roles, and access token)

- put the access token as a 'Bearer {token}' authorization header in your next request and try to ping
https://localhost:44302/api/account/Protected
Getting 200 status code in response means that everything is working fine.
