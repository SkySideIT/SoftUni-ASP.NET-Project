# 🎮 TechWorld – ASP.NET Core MVC Web Application

## 📌 Project Overview

TechWorld is a ASP.NET Core MVC web application that allows users to browse, filter, and manage video games, as well as maintain a wishlist and shopping cart.

The application demonstrates clean architecture, role-based access control, and full CRUD functionality, along with unit testing and user interaction features.

---

## ⚙️ Setup Instructions

1. Clone the repository:

```bash
git clone https://github.com/SkySideIT/SoftUni-ASP.NET-Project.git
```

2. Open in Visual Studio

3. Configure the database connection

Open appsettings.json and make sure the connection string points to your local SQL Server instance:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TechWorld;Trusted_Connection=True;Encrypt=False;"
}
```

4. Apply migrations:

```
Update-Database
```

5. Run the application

```
dotnet run
```

or simply press CTRL + F5 in Visual Studio.

---

## 🚀 Features

### 👤 User Features

* Browse all available games
* Search games by title
* Filter games by:

  * Genre
  * Platform
* View detailed game information
* Add/remove games from Wishlist
* Add/remove games from Cart
* Simulated purchase (clears cart)

---

### 🔐 Authentication & Authorization

* User registration and login
* Role-based access:

  * **User**
  * **Admin**
* Admin users are redirected to Admin Panel after login

---

### 🛠️ Admin Features

* Game Management Panel
* Create new games
* Edit existing games
* Delete games

---

## 🏗️ Architecture

The application follows a modular, multi-project architecture, where each project has a clear responsibility.

### 📂 Project Structure


#### 🌐 TechWorld.Web

* ASP.NET Core MVC application
* Contains:
  * Controllers
  * Views (Razor)
  * Program configuration
* Handles HTTP requests and UI rendering

---

#### 🧠 TechWorld.Services.Core

* Business logic layer
* Contains:
  * Services
  * Service interfaces
* Responsible for:
  * Application logic
  * Data processing
  * Communication with repository layer

---

#### 🗄️ TechWorld.Data

* Data access layer
* Contains:
  * ApplicationDbContext
  * Repository implementation
  * Entity configurations
  * Migrations
* Uses Entity Framework Core

---

#### 📦 TechWorld.Data.Models

* Domain models (entities)
* Contains:
  * Game
  * Genre
  * Platform
  * Publisher
  * ApplicationUser (Identity with GUID)
  * UserGame (Wishlist)
  * CartProduct (Cart)

---

#### 📑 TechWorld.Web.ViewModels

* View models used for UI
* Separates domain models from presentation layer
* Used for:
  * Data transfer to views
  * UI-specific shaping of data

---

#### ⚙️ TechWorld.GCommon

* Shared/common components
* Contains:
  * Custom exceptions
  * Constants
  * Validation helpers

---

#### 🧪 TechWorld.Services.Tests

* Unit tests project
* Uses:
  * NUnit
  * Moq
* Tests service layer logic independently from database

---

## 🔄 Architectural Principles

* Separation of concerns
* Dependency injection
* Service-based business logic
* Repository abstraction for data access

---

## ✅ Validations

* Duplicate prevention (Wishlist & Cart)
* Entity existence checks
* User-based ownership validation
* Exception handling with user-friendly messages

---

## 🌱 Data Seeding

The application seeds:

* Games
* Genres
* Platforms
* Publishers
* Roles:
  * Admin
  * User
* Default Admin account:
  * Email: `admin@techworld.com`
  * Password: `admin123`

---

## 🛒 Business Logic

### Wishlist

* Per-user wishlist system
* Users can add/remove games
* Prevents duplicates

### Cart

* Per-user cart system
* Prevents duplicate entries
* Simulated checkout clears cart

---

## 🔍 Search & Filtering

* Search by title (case-insensitive)
* Filter by Genre and Platform
* Combined filtering supported

---

## 📄 Pagination

* Games are split into pages
* Supports filtering + pagination together

---

## 🧪 Unit Testing

Implemented using:

* **NUnit**
* **Moq**

### ✔ Covered Services:

* CartService
* WishlistService
* GameService

### ✔ Tested Scenarios:

* Valid operations
* Invalid operations (exceptions)
* Edge cases
* Data mapping

---

## 📸 Screenshots

<img width="1236" height="1102" alt="image" src="https://github.com/user-attachments/assets/89a2ca77-6f23-458d-ae55-fb3133e8590c" />
<hr>
<img width="1263" height="606" alt="image" src="https://github.com/user-attachments/assets/623f2df1-6628-44c8-95ca-e54594f57ca9" />
<hr>
<img width="1245" height="632" alt="image" src="https://github.com/user-attachments/assets/b0b1accd-0075-4116-ae54-63b6859af750" />
<hr>
<img width="1663" height="1181" alt="image" src="https://github.com/user-attachments/assets/be31cf51-a6ff-4dfc-8c31-a4c279727b4e" />
<hr>
<img width="1127" height="1202" alt="image" src="https://github.com/user-attachments/assets/5dc923ad-bc4a-4ea3-8483-2aa9b7c75430" />
<hr>
<img width="1501" height="722" alt="image" src="https://github.com/user-attachments/assets/f302e136-a262-4b90-a2ce-3446e378fc05" />
<hr>
<img width="1493" height="778" alt="image" src="https://github.com/user-attachments/assets/31f71f99-37af-4b96-b9bd-a5e6d91f5991" />
<hr>
<img width="1497" height="666" alt="image" src="https://github.com/user-attachments/assets/c165d793-294d-48c5-834a-e9d2620af775" />
<hr>
<img width="1517" height="1098" alt="image" src="https://github.com/user-attachments/assets/f55edfc2-0238-499b-887d-a0c07bc6800a" />
<hr>
<img width="1253" height="1175" alt="image" src="https://github.com/user-attachments/assets/b82c7267-9642-4502-bb73-a55451a3bd8e" />
<hr>
<img width="1240" height="1216" alt="image" src="https://github.com/user-attachments/assets/638ffb44-b34f-4375-a671-cf20ddfcc132" />
<hr>
<img width="1710" height="783" alt="image" src="https://github.com/user-attachments/assets/85189386-6b07-4748-9d47-5eb99eb8b52b" />

---

## 🎯 Key Highlights

* Clean architecture and separation of concerns
* Fully functional admin panel
* User-specific data (wishlist & cart)
* Robust error handling
* Realistic unit testing with Moq
* Search, filtering, and pagination combined

---

## 👨‍💻 Author

* Danail Georgiev
