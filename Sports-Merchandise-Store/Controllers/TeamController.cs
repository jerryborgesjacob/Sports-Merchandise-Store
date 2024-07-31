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
    public class TeamController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static TeamController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44347/api/");
        }

        // GET: Team/List
        //[Authorize] Commented out until admin and user authorization is created
        
        public ActionResult List()
        {
            string url = "TeamData/ListTeams";
            HttpResponseMessage response = client.GetAsync(url).Result;

            Debug.WriteLine(response.StatusCode);
            IEnumerable<TeamDTO> teams = response.Content.ReadAsAsync<IEnumerable<TeamDTO>>().Result;
            return View(teams);
        }

        // Get a specified team from the API and pass it to the view.
        //[Authorize]
         public ActionResult Details(int id)
        {
            DetailsTeam ViewModel = new DetailsTeam();
            string url = "TeamData/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            // Debugging
            //Debug.WriteLine("The response code is:" + response.StatusCode);

            TeamDTO SelectedTeam = response.Content.ReadAsAsync<TeamDTO>().Result;
            // Debugging
           //Debug.WriteLine("Team Received:" + SelectedTeam.TeamName);

            ViewModel.SelectedTeam = SelectedTeam;

            return View(ViewModel);  
        }

        public ActionResult Error()
        {
            return View();
        }

        // GET: Team/New
        //[Authorize(Roles = "Admin")]
        public ActionResult New()
        {
            return View();
        }

        // POST: Team/Create
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public ActionResult Create(Team team)
        {
            string url = "TeamData/AddTeam";
            string jsonpayload = jss.Serialize(team);
            // Debugging
            Debug.WriteLine(jsonpayload);

            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";

            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Team/Edit/2
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            string url = "TeamData/FindTeam/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TeamDTO selectedTeam = response.Content.ReadAsAsync<TeamDTO>().Result;

            return View(selectedTeam);
        }

        // POST: Team/Update/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, Team team)
        {
            string url = "TeamData/UpdateTeam/" + id;
            string jsonpayload = jss.Serialize(team);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            // Debugging
            Debug.WriteLine(content);
            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        //GET: Team/Delete/2
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "TeamData/FindTeam/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            TeamDTO selectedTeam = response.Content.ReadAsAsync<TeamDTO>().Result;
            return View(selectedTeam);
        }

        // POST: Team/Delete/2
        [HttpPost]
        [Authorize(Users = "Admin")]
        public ActionResult Delete(int id)
        {
            string url = "TeamData/DeleteTeam/" + id;
            HttpContent content = new StringContent("");
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;

            if(response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

    }
}