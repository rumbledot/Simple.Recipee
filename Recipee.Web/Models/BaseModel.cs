using Dapper.Contrib.Extensions;

namespace Recipee.Web.Models;

public class BaseModel
{
    [Computed] public bool IsEditing { get; set; } = false;
}
