using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipee.Web.Models;

public class RecipeeIngredient : BaseModel
{
    public int RecipeeId { get; set; }
    
    public int IngredientId { get; set; }

    [Required]
    public float Amount { get; set; }

    public int MeasurementUnitId { get; set; }

    [Computed] public Ingredient Ingredient { get; set; }

    [Computed] public MeasurementUnit Unit { get; set; }
}