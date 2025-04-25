document.getElementById('schuelerForm').addEventListener('submit', async (e) => {
    e.preventDefault();
    const schueler = {
        klassen: [document.getElementById('klasse').value],
        geburtstag: document.getElementById('geburtstag').value,
        geschlecht: document.getElementById('geschlecht').value
    };

    await fetch('https://localhost:5001/api/Schueler', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(schueler)
    });

    loadSchueler();
});

async function loadSchueler() {
    const response = await fetch('https://localhost:5001/api/Schueler');
    const schueler = await response.json();
    const tbody = document.querySelector('#schuelerTable tbody');
    tbody.innerHTML = '';
    schueler.forEach(s => {
        const row = `<tr>
            <td>${s.id}</td>
            <td>${s.aktuelleKlasse}</td>
            <td>${s.alter}</td>
            <td>${s.geschlecht}</td>
            <td><button onclick="deleteSchueler(${s.id})">Löschen</button></td>
        </tr>`;
        tbody.innerHTML += row;
    });
}

async function deleteSchueler(id) {
    await fetch(`https://localhost:5001/api/Schueler/${id}`, {
        method: 'DELETE'
    });
    loadSchueler();
}

loadSchueler();