
function showModalChangePassword(button) {
    const userId = $(button).attr('data-userid');
    $('#formChangePasswordUser').attr('data-userid', userId);
}

$(document).ready(function () {

    $('.date-picker').datepicker({
        format: 'yyyy/mm/dd',
        autoclose: true,
        todayHighlight: true
    });

    $('.date-picker-btn').on('click', function () {
        $(this).closest('.input-group').find('.date-picker').datepicker('show');
    });

    $('.year-month-picker').datepicker({
        format: 'yyyy/mm',
        autoclose: true,
        todayHighlight: true,
        minViewMode: 1,
        startView: 1,
        maxViewMode: 2,
    });

    $('.year-month-picker-btn').on('click', function () {
        $(this).closest('.input-group').find('.year-month-picker').datepicker('show');
    });

    let htmlImageAvatar = null;

    $('#formUploadAvatar').on('change', async function () {
        htmlImageJob = $('#imageAvatar').html();
        const preview = await previewImage(this);
        const previewTag = $('<img>').addClass('rounded-circle img-fluid image-custom w-100 h-100').attr('src', preview);
        replaceTag("#imageAvatar", previewTag);
        $('#uploadImageAvatar').trigger('click');
    });

    $('#uploadImageAvatar').on('click', function () {
        changeStatusLoading('loading');
        const file = $('#formUploadAvatar')[0].files[0];
        const typeFolder = $(this).attr('data-type-folder');


        if (!file) {
            changeStatusLoading('stop')
            return;
        }

        const formData = new FormData();
        formData.append('file', file);
        formData.append('typeFolder', typeFolder);

        $.ajax({
            type: "POST",
            url: "/api/upload/avatar",
            data: formData,
            contentType: false,
            processData: false,
            success: function (data, textStatus, xhr) {
                $('input#imageAvatarInput').val(data.Path);
                const previewTag = $('<img>').addClass('rounded-circle img-fluid image-custom w-100 h-100').attr('src', data.Path);
                replaceTag("#imageAvatar", previewTag);
                changeStatusLoading('stop');
                $('#formUploadAvatar').val('');
            },
            error: function (xhr, textStatus) {
                replaceTag('#imageAvatar', htmlImageAvatar);
                showToast(xhr.responseJSON.message ?? 'Upload failed', 'text-white bg-danger');
                changeStatusLoading('stop');
                $('#formUploadAvatar').val('');
            }
        });
    });


    $('.password').click(function () {
        const inputId = $(this).attr('data-input');
        const typeInput = $(`#${inputId}`).attr('type');
        if (typeInput == 'password') {
            $(`#${inputId}`).attr('type', 'text');
            $(this).removeClass('fa-eye').addClass('fa-eye-slash');
        } else {
            $(`#${inputId}`).attr('type', 'password');
            $(this).removeClass('fa-eye-slash').addClass('fa-eye');
        }
    });

    if ($('#changePasswordUserModal').length) {
        $('#changePasswordUserModal').on('hidden.bs.modal', function () {
            $('#formChangePasswordUser')[0].reset();
            errorMessageContainer.addClass('d-none').text('');
        })
        $('#formChangePasswordUser').submit(function (e) {
            e.preventDefault();
            const formChangePassword = $(document.getElementById('formChangePasswordUser'));
            const userId = formChangePassword.attr('data-userId');
            const newPassword = formChangePassword.find('#newPassword').val();
            const confirmPassword = formChangePassword.find('#confirmPassword').val();

            const data = {
                newPassword: newPassword,
                confirmPassword: confirmPassword,
            };

            $.ajax({
                url: `/api/users/${userId}/change-password`,
                method: 'PUT',
                contentType: 'application/json',
                data: JSON.stringify(data),
                success: function (data, textStatus, xhr) {
                    showToast(data.Message.Message, 'text-white bg-success');
                    changeStatusModal('changePasswordUserModal', 'close');
                    if (data.shouldLogout) {
                        setTimeout(function () {
                            window.location.href = "/logout";
                        }, 3000);
                    }
                },
                error: function (xhr, textStatus) {
                    const errors = xhr.responseJSON;

                    if (errors && Array.isArray(errors)) {
                        errors.forEach(function (error) {
                            $(`#${error.Name}Error`).text(error.Error).removeClass('d-none');
                        });
                    } else {
                        showToast(xhr.responseJSON);
                    }
                }
            })
        });
    }
});
