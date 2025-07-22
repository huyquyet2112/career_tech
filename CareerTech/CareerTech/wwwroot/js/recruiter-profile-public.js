$(document).ready(function () {
    window.copyLink = function (button) {
        const $input = $('#shareLink');
        const text = $input.val();

        navigator.clipboard.writeText(text)
            .then(function () {
                $('#copySuccess').removeClass('d-none');
                showToast(stringCopyLocalizer.CopySuccessful, 'text-white bg-success')
            })
            .catch(function (err) {
                showToast(stringCopyLocalizer.CopyFailed, 'text-white bg-danger');
            });
    };

    function reloadPageWithParams(page) {
        const formData = $('#formQueryJobByRecruiterPublic').serializeArray();
        var filteredData = formData.filter(function (field) {
            return field.value.trim() !== "";
        });
        var query = $.param(filteredData);

        const pathName = window.location.pathname;
        const url = `${pathName}?page=${page}${query ? `&${query}` : ''}`;
        window.location.href = url;
    }

    $('#formQueryJobByRecruiterPublic').submit(function (e) {
        e.preventDefault();
        reloadPageWithParams()
        const page = $('li.page-item.active>').attr('data-page');
        reloadPageWithParams(page);
    })

    $('a.pagination-user.page-link').click(function () {
        const page = $(this).attr('data-page');
        reloadPageWithParams(page);
    });
})