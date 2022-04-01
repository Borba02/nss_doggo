using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace DogGo.Models
{
    public class Dog
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Hmmm... You should really add a Name...")]
        [MaxLength(35)]
        public string Name { get; set; }
        [Required]
        public int OwnerId { get; set; }
        [Required]
        [StringLength(55, MinimumLength = 5)]
        public string Breed { get; set; }
        public string Notes { get; set; }
        [DisplayName("Picture")]
        public string ImageUrl  { get; set; }
        public Owner Owner { get; set; }
    }
}
