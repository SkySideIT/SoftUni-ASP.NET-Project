# 🎮 TechWorld – ASP.NET Core MVC Web Application

**TechWorld** is a basic ASP.NET Core MVC web application designed as a simple online store for video games.  
The project demonstrates the fundamental structure of a full-stack web application using clean architecture, Entity Framework Core, and SOLID principles.  
It was created as part of a university assignment for the ASP.NET Core Basics course and serves as the foundation for a future Advanced ASP.NET project.

---

## 🚀 Features

- Full **MVC architecture** (Model–View–Controller)
- **Entity Framework Core** with SQL Server database
- **Repository + Service pattern** for clean separation of concerns
- CRUD operations for the main entity: **Game**
- **Server-side and client-side validation**
- **Responsive design** using **Bootstrap 5**
- **Razor Views** with Layout
- **Dependency Injection** and SOLID OOP principles
- **Default placeholder image** when a game has no cover
- Automatic deletion of unused publishers (if no games reference them)
- Clear navigation between all pages

---

## 🧱 Entities

The database contains four main entities:

| Entity | Description |
|---------|--------------|
| **Game** | Main entity representing a video game. Includes title, description, price, image, release date, genre, platform, and publisher. |
| **Genre** | Represents the category of the game (e.g. Action, Shooter, RPG). |
| **Platform** | Represents the platform where the game is available (e.g. PC, PlayStation, Xbox). |
| **Publisher** | Represents the company that published the game. Automatically deleted when no games reference it. |

---

## 🧠 Architecture Overview

- **Repository Layer:** Handles data access using EF Core.  
- **Service Layer:** Contains business logic and validation.  
- **Controller Layer:** Handles HTTP requests and orchestrates data between Service and View.  
- **Views:** Razor pages for displaying data and forms.

---

## 🛠️ Technologies Used

| Category | Technologies |
|-----------|---------------|
| **Framework** | ASP.NET Core 8.0 MVC |
| **Database** | Microsoft SQL Server |
| **ORM** | Entity Framework Core |
| **Frontend** | Razor, Bootstrap 5 |
| **Language** | C# 10+ |
| **Version Control** | Git / GitHub |

---

## ⚙️ Setup Instructions

### 1️⃣ Configure the database connection

Open appsettings.json and make sure the connection string points to your local SQL Server instance:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=TechWorld;Trusted_Connection=True;Encrypt=False;"
}
```

---

### 2️⃣ Apply migrations and seed the database

In Visual Studio Package Manager Console, run:

```
Update-Database
```

This will create the database and seed it with sample data.

---

### 3️⃣ Run the application

```
dotnet run
```

or simply press F5 in Visual Studio.

---

## 🧪 Default Data (Seed)

When the project runs for the first time, the database is automatically seeded with:

- Sample genres: Action, Shooter, RPG
- Sample platforms: PC, PlayStation, Xbox
- Example publishers and test games

---

## 📸 Screenshots

<img width="1568" height="1230" alt="image" src="https://github.com/user-attachments/assets/68774ac1-7bf9-4f98-8d51-a836bd58f9ec" />
<hr>
<img width="1573" height="1137" alt="image" src="https://github.com/user-attachments/assets/9932a9d8-b960-43c8-8519-7810c49a7789" />
<hr>
<img width="690" height="1171" alt="image" src="https://github.com/user-attachments/assets/253336b6-ff29-4ea1-b292-ab933dd7f6d4" />
<hr>
<img width="1350" height="1188" alt="image" src="https://github.com/user-attachments/assets/d54c4bd9-fb24-44fe-9804-a718ea8d0740" />
<hr>
<img width="1318" height="1225" alt="image" src="https://github.com/user-attachments/assets/15a3a12e-fe9e-467c-bd40-29451cc03e11" />
<hr>
<img width="1742" height="795" alt="image" src="https://github.com/user-attachments/assets/a345382d-e831-4f02-9ec9-c624783419c9" />

---

## 💾 Main Features Summary

Feature | Description
--------|-------------
Home Page | Displays the latest 3 games
Details Page | Shows full information about a game
Create / Edit | Allows adding or updating game entries with validation
Delete | Confirms and removes a game, deleting orphaned publishers automatically
Validation | Both client-side (Bootstrap + Data Annotations) and server-side checks
Design | Clean, responsive layout using Bootstrap 5 and Razor syntax

---

## 🧑‍💻 Author

Danail Georgiev
