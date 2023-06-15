using TP1.Models;

namespace newproject.Models
{
    public class Demandes
    {
        public Demande demande { get; set; }
     public IEnumerable<Demande> demandeslist { get; set; }
    }
}
