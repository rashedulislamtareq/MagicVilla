using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class VillaAPIController : ControllerBase
    {
        [HttpGet]
        public ActionResult<List<VillaDTO>> GetVillas()
        {
            return Ok(VillaStore.VillaList);
        }

        [HttpGet("{id}", Name = "GetVilla")]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            if (id == 0) { return BadRequest(); }
            var villa = VillaStore.VillaList.FirstOrDefault(x => x.Id == id);
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

            if (VillaStore.VillaList.FirstOrDefault(x => x.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("Custom", "Villa Already Exists");
                return BadRequest(ModelState);
            }

            if (villaDTO == null) { return BadRequest(villaDTO); }

            if (villaDTO.Id > 0) { return StatusCode(StatusCodes.Status500InternalServerError); }

            villaDTO.Id = VillaStore.VillaList.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;
            VillaStore.VillaList.Add(villaDTO);
            //return Ok(villaDTO);
            return CreatedAtRoute("GetVilla", new { Id = villaDTO.Id }, villaDTO);

        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        public ActionResult DeleteVilla(int id)
        {
            if (id == 0) return BadRequest();
            var data = VillaStore.VillaList.FirstOrDefault(x => x.Id == id);
            if (data == null) { return NotFound(); }

            VillaStore.VillaList.Remove(data);
            return NoContent();
        }

        [HttpPut("{id}", Name = "UpdateVilla")]
        public ActionResult UpdateVilla(int id, VillaDTO villaDTO)
        {
            if (villaDTO == null || id != villaDTO.Id)
            {
                return BadRequest(villaDTO);
            }

            var villa = VillaStore.VillaList.FirstOrDefault(x => x.Id == id);
            if (villa == null) { return NotFound(); }
            villa.Name = villaDTO.Name;
            villa.Sqft = villaDTO.Sqft;
            villa.Occupancy = villaDTO.Occupancy;

            return NoContent();

        }
    }
}
