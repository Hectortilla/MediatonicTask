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
    public class AnimalController : ControllerBase
    {
        private readonly DatabaseContext db;
        public AnimalController(DatabaseContext dbContext)
        {
            db = dbContext;
        }
        /// <summary>
        /// Get all the animals
        /// </summary>
        [HttpGet]
        public List<Animal> Get()
        {
            List<Animal> animals = new List<Animal>();
            animals = db.Animals.Include(animal => animal.Owner).ToList<Animal>();
            return animals;
        }

        /// <summary>
        /// Get a specific animal
        /// </summary>
        [HttpGet("{id}")]
        public Animal Get(int id)
        {
            Animal animal;
            animal = db.Animals.Include(animal => animal.Owner).FirstOrDefault(animal => animal.Id == id);
            return animal;
        }

        /// <summary>
        /// Creates a new animal
        /// </summary>
        [HttpPost]
        public IActionResult Post([FromBody] Animal animal)
        {
            if (ModelState.IsValid)
            {
                db.Animals.Add(animal);
                try
                {
                    db.SaveChanges();
                }
                catch (DbUpdateException e)
                {
                    return BadRequest(DBExceptionFormatter.format(e));
                }
            return new JsonResult("Animal saved");
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        /// <summary>
        /// Updates a specific animal
        /// </summary>
        [HttpPut()]
        public IActionResult Put([FromBody] Animal animal)
        {
            try
            {
                db.Entry(animal).State = EntityState.Modified;
                db.SaveChanges();
                return new JsonResult("Animal updated");
            }
            catch (DbUpdateException e)
            {
                return BadRequest(DBExceptionFormatter.format(e));
            }
        }


        /// <summary>
        /// Deletes a specific animal
        /// </summary>
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var animal = new Animal() { Id = id };
                db.Animals.Remove(animal);
                db.SaveChanges();
                return new JsonResult("Animal deleted");
            }
            catch (DbUpdateException e)
            {
                return BadRequest(DBExceptionFormatter.format(e));
            }
        }

        /// <summary>
        /// It feed a specific animal decreasing its hunger
        /// </summary>
        [HttpPut()][Route("/api/[controller]/Feed/{id}")]
        public IActionResult Feed(int id)
        {
            Animal animal = db.Animals.FirstOrDefault(animal => animal.Id == id);
            if (animal == null) {
                return BadRequest("Provide a valid id");
            }
            animal.Feed(db);
            return new JsonResult("Animal fed");
        }

        /// <summary>
        /// It pets a specific animal increasing its hapiness
        /// </summary>
        [HttpPut()][Route("/api/[controller]/Stroke/{id}")]
        public IActionResult Stroke(int id)
        {
            Animal animal = db.Animals.FirstOrDefault(animal => animal.Id == id);
            if (animal == null)
            {
                return BadRequest("Provide a valid id");
            }
            animal.Stroke(db);
            return new JsonResult("Animal stroked");
        }
    }
}
