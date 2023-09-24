using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Components;
using Recipee.Web.Data;
using Recipee.Web.Models;
using System.Data.SQLite;

namespace Recipee.Web.Pages;

public partial class Journal
{
    [Inject]
    public DatabaseService DB { get; set; }
    public string ConnectionString { get => DB.ConnectionString; }
    public List<Models.Journal> Cookings { get; set; }
    public Models.Journal NewJournal { get; set; }
    public List<Models.Recipee> AvailableRecipees { get; set; }
    public string ErrorMsg { get; set; } = "";

    protected override void OnInitialized()
    {
        using var conn = new SQLiteConnection(ConnectionString);
        AvailableRecipees = conn.GetAll<Models.Recipee>().ToList() ?? new List<Models.Recipee>();
        Cookings = conn.GetAll<Models.Journal>()?.ToList() ?? new List<Models.Journal>();
        foreach (var item in Cookings)
        {
            item.Recipee = AvailableRecipees.FirstOrDefault(x => x.Id == item.RecipeeId);
        }
        NewJournal = new Models.Journal();
        NewJournal.DateCreated = DateTime.Now;

    }

    private void AddNewJournal() 
    {
        if(NewJournal.RecipeeId == -1) 
        {
            ErrorMsg = "Please select a Recipee";
            return;
        }

        NewJournal.Recipee = AvailableRecipees.FirstOrDefault(x => x.Id == NewJournal.RecipeeId);
        NewJournal.DateUpdated = DateTime.Now;

        using var conn = new SQLiteConnection(ConnectionString);
        conn.Insert(NewJournal);

        Cookings.Add(NewJournal);
        NewJournal = new Models.Journal();
        NewJournal.DateCreated = DateTime.Now;
    }

    private void EditRow(Models.Journal item, bool isEditing)
    {
        item.IsEditing = isEditing;
    }

    private async void SaveRow(Models.Journal item)
    {
        EditRow(item, false);
        item.Recipee = AvailableRecipees.FirstOrDefault(x => x.Id == item.RecipeeId);
        item.DateUpdated = DateTime.Now;

        using var conn = new SQLiteConnection(ConnectionString);
        await conn.UpdateAsync(item);
    }

    private void RemoveRow(Models.Journal item)
    {
        if (item.Id > 0)
        {
            using var conn = new SQLiteConnection(ConnectionString);
            conn.Delete(item);
        }

        Cookings.Remove(item);
    }
}
