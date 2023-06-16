namespace MagicVilla_VillaAPI.Models
{
    public class Villa
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreationDate { get; set; }
        public int Sqft { get; set; }
        public int Occupancy { get; set; }
    }
}
