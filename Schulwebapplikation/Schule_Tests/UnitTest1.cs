using NUnit.Framework;
using Schulwebapplikation.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Schule_Tests
{
    [TestFixture]
    public class SchuleTests
    {
        private Schule _schule;
        private Schueler _schueler1;
        private Schueler _schueler2;
        private Klassenraum _klassenraum;

        [SetUp]
        public void SetUp()
        {
            _schule = new Schule();
            _schueler1 = new Schueler("Anna", "10A", new DateTime(2005, 5, 15), "weiblich");
            _schueler2 = new Schueler("Ben", "10A", new DateTime(2006, 3, 10), "männlich");
            _klassenraum = new Klassenraum("R101", 50.5f, 30, true);
        }

        [Test]
        public void AddSchuelerToSchule_IncreasesSchuelerCount()
        {
            // Arrange
            int initialCount = _schule.AnzahlSchueler;

            // Act
            _schule.AddSchuelerToSchule(_schueler1);

            // Assert
            Assert.That(_schule.AnzahlSchueler, Is.EqualTo(initialCount + 1));
            Assert.That(_schule.SchuelerList.Contains(_schueler1), Is.True);
        }

        [Test]
        public void AddKlassenraumToSchule_IncreasesKlassenraumCount()
        {
            // Arrange
            int initialCount = _schule.AnzahlKlassenRaum;

            // Act
            _schule.AddKlassenraumToSchule(_klassenraum);

            // Assert
            Assert.That(_schule.AnzahlKlassenRaum, Is.EqualTo(initialCount + 1));
            Assert.That(_schule.KlassenraumList.Contains(_klassenraum), Is.True);
        }

        [Test]
        public void AnzahlRauemeCynap_ReturnsOnlyCynapRooms()
        {
            // Arrange
            var nonCynapRoom = new Klassenraum("R102", 40.0f, 25, false);
            _schule.AddKlassenraumToSchule(_klassenraum);
            _schule.AddKlassenraumToSchule(nonCynapRoom);

            // Act
            var cynapRooms = _schule.AnzahlRauemeCynap();

            // Assert
            Assert.That(cynapRooms.Count, Is.EqualTo(1));
            Assert.That(cynapRooms.All(r => r.HasCynap), Is.True);
        }

        [Test]
        public void AnzahlRauemeCynap_EmptyList_ReturnsEmptyList()
        {
            // Act
            var cynapRooms = _schule.AnzahlRauemeCynap();

            // Assert
            Assert.That(cynapRooms, Is.Empty);
        }

        [Test]
        public void DurchschnittsalterSchueler_ReturnsCorrectAverage()
        {
            // Arrange
            _schule.AddSchuelerToSchule(_schueler1); // Age ~20 (2025-2005)
            _schule.AddSchuelerToSchule(_schueler2); // Age ~19 (2025-2006)

            // Act
            float averageAge = _schule.DurchschnittsalterSchueler();

            // Assert
            Assert.That(averageAge, Is.EqualTo(19.5f).Within(0.1f));
        }

        [Test]
        public void DurchschnittsalterSchueler_EmptyList_ReturnsZero()
        {
            // Act
            float averageAge = _schule.DurchschnittsalterSchueler();

            // Assert
            Assert.That(averageAge, Is.EqualTo(0));
        }

        [Test]
        public void BerechneFrauenanteilInProzent_ReturnsCorrectPercentage()
        {
            // Arrange
            _schule.AddSchuelerToSchule(_schueler1); // Female
            _schule.AddSchuelerToSchule(_schueler2); // Male

            // Act
            double frauenAnteil = _schule.BerechneFrauenanteilInProzent(_schule.SchuelerList, "10A");

            // Assert
            Assert.That(frauenAnteil, Is.EqualTo(50.0).Within(0.1));
        }

        [Test]
        public void BerechneFrauenanteilInProzent_EmptyClass_ReturnsZero()
        {
            // Act
            double frauenAnteil = _schule.BerechneFrauenanteilInProzent(_schule.SchuelerList, "10B");

            // Assert
            Assert.That(frauenAnteil, Is.EqualTo(0));
        }

        [Test]
        public void BerechneFrauenanteilInProzent_NullList_ThrowsArgumentNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => _schule.BerechneFrauenanteilInProzent(null, "10A"));
        }

        [Test]
        public void KannKlasseUnterrichten_SufficientCapacity_ReturnsTrue()
        {
            // Arrange
            _schule.AddSchuelerToSchule(_schueler1);
            _schule.AddSchuelerToSchule(_schueler2);
            _schule.AddKlassenraumToSchule(_klassenraum);

            // Act
            bool canTeach = _schule.KannKlasseUnterrichten("10A", "R101");

            // Assert
            Assert.That(canTeach, Is.True);
        }

        [Test]
        public void KannKlasseUnterrichten_InsufficientCapacity_ReturnsFalse()
        {
            // Arrange
            _schule.AddSchuelerToSchule(_schueler1);
            _schule.AddSchuelerToSchule(_schueler2);
            var smallRoom = new Klassenraum("R103", 20.0f, 1, false);
            _schule.AddKlassenraumToSchule(smallRoom);

            // Act
            bool canTeach = _schule.KannKlasseUnterrichten("10A", "R103");

            // Assert
            Assert.That(canTeach, Is.False);
        }

        [Test]
        public void KannKlasseUnterrichten_NonExistentRoom_ReturnsFalse()
        {
            // Arrange
            _schule.AddSchuelerToSchule(_schueler1);

            // Act
            bool canTeach = _schule.KannKlasseUnterrichten("10A", "R999");

            // Assert
            Assert.That(canTeach, Is.False);
        }

        [Test]
        public void KannKlasseUnterrichten_EmptyClass_ReturnsTrue()
        {
            // Arrange
            _schule.AddKlassenraumToSchule(_klassenraum);

            // Act
            bool canTeach = _schule.KannKlasseUnterrichten("10B", "R101");

            // Assert
            Assert.That(canTeach, Is.True); // Empty class fits in any room
        }

        [Test]
        public void AnzahlSchuelerGeschlecht_ReturnsCorrectCountString()
        {
            // Arrange
            _schule.AddSchuelerToSchule(_schueler1); // Female
            _schule.AddSchuelerToSchule(_schueler2); // Male

            // Act
            string geschlechtCount = _schule.AnzahlSchuelerGeschlecht;

            // Assert
            Assert.That(geschlechtCount, Is.EqualTo("männliche: 1 / weibliche: 1"));
        }

        [Test]
        public void AnzahlSchuelerGeschlecht_EmptyList_ReturnsZeroCounts()
        {
            // Act
            string geschlechtCount = _schule.AnzahlSchuelerGeschlecht;

            // Assert
            Assert.That(geschlechtCount, Is.EqualTo("männliche: 0 / weibliche: 0"));
        }

        [Test]
        public void Person_Geschlecht_InvalidInput_SetsUnbekannt()
        {
            // Arrange
            var person = new Person(new DateTime(2000, 1, 1), "invalid");

            // Act
            string geschlecht = person.Geschlecht;

            // Assert
            Assert.That(geschlecht, Is.EqualTo("unbekannt"));
        }

        [Test]
        public void Person_Geschlecht_ValidInput_SetsCorrectly()
        {
            // Arrange
            var person = new Person(new DateTime(2000, 1, 1), "weiblich");

            // Act
            string geschlecht = person.Geschlecht;

            // Assert
            Assert.That(geschlecht, Is.EqualTo("weiblich"));
        }

        [Test]
        public void Person_Geschlecht_NullInput_SetsUnbekannt()
        {
            // Arrange
            var person = new Person(new DateTime(2000, 1, 1), null);

            // Act
            string geschlecht = person.Geschlecht;

            // Assert
            Assert.That(geschlecht, Is.EqualTo("unbekannt"));
        }

        [Test]
        public void Schueler_Alter_CalculatesCorrectly()
        {
            // Arrange
            var schueler = new Schueler("Test", "10A", new DateTime(2005, 5, 15), "männlich");

            // Act
            int alter = schueler.Alter;

            // Assert
            Assert.That(alter, Is.EqualTo(DateTime.Today.Year - 2005));
        }

        [Test]
        public void Schueler_Alter_BirthdayToday_ReturnsCorrectAge()
        {
            // Arrange
            var today = DateTime.Today;
            var schueler = new Schueler("Test", "10A", today.AddYears(-20), "männlich");

            // Act
            int alter = schueler.Alter;

            // Assert
            Assert.That(alter, Is.EqualTo(20));
        }
    }
}