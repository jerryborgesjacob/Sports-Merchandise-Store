using Sports_Merchandise_Store.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace Sports_Merchandise_Store.Controllers
{
    public class F1MerchandiseController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static F1MerchandiseController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44347/api/");
        }
        // GET: F1Merchandise
        public ActionResult List()
        {
            string url = "F1MerchandiseData/ListF1Merchandise";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<F1MerchandiseDTO> F1Merch = response.Content.ReadAsAsync<IEnumerable<F1MerchandiseDTO>>().Result;
            return View(F1Merch);
        }
        // GET: F1Merchandise/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            string url = "F1MerchandiseData/FindF1Merchandise/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            F1MerchandiseDTO SelectedF1Merchandise = response.Content.ReadAsAsync<F1MerchandiseDTO>().Result;


            //ViewModel.SelectedSoccerMerchandise = SelectedSoccerMerchandise;

            return View(); //viewmodel
        }

        // GET: F1Merchandise/Create
        [Authorize(Roles = "Admin")]
        public ActionResult New()
        {

            return View();
        }

        // POST: F1Merchandise/Create
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Create(F1Merchandise F1Merchandise)
        {
            string url = "SoccerMerchandiseData/AddSoccerMerchandise";
            string jsonpayload = jss.Serialize(F1Merchandise);

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

        // GET: F1Merchandise/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            string url = "F1MerchandiseData/FindF1Merchandise/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            F1MerchandiseDTO SelectedF1Merchandise = response.Content.ReadAsAsync<F1MerchandiseDTO>().Result;
            return View(SelectedF1Merchandise);
        }
        // POST: F1Merchandise/Edit/5
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult Update(int id, F1Merchandise F1Merchandise)
        {
            string url = "F1MerchandiseData/UpdateF1Merchandise/" + id;
            string jsonpayload = jss.Serialize(F1Merchandise);
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
        // GET: F1Merchandise/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult DeleteConfirm(int id)
        {
            string url = "F1MerchandiseData/FindF1Merchandise/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;

            F1MerchandiseDTO SelectedF1Merchandise = response.Content.ReadAsAsync<F1MerchandiseDTO>().Result;
            return View(SelectedF1Merchandise);
        }
        // POST: F1Merchandise/Delete/5
        [HttpPost]
        [Authorize(Users = "Admin")]
        public ActionResult Delete(int id)
        {
            string url = "F1MerchandiseData/DeleteF1Merchandise/" + id;
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
    }
}
