$(document).ready(function () {
    window.addJobPostStatusApproval = async function (button) {
        $('#formAddJobPostStatusApproval')[0].reset();
        const userId = $(button).attr('data-userid');
        const jobPostId = $(button).attr('data-jobPostid');
        $('#addJobPostStatusApprovalModal').modal('show');
        $('#addJobPostStatusApprovalModal').find('#createJobPostApproval').attr('data-userid', userId);
        $('#addJobPostStatusApprovalModal').find('#createJobPostApproval').attr('data-jobPostid', jobPostId);
        $('#statusJobPostApproval').selectpicker('destroy');
        $('#statusJobPostApproval').selectpicker();
        $('#reasonJobPostApproval').selectpicker('destroy');
        $('#reasonJobPostApproval').selectpicker();

    }

    window.editJobPostApproval = async function (button) {
        $('#formEditJobPostApproval')[0].reset();
        const userId = $(button).attr('data-userid');
        const jobPostApprovalId = $(button).attr('data-jobPostApprovalid');

        const response = await fetch(`/api/job-post-approvals/${jobPostApprovalId}`);
        const data = await response.json();

        $(`#editStatusJobPostApproval > option`).each(function (index, option) {
            if (option.value == data.Status) {
                $(option).prop('selected', true);
                return false;
            }
        });

        $('#editStatusJobPostApproval').selectpicker('destroy');
        $('#editStatusJobPostApproval').selectpicker();

        $(`#editReasonJobPostApproval > option`).each(function (index, option) {
            if (option.value == data.Reason) {
                $(option).prop('selected', true);
                return false;
            }
        });

        $('#editReasonJobPostApproval').selectpicker('destroy');
        $('#editReasonJobPostApproval').selectpicker();

        $('#editJobPostApprovalModal').modal('show');
        $('#editJobPostApprovalModal').find('#editJobPostApproval').attr('data-userid', userId);
        $('#editJobPostApprovalModal').find('#editJobPostApproval').attr('data-jobPostApprovalid', jobPostApprovalId);
    }
 
    $('#createJobPostApproval').on('click', function () {
        $('.error-message').text('');
        const userId = $(this).attr('data-userid');
        const jobPostId = $(this).attr('data-jobPostid');
        const status = $('#statusJobPostApproval').val();
        const reason = $('#reasonJobPostApproval').val();

        const data = {
            jobPostId: jobPostId,
            userId: userId,
            status: status,
            reason: reason
        };

        $.ajax({
            url: `/api/admin/job-post-approvals`,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function () {
                window.location.reload();
            },
            error: function (xhr, textStatus) {
                const errors = xhr.responseJSON;

                if (errors && Array.isArray(errors)) {
                    errors.forEach(function (error) {
                        $(`#${error.Name}-error`).text(error.Error).removeClass('d-none');
                    });
                } else {
                    showToast(xhr.responseJSON);
                }
            }
        })
    });

    $('#editJobPostApproval').on('click', function () {
        $('.error-message').text('');
        const userId = $(this).attr('data-userid');
        const jobPostApprovalId = $(this).attr('data-jobPostApprovalid');
        const status = $('#editStatusJobPostApproval').val();
        const reason = $('#editReasonJobPostApproval').val();

        const data = {
            userId: userId,
            status: status,
            reason: reason,
            jobPostApprovalId: jobPostApprovalId
        };

        $.ajax({
            url: `/api/admin/job-post-approvals`,
            type: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function () {
                window.location.reload();
            },
            error: function (xhr, textStatus) {
                const errors = xhr.responseJSON;

                if (errors && Array.isArray(errors)) {
                    errors.forEach(function (error) {
                        $(`#${error.Name}-error`).text(error.Error).removeClass('d-none');
                    });
                } else {
                    showToast(xhr.responseJSON);
                }
            }
        })
    });
});