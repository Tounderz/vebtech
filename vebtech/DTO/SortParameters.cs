using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using vebtech.Enum;

namespace vebtech.DTO
{
    public class SortParameters
    {
        public string OrderBy { get; set; } = String.Empty;

        [SwaggerParameter("0 - ascending, 1 - descending", Required = false)]
        public SortDirection OrderAsc { get; set; }
    }
}
