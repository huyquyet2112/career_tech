$(document).ready(function () {
    window.unSaveJobPost = function (button) {
        const saveId = $(button).attr('data-saveid');
        $('#unSaveJobPostModal').modal('show');
        $('#formUnSaveJobPost').attr('data-saveid', saveId);
    }

    $('a.pagination-user.page-link').click(function () {
        const page = $(this).attr('data-page');
        const url = `${pathName}?page=${page}${query ? `&${query}` : ''}`;
        window.location.href = url;
    })


    $('#formUnSaveJobPost').submit(function (e) {
        e.preventDefault();
        const saveId = $(this).attr('data-saveid');

        $.ajax({
            url: `/api/applicants/saved-jobs/${saveId}`,
            method: "DELETE",
            success: function () {
                window.location.reload();
            },
            error: function (xhr) {
                showToast(xhr.responseJSON?.Message, 'text-white bg-danger');
            }

        })
    })
});