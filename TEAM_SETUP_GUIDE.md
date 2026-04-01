# 👥 TMS Project - Team Setup Guide

## 🎯 For All Team Members (Person B, C, D)

### **Prerequisites**
- Visual Studio Community 2026 (or newer)
- .NET 10 SDK installed
- Git installed

---

## 📥 **Step 1: Clone the Project**

Open PowerShell and run:

```powershell
git clone https://github.com/YOUR_USERNAME/TMS-Project.git
cd TMS-Project
```

Replace `YOUR_USERNAME` with the actual GitHub username.

---

## 🔧 **Step 2: Restore Dependencies**

```powershell
dotnet restore
```

This downloads all NuGet packages.

---

## 🚀 **Step 3: Run the Project**

```powershell
dotnet run
```

**Wait for:** `Now listening on: https://localhost:7xxx`

---

## ✅ **Step 4: Verify Everything Works**

1. Open browser: `https://localhost:7095`
2. Go to: `/TestDatabase`
3. You should see:
   - ✅ Connection Successful!
   - 📊 All sample data loaded
   - 🚚 Tours and deliveries visible

---

## 📂 **Project Structure Overview**

```
TMS Project/
├── Models/               ← 7 Entity models (Person A)
├── Data/
│   ├── TmsDbContext.cs  ← Database context
│   └── DbInitializer.cs ← Sample data seeding
├── Pages/
│   ├── Transporteurs/   ← Person B CRUD pages
│   ├── Camions/         ← Person B CRUD pages
│   ├── Chauffeurs/      ← Person B CRUD pages
│   ├── Tournees/        ← Person C pages
│   ├── Livraisons/      ← Person C pages
│   └── Dashboard/       ← Person D dashboard
├── Program.cs           ← App configuration
├── appsettings.json     ← Database connection
└── Database/
    └── TMS_Database_Script.sql ← Backup SQL script
```

---

## 👥 **Task Assignment**

### **👤 Person A - COMPLETED ✅**
- Database models & schema
- Entity Framework setup
- Sample data initialization
- Database testing page

### **👤 Person B - TRANSPORTERS MODULE**
Create CRUD pages in `/Pages/Transporteurs/`, `/Pages/Camions/`, `/Pages/Chauffeurs/`:
- **Index.cshtml** - List all records
- **Create.cshtml** - Add new record
- **Edit.cshtml** - Edit existing record
- **Details.cshtml** - View details
- **Delete** functionality

### **👤 Person C - ROUTES & DELIVERIES**
Create pages in `/Pages/Tournees/`, `/Pages/Livraisons/`:
- Create route with automatic assignments
- Assign trucks (Camion)
- Assign drivers (Chauffeur)
- Assign deliveries
- Calculate costs automatically
- Update delivery status

### **👤 Person D - DASHBOARD & INTERFACE**
Create `/Pages/Dashboard/`:
- Dashboard with key statistics
- Cost reports using `vw_CoutParTransporteur` view
- Active tours using `vw_TourneeActives` view
- Professional layout & design
- Navigation menu
- Success/error messages
- Optional: PDF report generation

---

## 🔄 **Git Workflow for Team**

### **Before Starting Work:**
```powershell
git pull origin main
```

### **After Making Changes:**
```powershell
git add .
git commit -m "Person B: Added Transporteur CRUD pages"
git push origin main
```

### **Update Your Code:**
```powershell
git pull origin main
```

---

## 📊 **Database Info**

- **Database Name:** `TMSDatabase`
- **Type:** SQL Server LocalDB (built-in)
- **Connection:** Automatic
- **Sample Data:** Auto-seeded on first run
- **Reset:** Delete LocalDB instance and restart app

---

## ⚠️ **Important Notes**

1. **Never push these files to Git:**
   - `bin/` folder
   - `obj/` folder
   - `.vs/` folder
   - `appsettings.Development.json`
   - (`.gitignore` handles this automatically)

2. **LocalDB Database:**
   - Located: `C:\Users\YourUsername\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\mssqllocaldb\`
   - Automatically created on first run
   - No installation needed

3. **First Run:**
   - Takes ~5 seconds (creating everything)
   - Next runs are instant

4. **Port Numbers:**
   - Each developer might get different ports
   - Just use whatever is shown in terminal
   - Default is around 7095

---

## 🆘 **Troubleshooting**

### **"Database connection failed"**
```powershell
# Delete LocalDB and let it recreate
rm -r "C:\Users\$env:USERNAME\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\mssqllocaldb"
dotnet run
```

### **"Package not found"**
```powershell
dotnet restore
dotnet build
```

### **"Port already in use"**
Just close the previous instance and run again.

### **"Build errors"**
```powershell
dotnet clean
dotnet build
dotnet run
```

---

## 📞 **Questions?**

Ask Person A (database architect) for:
- Database schema questions
- Model relationships
- Connection issues
- Data initialization

---

## ✨ **Ready to Code!**

Everything is set up. Each person can now focus on their module independently while sharing the same database! 🚀
