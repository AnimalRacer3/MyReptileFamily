function addReptile() {
    const name = document.getElementById('name').value;
    const species = document.getElementById('species').value;
    const morph = document.getElementById('morph').value;
    const birthdate = document.getElementById('birthdate').value;

    if (name && species) {
        const reptileDiv = document.createElement('div');
        reptileDiv.classList.add('reptile-card');
        reptileDiv.innerHTML = `<h3>${name}</h3><p>Species: ${species}</p><p>Morph: ${morph}</p><p>Birthdate: ${birthdate}</p>`;
        document.getElementById('family-tree').appendChild(reptileDiv);
    } else {
        alert('Please fill in all required fields');
    }
}