using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Components;
using Recipee.Web.Data;
using System.Data.SQLite;

namespace Recipee.Web.Pages;

public partial class RecipeeType
{
    [Inject]
    public NavigationManager Navigate { get; set; }
    [Inject]
    public DatabaseService DB { get; set; }
    public string ConnectionString { get => DB.ConnectionString; }
    public List<Models.RecipeeType> RecipeeTypes { get; set; }
    public Models.RecipeeType NewType { get; set; }

    protected override void OnInitialized()
    {
        using var conn = new SQLiteConnection(ConnectionString);
        RecipeeTypes = conn.GetAll<Models.RecipeeType>()?.ToList() ?? new List<Models.RecipeeType>();
        NewType = new Models.RecipeeType();
    }

    private void AddNewType() 
    {
        using var conn = new SQLiteConnection(ConnectionString);
        conn.Insert(NewType);

        RecipeeTypes.Add(NewType);
        NewType = new Models.RecipeeType();
    }

    private void EditRow(Models.RecipeeType item, bool isEditing)
    {
        item.IsEditing = isEditing;
    }

    private async void SaveRow(Models.RecipeeType item)
    {
        EditRow(item, false);

        using var conn = new SQLiteConnection(ConnectionString);
        await conn.UpdateAsync(item);
    }

    private void RemoveRow(Models.RecipeeType item)
    {
        if (item.Id > 0)
        {
            using var conn = new SQLiteConnection(ConnectionString);
            conn.Delete(item);
        }

        RecipeeTypes.Remove(item);
    }
}
