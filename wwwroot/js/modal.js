'use strict';

let myModal = new bootstrap.Modal(document.getElementById('my-modal'), {
    keyboard: false
})

function loadModalContent(url) {
    fetch(url).then(r => r.text()).then(html => {
        setModalContent(html)
    })
}

function setModalContent(html) {
    let modalContent = $("#my-modal .modal-dialog")
    modalContent.empty()
    modalContent.html(html)
    myModal.show()
}

$(document).on('click', "a.open-in-modal", (e) => {
    e.preventDefault()
    let el = $(e.target)
    let url = el.attr("href")
    loadModalContent(url)
})

$(document).on('click', "button.open-in-modal", (e) => {
    let el = $(e.target)
    let url = el.attr("formaction")
    loadModalContent(url)
})

$(document).on('submit', "form.ajax-form", async e => {
    e.preventDefault()

    let url = $(e.target).attr('action')
    let data = $(e.target).serializeArray()


    let formData = new FormData()

    $(e.target).find("input[type=file]").each((i,el) => {
        console.log(el)
        formData.append($(el).attr('name'), el.files[0])
    })

    data.forEach(x => {
        formData.append(x.name, x.value)
    })

    await fetch(url, {
        method: "POST",
        body: formData
    }).then(r => {
        if (r.redirected) {
            window.location.reload()
        } else {
            return r.text()
        }
    }).then(html => {
        setModalContent(html)

        let mapFromData = $('#umap').data('leafletMap');

        if (mapFromData === undefined) {
            mapFromData = L.map('umap').setView([50.4509558, 30.5174125], 13);

            L.tileLayer('https://tile.openstreetmap.org/{z}/{x}/{y}.png', {
                maxZoom: 19,
                attribution: '&copy; <a href="http://www.openstreetmap.org/copyright">OpenStreetMap</a>'
            }).addTo(mapFromData);

            $('#umap').data('leafletMap', mapFromData);
            mapFromData.on('click', onMapClick);
        }
    })
})