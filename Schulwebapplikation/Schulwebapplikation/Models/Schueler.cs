namespace Schulwebapplikation.Models
{
    public class Schueler : Person
    {
        public string Name { get; set; } // Neue Eigenschaft für den Namen des Schülers
        public string Klasse { get; set; }
        public List<string> klassen = new List<string>();

        public void AddKlasse(string klasse)
        {
            if (!klassen.Contains(klasse))
            {
                klassen.Add(klasse);
            }
        }

        public int Alter
        {
            get
            {
                int alter = DateTime.Today.Year - Geburtstag.Year;
                return alter;
            }
            set { }
        }

        public void ZähleSchülerProKlasse(List<Schueler> schuelerListe)
        {
            foreach (Schueler schueler in schuelerListe)
            {
                if (!klassen.Contains(schueler.Klasse))
                {
                    klassen.Add(schueler.Klasse);
                }
            }
            foreach (string klasse in klassen)
            {
                int anzahl = 0;
                foreach (Schueler schueler in schuelerListe)
                {
                    if (schueler.Klasse == klasse)
                    {
                        anzahl++;
                    }
                }
                Console.WriteLine($"Klasse {klasse}: {anzahl} Schüler");
            }
        }

        public Schueler(string name, string klasse, DateTime geburtstag, string geschlecht) : base(geburtstag, geschlecht)
        {
            Name = name;
            Klasse = klasse;
            AddKlasse(klasse);
        }
    }
}