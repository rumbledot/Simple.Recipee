using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Components;
using Recipee.Web.Data;
using Recipee.Web.Models;
using System.Data.SQLite;

namespace Recipee.Web.Pages;

public partial class RecipeeDetails
{
    [Inject]
    public DatabaseService DB { get; set; }
    [Inject]
    public NavigationManager Navigate { get; set; }

    [Parameter]
    public int? Id { get; set; }

    public string ConnectionString { get => DB.ConnectionString; }
    public Models.Recipee ActiveRecipee { get; set; }
    public int CurrentInstructionIdx { get; set; }
    public List<RecipeeInstruction> Instructions { get; set; }
    public RecipeeInstruction NewInstruction { get; set; }
    public List<RecipeeIngredient> Ingredients { get; set; }
    public RecipeeIngredient NewIngredient { get; set; }
    public List<Ingredient> AvailableIngredients { get; set; }
    public List<MeasurementUnit> AvailableMeasurements { get; set; }
    public List<Models.RecipeeType> AvailableTypes { get; set; }

    protected override void OnInitialized()
    {
        using var conn = new SQLiteConnection(ConnectionString);
        AvailableIngredients = conn.GetAll<Ingredient>()?.ToList() ?? new List<Ingredient>();
        AvailableMeasurements = conn.GetAll<MeasurementUnit>()?.ToList() ?? new List<MeasurementUnit>();
        AvailableTypes = conn.GetAll<Models.RecipeeType>()?.ToList() ?? new List<Models.RecipeeType>();

        if (Id.HasValue)
        {
            ActiveRecipee = conn.Get<Models.Recipee>(Id.Value);
            Instructions = conn.Query<RecipeeInstruction>("SELECT * FROM RecipeeInstructions WHERE RecipeeId=@Id ORDER BY StepNo", new { Id = Id.Value }).ToList();
            Ingredients = conn.Query<RecipeeIngredient>("SELECT * FROM RecipeeIngredients WHERE RecipeeId=@Id", new { Id = Id.Value }).ToList();
        }
        else
        {
            ActiveRecipee = new Models.Recipee();
            ActiveRecipee.DateCreated = DateTime.Now;
            ActiveRecipee.DateUpdated = DateTime.Now;
        }

        NewInstruction = new RecipeeInstruction();
        NewIngredient = new RecipeeIngredient();
    }

    private void CancelEdit()
    {
        Navigate.NavigateTo("/");
    }

    private async void SaveRecipee()
    {
        if ((Instructions is null || Instructions.Count <= 0) || (Ingredients is null || Ingredients.Count <= 0))
        {
            return;
        }

        using var conn = new SQLiteConnection(ConnectionString);
        if (Id.HasValue)
        {
            ActiveRecipee.DateUpdated = DateTime.Now;
            await conn.UpdateAsync(ActiveRecipee);
        }
        else
        {
            await conn.InsertAsync(ActiveRecipee);
        }

        var recipeeId = ActiveRecipee.Id;

        conn.Query("DELETE FROM RecipeeInstructions WHERE RecipeeId=@Id", new { Id = recipeeId });
        RecipeeInstruction instruction;
        for (int i = 0; i < Instructions.Count; i++)
        {
            instruction = Instructions[i];
            instruction.RecipeeId = recipeeId;
            instruction.StepNo = i + 1;
            conn.Insert(instruction);
        }

        conn.Query("DELETE FROM RecipeeIngredients WHERE RecipeeId=@Id", new { Id = recipeeId });
        foreach (var ingredient in Ingredients)
        {
            ingredient.RecipeeId = recipeeId;
            conn.Insert(ingredient);
        }

        Navigate.NavigateTo("/");
    }

    private void InstructionOnDrag(RecipeeInstruction item)
    {
        CurrentInstructionIdx = Instructions.IndexOf(item);
    }

    private void InstructionOnDrop(RecipeeInstruction item)
    {
        if (item is null)
        {
            return;
        }

        var dropIdx = Instructions.IndexOf(item);

        var dropItem = Instructions[CurrentInstructionIdx];
        Instructions.Remove(dropItem);
        Instructions.Insert(dropIdx, dropItem);

        StateHasChanged();
    }

    private void AddInstruction()
    {
        if(Instructions is null) { Instructions = new List<RecipeeInstruction>(); }
        Instructions.Add(NewInstruction);

        NewInstruction = new RecipeeInstruction();
    }

    private void EditInstructionRow(RecipeeInstruction item, bool isEditing)
    {
        item.IsEditing = isEditing;
    }

    private void SaveInstructionRow(RecipeeInstruction item)
    {
        EditInstructionRow(item, false);
    }

    private void RemoveInstructionRow(RecipeeInstruction item)
    {
        EditInstructionRow(item, false);
        Instructions.Remove(item);
    }

    private void AddIngredient()
    {
        if (Ingredients is null) { Ingredients = new List<RecipeeIngredient>(); }
        NewIngredient.Ingredient = AvailableIngredients.FirstOrDefault(x => x.Id == NewIngredient.IngredientId);
        NewIngredient.Unit = AvailableMeasurements.FirstOrDefault(x => x.Id == NewIngredient.MeasurementUnitId);
        Ingredients.Add(NewIngredient);

        NewIngredient = new RecipeeIngredient();
        NewIngredient.MeasurementUnitId = -1;
        NewIngredient.IngredientId = -1;
    }

    private void EditIngredientRow(RecipeeIngredient item, bool isEditing)
    {
        item.IsEditing = isEditing;
    }

    private void SaveIngredientRow(RecipeeIngredient item)
    {
        EditIngredientRow(item, false);
    }

    private void RemoveIngredientRow(RecipeeIngredient item)
    {
        EditIngredientRow(item, false);
        Ingredients.Remove(item);
    }
}
