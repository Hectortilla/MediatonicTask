using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using PetData.Mixins;

namespace PetData.Models
{
    public enum AnimalType { Mamal, Reptile, Bird };
    public static class AnimalStats {
        public static int multiplier = 10;
        public static Dictionary<int, int> stats = new Dictionary<int, int>() {
            {(int) AnimalType.Mamal, 1},
            {(int) AnimalType.Reptile, 2},
            {(int) AnimalType.Bird, 3}
        };
    }
    public class Animal: IHasCreationLastModified
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public User Owner { get; set; }
        [Required]
        public AnimalType Type { get; set; }
        public int Hunger { get; set; } = 0;
        public int Happiness { get; set; } = 0;
        public DateTime? Created { get; set; }
        public DateTime? LastModified { get; set; }

        // It will increase it's hapinnes AnymalType * 10
        public void Stroke(DatabaseContext db) {
            int animalStat = AnimalStats.stats.GetValueOrDefault((int)this.Type);
            this.Happiness += animalStat * AnimalStats.multiplier;
            db.Animals.Update(this);
            db.SaveChanges();
        }
        // It will decrease it's hunger AnymalType * 10
        public void Feed(DatabaseContext db)
        {
            int animalStat = AnimalStats.stats.GetValueOrDefault((int)this.Type);
            this.Hunger -= animalStat * AnimalStats.multiplier;
            db.Animals.Update(this);
            db.SaveChanges();
        }
        // It will increase it's hunger and decrease its happiness according to its type
        public void LifeCycle() {
            int animalStat = AnimalStats.stats.GetValueOrDefault((int)this.Type);
            this.Hunger += animalStat;
            this.Happiness -= animalStat;
        }
        // It applies the live cycle to all animals in the DB
        public static void AnimalsLifeCycle(DatabaseContext db)
        {
            List<Animal> animals = new List<Animal>();
            foreach (Animal animal in db.Animals.ToList<Animal>())
            {
                animal.LifeCycle();
                db.Animals.Update(animal);
            }
            db.SaveChanges();
        }
    }
}
