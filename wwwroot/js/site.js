'use strict';

$(() => {
    const currentUrl = window.location.href;

    const urlParts = currentUrl.split('/');
    const controller = urlParts[urlParts.length - 2];
    const action = urlParts[urlParts.length - 1];

    if (action === "ShowUsers" || controller === "Show") {
        $('.btn-close').hide();
    }
});

function getToken() {
    const tokenInput = $('input[name="__RequestVerificationToken"]');
    if (tokenInput.length > 0) {
        return tokenInput.val();
    } else {
        return null;
    }
}

$(document).on('click', "button.post-button", async e => {
    let button = $(e.target)
    let url = button.attr("formaction")
    let body = JSON.parse(button.attr("data-body"))
    let message = button.attr("data-message")

    Swal.fire({
        title: message,
        showDenyButton: false,
        showCancelButton: true,
        confirmButtonText: "Yes",
        cancelButtonText: `No`
    }).then(r => {
        if (r.isConfirmed) {
            fetch(url, {
                method: "post",
                headers: {
                    "XSRF-TOKEN": getToken(),
                    "Content-Type": "application/json"
                },
                body: JSON.stringify(body)
            }).then(r => r.json())
                .then(data => {
                    if (data.ok) {
                        Swal.fire("Success!", "", "success")
                            .then(() => window.location.reload())
                    }
                    else {
                        Swal.fire("Error!", data.Error, "error")
                    }
                })
        }
    })
})