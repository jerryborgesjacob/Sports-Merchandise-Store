using Sports_Merchandise_Store.Models;
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
    public class DriverController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static DriverController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44320/api/");
        }

        // GET: Driver
        public ActionResult List()
        {
            //objective: communicate with our Driver data API to retrieve a list of Drivers
            //curl: https://localhost:44320/api/DriverData/ListDrivers

            string url = "DriverData/ListDrivers";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<DriverDTO> drivers = response.Content.ReadAsAsync<IEnumerable<DriverDTO>>().Result;
            return View(drivers);
        }

        // GET: Driver/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }
        public ActionResult Error()
        {
            return View();
        }

        public ActionResult New()
        {
            return View();
        }

        // GET: Driver/Create
        [HttpPost]
        public ActionResult Create(Driver driver)
        {
            //objective: Add a new Driver into the Database using the API
            //curl -H "Content-Type:application/json" -d @Driver.json https://localhost:44324/api/DriverData/AddDriver

            string url = "DriverData/AddDriver";
            string jsonpayload = jss.Serialize(driver);
            Debug.WriteLine(jsonpayload);

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

        // POST: Driver/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Driver/Edit/5
        public ActionResult Edit(int id)
        {
            //the existing driver information
            string url = "DriverData/GetDriver/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DriverDTO SelectedDriver = response.Content.ReadAsAsync<DriverDTO>().Result;

            return View(SelectedDriver);
        }

        // POST: Driver/Edit/5
        [HttpPost]
        public ActionResult Update(int id, DriverDTO driver)
        {
            string url = "DriverData/UpdateDriver/" + id;
            string jsonpayload = jss.Serialize(driver);
            HttpContent content = new StringContent(jsonpayload);
            content.Headers.ContentType.MediaType = "application/json";
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            Debug.WriteLine(content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("List");
            }
            else
            {
                return RedirectToAction("Error");
            }
        }

        // GET: Driver/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "DriverData/GetDriver/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            DriverDTO selectedDriver = response.Content.ReadAsAsync<DriverDTO>().Result;
            return View(selectedDriver);
        }

        // POST: Driver/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "DriverData/DeleteDriver/" + id;
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
