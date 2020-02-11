using Xunit;
using PetAPI.Controllers;
using Microsoft.EntityFrameworkCore;
using PetData;
using PetData.Models;
using System.Linq;

namespace PetTests
{
    public class UserControllerTest
    {
        private DbContextOptions options = new DbContextOptionsBuilder<DatabaseContext>().UseInMemoryDatabase(databaseName: "pet").Options;

        [Fact]
        public void GetEmpty()
        {
            using (DatabaseContext db = new DatabaseContext(options)) {
                db.Database.EnsureDeleted();
                UserController _userController = new UserController(db);
                Assert.Empty(_userController.Get());
            }
        }
        [Fact]
        public void GetSingle()
        {
            using (DatabaseContext db = new DatabaseContext(options))
            {
                db.Database.EnsureDeleted();
                db.Users.Add(new User { Id = 1, Name = "Hector", Surname = "Soria" });
                db.SaveChanges();

                UserController _userController = new UserController(db);
                Assert.Single(_userController.Get());
            }
        }
        [Fact]
        public void GetMulti()
        {
            using (DatabaseContext db = new DatabaseContext(options))
            {
                db.Database.EnsureDeleted();
                db.Users.Add(new User { Id = 1, Name = "Hector", Surname = "Soria" });
                db.Users.Add(new User { Id = 2, Name = "Rocteh", Surname = "Airos" });
                db.SaveChanges();

                UserController _userController = new UserController(db);
                Assert.Equal(2, _userController.Get().Count);
            }
        }
        [Fact]
        public void GetById()
        {
            using (DatabaseContext db = new DatabaseContext(options))
            {
                db.Database.EnsureDeleted();
                db.Users.Add(new User { Id = 1, Name = "Hector", Surname = "Soria" });
                db.Users.Add(new User { Id = 2, Name = "Rocteh", Surname = "Airos" });
                db.SaveChanges();

                UserController _userController = new UserController(db);
                Assert.Equal(1, _userController.Get(1).Id);
                Assert.Equal(2, _userController.Get(2).Id);
            }
        }
        [Fact]
        public void PostUser()
        {
            using (DatabaseContext db = new DatabaseContext(options))
            {
                db.Database.EnsureDeleted();
                UserController _userController = new UserController(db);
                _userController.Post(new User { Id = 1, Name = "Hector", Surname = "Soria" });
                Assert.Single(db.Users.ToList<User>());
                _userController.Post(new User { Id = 2, Name = "Rocteh", Surname = "Airos" });
                Assert.Equal(2, db.Users.ToList<User>().Count);

            }
        }
        [Fact]
        public void PutUser()
        {
            using (DatabaseContext db = new DatabaseContext(options))
            {
                db.Database.EnsureDeleted();
                db.Users.Add(new User { Id = 1, Name = "Hector", Surname = "Soria" });
                db.SaveChanges();
            }
            using (DatabaseContext db = new DatabaseContext(options))
            {
                UserController _userController = new UserController(db);
                _userController.Put(new User { Id = 1, Name = "Rocteh", Surname = "Airos" });
                Assert.Equal("Rocteh", db.Users.FirstOrDefault<User>().Name);
            }
        }
        [Fact]
        public void DeleteUser()
        {
            using (DatabaseContext db = new DatabaseContext(options))
            {
                db.Database.EnsureDeleted();
                db.Users.Add(new User { Id = 1, Name = "Hector", Surname = "Soria" });
                db.SaveChanges();
            }
            using (DatabaseContext db = new DatabaseContext(options))
            {
                UserController _userController = new UserController(db);
                _userController.Delete(1);
                Assert.Null(db.Users.FirstOrDefault<User>());
            }
        }
    }
}
