$(document).ready(function () {

    $('#modalForgotPassword').on('hidden.bs.modal', function () {
        $('#formForgotPassword')[0].reset();
    });

    $('#modalVerificationCode').on('hidden.bs.modal', function () {
        $('#formVerificationCode')[0].reset();
    });

    $('#resetPasswordModal').on('hidden.bs.modal', function () {
        $('#formResetPassword')[0].reset();
    });

    if ($('#formForgotPassword').length) {
        $('#formForgotPassword').submit(function (e) {
            $('#errorForgotPassword').text('');
            e.preventDefault();

            const email = $(this).find('input[name="email"]').val();

            const data = {
                Email: email
            };

            $.ajax({
                url: `/api/users/forgot-password`,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(data),
                success: function (dataReturn) {
                    if (dataReturn.Success == true) {
                        changeStatusModal('modalForgotPassword', 'close');
                        $('#modalVerificationCode').modal('show');
                        $('#formVerificationCode').find('#emailVerificationCode').val(dataReturn.Email);
                    }
                },
                error: function (xhr) {
                    $('#errorForgotPassword').removeClass('d-none').text(xhr.responseJSON.Mesage.Message);
                }

            });
        });
    }

    if ($('#formVerificationCode').length) {

        $('#resendCode').on('click', function () {
            $('#notificationResendCode').text('');

            const email = $('#emailVerificationCode').val();
            const codeInputs = $('#formVerificationCode').find('.code-input');
            const $msg = $('#notificationResendCode');
            const msgCodeSent = $msg.data('msg-code-sent');
            const msgErrorRetry = $msg.data('msg-error-retry');
            codeInputs.val('');

            const data = {
                Email: email
            };

            $.ajax({
                url: `/api/users/forgot-password`,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(data),
                success: function (dataReturn) {
                    if (dataReturn.Success == true) {
                        $('#notificationResendCode').removeClass('d-none').text(msgCodeSent);
                        $('#formVerificationCode').find('#emailVerificationCode').val(dataReturn.Email);
                    }
                },
                error: function (xhr) {
                    $('#notificationResendCode').removeClass('d-none').text(msgErrorRetry);
                }

            });
        });

        $('#formVerificationCode').submit(function (e) {
            $('#errorVerificationCode').text('');
            $('#notificationResendCode').text('');
            e.preventDefault();

            const email = $('#emailVerificationCode').val();
            const codeInputs = $(this).find('.code-input');

            let code = '';
            let hasEmpty = false;

            codeInputs.each(function () {
                const val = $(this).val().trim();
                if (val === '') {
                    hasEmpty = true;
                }
                code += val;
            });

            if (hasEmpty || code.length !== 6) {
                $('#errorVerificationCode').removeClass('d-none').text("Please enter the full 6-digit verification code.");
                return;
            }

            const data = {
                Email: email,
                Code: code
            };

            $.ajax({
                url: `/api/users/verification-code`,
                type: 'POST',
                contentType: 'application/json',
                data: JSON.stringify(data),
                success: function (dataReturn) {
                    if (dataReturn.Success == true) {
                        changeStatusModal('modalVerificationCode', 'close');
                        $('#resetPasswordModal').modal('show');
                        $('#formResetPassword').find('#emailResetPassword').val(dataReturn.Email);
                    }
                },
                error: function (xhr) {
                    $('#errorVerificationCode').removeClass('d-none').text(xhr.responseJSON.Message.Message);
                    codeInputs.val('');
                }

            });
        });
    }

    if ($('#formResetPassword').length) {
        $('#formResetPassword').submit(function (e) {
            $('#errorResetPassword').text('');
            $('.error-message').text('');
            e.preventDefault();

            const email = $('#emailResetPassword').val();
            const newPassword = $(this).find('#newPassword').val();
            const confirmPassword = $(this).find('#confirmPassword').val();


            const data = {
                Email: email,
                NewPassword: newPassword,
                ConfirmPassword: confirmPassword
            };

            $.ajax({
                url: `/api/users/reset-password`,
                type: 'PUT',
                contentType: 'application/json',
                data: JSON.stringify(data),
                success: function (dataReturn) {
                    changeStatusModal('resetPasswordModal', 'close');
                    showToast(dataReturn.Message, 'text-white bg-success');                
                },
                error: function (xhr) {
                    
                    codeInputs.val('');
                    const errors = xhr.responseJSON;

                    if (errors && Array.isArray(errors)) {
                        errors.forEach(function (error) {
                            $(`#${error.Name}Error`).text(error.Error).removeClass('d-none');
                        });
                    } else {
                        $('#errorResetPassword').removeClass('d-none').text(xhr.responseJSON.Message);
                    }
                }

            });
        });
    }
});