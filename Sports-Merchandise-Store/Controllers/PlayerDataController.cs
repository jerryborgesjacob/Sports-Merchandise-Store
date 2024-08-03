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

    }
}
