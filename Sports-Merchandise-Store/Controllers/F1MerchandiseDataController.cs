using Sports_Merchandise_Store.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Sports_Merchandise_Store.Controllers
{
    public class F1MerchandiseDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        ///<summary>
        ///Returns all the F1 merchandise data from the F1Merchandise table
        ///</summary>
        ///<returns>
        ///The list of F1 Merchandise and their Details
        ///</returns>
        ///<example>
        ///GET: api/F1MerchandiseData/ListF1Merchandise 
        ///</example>
        [HttpGet]
        [Route("api/F1MerchandiseData/ListF1Merchandise")]
        public IHttpActionResult ListF1Merchandise()
        {
            List<F1Merchandise> F1Merch = db.F1Merchandise.ToList();
            List<F1MerchandiseDTO> F1MerchDTOs = new List<F1MerchandiseDTO>();
            F1Merch.ForEach(m => F1MerchDTOs.Add(new F1MerchandiseDTO
            {
                ItemId = m.ItemId,
                ItemName = m.ItemName,
                ItemType = m.ItemType,
                ItemSize = m.ItemSize,
                Price = m.Price,
                Quantity = m.Quantity,
                F1TeamName = m.F1_Team.F1TeamName,
                DriverName = F1Merch.Driver.FirstName + F1Merch.Driver.LastName
            }));
            return Ok(F1MerchDTOs);
        }


        /// <summary>
        /// Returns the details of the F1 merchandise with the provided id.
        /// </summary>
        /// <returns>
        /// A list of the specified merchandise.
        /// </returns>
        /// <example>
        /// GET: api/F1MerchandiseData/FindMerchandise/1 
        /// </example>
        [HttpGet]
        [ResponseType(typeof(F1MerchandiseDTO))]
        [Route("api/F1MerchandiseData/FindMerchandise/{id}")]
        public IHttpActionResult FindMerchandise(int id)
        {

            F1Merchandise F1Merch = db.F1Merchandise.Find(id);
            F1MerchandiseDTO F1MerchandiseDTO = new F1MerchandiseDTO()
            {
                ItemId = F1Merch.ItemId,
                ItemName = F1Merch.ItemName,
                ItemType = F1Merch.ItemType,
                ItemSize = F1Merch.ItemSize,
                Price = F1Merch.Price,
                Quantity = F1Merch.Quantity,
                F1TeamName = F1Merch.Team.TeamName,
                DriverName = F1Merch.Driver.FirstName + F1Merch.Driver.LastName
            };
            if (F1Merch == null)
            {
                return NotFound();
            }

            return Ok(F1MerchandiseDTO);
        }

        // UPDATE

        /// <summary>
        /// Updates the details of F1 Merchandise.
        /// </summary>
        /// <returns>
        /// HEADER = 204 (Success, No content response)
        /// or
        /// HEADER = 400 (BAD REQUEST)
        /// or
        /// HEADER = 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/F1MerchandiseData/F1MerchandisePlayer/1
        /// FORM DATA: JSON Team object
        /// </example>
        [HttpPost]
        [ResponseType(typeof(void))]
        [Route("api/F1MerchandiseData/F1MerchandisePlayer/{id}")]
        public IHttpActionResult UpdateF1Merchandise(int id, F1Merchandise F1Merchandise)
        {
            //Model State validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //player id validation
            if (id != F1Merchandise.ItemId)
            {
                return BadRequest();
            }
            // Mark the 'player' entity as modified, so changes will be tracked and saved to the database.
            db.Entry(F1Merchandise).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!F1MerchandiseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return StatusCode(HttpStatusCode.NoContent);
        }


        /// <summary>
        /// Adds new Merchandise with its details in the F1 Merchandise table.
        /// </summary>
        /// <returns>
        /// HEADER: 200 = Success
        /// or
        /// HEADER: 400 = Bad Request
        /// or
        /// HEADER: 404 = Not Found
        /// </returns>
        /// <example>
        /// POST: api/F1MerchandiseData/AddF1Merchandise
        /// FORM DATA: Team JSON object
        /// </example>
        [HttpPost]
        [ResponseType(typeof(Player))]
        [Route("api/F1MerchandiseData/AddF1Merchandise")]
        public IHttpActionResult AddF1Merchandise(F1Merchandise F1Merchandise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Adds player to the database and saves it
            db.F1Merchandise.Add(F1Merchandise);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { controller = "F1MerchandiseController", id = F1Merchandise.ItemId }, F1Merchandise);
        }


        /// <summary>
        /// Deletes Merchandise from the database by its id.
        /// </summary>
        /// <returns>
        /// HEADER: 200 = OK
        /// or
        /// HEADER: 404 = Not Found
        /// </returns>
        /// <example>
        /// DELETE: api/F1MerchandiseData/DeleteF1Merchandise/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(F1Merchandise))]
        [HttpPost]
        [Route("api/F1MerchandiseData/DeleteF1Merchandise/{id}")]
        //[Authorize(Roles = "Admin")] // Allow only admins to delete a player.
        public IHttpActionResult DeleteF1Merchandise(int id)
        {
            // Finds Merchandise with the provided id in the database
            F1Merchandise F1Merchandise = db.F1Merchandise.Find(id);
            if (F1Merchandise == null)
            {
                return NotFound();
            }

            db.F1Merchandise.Remove(F1Merchandise);
            db.SaveChanges();

            return Ok();
        }
        // Check if a F1 Merchandise with a specified id exists in the database.
        private bool F1MerchandiseExists(int id)
        {
            return db.F1Merchandise.Count(m => m.ItemId == id) > 0;
        }

    }
}
