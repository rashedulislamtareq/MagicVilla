using AutoMapper;
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
        private readonly IMapper _mapper;

        public VillaAPIController(ApplicationDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            var villaList = await _db.Villas.ToListAsync();
            return Ok(_mapper.Map<List<VillaDTO>>(villaList));
        }

        [HttpGet("{id}", Name = "GetVilla")]
        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            if (id == 0) { return BadRequest(); }
            var villa = await _db.Villas.FirstOrDefaultAsync(x => x.Id == id);
            if (villa == null) { return NotFound(); }

            var villaDto = _mapper.Map<VillaDTO>(villa);
            return Ok(villaDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateVilla(VillaCreateDTO villaDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (await _db.Villas.FirstOrDefaultAsync(x => x.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("Custom", "Villa Already Exists");
                return BadRequest(ModelState);
            }

            if (villaDTO == null) { return BadRequest(villaDTO); }

            var villa = _mapper.Map<Villa>(villaDTO);
            villa.CreationDate = DateTime.Now;

            await _db.Villas.AddAsync(villa);
            await _db.SaveChangesAsync();
            return CreatedAtRoute("GetVilla", new { Id = villa.Id }, villa);

        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        public async Task<ActionResult> DeleteVilla(int id)
        {
            if (id == 0) return BadRequest();
            var data = await _db.Villas.FirstOrDefaultAsync(x => x.Id == id);
            if (data == null) { return NotFound(); }

            await Task.Run(() => _db.Villas.Remove(data));
            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("{id}", Name = "UpdateVilla")]
        public async Task<ActionResult> UpdateVilla(int id, [FromBody] VillaUpdateDTO villaDTO)
        {
            if (villaDTO == null || id != villaDTO.Id)
            {
                return BadRequest(villaDTO);
            }

            //var villa = await _db.Villas.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
            var villa = _mapper.Map<Villa>(villaDTO);
            if (villa == null) { return NotFound(); }

            villa.UpdateDate = DateTime.Now;

            await Task.Run(() => _db.Villas.Update(villa));
            await _db.SaveChangesAsync();

            return NoContent();

        }
    }
}
