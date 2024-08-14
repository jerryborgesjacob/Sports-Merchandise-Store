using Sports_Merchandise_Store.Models;
using Sports_Merchandise_Store.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Sports_Merchandise_Store.Controllers
{
    public class SoccerMerchandiseController : Controller
    {

        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static SoccerMerchandiseController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44347/api/");
        }
        // GET:SoccerMerchandise/List
        public ActionResult List()
        {
            string url = "SoccerMerchandiseData/ListSoccerMerchandise";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<SoccerMerchandiseDTO> SoccerMerch = response.Content.ReadAsAsync<IEnumerable<SoccerMerchandiseDTO>>().Result;
            return View(SoccerMerch);
        }

        // Get specified soccer merch from the API and pass it to the view.
        //[Authorize]
        public ActionResult Details(int id)
        {
            //DetailsSoccerMerchandise ViewModel = new DetailsSoccerMerchandise();
            string url = "SoccerMerchandiseData/FindSoccerMerchandise/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            SoccerMerchandiseDTO SelectedSoccerMerchandise = response.Content.ReadAsAsync<SoccerMerchandiseDTO>().Result;
            

            //ViewModel.SelectedSoccerMerchandise = SelectedSoccerMerchandise;

            return View(); //viewmodel
        }

        // GET: SoccerMerchandise/New
        //[Authorize(Roles = "Admin")]
        public ActionResult New()
        {
            
            return View();
        }

        // POST: SoccerMerchandise/Create
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public ActionResult Create(SoccerMerchandise SoccerMerchandise)
        {
            string url = "SoccerMerchandiseData/AddSoccerMerchandise";
            string jsonpayload = jss.Serialize(SoccerMerchandise);

            HttpContent content = new StringContent(jsonpayload);
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

        // GET: SoccerMerchandise/Edit/2
        //[Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            string url = "SoccerMerchandiseData/FindSoccerMerchandise/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            SoccerMerchandiseDTO SelectedSoccerMerchandise = response.Content.ReadAsAsync<SoccerMerchandiseDTO>().Result;
            return View(SelectedSoccerMerchandise);
        }

        // POST: SoccerMerchandise/Update/2
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        public ActionResult Update(int id, SoccerMerchandise SoccerMerchandise)
        {
            string url = "SoccerMerchandiseData/UpdateSoccerMerchandise/" + id;
            string jsonpayload = jss.Serialize(SoccerMerchandise);
            HttpContent content = new StringContent(jsonpayload);
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

        //GET: SoccerMerchandise/Delete/2
        //[Authorize(Roles = "Admin")]

        public ActionResult DeleteConfirm(int id)
        {
            string url = "SoccerMerchandiseData/FindSoccerMerchandise/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            SoccerMerchandiseDTO SelectedSoccerMerchandise = response.Content.ReadAsAsync<SoccerMerchandiseDTO>().Result;
            return View(SelectedSoccerMerchandise);
        }

        // POST: SoccerMerchandise/Delete/2
        //[HttpPost]
        //[Authorize(Users = "Admin")]
        public ActionResult Delete(int id)
        {
            string url = "SoccerMerchandiseData/DeleteSoccerMerchandise/" + id;
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