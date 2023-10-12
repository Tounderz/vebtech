using Swashbuckle.AspNetCore.Annotations;
using vebtech.Domain.Models.Enums;

namespace vebtech.Domain.Models.DTO;

public class SortParameters
{
    public string OrderBy { get; set; } = string.Empty;

    [SwaggerParameter("0 - ascending, 1 - descending", Required = false)]
    public SortDirection OrderAsc { get; set; }
}