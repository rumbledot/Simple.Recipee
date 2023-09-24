using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipee.Web.Models;

public class Recipee : BaseModel
{
    public int Id { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime DateUpdated { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Title { get; set; }

    public string Description { get; set; }

    public int RecipeeTypeId { get; set; }

    [Computed] public RecipeeType RecipeeType { get; set; }

    [Computed] public List<RecipeeInstruction> Instructions { get; set; }

    [Computed] public List<RecipeeIngredient> Ingredients { get; set; }
}