using Sports_Merchandise_Store.Migrations;
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
    /// <info>
    /// API CONTROLLER FOR THE FOOTBALL MERCH TABLE
    /// </info>
    public class SoccerMerchandiseDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        //List Soccer Merch
        ///<summary>
        ///Returns all the soccer merch data from the soccer merch table
        ///</summary>
        ///<returns>
        ///The list of soccer merch and their info
        ///</returns>
        ///<example>
        ///GET: api/SoccerMerchandiseData/ListSoccerMerchandise 
        ///</example>
        [HttpGet]
        [Route("api/SoccerMerchandiseData/ListSoccerMerchandise")]
        public IHttpActionResult ListSoccerMerchandise()
        {
            List<SoccerMerchandise> SoccerMerch = db.SoccerMerch.ToList();
            List<SoccerMerchandiseDTO> SoccerMerchDTOs = new List<SoccerMerchandiseDTO>();
            SoccerMerch.ForEach(m => SoccerMerchDTOs.Add(new SoccerMerch
            {
                ItemId = m.ItemId,
                ItemName = m.ItemName,
                ItemType = m.ItemType,
                ItemSize = m.ItemSize,
                Price = m.Price,
                Quantity = m.Quantity,
                TeamName = m.TeamName,
                PlayerName = m.PlayerName
            }));
            return Ok(SoccerMerchDTOs);
        }

        //Find merch by id
        /// <summary>
        /// Returns the details of the merch with the provided id.
        /// </summary>
        /// <returns>
        /// A list of the specified id merch.
        /// </returns>
        /// <example>
        /// GET: api/SoccerMerchandiseData/FindMerchandise/1 
        /// </example>
        [HttpGet]
        [ResponseType(typeof(SoccerMerchandiseDTO))]
        [Route("api/SoccerMerchandiseData/FindMerchandise/{id}")]
        public IHttpActionResult FindMerchandise(int id)
        {

            SoccerMerchandise SoccerMerch = db.SoccerMerchs.Find(id);
            SoccerMerchandiseDTO SoccerMerchandiseDTO = new SoccerMerchandiseDTO()
            {
                ItemId = SoccerMerch.ItemId,
                ItemName = SoccerMerch.ItemName,
                ItemType = SoccerMerch.ItemType,
                ItemSize = SoccerMerch.ItemSize,
                Price = SoccerMerch.Price,
                Quantity = SoccerMerch.Quantity,
                TeamName = SoccerMerch.Team.TeamName,
                PlayerName = SoccerMerch.Player.PlayerName
            };
            if (SoccerMerch == null)
            {
                return NotFound();
            }

            return Ok(SoccerMerchandiseDTO);
        }

        // Update Merch

        /// <summary>
        /// Updates the details of soccer merch.
        /// </summary>
        /// <returns>
        /// HEADER = 204 (Success, No content response)
        /// or
        /// HEADER = 400 (BAD REQUEST)
        /// or
        /// HEADER = 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/SoccerMerchandiseData/SoccerMerchandisePlayer/1
        /// FORM DATA: JSON Team object
        /// </example>
        [HttpPost]
        [ResponseType(typeof(void))]
        [Route("api/SoccerMerchandiseData/SoccerMerchandisePlayer/{id}")]
        public IHttpActionResult UpdateSoccerMerchandise(int id, SoccerMerchandise SoccerMerchandise)
        {
            //Model State validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //player id validation
            if (id != SoccerMerchandise.ItemId)
            {
                return BadRequest();
            }
            // Mark the 'player' entity as modified, so changes will be tracked and saved to the database.
            db.Entry(SoccerMerchandise).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SoccerMerchandiseExists(id))
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

        //Add Soccer Merch
        /// <summary>
        /// Adds new merch with its details in the football merch table.
        /// </summary>
        /// <returns>
        /// HEADER = 201 (CREATED)
        /// CONTENT: Item ID, Item Data
        /// or
        /// HEADER = 400 (BAD REQUEST)
        /// </returns>
        /// <example>
        /// POST: api/SoccerMerchandiseData/AddSoccerMerchandise
        /// FORM DATA: Team JSON object
        /// </example>
        [HttpPost]
        [ResponseType(typeof(Player))]
        [Route("api/SoccerMerchandiseData/AddSoccerMerchandise")]
        public IHttpActionResult AddSoccerMerchandise(SoccerMerchandise SoccerMerchandise)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Adds player to the database and saves it
            db.SoccerMerch.Add(SoccerMerchandise);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { controller = "SoccerMerchandiseController", id = SoccerMerchandise.ItemId }, SoccerMerchandise);
        }

        //Delete Soccer Merch
        /// <summary>
        /// Deletes merch from the database by its id.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// DELETE: api/SoccerMerchandiseData/DeleteSoccerMerchandise/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(SoccerMerchandise))]
        [HttpPost]
        [Route("api/SoccerMerchandiseData/DeleteSoccerMerchandise/{id}")]
        //[Authorize(Roles = "Admin")] // Allow only admins to delete a player.
        public IHttpActionResult DeleteSoccerMerchandise(int id)
        {
            // Finds merch with the provided id in the database
            SoccerMerchandise SoccerMerchandise = db.SoccerMerch.Find(id);
            if (SoccerMerchandise == null)
            {
                return NotFound();
            }
            // Removes the player and saves the changes.
            db.Players.Remove(SoccerMerchandise);
            db.SaveChanges();

            return Ok();
        }
        // Check if a soccer merch with a specified id exists in the database.
        private bool SoccerMerchandiseExists(int id)
        {
            return db.SoccerMerch.Count(m => m.ItemId == id) > 0;
        }
    }
}
