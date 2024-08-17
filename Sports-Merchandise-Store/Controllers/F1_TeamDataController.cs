using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Sports_Merchandise_Store.Models;

namespace Sports_Merchandise_Store.Controllers
{
    public class F1_TeamDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Teams in the table in the database.
        /// </summary>
        /// <returns>
        /// List of teams and their details in the table.
        /// </returns>
        /// <example>
        /// // GET: api/TeamData/ListTeam => Data of teams in the table
        /// </example>
        [HttpGet]
        [Route("api/F1TeamData/ListF1Team")]
        
        public IEnumerable<F1_TeamDTO> ListF1Team()
        {
            List<F1_Team> F1Teams = db.F1_Teams.ToList();
            List<F1_TeamDTO> F1_TeamDTOs = new List<F1_TeamDTO>();

            F1Teams.ForEach(a => F1_TeamDTOs.Add(new F1_TeamDTO()
            {
                F1TeamId = a.F1TeamId,
                F1TeamName = a.F1TeamName,
                EngineSupplier = a.EngineSupplier,
                Country = a.Country
            }));

            return F1_TeamDTOs;
        }

        /// <summary>
        /// Returns the F1 Team details with the specified TeamId
        /// </summary>
        /// <param name="id">TeamId of the Team</param>
        /// <returns>
        /// HEADER: 200 (Status Code for OK)
        /// </returns>
        /// <example>
        /// // GET: api/F1_TeamData/5 => Data of Team with TeamId 5
        /// </example>

        // GET: api/F1_TeamData/5
        [ResponseType(typeof(F1_Team))]
        public IHttpActionResult GetF1_Team(int id)
        {
            F1_Team f1_Team = db.F1_Teams.Find(id);
            if (f1_Team == null)
            {
                return NotFound();
            }

            return Ok(f1_Team);
        }

        
        /// <summary>
        /// Updates the Team details of a particular team with the POST data input
        /// </summary>
        /// <param name="id">The TeamId in the table (primary key)</param>
        /// <param name="team">JSON Form Data of a Team</param>
        /// <returns>
        /// Status Code 
        /// HEADER: 200 = Success
        /// or
        /// HEADER: 400 = Bad Request
        /// or
        /// HEADER: 404 = Not Found
        /// </returns>
        /// <example>
        /// POST: api/F1_TeamData/UpdateTeam/5
        /// FORM Data: Team JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateF1_Team(int id, F1_Team f1_Team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != f1_Team.F1TeamId)
            {
                return BadRequest();
            }

            db.Entry(f1_Team).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!F1_TeamExists(id))
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
        /// Add New Team details onto the Team table in the Database
        /// </summary>
        /// <param name="team">JSON Form Data of the Team</param>
        /// Status Code 
        /// HEADER: 200 = Success
        /// or
        /// HEADER: 400 = Bad Request
        /// or
        /// HEADER: 404 = Not Found
        /// <example>
        /// POST: api/F1_TeamData/AddTeam
        /// FORM Data: Team JSON Object
        /// </example>

        [ResponseType(typeof(F1_Team))]
        [HttpPost]
        public IHttpActionResult AddF1_Team(F1_Team f1_Team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.F1_Teams.Add(f1_Team);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = f1_Team.F1TeamId }, f1_Team);
        }

        /// <summary>
        /// Deletes Team data from the database with TeamId.
        /// </summary>
        /// <param name="id">The primary key of the Team</param>
        /// <returns>
        /// HEADER: 200 = OK
        /// or
        /// HEADER: 404 = Not Found
        /// </returns>
        /// <example>
        /// POST: api/F1_TeamData/DeleteTeam/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(F1_Team))]
        [Route("api/F1_TeamData/DeleteF1Team/{id}")]
        [HttpPost]
        public IHttpActionResult DeleteF1_Team(int id)
        {
            F1_Team f1_Team = db.F1_Teams.Find(id);
            if (f1_Team == null)
            {
                return NotFound();
            }

            db.F1_Teams.Remove(f1_Team);
            db.SaveChanges();

            return Ok(f1_Team);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool F1_TeamExists(int id)
        {
            return db.F1_Teams.Count(e => e.F1TeamId == id) > 0;
        }
    }
}
