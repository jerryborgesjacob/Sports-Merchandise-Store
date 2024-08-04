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
/// API CONTROLLER FOR THE FOOTBALL PLAYERS TABLE
/// </info>

namespace Sports_Merchandise_Store.Controllers
{
    public class PlayerDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //List Players
        ///<summary>
        ///Returns all the players from teh players' table
        ///</summary>
        ///<returns>
        ///The list of players and their info
        ///</returns>
        ///<example>
        ///GET: api/PlayerData/ListPlayers --> (PlayerID, PlayerName, PlayerPosition, ShirtNumber, current Team)
        ///</example>

        [HttpGet]
        [Route("api/PlayerData/ListPlayers")]
        public IHttpActionResult ListPlayers()
        {
            List<Player> Players = db.Players.ToList();
            List<PlayerDTO> PlayerDTOs = new List<PlayerDTO>();
            Players.ForEach(p => PlayerDTOs.Add(new PlayerDTO
            {
                PlayerId = p.PlayerId,
                PlayerName = p.PlayerName,
                PlayerPosition = p.PlayerPosition,
                ShirtNumber = p.ShirtNumber,
                TeamName = p.Team.TeamName
            }));
            return Ok(PlayerDTOs);
        }

        //Find a player by id
        /// <summary>
        /// Returns the details of the player with the provided id.
        /// </summary>
        /// <returns>
        /// A list of the specified id player.
        /// </returns>
        /// <example>
        /// GET: api/PlayerData/1 --> (PLayerId, PlayerName, PlayerPosition, ShirtNumber, PlayerTeamId = TeamName) --> (1, Cole Palmer, RW, 20, Chelsea) 
        /// </example>
        [HttpGet]
        [ResponseType(typeof(PlayerDTO))]
        public IHttpActionResult FindPlayer(int id)
        {   
            
            Player Player = db.Players.Find(id);
            PlayerDTO PlayerDTO = new PlayerDTO()
            {
                PlayerId = Player.PlayerId,
                PlayerName = Player.PlayerName,
                PlayerPosition = Player.PlayerPosition,
                ShirtNumber = Player.ShirtNumber,
                TeamName = Player.Team.TeamName
            };
            if(Player == null)
            {
                return NotFound();
            }

            return Ok(PlayerDTO);
        }

        // Update Player

        /// <summary>
        /// Updates the details of a player.
        /// </summary>
        /// <returns>
        /// HEADER = 204 (Success, No content response)
        /// or
        /// HEADER = 400 (BAD REQUEST)
        /// or
        /// HEADER = 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// POST: api/PlayerData/UpdatePlayer/1
        /// FORM DATA: JSON Team object
        /// </example>
        [HttpPost]
        [ResponseType(typeof(void))]
        [Route("api/PlayerData/UpdatePlayer/{id}")]
        public IHttpActionResult UpdatePlayer(int id, Player player)
        {
            //Model State validation
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //player id validation
            if(id != player.PlayerId)
            {
                return BadRequest();
            }
            // Mark the 'player' entity as modified, so changes will be tracked and saved to the database.
            db.Entry(player).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(id))
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

        //Add Player
        /// <summary>
        /// Adds a new player with its details in the football players table.
        /// </summary>
        /// <returns>
        /// HEADER = 201 (CREATED)
        /// CONTENT: Player ID, Player Data
        /// or
        /// HEADER = 400 (BAD REQUEST)
        /// </returns>
        /// <example>
        /// POST: api/PlayerData/AddPlayer
        /// FORM DATA: Team JSON object
        /// </example>
        [HttpPost]
        [ResponseType(typeof(Player))]
        [Route("api/PlayerData/AddPlayer")]
        public IHttpActionResult AddPlayer(Player player)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            // Adds player to the database and saves it
            db.Players.Add(player);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { controller = "PlayerController", id = player.PlayerId }, player);
        }

        //Delete Player
        /// <summary>
        /// Deletes a player from the database by its id.
        /// </summary>
        /// <returns>
        /// HEADER: 200 (OK)
        /// or
        /// HEADER: 404 (NOT FOUND)
        /// </returns>
        /// <example>
        /// DELETE: api/PlayerData/DeletePlayer/5
        /// FORM DATA: (empty)
        /// </example>
        [ResponseType(typeof(Player))]
        [HttpPost]
        [Route("api/PlayerData/DeletePlayer/{id}")]
        //[Authorize(Roles = "Admin")] // Allow only admins to delete a player.
        public IHttpActionResult DeletePlayer(int id)
        {
            // Finds the player with the provided id in the database
            Player player = db.Players.Find(id);
            if (player == null)
            {
                return NotFound();
            }
            // Removes the player and saves the changes.
            db.Players.Remove(player);
            db.SaveChanges();

            return Ok();
        }



        // Check if a player with a specified id exists in the database.
        private bool PlayerExists(int id)
        {
            return db.Players.Count(p => p.PlayerId == id) > 0;
        }

    }
}
