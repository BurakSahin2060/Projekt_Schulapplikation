namespace Schulwebapplikation.Models
{
    public class Klassenraum
    {
        public string RaumName { get; set; }
        public float RaumInQm { get; set; }
        public int Plaetze { get; set; }
        public bool HatCynap { get; set; }

        public List<Schueler> SchuelerImRaum { get; set; } = new();

        public Klassenraum(string raumName, float raumInQm, int plaetze, bool hatCynap)
        {
            RaumName = raumName;
            RaumInQm = raumInQm;
            Plaetze = plaetze;
            HatCynap = hatCynap;
        }
    }
}
