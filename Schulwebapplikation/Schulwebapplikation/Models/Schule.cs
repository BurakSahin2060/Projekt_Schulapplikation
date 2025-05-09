using System.Linq;

namespace Schulwebapplikation.Models
{
    public class Schule
    {
        public List<Schueler> SchuelerListe { get; set; } = new();
        public List<Klassenraum> KlassenraumListe { get; set; } = new();

        public void AddSchueler(Schueler schueler)
        {
            SchuelerListe.Add(schueler);
        }

        public void AddKlassenraum(Klassenraum klassenraum)
        {
            KlassenraumListe.Add(klassenraum);
        }

        public int AnzahlSchueler => SchuelerListe.Count;

        public int AnzahlKlassenraeume => KlassenraumListe.Count;

        public List<Klassenraum> RaeumeMitCynap()
        {
            return KlassenraumListe.Where(r => r.HatCynap).ToList();
        }

        public double Durchschnittsalter()
        {
            if (AnzahlSchueler == 0) return 0;
            return SchuelerListe.Average(s => s.Alter);
        }

        public double FrauenanteilInProzent(string klasse)
        {
            var schuelerDerKlasse = SchuelerListe.Where(s => s.AktuelleKlasse == klasse).ToList();
            if (!schuelerDerKlasse.Any()) return 0;

            int anzahlFrauen = schuelerDerKlasse.Count(s => s.Geschlecht == "weiblich");
            return (double)anzahlFrauen / schuelerDerKlasse.Count * 100;
        }

        public bool KannKlasseUnterrichten(string klasse, string raumName)
        {
            int anzahlSchueler = SchuelerListe.Count(s => s.AktuelleKlasse == klasse);
            Klassenraum? raum = KlassenraumListe.FirstOrDefault(r => r.RaumName == raumName);

            return raum != null && raum.Plaetze >= anzahlSchueler;
        }

        public string AnzahlSchuelerNachGeschlecht
        {
            get
            {
                int maennlich = SchuelerListe.Count(s => s.Geschlecht == "männlich");
                int weiblich = SchuelerListe.Count(s => s.Geschlecht == "weiblich");
                return $"männliche: {maennlich} / weibliche: {weiblich}";
            }
        }

        public int AnzahlKlassenProSchueler(Schueler schueler)
        {
            return schueler.Klassen.Count;
        }
    }
}
