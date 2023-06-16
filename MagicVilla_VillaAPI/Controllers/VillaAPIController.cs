using MagicVilla_VillaAPI.Data;
using MagicVilla_VillaAPI.Models;
using MagicVilla_VillaAPI.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_VillaAPI.Controllers
{
    [ApiController]
    [Route("api/[Controller]")]
    public class VillaAPIController : ControllerBase
    {
        [HttpGet]
        public List<VillaDTO> GetVillas()
        {
            return VillaStore.VillaList;
        }

        [HttpGet("{id}")]
        public VillaDTO GetVilla(int id)
        {
            return VillaStore.VillaList.FirstOrDefault(x => x.Id == id);
        }
    }
}
