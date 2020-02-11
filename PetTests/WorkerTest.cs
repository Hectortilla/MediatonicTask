using Xunit;
using PetAPI.Controllers;
using Microsoft.EntityFrameworkCore;
using PetData;
using PetData.Models;
using System.Linq;
using PetTasks;
using System.Collections.Generic;

namespace PetTests
{
    public class WorkerTest
    {
        private DbContextOptions options = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase(databaseName: "pet").Options;

        [Fact]
        public void GetEmpty()
        {

            AnimalType animalType = AnimalType.Mamal;
            User user = new User { Id = 1, Name = "Hector", Surname = "Soria" };
            Animal animal = new Animal { OwnerId = 1, Type = animalType };

            using (DatabaseContext db = new DatabaseContext(options))
            {
                db.Database.EnsureDeleted();
                db.Users.Add(user);
                db.Animals.Add(animal);
                db.SaveChanges();
            }
            using (DatabaseContext db = new DatabaseContext(options))
            {
                Assert.Equal(0, db.Animals.FirstOrDefault<Animal>().Happiness);
                Assert.Equal(0, db.Animals.FirstOrDefault<Animal>().Hunger);
                Worker.UpdateAnimals(db);
                Assert.Equal(
                    -AnimalStats.stats.GetValueOrDefault((int)animalType),
                    db.Animals.FirstOrDefault<Animal>().Happiness
                );
                Assert.Equal(
                    AnimalStats.stats.GetValueOrDefault((int)animalType),
                    db.Animals.FirstOrDefault<Animal>().Hunger
                );
            }
        }
    }
}
