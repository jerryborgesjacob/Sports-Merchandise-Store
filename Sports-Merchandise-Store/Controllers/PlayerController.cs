using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Net.Http;
using System.Diagnostics;
using System.Security.Policy;
using Sports_Merchandise_Store.Models;
using Sports_Merchandise_Store.Models.ViewModels;
using Sports_Merchandise_Store.Migrations;

namespace Sports_Merchandise_Store.Controllers
{
    public class PlayerController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static PlayerController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44347/api/");
        }
        // GET: Player/List
        public ActionResult List()
        {
            string url = "PlayerData/ListPlayers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<PlayerDTO> players = response.Content.ReadAsAsync<IEnumerable<PlayerDTO>>().Result;
            return View(players);
        }

        // Get a specified player from the API and pass it to the view.
        [Authorize]
        public ActionResult Details(int id)
        {
            DetailsPlayer ViewModel = new DetailsPlayer();
            string url = "PlayerData/FindPlayer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            PlayerDTO SelectedPlayer = response.Content.ReadAsAsync<PlayerDTO>().Result;
            // Debugging
            //Debug.WriteLine("Player Received:" + SelectedPlayer.TeamPlayer);

            ViewModel.SelectedPlayer = SelectedPlayer;

            return View(ViewModel);
        }

        // GET: Player/New
        [Authorize(Roles = "Admin")]
        public ActionResult New()
        {
            // Fetch the team list to make it available for selection
            string url = "TeamData/ListTeams";
            HttpResponseMessage response = client.GetAsync(url).Result;
            IEnumerable<TeamDTO> teams = response.Content.ReadAsAsync<IEnumerable<TeamDTO>>().Result;
            // Make teams available for the dropdown menu in the view
            ViewBag.Teams = new SelectList(teams, "TeamId", "TeamName");
            return View();
        }

        // POST: Player/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(Player player)
        {
            string url = "PlayerData/AddPlayer";
            string jsonpayload = jss.Serialize(player);
            // Debugging
            //Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response.StatusCode);

            // Debugging
            //string responseContent = response.Content.ReadAsStringAsync().Result;
            //Debug.WriteLine(responseContent);


            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Player/Edit/2
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            string url = "PlayerData/FindPlayer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            PlayerDTO SelectedPlayer = response.Content.ReadAsAsync<PlayerDTO>().Result;

            // Fetch the team list to make it available for selection
            string TeamUrl = "TeamData/ListTeams";
            HttpResponseMessage TeamResponse = client.GetAsync(TeamUrl).Result;
            IEnumerable<TeamDTO> teams = TeamResponse.Content.ReadAsAsync<IEnumerable<TeamDTO>>().Result;

            // Make teams available for the dropdown menu in the view
            ViewBag.Teams = new SelectList(teams, "TeamId", "TeamName", SelectedPlayer.TeamName);

            return View(SelectedPlayer);
        }

        // POST: Player/Update/2
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, Player player)
        {
            string url = "PlayerData/UpdatePlayer/" + id;
            string jsonpayload = jss.Serialize(player);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            // Debugging
            //Debug.WriteLine("Response is: " + response);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        //GET: Player/Delete/2
        [Authorize(Roles = "Admin")]

        public ActionResult DeleteConfirm(int id)
        {
            string url = "PlayerData/FindPlayer/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            //Debug.WriteLine("Response is: " + response);

            PlayerDTO SelectedPlayer = response.Content.ReadAsAsync<PlayerDTO>().Result;
            return View(SelectedPlayer);
        }

        // POST: Player/Delete/2
        //[HttpPost]
        [Authorize(Users = "Admin")]
        public ActionResult Delete(int id)
        {
            string url = "PlayerData/DeletePlayer/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}