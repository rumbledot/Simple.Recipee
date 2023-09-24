using Dapper.Contrib.Extensions;
using DapperExtensions.Mapper;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Recipee.Web.Models;

public class RecipeeType : BaseModel
{
    public int Id { get; set; }

    [Required]
    [MaxLength(50)]
    public string Description { get; set; }
}

public class RecipeeTypeMapper : ClassMapper<RecipeeType>
{
    public RecipeeTypeMapper()
    {
        Table("RecipeeType");
        Map(c => c.Id).Column("Id").Key(KeyType.Identity);
        Map(c => c.Description).Column("Description");
        Map(c => c.IsEditing).Ignore();
    }
}