using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Sports_Merchandise_Store.Models;

/// <info>
/// API CONTROLLER FOR THE FOOTBALL TEAMS TABLE
/// </info>

namespace Sports_Merchandise_Store.Controllers
{
    public class TeamDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // ListTeams

        /// <summary>
        /// Returns all the teams from the Teams table.
        /// </summary>
        /// <returns>
        /// The list of the teams and their info.
        /// </returns>
        /// <example>
        /// GET: api/TeamData/ListTeams --> (TeamId, TeamName, League, TeamCountry, TeamBudget)
        /// </example>

        [HttpGet]
        [Route("api/TeamData/ListTeams")]
        // Set up GET request and Route
        public IHttpActionResult ListTeams()
        {
            List<Team> Teams = db.Teams.ToList();

            List<TeamDTO> TeamDTOs = new List<TeamDTO>();
            Teams.ForEach(t => TeamDTOs.Add(new TeamDTO()
            {
                TeamId = t.TeamId,
                TeamName = t.TeamName,
                League = t.League,
                TeamCountry = t.TeamCountry,
                TeamBudget = t.TeamBudget
            }));
            return Ok(TeamDTOs);
        }

        // Find Team with specific id

        /// <summary>
        /// Returns the details of the team with the provided id.
        /// </summary>
        /// <returns>
        /// A list of the specified id Team.
        /// </returns>
        /// <example>
        /// GET: api/TeamData/1 --> (TeamId, TeamName, League, TeamCountry, TeamBudget) --> (1, Chelsea, Premier League, England, 2,800,000,000) 
        /// </example>

        [HttpGet]
        [ResponseType(typeof(Team))]
        [Route("api/TeamData/FindTeam/{id}")]
        public IHttpActionResult FindTeam(int id)
        {
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return NotFound();
            }
            return Ok(team);
        }

        // UpdateTeam with specific id

        /// <summary>
        /// Updates the details of the team.
        /// </summary>
        /// <returns>
        /// HEADER = 204 (Success, No content response)
        /// or
        /// HEADER = 400 (BAD REQUEST)
        /// or
        /// HEADER = 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/TeamData/UpdateTeam/1
        /// FORM DATA: JSON Team object
        /// </example>
        [HttpPost]
        [ResponseType(typeof(void))]
        [Route("api/TeamData/UpdateTeam/{id}")]
        public IHttpActionResult UpdateTeam(int id, Team team)
        {
            // Model State invalid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Team id invalid
            if (id != team.TeamId)
            {
                return BadRequest();
            }
            // Mark the 'team' entity as modified, so changes will be tracked and saved to the database.
            db.Entry(team).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(id))
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

        // AddTeam

        /// <summary>
        /// Adds a new team with its details in the football teams table.
        /// </summary>
        /// <returns>
        /// HEADER = 201 (CREATED)
        /// CONTENT: Team ID, Team Data
        /// or
        /// HEADER = 400 (BAD REQUEST)
        /// </returns>
        /// <example>
        /// POST: api/TeamData/AddTeam
        /// FORM DATA: Team JSON object
        /// </example>
        [ResponseType(typeof(Team))]
        [HttpPost]
        [Route("api/TeamData/AddTeam")]

        public IHttpActionResult AddTeam(Team team)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Adds the team to the database and saves it.
            db.Teams.Add(team);
            db.SaveChanges();

            //Debugging
            //Debug.WriteLine(ModelState);
            //Debug.WriteLine(team);

            return CreatedAtRoute("DefaultApi", new { controller = "TeamController", id = team.TeamId }, team);
        }

        //DeleteTeam

        /// <summary>
        /// Deletes a Team from the database by its id.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// DELETE: api/TeamData/DeleteTeam/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Team))]
        [HttpPost]
        [Route("api/TeamData/DeleteTeam/{id}")]
        //[Authorize(Roles = "Admin")] // Allow only admins to delete a team.
        public IHttpActionResult DeleteTeam(int id)
        {   
            // Finds the team with the provided id in the database
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return NotFound();
            }
            // Removes the team and saves the changes.
            db.Teams.Remove(team);
            db.SaveChanges();

            return Ok();
        }

        // Check if a team with a specified id exists in the database.
        private bool TeamExists(int id)
        {
            return db.Teams.Count(t => t.TeamId == id) > 0;
        }
    }
}

