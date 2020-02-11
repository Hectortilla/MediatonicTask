using Xunit;
using PetAPI.Controllers;
using Microsoft.EntityFrameworkCore;
using PetData;
using PetData.Models;
using System.Linq;
using System.Collections.Generic;

namespace PetTests
{
    public class AnimalControllerTest
    {
        private DbContextOptions options = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase(databaseName: "pet").Options;

        [Fact]
        public void GetEmpty()
        {
            using (DatabaseContext db = new DatabaseContext(options))
            {
                db.Database.EnsureDeleted();
                AnimalController _animalController = new AnimalController(db);
                Assert.Empty(_animalController.Get());
            }
        }
        [Fact]
        public void GetSingle()
        {
            using (DatabaseContext db = new DatabaseContext(options))
            {
                db.Database.EnsureDeleted();
                db.Users.Add(new User { Id = 1, Name = "Hector", Surname = "Soria" });
                db.Animals.Add(new Animal { OwnerId = 1});
                db.SaveChanges();

                AnimalController _animalController = new AnimalController(db);
                Assert.Single(_animalController.Get());
            }
        }
        [Fact]
        public void GetMulti()
        {
            using (DatabaseContext db = new DatabaseContext(options))
            {
                db.Database.EnsureDeleted();
                db.Users.Add(new User { Id = 1, Name = "Hector", Surname = "Soria" });
                db.Animals.Add(new Animal { OwnerId = 1 });
                db.Animals.Add(new Animal { OwnerId = 1 });
                db.SaveChanges();

                AnimalController _animalController = new AnimalController(db);
                Assert.Equal(2, _animalController.Get().Count);
            }
        }
        [Fact]
        public void GetById()
        {
            User user = new User { Id = 1, Name = "Hector", Surname = "Soria" };
            Animal animal1 = new Animal { OwnerId = 1 };
            Animal animal2 = new Animal { OwnerId = 1 };

            using (DatabaseContext db = new DatabaseContext(options))
            {
                db.Database.EnsureDeleted();
                db.Users.Add(user);
                db.Animals.Add(animal1);
                db.Animals.Add(animal2);
                db.SaveChanges();

                AnimalController _animalController = new AnimalController(db);
                Assert.Equal(animal1.Id, _animalController.Get(animal1.Id).Id);
                Assert.Equal(animal2.Id, _animalController.Get(animal2.Id).Id);
            }
        }
        [Fact]
        public void PostAnimal()
        {
            using (DatabaseContext db = new DatabaseContext(options))
            {
                db.Database.EnsureDeleted();
                db.Users.Add(new User { Id = 1, Name = "Hector", Surname = "Soria" });
                db.SaveChanges();
            }
            using (DatabaseContext db = new DatabaseContext(options))
            {
                AnimalController _animalController = new AnimalController(db);
                _animalController.Post(new Animal { OwnerId = 1});
                Assert.Single(db.Animals.ToList<Animal>());
                _animalController.Post(new Animal { OwnerId = 1 });
                Assert.Equal(2, db.Animals.ToList<Animal>().Count);

            }
        }
        [Fact]
        public void PutAnimal()
        {
            using (DatabaseContext db = new DatabaseContext(options))
            {
                db.Database.EnsureDeleted();
                db.Users.Add(new User { Id = 1, Name = "Hector", Surname = "Soria" });
                db.Animals.Add(new Animal { OwnerId = 1 });
                db.SaveChanges();
            }
            using (DatabaseContext db = new DatabaseContext(options))
            {
                AnimalController _animalController = new AnimalController(db);
                _animalController.Put(new Animal { Id = 1, OwnerId = 1, Type = AnimalType.Reptile });
                Assert.Equal(AnimalType.Reptile, db.Animals.FirstOrDefault<Animal>().Type);
            }
        }
        [Fact]
        public void DeleteAnimal()
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
                AnimalController _animalController = new AnimalController(db);
                _animalController.Delete(animal.Id);
                Assert.Null(db.Animals.FirstOrDefault<Animal>());
            }
        }
        [Fact]
        public void FeedAnimal()
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
                AnimalController _animalController = new AnimalController(db);
                Assert.Equal(0, db.Animals.FirstOrDefault<Animal>().Hunger);
                _animalController.Feed(animal.Id);
                Assert.Equal(
                    -AnimalStats.stats.GetValueOrDefault((int)animalType) * AnimalStats.multiplier,
                    db.Animals.FirstOrDefault<Animal>().Hunger
                );
            }
        }
        [Fact]
        public void StrokeAnimal()
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
                AnimalController _animalController = new AnimalController(db);
                Assert.Equal(0, db.Animals.FirstOrDefault<Animal>().Happiness);
                _animalController.Stroke(animal.Id);
                Assert.Equal(
                    AnimalStats.stats.GetValueOrDefault((int)animalType) * AnimalStats.multiplier,
                    db.Animals.FirstOrDefault<Animal>().Happiness
                );
            }
        }
    }
}
