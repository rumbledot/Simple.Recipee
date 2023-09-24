using Dapper.Contrib.Extensions;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipee.Web.Models;

public class Journal : BaseModel
{
    public int Id { get; set; }

    public int? RecipeeId { get; set; }

    [Computed] public Recipee? Recipee { get; set; }

    public DateTime DateCreated { get; set; }

    public DateTime? DateUpdated { get; set; }

    public string Description { get; set; }
}