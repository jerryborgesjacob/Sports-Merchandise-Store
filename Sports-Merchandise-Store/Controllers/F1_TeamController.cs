using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Sports_Merchandise_Store.Models;
using Sports_Merchandise_Store.Migrations;
using System.Diagnostics;

namespace Sports_Merchandise_Store.Controllers
{
    public class F1_TeamController : Controller
    {
        private static readonly HttpClient client;
        private JavaScriptSerializer jss = new JavaScriptSerializer();

        static F1_TeamController()
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("https://localhost:44320/api/");
        }
        // GET: F1_Team
        public ActionResult List()
        {
            //objective: communicate with our Team data API to retrieve a list of Teams
            //curl: https://localhost:44320/api/TeamData/ListTeams

            string url = "/TeamData/ListTeam";
            HttpResponseMessage response = client.GetAsync(url).Result;

            IEnumerable<F1_TeamDTO> F1teams = response.Content.ReadAsAsync<IEnumerable<F1_TeamDTO>>().Result;
            return View(F1teams);
        }

        // GET: F1_Team/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // POST: F1_Team/Create
        [HttpPost]
        public ActionResult Create(F1_TeamDTO F1team)
        {
            string url = "TeamData/AddF1Team";
            string jsonpayload = jss.Serialize(F1team);
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

        // POST: F1_Team/Create
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

        // GET: F1_Team/Edit/5
        public ActionResult Edit(int id)
        {
            //the existing team information
            string url = "TeamData/GetTeam/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            F1_TeamDTO F1SelectedTeam = response.Content.ReadAsAsync<F1_TeamDTO>().Result;

            return View(F1SelectedTeam);
        }

        // POST: F1_Team/Edit/5
        [HttpPost]
        public ActionResult Update(int id, F1_TeamDTO team)
        {
            string url = "TeamData/UpdateF1Team/" + id;
            string jsonpayload = jss.Serialize(team);
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

        // GET: F1_Team/Delete/5
        public ActionResult DeleteConfirm(int id)
        {
            string url = "TeamData/GetF1Team/" + id;
            HttpResponseMessage response = client.GetAsync(url).Result;
            F1_TeamDTO F1selectedTeam = response.Content.ReadAsAsync<F1_TeamDTO>().Result;
            return View(F1selectedTeam);
        }

        // POST: F1_Team/Delete/5
        [HttpPost]
        public ActionResult Delete(int id)
        {
            string url = "TeamData/DeleteF1Team/" + id;
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

        //upload image for F1 TEAMS
        [HttpPost]
        [Route("api/F1_TeamData/UploadImage")]
        public IHttpActionResult UploadImage(int id)
        {
            if (!Request.Content.IsMimeMultipartContent)
            {
                return BadRequest("Unsupported file");
             }
        var provider = MultipartFormDataStreamProvider(HttpContext.Current.Server.MapPath("~/App_Data"));
        var result = Request.Content.ReadAsMultipartAsync(provider).Result;

        var file = result.FileData.FirstOfDefault();
        if(file == null)
        {
            return BadRequest("No file uploaded.");
        }

        var filepath = file.LocalFileName;
        var filename = Path.GetFileName(filepath);
        var imageUrl = $"/Uploads/{filename}";

        return Ok(new { ImageUrl = imageUrl });
    }

    private object MultipartFormDataStreamProvider(object value)
    {
        throw new NotImplementedException();
    }
    }
}
