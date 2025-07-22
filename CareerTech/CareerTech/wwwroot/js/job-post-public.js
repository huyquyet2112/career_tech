$(document).ready(function () {
    function reloadPageWithParams(page) {
        const formData = $('#formQueryJobPublic').serializeArray();
        var filteredData = formData.filter(function (field) {
            return field.value.trim() !== "";
        });
        var query = $.param(filteredData);

        const pathName = window.location.pathname;
        const url = `${pathName}?page=${page}${query ? `&${query}` : ''}`;
        window.location.href = url;
    }

    $('#formQueryJobPublic').submit(function (e) {
        e.preventDefault();
        reloadPageWithParams()
        const page = $('li.page-item.active>').attr('data-page');
        reloadPageWithParams(page);
    })

    $('a.pagination-user.page-link').click(function () {
        const page = $(this).attr('data-page');
        reloadPageWithParams(page);
    })
})