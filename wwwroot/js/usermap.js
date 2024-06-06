'use strict';

let map;

$(document).on('shown.bs.modal', function (e) {

    console.log(e)

    let mapFromData = $('#umap').data('leafletMap');

    if (mapFromData === undefined) {
        mapFromData = L.map('umap').setView([50.4509558, 30.5174125], 13);

        L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
            maxZoom: 19,
            attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
        }).addTo(mapFromData);

        $('#umap').data('leafletMap', mapFromData);
    }

    if ($(e.relatedTarget).hasClass('edit')) {
        console.log(true)
        //function loadMarkers() {
        //    fetch("/api/map/markers")
        //        .then(r => r.json())
        //        .then(list => {
        //            //TODO
        //            list.forEach(i => addToMap(i))
        //        });
        //};

        //    loadMarkers();
    }

    mapFromData.on('click', onMapClick);
    map = mapFromData;
});

function addToMap(markerData) {
    let marker = L.marker([markerData.Lat, markerData.Lng]).addTo(map);
}

async function onMapClick(e) {

    Swal.fire({
        title: "Маркер",
        text: "Створено маркер",
        icon: "success",
    }).then((result) => {
        if (result.isConfirmed) {

            let marker =
            {
                Lat: e.latlng.lat,
                Lng: e.latlng.lng,
            }

            $("input.lat").val(e.latlng.lat);
            $("input.lng").val(e.latlng.lng);

            addToMap(marker);
        }
    });
}


function RemoveFromMap(markerId) {
    let marker = markers.find(x => x.id === markerId);
    markers = markers.filter(x => x.id !== markerId)
    map.removeLayer(marker);
    map.invalidateSize();
}

$(document).on('click', 'button.delete-marker', e => {
    let id = Number($(e.target).data('id'));

    fetch("/api/map/marker/", {
        method: "DELETE",
        headers: {
            "Content-Type": "application/json"
        },
        body: JSON.stringify({ id: id })
    }).then(r => r.json()).then(response => {
        if (response.ok) {
            Swal.fire("Success", "Delete marker", "success");
            //RemoveFromMap(id);
        } else {
            Swal.fire("Error", "Not delete", "error");
        }
    });
});