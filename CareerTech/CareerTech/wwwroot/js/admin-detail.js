$(document).ready(function () {
    if ($('#formUpdateDetailAdmin').length) {
        const formUpdateDetailAdmin = $('#formUpdateDetailAdmin');
        let originalDataHtml = '';
        let originalData = {};

        function resetForm() {
            formUpdateDetailAdmin.find('.editButton').removeClass('d-none');
            formUpdateDetailAdmin.find('.saveButton').addClass('d-none');
            formUpdateDetailAdmin.find('.cancelButton').addClass('d-none');
            formUpdateDetailAdmin.find('.remove-disabled').prop('disabled', true);
            formUpdateDetailAdmin.find('.remove-readonly').prop('readonly', true);
        }


        formUpdateDetailAdmin.find('.editButton').click(function () {
            formUpdateDetailAdmin.find('.editButton').addClass('d-none');
            formUpdateDetailAdmin.find('.saveButton').removeClass('d-none');
            formUpdateDetailAdmin.find('.cancelButton').removeClass('d-none');
            formUpdateDetailAdmin.find('.remove-disabled').prop('disabled', false);
            formUpdateDetailAdmin.find('.remove-readonly').prop('readonly', false);
            originalDataHtml = formUpdateDetailAdmin.find('#imageAvatar').html();
            formUpdateDetailAdmin.find('input').each(function () {
                originalData[$(this).attr('name')] = $(this).val();
            });
        });

        formUpdateDetailAdmin.find('.cancelButton').click(function () {
            $.each(originalData, function (name, value) {
                $(`input[name="${name}"]`).val(value).trigger('change');
            });

            formUpdateDetailAdmin.find('#imageAvatar').html(originalDataHtml);
            resetForm();
        });

        formUpdateDetailAdmin.submit(function (e) {
            $('.error-message').text('');
            e.preventDefault();
            const userId = $(this).attr('data-userid');
            const avatar = formUpdateDetailAdmin.find('input[name="avatar"]').val() || null;
            const oldAvatar = formUpdateDetailAdmin.find('input[name="oldAvatar"]').val() || null;
            const name = formUpdateDetailAdmin.find('input[name="name"]').val() || null;

            const data = {
                avatar: avatar,
                oldAvatar: oldAvatar,
                name: name
            }

            $.ajax({
                url: `/api/admin/${userId}/basic-info`,
                type: 'PUT',
                contentType: 'application/json',
                data: JSON.stringify(data),
                success: function (dataReturn) {
                    showToast(dataReturn.Message, 'text-white bg-success');
                    window.location.reload();
                },
                error: function (xhr) {
                    const errors = xhr.responseJSON;

                    if (errors && Array.isArray(errors)) {
                        errors.forEach(function (error) {
                            $(`#${error.Name}Error`).text(error.Error).removeClass('d-none');
                        });
                    } else {
                        showToast(xhr.responseJSON);
                    }
                }
            });
        });
    }
});