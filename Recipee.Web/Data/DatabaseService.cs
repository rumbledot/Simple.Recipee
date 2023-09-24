using Slapper;
using System.Data.SQLite;

namespace Recipee.Web.Data;

public class DatabaseService
{
    public string ConnectionString { get; private set; }

    public DatabaseService()
    {
        var databaseFile = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "recipee.db");
        ConnectionString = $"Data Source={databaseFile};Version=3";

        if (!File.Exists(databaseFile))
        {
            SQLiteConnection.CreateFile(databaseFile);
            using var conn = new SQLiteConnection(ConnectionString);
            conn.Open();
            var sql = @"CREATE TABLE Recipees (
                Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT
                , DateCreated DATETIME NOT NULL
                , DateUpdated DATETIME NULL
                , Title varchar(50) NULL
                , Description TEXT NULL
                , RecipeeTypeId INTEGER NOT NULL);
                CREATE TABLE RecipeeInstructions (
                    RecipeeId INTEGER NOT NULL
                    , StepNo INTEGER NOT NULL
                    , Description TEXT NULL);
                CREATE TABLE RecipeeIngredients (
                    RecipeeId INTEGER NOT NULL
                    , IngredientId INTEGER NOT NULL
                    , Amount DECIMAL(8,2)
                    , MeasurementUnitId INTEGER NULL);
                CREATE TABLE Ingredients (
                    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT
                    , Name VARCHAR(50) NULL
                    , Description TEXT NULL);
                CREATE TABLE MeasurementUnits (
                    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT
                    , Name VARCHAR(20) NOT NULL
                    , Description VARCHAR(100) NULL);
                CREATE TABLE Journals (
                    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT
                    , RecipeeId NULL
                    , DateCreated DATETIME NOT NULL
                    , DateUpdated DATETIME NULL
                    , Description TEXT NULL);
                CREATE TABLE RecipeeTypes (
                    Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT
                    , Description VARCHAR(50) NULL);";
            var com = new SQLiteCommand(sql, conn);
            com.ExecuteNonQuery();

            sql = @"
                INSERT INTO RecipeeTypes (Description)
                VALUES
                ('main'), ('bake'), ('asian');
                INSERT INTO Ingredients (Name, Description)
                VALUES
                ('Salt', 'Salty stuff'),('Sugar', 'Sweet stuff')
                ,('White Pepper', 'White hot stuff'),('Black Pepper', 'Black Hot stuff')
                ,('Chili', 'Very spicy stuff'),('MSG', 'Yum yum stuff');
                INSERT INTO MeasurementUnits (Name, Description)
                VALUES
                ('pinch','a touch of'),('ts,','tea spoon'),('sp','spoon'),('gr','gram'),('kg','kilogram'),('ml','mili-liter'),('l','liter')";
            com = new SQLiteCommand(sql, conn);
            com.ExecuteNonQuery();

            conn.Close();
        }
    }
}
