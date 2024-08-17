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
using Sports_Merchandise_Store.Models;

namespace Sports_Merchandise_Store.Controllers
{
    public class DriverDataController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        /// <summary>
        /// Returns all Drivers in the table in the database.
        /// </summary>
        /// <returns>
        /// List of drivers and their details in the table.
        /// </returns>
        /// <example>
        /// // GET: api/DriverData/ListDrivers => Data of drivers in the table
        /// </example>
        [Route("api/DriverData/ListDrivers")]
        public IEnumerable<DriverDTO> ListDrivers()
        {
            List<Driver> Drivers = db.Drivers.ToList();
            List<DriverDTO> DriverDTOs = new List<DriverDTO>();

            Drivers.ForEach(a => DriverDTOs.Add(new DriverDTO()
            {
                DriverId = a.DriverId,
                FirstName = a.FirstName,
                LastName = a.LastName,
                F1TeamId = a.F1TeamId
            }));

            return DriverDTOs;
        }

        /// <summary>
        /// Returns the driver details with the specified DriverId
        /// </summary>
        /// <param name="id">DriverId of the Driver</param>
        /// <returns>
        /// HEADER: 200 (Status Code for OK)
        /// </returns>
        /// <example>
        /// // GET: api/DriverData/5 => Data of driver with DriverId 5
        /// </example>
        [ResponseType(typeof(Driver))]
        public IHttpActionResult GetDriver(int id)
        {
            Driver driver = db.Drivers.Find(id);
            if (driver == null)
            {
                return NotFound();
            }

            return Ok(driver);
        }

        /// <summary>
        /// Updates the Driver details of a particular driver with the POST data input
        /// </summary>
        /// <param name="id">The DriverId in the table (primary key)</param>
        /// <param name="driver">JSON Form Data of a Driver</param>
        /// <returns>
        /// Status Code 
        /// HEADER: 200 = Success
        /// or
        /// HEADER: 400 = Bad Request
        /// or
        /// HEADER: 404 = Not Found
        /// </returns>
        /// <example>
        /// POST: api/DriverData/UpdateDriver/5
        /// FORM Data: Driver JSON Object
        /// </example>
        [ResponseType(typeof(void))]
        [HttpPost]
        public IHttpActionResult UpdateDriver(int id, Driver driver)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != driver.DriverId)
            {
                return BadRequest();
            }

            db.Entry(driver).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DriverExists(id))
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

        /// <summary>
        /// Add New Driver details onto the Driver table in the Database
        /// </summary>
        /// <param name="driver">JSON Form Data of the Driver</param>
        /// Status Code 
        /// HEADER: 200 = Success
        /// or
        /// HEADER: 400 = Bad Request
        /// or
        /// HEADER: 404 = Not Found
        /// <example>
        /// POST: api/DriverData/AddDriver
        /// FORM Data: Driver JSON Object
        /// </example>

        [ResponseType(typeof(Driver))]
        [HttpPost]
        public IHttpActionResult AddDriver(Driver driver)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Drivers.Add(driver);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = driver.DriverId }, driver);
        }

        /// <summary>
        /// Deletes Driver data from the database with DriverId.
        /// </summary>
        /// <param name="id">The primary key of the Driver</param>
        /// <returns>
        /// HEADER: 200 = OK
        /// or
        /// HEADER: 404 = Not Found
        /// </returns>
        /// <example>
        /// POST: api/DriverData/DeleteDriver/5
        /// FORM DATA: (empty)
        /// </example>
        
        [ResponseType(typeof(Driver))]
        [HttpPost]
        public IHttpActionResult DeleteDriver(int id)
        {
            Driver driver = db.Drivers.Find(id);
            if (driver == null)
            {
                return NotFound();
            }

            db.Drivers.Remove(driver);
            db.SaveChanges();

            return Ok(driver);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DriverExists(int id)
        {
            return db.Drivers.Count(e => e.DriverId == id) > 0;
        }
    }
}
