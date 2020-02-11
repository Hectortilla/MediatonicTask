using System;
using System.ComponentModel.DataAnnotations;
using PetData.Mixins;

namespace PetData.Models
{
    public class User : IHasCreationLastModified
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
