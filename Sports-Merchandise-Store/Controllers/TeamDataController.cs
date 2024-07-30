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
using Passion_Project.Models;
using Sports_Merchandise_Store.Models;

/// <info>
/// API CONTROLLER FOR THE FOOTBALL TEAMS TABLE
/// </info>

namespace Sports_Merchandise_Store.Controllers
{
    public class TeamDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

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
        public IEnumerable<TeamDTO> ListTeams()
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
            return TeamDTOs;
        }

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
        public IHttpActionResult GetTeam(int id)
        {
            Team team = db.Teams.Find(id);
            if (team == null)
            {
                return NotFound();
            }
            return Ok(team);
        }
    }
}
