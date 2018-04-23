using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiPaises.Models;

namespace WebApiPaises.Controllers
{
    [Produces("application/json")]
    [Route("api/Pais/{PaisId}/Provincia")]
    public class ProvinciaController : Controller
    {
        private ApplicationDbContext context;

        public ProvinciaController(ApplicationDbContext contex)
        {
            this.context = contex;
        }

        [HttpGet]
        public IEnumerable<Provincia> GetAll(int paisId)
        {
            return context.Provincias.ToList().Where(p=>p.PaisId.Equals(paisId));
        }

        [HttpGet("{id}", Name = "provinciaById")]
        public IActionResult GetById(int id)
        {
            var provincia = context.Provincias.FirstOrDefault(p => p.Id == id);

            if (provincia == null)
            {
                return NotFound();
            }

            return new ObjectResult(provincia);
        }

        [HttpPost]
        public IActionResult Create([FromBody] Provincia provincia, int paisId)
        {
            provincia.PaisId = paisId;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            context.Provincias.Add(provincia);
            context.SaveChanges();

            return new CreatedAtRouteResult("provinciaById", new {id = provincia.Id}, provincia);
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromBody] Provincia provincia, int id)
        {
            if (provincia.Id != id)
            {
                return BadRequest();
            }

            context.Entry(provincia).State = EntityState.Modified;
            context.SaveChanges();

            return Ok();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var provincia = context.Provincias.FirstOrDefault(p => p.Id.Equals(id));

            if (provincia == null)
            {
                return NotFound();
            }

            context.Provincias.Remove(provincia);
            context.SaveChanges();
            return Ok(provincia);


        }
    }
}