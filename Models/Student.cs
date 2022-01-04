using System.ComponentModel.DataAnnotations.Schema;

namespace ImageUpload.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string ProfilePicture { get; set; }

        [NotMapped]
        public IFormFile StudentProfile { get; set; }
    }
}
