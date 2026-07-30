# 🏨 Sireen API

Sireen API is a **Hotel Management System** built with **ASP.NET Core Web API**. It provides a complete backend solution for managing hotels, rooms, bookings, and users. The system allows customers to search for hotels, book available rooms, and manage their reservations, while administrators can manage hotels, rooms, images, and other system resources.

## ✨ Features

### 👤 User Features

* User registration and login.
* Secure authentication and authorization.
* Browse available hotels.
* View hotel details.
* Browse available rooms.
* Book hotel rooms.
* Manage personal bookings.
* Cancel bookings.

### 🏨 Hotel Management

* Add new hotels.
* Update hotel information.
* Delete hotels.
* Upload hotel images.
* View hotel details.

### 🛏️ Room Management

* Add new rooms.
* Update room details.
* Delete rooms.
* Upload room images.
* Manage room availability.

### 📷 Image Management

* Upload hotel images.
* Upload room images.
* Store and retrieve image URLs.

### 🔒 Security

* JWT Authentication.
* Role-Based Authorization.
* Protected API endpoints.

## 🛠️ Technologies Used

* ASP.NET Core Web API
* C#
* Entity Framework Core
* SQL Server
* ASP.NET Core Identity
* JWT Authentication
* AutoMapper
* Swagger / OpenAPI


## 🚀 Getting Started

### Prerequisites

* .NET SDK 8.0 (or your project version)
* SQL Server
* Visual Studio 2022 or Visual Studio Code

### Installation

1. Clone the repository.

```bash
git clone https://github.com/MaryamAshraf4/SireenAPI.git
```

2. Navigate to the project.

```bash
cd SireenAPI
```

3. Update the connection string in **appsettings.json**.

4. Apply database migrations.

```bash
dotnet ef database update
```

5. Run the project.

```bash
dotnet run
```
