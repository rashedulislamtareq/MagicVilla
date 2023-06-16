using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MagicVilla_VillaAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class VillaAPIController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public VillaAPIController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            return Ok(_db.Villas.ToList());
        }

        [HttpGet("{id}", Name = "GetVilla")]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0) { return BadRequest(); }
            var villa = _db.Villas.FirstOrDefault(x => x.Id == id);
            if (villa == null) { return NotFound(); }
            return Ok(villa);
        }

        [HttpPost]
        public ActionResult CreateVilla(VillaDTO villaDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (_db.Villas.FirstOrDefault(x => x.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("Custom", "Villa Already Exists");
                return BadRequest(ModelState);
            }

            if (villaDTO == null) { return BadRequest(villaDTO); }

            if (villaDTO.Id > 0) { return StatusCode(StatusCodes.Status500InternalServerError); }
            Villa villa = new()
            {
                Id = villaDTO.Id,
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                Sqft = villaDTO.Sqft,
                Amenity = villaDTO.Amenity,
                ImageUrl = villaDTO.ImageUrl,
                CreationDate = DateTime.Now,
                Occupancy = villaDTO.Occupancy,
                Rate = villaDTO.Rate
            };
            _db.Villas.Add(villa);
            _db.SaveChanges();
            return CreatedAtRoute("GetVilla", new { Id = villaDTO.Id }, villaDTO);

        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        public ActionResult DeleteVilla(int id)
        {
            if (id == 0) return BadRequest();
            var data = _db.Villas.FirstOrDefault(x => x.Id == id);
            if (data == null) { return NotFound(); }

            _db.Villas.Remove(data);
            _db.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id}", Name = "UpdateVilla")]
        public ActionResult UpdateVilla(int id, VillaDTO villaDTO)
        {
            if (villaDTO == null || id != villaDTO.Id)
            {
                return BadRequest(villaDTO);
            }

            var villa = _db.Villas.AsNoTracking().FirstOrDefault(x => x.Id == id);
            if (villa == null) { return NotFound(); }

            villa.Name = villaDTO.Name;
            villa.Details = villaDTO.Details;
            villa.Sqft = villaDTO.Sqft;
            villa.Amenity = villaDTO.Amenity;
            villa.ImageUrl = villaDTO.ImageUrl;
            villa.CreationDate = DateTime.Now;
            villa.Occupancy = villaDTO.Occupancy;
            villa.Rate = villaDTO.Rate;

            _db.Villas.Update(villa);
            _db.SaveChanges();

            return NoContent();

        }
    }
}
