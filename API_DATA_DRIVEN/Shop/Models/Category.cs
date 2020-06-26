using System.ComponentModel.DataAnnotations;

namespace Shop.Models
{
    // [Table("Categoria")]
    public class Category
    {
        [Key]
        // [Column("Cat_ID")]
        public long Id { get; set; }
        
        [MaxLength(60, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres")]
        [MinLength(3, ErrorMessage = "Este campo deve conter entre 3 e 60 caracteres")]
        // [DataType("nvarchar")]
        public string Title { get; set; }
    }
}