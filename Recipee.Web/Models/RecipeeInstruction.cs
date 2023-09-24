using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipee.Web.Models;

public class RecipeeInstruction : BaseModel
{
    public int RecipeeId { get; set; }

    public int StepNo { get; set; }

    [Required]
    public string Description { get; set; }
}