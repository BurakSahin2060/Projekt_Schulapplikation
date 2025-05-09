namespace Schulwebapplikation.Models
{
    public class Schueler : Person
    {
        public List<string> Klassen { get; set; } = new();

        public int Alter
        {
            get
            {
                int alter = DateTime.Today.Year - Geburtstag.Year;
                if (DateTime.Today < Geburtstag.AddYears(alter)) alter--;
                return alter;
            }
        }

        public Schueler(string klasse, DateTime geburtstag, string geschlecht)
            : base(geburtstag, geschlecht)
        {
            AddKlasse(klasse);
        }

        public void AddKlasse(string klasse)
        {
            if (!Klassen.Contains(klasse))
            {
                Klassen.Add(klasse);
            }
        }

        public string AktuelleKlasse => Klassen.LastOrDefault();

        public Dictionary<string, int> ZaehleSchuelerProKlasse(List<Schueler> schuelerListe)
        {
            Dictionary<string, int> ergebnis = new();
            foreach (var schueler in schuelerListe)
            {
                string klasse = schueler.AktuelleKlasse;
                if (!ergebnis.ContainsKey(klasse))
                    ergebnis[klasse] = 0;
                ergebnis[klasse]++;
            }
            return ergebnis;
        }
    }
}
