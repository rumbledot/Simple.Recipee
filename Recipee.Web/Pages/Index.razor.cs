using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Components;
using Recipee.Web.Data;
using Recipee.Web.Models;
using System.Data.SQLite;

namespace Recipee.Web.Pages;

public partial class Index
{
    [Inject]
    public DatabaseService DB { get; set; }
    [Inject]
    public NavigationManager Navigate { get; set; }
    public string ConnectionString { get => DB.ConnectionString; }
    public List<Models.Recipee> Recipees { get; set; }

    protected override void OnInitialized()
    {
        using var conn = new SQLiteConnection(ConnectionString);
        Recipees = conn.GetAll<Models.Recipee>()?.ToList() ?? new List<Models.Recipee>();
    }

    private void AddNewRecipee() 
    {
        Navigate.NavigateTo("/recipee/details");
    }

    private void EditRow(Models.Recipee item, bool isEditing)
    {
        Navigate.NavigateTo($"/recipee/details/{item.Id}");
    }

    private void RemoveRow(Models.Recipee item)
    {
        //if (item.Id > 0)
        //{
        //    using var conn = new SQLiteConnection(ConnectionString);
        //    conn.Delete(item);
        //}

        Recipees.Remove(item);
    }
}