using DotNetArchitecture.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace DotNetArchitecture.Models.DBModel
{
    public class User : BaseModel
    {
        [Column(TypeName = "nvarchar(100)")]
        public string? Name { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        public string? Email { get; set; }
    }
}
