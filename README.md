# PROG8050-PROJECT

## The electronic e-commerce system.

The Group No. 4

- Ashitha Thattarathodi Abdussalam 8734189
- Gyoungsik Moon 8784733
- Jintu Orakkottu Baby 8700329
- Lenora Michael 8732280 

The Project is based on [C# Project of Inventory Management System](https://www.youtube.com/watch?v=tF7zShUZG7E)

## SQLite Database

SQLite is a file-based small database engine, there is no network connection. so, it is suitable for a small project.

### Location of Database file
"ProjectDir/database.sqlite". it will copy to output directory at compile time.

### SQLite Tools

There are many sqlite tools. Anything is ok to manage.

- SQLite DB Browser [Link](https://sqlitebrowser.org/)

### SQLite Usage

- System.Data.SQLite Website [Link](http://system.data.sqlite.org/index.html/doc/trunk/www/index.wiki)
- System.Data.SQLite Usage [Link](https://zetcode.com/csharp/sqlite/)


Code example

``` Csharp
Core/SQLiteDBManager.cs // Database Connection Manage Class - Singlton

// Usage - Refer to Views/LoginViewModel.cs

SQLiteDBManager manager = SQLiteDBManager.Instance;     // no class construct due to singleton

SQLiteDataReader result = manager.ExecuteReader("SELECT * FROM USER"); 
// ExecuteReader = Return multiple data from query execution.

int result = manager.ExecuteNonQuery("UPDATE user SET password = 'new password' WHERE index = '1'");
// ExecuteNonquery = Return single number of query result such as insert, update.
```


### Project Interface Outline
![outline](./Project-Outline.drawio.png)


#### UI Detail
![UI-Outline](./Document/UI_Outline/UI-Outline.png)
![Login](./Document/UI_Outline/View00_Login.png)
![Home](./Document/UI_Outline/View01_Home.png)
![Product](./Document/UI_Outline/View02_Product.png)
![Category](./Document/UI_Outline/View03_Category.png)
![Promotion](./Document/UI_Outline/View04_Promotion.png)
![Customer](./Document/UI_Outline/View05_Customer.png)
![Order](./Document/UI_Outline/View06_Order.png)
![User](./Document/UI_Outline/View07_User.png)