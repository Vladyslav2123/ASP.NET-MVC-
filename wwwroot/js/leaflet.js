'use strict';

let markers = [];

let map = L.map('map').setView([50.4509558, 30.5174125], 13);

L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
    maxZoom: 19,
    attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
}).addTo(map);

function addToMap(markerData) {
    let marker = L.marker([markerData.lat, markerData.lng])
        .bindPopup(`<h2>${markerData.title}</h2>`)
        .addTo(map);

    marker.id = markerData.id;
    markers.push(marker);
}

function loadMarkers() {
    fetch("/api/map/markers")
        .then(r => r.json())
        .then(list => {
            list.forEach(i => addToMap(i))
        });
};

loadMarkers();