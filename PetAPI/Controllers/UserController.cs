using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PetData;
using PetData.Models;
using PetData.Utils;

namespace PetAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly DatabaseContext db;
        public UserController(DatabaseContext dbContext)
        {
            db = dbContext;
        }
        /// <summary>
        /// Get all the users
        /// </summary>
        [HttpGet]
        public List<User> Get()
        {
            List<User> users = new List<User>();
            users = db.Users.ToList<User>();
            return users;
        }

        /// <summary>
        /// Get a specific user
        /// </summary>
        [HttpGet("{id}")]
        public User Get(int id)
        {
            User user;
            user = db.Users.FirstOrDefault(user => user.Id == id);
            return user;
        }

        /// <summary>
        /// Creates a new user
        /// </summary>
        [HttpPost]
        public IActionResult Post(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(user);
                try
                { 
                    db.SaveChanges();
                }
                catch (DbUpdateException e) {
                    return BadRequest(DBExceptionFormatter.format(e));
                }
                return new JsonResult("User saved");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Updates a specific user
        /// </summary>
        [HttpPut()]
        public IActionResult Put([FromBody] User user)
        {
            try
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return new JsonResult("User updated");
            }
            catch (DbUpdateException e)
            {
                return BadRequest(DBExceptionFormatter.format(e));
            }
        }

        /// <summary>
        /// Deletes a specific user
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var user = new User(){Id = id};
                db.Users.Remove(user);
                db.SaveChanges();
                return new JsonResult("User deleted");
            }
            catch (DbUpdateException e)
            {
                return BadRequest(DBExceptionFormatter.format(e));
            }
        }
    }
}
