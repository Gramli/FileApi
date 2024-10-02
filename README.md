 <img align="left" width="116" height="116" src=".\doc\img\fileApi_icon.png" />

# Clean Architecture FileApi
[![.NET Build and Test](https://github.com/Gramli/FileApi/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Gramli/FileApi/actions/workflows/dotnet.yml)
[![Angular Build](https://github.com/Gramli/FileApi/actions/workflows/angular.yaml/badge.svg)](https://github.com/Gramli/FileApi/actions/workflows/angular.yaml)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/5d085f95f33a4412aa9bf3fe4904a151)](https://www.codacy.com/gh/Gramli/FileApi/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=Gramli/FileApi&amp;utm_campaign=Badge_Grade)
[![Codacy Badge](https://app.codacy.com/project/badge/Coverage/5d085f95f33a4412aa9bf3fe4904a151)](https://www.codacy.com/gh/Gramli/FileApi/dashboard?utm_source=github.com&utm_medium=referral&utm_content=Gramli/FileApi&utm_campaign=Badge_Coverage)

This full-stack solution allows for file uploads and downloads. The backend is a C# REST API, demonstrating how to create a clean, modern API using Clean Architecture, minimal APIs, and various design patterns. The frontend is an Angular project that demonstrates file upload and download functionality.

Example solution allows to **upload/download files with .txt, .json, .csv, .xml, .yml extensions** and get collection of uploaded files. Also allow to **convert between json, xml, yml formats**.

Two methods for downloading files are demonstrated:
* Using the **GetFileAsync** extension method, which returns the file as a byte array.
* Using the **GetJsonFileAsync** method, which returns the file inside a JSON object. The file is converted to a Base64 string to preserve encoding.

# Menu
- [Clean Architecture FileApi](#clean-architecture-fileapi)
- [Menu](#menu)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Get Started](#get-started)
    - [Run Solution](#run-solution)
    - [Run only Backend](#run-only-backend)
- [Motivation](#motivation)
  - [Backend Architecture](#backend-architecture)
  - [Frontend Example](#frontend-example)
  - [Technologies](#technologies)

# Prerequisites
* **.NET SDK 8.0+**
* **Angular CLI 18+**
* **Node.js 18.19.1+**

# Installation

To install the project using Git Bash:

1. Clone the repository:
   ```bash
   git clone https://github.com/Gramli/FileApi.git
   ```
2. Navigate to the project directory:
   ```bash
   cd FileApi/src
   ```
3. Install the backend dependencies:
   ```bash
   dotnet restore
   ```
4. Navigate to the frontend directory and install dependencies:
   ```bash
   cd File.Frontend
   npm install
   ```
5. Run the solution in Visual Studio 2019+ by selecting the "Run API and FE" startup item to start both the API and the frontend servers. More about run in next section.

# Get Started

**Expected IDE: Visual Studio 2019+**

### Run Solution
Simply select the **"Run API and FE"** startup item. This will start both the API and the frontend servers.

### Run only Backend
Select the **"File.API"** startup item and try it. Or in case You need to edit Content Type, use Postman. [Example gif of how to change ContentType in Postman](doc/img/contentType.gif).

![Swagger](doc/img/upload.gif)

# Motivation
The main motivation is to create a practical example of a minimal API using Clean Architecture, while experimenting with libraries for validation and mapping, and to further improve my skills with Angular.

## Backend Architecture

Projects folows **[Clean Architecture](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures#clean-architecture)**, but application layer is splitted to Core and Domain projects where Core project holds business rules and Domain project contains business entities.

As Minimal API allows to inject handlers into endpoint map methods, I decided to do not use **[MediatR](https://github.com/jbogard/MediatR)**, but still every endpoint has its own request and handler. Solution folows **[CQRS pattern](https://learn.microsoft.com/en-us/azure/architecture/patterns/cqrs)**, it means that handlers are separated by commands and queries, command handlers handle command requests and query handlers handle query requests. Also repositories (**[Repository pattern](https://learn.microsoft.com/en-us/aspnet/mvc/overview/older-versions/getting-started-with-ef-5-using-mvc-4/implementing-the-repository-and-unit-of-work-patterns-in-an-asp-net-mvc-application)**) are separated by command and queries.

Instead of throwing exceptions, project use **[Result pattern](https://www.forevolve.com/en/articles/2018/03/19/operation-result/)** (using [FluentResuls package](https://github.com/altmann/FluentResults)) and for returning exact http response, every handler returns data wraped into HttpDataResponse object which contains also error messages collection and http response code.

Important part of every project are **[tests](https://github.com/Gramli/WeatherApi/tree/main/src/Tests)**. When writing tests we want to achieve [optimal code coverage](https://stackoverflow.com/questions/90002/what-is-a-reasonable-code-coverage-for-unit-tests-and-why). I think that every project has its own optimal code coverage number by it's need and I always follow the rule: **cover your code to be able refactor without worry about functionality change**.

In this solution, each 'code' project has its own unit test project and every **unit test** project copy the same directory structure as 'code' project, which is very helpful for orientation in test project. Infrastructure project has also **integration tests**, because for format conversion is used third party library and we want to know that conversion works always as expected (for example when we update library version).

## Frontend Example
The frontend is a simple Angular 18 project that demonstrates how to upload and download files as blobs or FormData from the C# API. Files are saved to the Downloads folder using the [file-saver](https://www.npmjs.com/package/file-saver) library. For styling, the project utilizes [Bootstrap](https://getbootstrap.com/). Additionally, there are examples of displaying modals with [ng-bootstrap](https://www.npmjs.com/package/@ng-bootstrap/ng-bootstrap) and toasts/notifications with [angular-notifier](https://www.npmjs.com/package/gramli-angular-notifier).

## Technologies
* [ASP.NET Core 8](https://learn.microsoft.com/en-us/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-8.0)
* [Entity Framework Core InMemory](https://learn.microsoft.com/en-us/ef/core/providers/in-memory/?tabs=dotnet-core-cli)
* [SmallApiToolkit](https://github.com/Gramli/SmallApiToolkit)
* [Mapster](https://github.com/MapsterMapper/Mapster)
* [FluentResuls](https://github.com/altmann/FluentResults)
* [Validot](https://github.com/bartoszlenar/Validot)
* [GuardClauses](https://github.com/ardalis/GuardClauses)
* [Moq](https://github.com/moq/moq4)
* [Xunit](https://github.com/xunit/xunit)
* [ChoETL](https://github.com/Cinchoo/ChoETL)
* [Angular 18](https://angular.dev)
