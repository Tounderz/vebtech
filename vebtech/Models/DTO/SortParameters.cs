using Swashbuckle.AspNetCore.Annotations;
using vebtech.Models.Enums;

namespace vebtech.Models.DTO;

public class SortParameters
{
    public string OrderBy { get; set; } = String.Empty;

    [SwaggerParameter("0 - ascending, 1 - descending", Required = false)]
    public SortDirection OrderAsc { get; set; }
}