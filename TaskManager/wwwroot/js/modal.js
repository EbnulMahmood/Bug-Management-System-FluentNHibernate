
const showModal = (url, PlaceHolderElement) => {
    const decodeUrl = decodeURIComponent(url);
    $.get(decodeUrl).done(function (data) {
        PlaceHolderElement.html(data);
        PlaceHolderElement.find('.modal').modal('show');
    })
}

const modalAction = (PlaceHolderElement, that) => {
    const form = that.parents('.modal').find('form');
    const actionUrl = form.attr('action');
    const sendData = form.serialize();
    $.post(actionUrl, sendData).done(function (data) {
        PlaceHolderElement.find('.modal').modal('hide');
        window.location.reload();
    })
}

// qa modal
$(function () {
    const PlaceHolderElementDev = $('#modal-placeholder-qa');
    $(document).on("click", ".btn-qa-delete", function() {
        const id = $(this).attr('data-qa-id');
        const url = `QA/Delete/${id}`;
        showModal(url, PlaceHolderElementDev);
    });

    PlaceHolderElementDev.on('click', '[data-bs-save="modal"]', function (e) {
        modalAction(PlaceHolderElementDev, $(this))
    })
})

// dev modal
$(function () {
    const PlaceHolderElementDev = $('#modal-placeholder-dev');
    $(document).on("click", ".btn-dev-delete", function() {
        const id = $(this).attr('data-dev-id');
        const url = `Developer/Delete/${id}`;
        showModal(url, PlaceHolderElementDev);
    });

    PlaceHolderElementDev.on('click', '[data-bs-save="modal"]', function (e) {
        modalAction(PlaceHolderElementDev, $(this))
    })
})