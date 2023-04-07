using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Middleware.Models
{
    public class StudentDetails
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ValidateUnique("Students", "StudentName")]
        public string StudentName { get; set; }
        [Required(ErrorMessage = "Please enter name"), MaxLength(30)]

        public string StudentEmail { get; set; }
        [Required(ErrorMessage = "Please enter email")]
        [DisplayName("StudentAddress")]
        public string StudentAddress { get; set; }
        [Required(ErrorMessage = "Please enter address")]
        [RegularExpression(@"^\$?\d+(\.(\d{2}))?$")]
        public int StudentAge { get; set; }
    }
}
