﻿document.addEventListener("DOMContentLoaded", () => {
    const baseUrl = "http://localhost:5287";

    // Schüler hinzufügen
    document.getElementById("addStudentForm").addEventListener("submit", async (event) => {
        event.preventDefault();
        const name = document.getElementById("name").value;
        const geburtstag = document.getElementById("geburtstag").value;
        const geschlecht = document.getElementById("geschlecht").value;
        const klasse = document.getElementById("klasse").value;

        try {
            const response = await fetch(`${baseUrl}/api/schule/addSchueler`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    name,
                    geburtstag: new Date(geburtstag).toISOString(),
                    geschlecht,
                    klasse
                })
            });

            const result = await response.json();
            if (!response.ok) {
                throw new Error(result || "Fehler beim Hinzufügen des Schülers.");
            }
            document.getElementById("output").textContent = result;
        } catch (error) {
            document.getElementById("output").textContent = `Fehler: ${error.message}`;
        }
    });

    // Klassenraum hinzufügen
    document.getElementById("addRoomForm").addEventListener("submit", async (event) => {
        event.preventDefault();
        const raumName = document.getElementById("raumName").value;
        const raumGroesse = parseFloat(document.getElementById("raumGroesse").value);
        const raumPlaetze = parseInt(document.getElementById("raumPlaetze").value);
        const raumCynap = document.getElementById("raumCynap").checked;

        try {
            const response = await fetch(`${baseUrl}/api/schule/addKlassenraum`, {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify({
                    name: raumName,
                    raumInQm: raumGroesse,
                    plaetze: raumPlaetze,
                    hasCynap: raumCynap
                })
            });

            const result = await response.json();
            if (!response.ok) {
                throw new Error(result || "Fehler beim Hinzufügen des Klassenraums.");
            }
            document.getElementById("output").textContent = result;
        } catch (error) {
            document.getElementById("output").textContent = `Fehler: ${error.message}`;
        }
    });

    // Alle Schüler abrufen
    document.getElementById("getAllStudents").addEventListener("click", async () => {
        try {
            const response = await fetch(`${baseUrl}/api/schule/getAllSchueler`);
            if (!response.ok) {
                throw new Error("Fehler beim Abrufen der Schüler.");
            }
            const students = await response.json();
            const list = document.getElementById("studentsList");
            list.innerHTML = students.map(s => {
                const birthDate = new Date(s.geburtstag).toLocaleDateString("de-DE");
                return `<li>${s.name} - ${s.klasse} - ${s.geschlecht} - Geburtstag: ${birthDate}</li>`;
            }).join("");
        } catch (error) {
            document.getElementById("output").textContent = `Fehler: ${error.message}`;
        }
    });

    // Schüler einer Klasse abrufen
    document.getElementById("getStudentsByClass").addEventListener("click", async () => {
        const klasse = document.getElementById("classFilter").value.trim();
        if (!klasse) {
            document.getElementById("output").textContent = "Bitte eine Klasse eingeben.";
            return;
        }
        try {
            const response = await fetch(`${baseUrl}/api/schule/getSchuelerByKlasse/${klasse}`);
            if (!response.ok) {
                throw new Error("Fehler beim Abrufen der Schüler.");
            }
            const students = await response.json();
            const list = document.getElementById("classStudentsList");
            list.innerHTML = students.map(s => {
                const birthDate = new Date(s.geburtstag).toLocaleDateString("de-DE");
                return `<li>${s.name} - ${s.klasse} - ${s.geschlecht} - Geburtstag: ${birthDate}</li>`;
            }).join("");
        } catch (error) {
            document.getElementById("output").textContent = `Fehler: ${error.message}`;
        }
    });

    // Klassenraum prüfen
    document.getElementById("checkRoom").addEventListener("click", async () => {
        const klasse = document.getElementById("checkClass").value.trim();
        const raumName = document.getElementById("checkRoomName").value.trim();
        if (!klasse || !raumName) {
            document.getElementById("output").textContent = "Bitte Klasse und Raumname eingeben.";
            return;
        }
        try {
            const response = await fetch(`${baseUrl}/api/schule/kannUnterrichten/${klasse}/${raumName}`);
            if (!response.ok) {
                throw new Error("Fehler bei der Prüfung.");
            }
            const result = await response.json();
            document.getElementById("roomCheckResult").textContent = result;
        } catch (error) {
            document.getElementById("output").textContent = `Fehler: ${error.message}`;
        }
    });

    // Durchschnittsalter abrufen
    document.getElementById("getAverageAge").addEventListener("click", async () => {
        try {
            const response = await fetch(`${baseUrl}/api/schule/durchschnittsalter`);
            if (!response.ok) {
                throw new Error("Fehler beim Abrufen des Durchschnittsalters.");
            }
            const result = await response.json();
            document.getElementById("statsOutput").textContent = `Durchschnittsalter: ${result.toFixed(2)} Jahre`;
        } catch (error) {
            document.getElementById("output").textContent = `Fehler: ${error.message}`;
        }
    });

    // Frauenanteil abrufen
    document.getElementById("getFemalePercentage").addEventListener("click", async () => {
        const klasse = document.getElementById("frauenKlasse").value.trim();
        if (!klasse) {
            document.getElementById("output").textContent = "Bitte eine Klasse eingeben.";
            return;
        }
        try {
            const response = await fetch(`${baseUrl}/api/schule/frauenanteil/${klasse}`);
            if (!response.ok) {
                throw new Error("Fehler beim Abrufen des Frauenanteils.");
            }
            const result = await response.json();
            document.getElementById("statsOutput").textContent = `Frauenanteil in Klasse ${klasse}: ${result.toFixed(2)}%`;
        } catch (error) {
            document.getElementById("output").textContent = `Fehler: ${error.message}`;
        }
    });
});