using Schulwebapplikation.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace Schule_Tests
{
    [TestFixture]
    public class SchuleTests
    {
        private Schule schule;

        [SetUp]
        public void Setup()
        {
            schule = new Schule();
        }

        [Test]
        public void Person_Geschlecht_InvalidValue_SetsUnbekannt()
        {
            var person = new Person(new DateTime(2000, 1, 1), "invalid");
            Assert.AreEqual("unbekannt", person.Geschlecht);
        }

        [Test]
        public void Person_Geschlecht_ValidValue_SetsCorrectly()
        {
            var person = new Person(new DateTime(2000, 1, 1), "männlich");
            Assert.AreEqual("männlich", person.Geschlecht);
        }

        [Test]
        public void Schueler_Alter_Calculation_Correct()
        {
            var schueler = new Schueler("TestSchueler", "10A", new DateTime(2005, 1, 1), "männlich");
            int expectedAge = DateTime.Today.Year - 2005;
            Assert.AreEqual(expectedAge, schueler.Alter);
        }

        [Test]
        public void Schule_AddSchueler_IncreasesCount()
        {
            var schueler = new Schueler("TestSchueler", "10A", new DateTime(2005, 1, 1), "männlich");
            schule.AddSchuelerToSchule(schueler);
            Assert.AreEqual(1, schule.AnzahlSchueler);
        }

        [Test]
        public void Schule_AnzahlRauemeCynap_ReturnsOnlyCynapRooms()
        {
            var raum1 = new Klassenraum(50, 30, true);
            var raum2 = new Klassenraum(40, 25, false);
            schule.AddKlassenraumToSchule(raum1);
            schule.AddKlassenraumToSchule(raum2);

            var cynapRooms = schule.AnzahlRauemeCynap();
            Assert.AreEqual(1, cynapRooms.Count);
            Assert.IsTrue(cynapRooms[0].HasCynap);
        }

        [Test]
        public void Schule_DurchschnittsalterSchueler_CorrectCalculation()
        {
            schule.AddSchuelerToSchule(new Schueler("TestSchueler1", "10A", new DateTime(2005, 1, 1), "männlich"));
            schule.AddSchuelerToSchule(new Schueler("TestSchueler2", "10B", new DateTime(2007, 1, 1), "weiblich"));

            float expectedAverage = (float)((DateTime.Today.Year - 2005) + (DateTime.Today.Year - 2007)) / 2;
            Assert.AreEqual(expectedAverage, schule.DurchschnittsalterSchueler());
        }

        [Test]
        public void Schule_BerechneFrauenanteilInProzent_CorrectCalculation()
        {
            var schuelerList = new List<Schueler>
            {
                new Schueler("TestSchueler1", "10A", new DateTime(2005, 1, 1), "männlich"),
                new Schueler("TestSchueler2", "10A", new DateTime(2005, 1, 1), "weiblich"),
                new Schueler("TestSchueler3", "10A", new DateTime(2005, 1, 1), "weiblich")
            };

            double result = schule.BerechneFrauenanteilInProzent(schuelerList, "10A");
            Assert.AreEqual(66.66666666666666, result, 0.01);
        }

        [Test]
        public void Schule_KannKlasseUnterrichten_EnoughSpace_ReturnsTrue()
        {
            schule.AddSchuelerToSchule(new Schueler("TestSchueler1", "10A", new DateTime(2005, 1, 1), "männlich"));
            schule.AddSchuelerToSchule(new Schueler("TestSchueler2", "10A", new DateTime(2005, 1, 1), "weiblich"));
            schule.AddKlassenraumToSchule(new Klassenraum(50, 0, true)); // Plaetze ist irrelevant

            bool result = schule.KannKlasseUnterrichten("10A", "50");
            Assert.IsTrue(result); // 50 m² / 2 m² pro Schüler = 25 Schüler, 2 Schüler < 25
        }

        [Test]
        public void Schule_KannKlasseUnterrichten_NotEnoughSpace_ReturnsFalse()
        {
            schule.AddSchuelerToSchule(new Schueler("TestSchueler1", "10A", new DateTime(2005, 1, 1), "männlich"));
            schule.AddSchuelerToSchule(new Schueler("TestSchueler2", "10A", new DateTime(2005, 1, 1), "weiblich"));
            schule.AddKlassenraumToSchule(new Klassenraum(2, 0, true)); // Plaetze ist irrelevant

            bool result = schule.KannKlasseUnterrichten("10A", "2");
            Assert.IsFalse(result); // 2 m² / 2 m² pro Schüler = 1 Schüler, 2 Schüler > 1
        }

        [Test]
        public void Schule_AnzahlSchuelerGeschlecht_CorrectCount()
        {
            schule.AddSchuelerToSchule(new Schueler("TestSchueler1", "10A", new DateTime(2005, 1, 1), "männlich"));
            schule.AddSchuelerToSchule(new Schueler("TestSchueler2", "10B", new DateTime(2005, 1, 1), "weiblich"));

            string result = schule.AnzahlSchuelerGeschlecht;
            Assert.AreEqual("männliche: 1 / weibliche: 1", result);
        }
    }
}