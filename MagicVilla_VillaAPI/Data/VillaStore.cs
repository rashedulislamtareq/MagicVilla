﻿using MagicVilla_VillaAPI.Models.DTO;

namespace MagicVilla_VillaAPI.Data
{
    public static class VillaStore
    {
        public static List<VillaDTO> VillaList = new List<VillaDTO>()
            {
                new VillaDTO{Id=1, Name="Villa 01"},
                new VillaDTO{Id=2, Name="Villa 02"}
            };
    }
}
