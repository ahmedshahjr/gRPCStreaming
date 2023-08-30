# Bi-Directional Streaming with gRPC in .NET Core
This repository contains an example project showcasing how to implement bi-directional streaming using gRPC in .NET Core application.

## Overview
Bi-directional streaming is a communication pattern where both the client and server can send a stream of messages to each other concurrently. This pattern is useful for scenarios where continuous communication is needed.
This example show how to validate a number if it's prime or not using gRPC's bi-directional streaming. Clients can send messages to the server, and the server broadcasts those messages to clients.

## Prerequisites
- .NET Core 7
- Visual Studio (2022)

## Getting Started
Navigate To The Solution gRPC.
Right Click the Solution and Select (Multiple Startup Projects).
Start Client and Server Project.

## Testing
Run the project using Swagger or Postman to send a Prime Number Request.

### Swagger
Navigate to https://localhost:7230/swagger/index.html in your browser.
Send a Prime Number Request from the Swagger UI.

### Postman
Alternatively, you can use Postman to test the project using the following curl command
Paste this curl in Postman (curl --location 'https://localhost:7230/api/PrimeNumber')
