using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Components;
using Recipee.Web.Data;
using Recipee.Web.Models;
using System.Data.SQLite;

namespace Recipee.Web.Pages;

public partial class CookingIngredients
{
    [Inject]
    public DatabaseService DB { get; set; }
    public string ConnectionString { get => DB.ConnectionString; }
    public List<Ingredient>? Ingredients { get; set; }
    public Ingredient NewIngredient { get; set; }

    protected override void OnInitialized()
    {
        using var conn = new SQLiteConnection(ConnectionString);
        Ingredients = conn.GetAll<Ingredient>().ToList() ?? null;
        NewIngredient = new Ingredient();
    }

    private void AddNewIngredient() 
    {
        using var conn = new SQLiteConnection(ConnectionString);
        conn.Insert(NewIngredient);

        Ingredients.Add(NewIngredient);
        NewIngredient = new Ingredient();
    }

    private void EditRow(Models.Ingredient item, bool isEditing)
    {
        item.IsEditing = isEditing;
    }

    private async void SaveRow(Models.Ingredient item)
    {
        EditRow(item, false);

        using var conn = new SQLiteConnection(ConnectionString);
        await conn.UpdateAsync(item);
    }

    private void RemoveRow(Models.Ingredient item)
    {
        if (item.Id > 0)
        {
            using var conn = new SQLiteConnection(ConnectionString);
            conn.Delete(item);
        }

        Ingredients.Remove(item);
    }
}
