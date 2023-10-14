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
using October142023.Models;
using Swashbuckle.Swagger;


namespace October142023.Controllers
{
    public class PeopleController : ApiController
    {
        private UACEntities db = new UACEntities();

        // GET: api/People
        public IQueryable<Person> GetPerson()
        {
            return db.Person;
        }

        // GET: api/People/5
        [ResponseType(typeof(Person))]
        public IHttpActionResult GetPerson(int id)
        {
            Person person = db.Person.Find(id);
            if (person == null)
            {
                return NotFound();
            }

            return Ok(person);
        }

        // PUT: api/People/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPerson(int id, Person person)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != person.Id)
            {
                return BadRequest();
            }

            db.Entry(person).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
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

        
        public HttpResponseMessage PostPerson(Person person)
        {
            if (!ModelState.IsValid)
            {
                var response = Request.CreateErrorResponse(HttpStatusCode.BadRequest, new Exception("Invalid Model"));
                return response;
            }

            db.Person.Add(person);
            db.SaveChanges();
            var okresponse = Request.CreateResponse<Person>(HttpStatusCode.OK, person);
            return okresponse;
        }

        // DELETE: api/People/5
        [ResponseType(typeof(Person))]
        public IHttpActionResult DeletePerson(int id)
        {
            Person person = db.Person.Find(id);
            if (person == null)
            {
                return NotFound();
            }

            db.Person.Remove(person);
            db.SaveChanges();

            return Ok(person);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PersonExists(int id)
        {
            return db.Person.Count(e => e.Id == id) > 0;
        }
    }
}