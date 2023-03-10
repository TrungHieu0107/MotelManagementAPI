

$(document).ready(function () {



});

function showConfirm(body, func) {
    let confirm = '<div id="confirm-modal" class="modal fade">' +
        '<div class="modal-dialog">' +
        '<div class="modal-content">' +
        '<div class="modal-header">' +
        '<h5 class="modal-title">Nhắc nhở</h5>' +
        '<button type="button" class="close" data-dismiss="modal" aria-label="Close">' +
        '<span aria-hidden="true">&times;</span>' +
        '</button>' +
        '</div>' +
        '<form id="delete-object-form">' +
        '<div class="modal-body">' +
        '<div id="confirm-body">' +
        '</div>' +
        '<div class="modal-footer">' +
        '<button type="button" class="btn btn-outline-secondary" data-dismiss="modal">Không</button>' +
        '<button type="button" class="btn btn-outline-warning" id="confirm-yes-button">Có</button>' +
        '</div>' +
        '</div>' +
        '</form>' +
        '</div>' +
        '</div>' +
        '</div>';

    $('body').append(confirm).ready(() => {
        $('#confirm-modal').modal({
            modal: true,
            title: "popup title",
            width: 200,
            height: 'auto',
            draggable: true,
            resizable: true,
        });
        $('#confirm-body').append(body);

        $('#confirm-yes-button').on('click', function () {
            func();

            $('#confirm-modal').remove();
            $('.modal-backdrop').remove();
        });

    })
}