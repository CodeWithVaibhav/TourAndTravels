namespace TourAndTravels.Models
{
    public class CommonInputModel : Paging
    {
        public string SearchKey { get; set; }
        public string LDAPPath { get; set; }
        public bool Freshload { get; set; }
        
    }
}
