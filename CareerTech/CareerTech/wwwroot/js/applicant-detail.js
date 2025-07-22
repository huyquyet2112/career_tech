$(document).ready(function () {
    if ($('#formUpdateBasicInforApplicant').length) {
        const formUpdateBasicInforApplicant = $('#formUpdateBasicInforApplicant');
        let originalDataHtml = '';
        let originalData = {};

        function resetForm() {
            formUpdateBasicInforApplicant.find('.editButton').removeClass('d-none');
            formUpdateBasicInforApplicant.find('.saveButton').addClass('d-none');
            formUpdateBasicInforApplicant.find('.cancelButton').addClass('d-none');
            formUpdateBasicInforApplicant.find('.remove-disabled').prop('disabled', true);
            formUpdateBasicInforApplicant.find('.remove-readonly').prop('readonly', true);
            formUpdateBasicInforApplicant.find('#gender').selectpicker('destroy');
            formUpdateBasicInforApplicant.find('#gender').selectpicker();
        }

        formUpdateBasicInforApplicant.find('.editButton').click(function () {
            formUpdateBasicInforApplicant.find('.editButton').addClass('d-none');
            formUpdateBasicInforApplicant.find('.saveButton').removeClass('d-none');
            formUpdateBasicInforApplicant.find('.cancelButton').removeClass('d-none');
            formUpdateBasicInforApplicant.find('.remove-disabled').prop('disabled', false);
            formUpdateBasicInforApplicant.find('.remove-readonly').prop('readonly', false);
            formUpdateBasicInforApplicant.find('#gender').selectpicker('destroy');
            formUpdateBasicInforApplicant.find('#gender').selectpicker();


            originalDataHtml = formUpdateBasicInforApplicant.find('#imageAvatar').html();
            formUpdateBasicInforApplicant.find('input, select').each(function () {
                originalData[$(this).attr('name')] = $(this).val();
            });
        });

        formUpdateBasicInforApplicant.find('.cancelButton').click(function () {
            $.each(originalData, function (name, value) {
                $(`select[name="${name}"], input[name="${name}"]`).val(value).trigger('change');
            });

            formUpdateBasicInforApplicant.find('#imageAvatar').html(originalDataHtml);
            resetForm();
        });

        formUpdateBasicInforApplicant.submit(function (e) {
            $('.error-message').text('');
            e.preventDefault();
            const userId = $(this).attr('data-userid');
            const avatar = formUpdateBasicInforApplicant.find('input[name="avatar"]').val() || null;
            const oldAvatar = formUpdateBasicInforApplicant.find('input[name="oldAvatar"]').val() || null;
            const name = formUpdateBasicInforApplicant.find('input[name="name"]').val() || null;
            const gender = formUpdateBasicInforApplicant.find('select[name="gender"]').val();
            const phoneNumber = formUpdateBasicInforApplicant.find('input[name="phoneNumber"]').val() || null;
            const birthday = formUpdateBasicInforApplicant.find('input[name="birthday"]').val() || null;
            const address = formUpdateBasicInforApplicant.find('input[name="address"]').val() || null;
            const description = formUpdateBasicInforApplicant.find('textarea[name="description"]').val() || null;

            const data = {
                avatar: avatar,
                oldAvatar: oldAvatar,
                name: name,
                phoneNumber: phoneNumber,
                gender: gender,
                birthday: birthday,
                address: address,
                description: description
            };

            $.ajax({
                url: `/api/applicants/${userId}/basic-info`,
                type: 'PUT',
                contentType: 'application/json',
                data: JSON.stringify(data),
                success: function (dataReturn) {
                    showToast(dataReturn.Message.Message, 'text-white bg-success');
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
            })
        });
    }

    if ($('#formUploadCV').length) {
        const formUploadCV = $('#formUploadCV');
        formUploadCV.submit(function (e) {
            e.preventDefault();

            const userId = $(this).attr('data-userid');
            const fileInput = formUploadCV.find('input[name="fileCV"]')[0];
            const file = fileInput.files[0];

            let formData = new FormData();
            formData.append('fileCV', file);

            $.ajax({
                url: `/api/upload/${userId}/fileCV`,
                method: 'POST',
                data: formData,
                processData: false,
                contentType: false,
                success: function (dataReturn) {
                    if (dataReturn.File) {
                        $('.cvNotFound').addClass('d-none');
                        const fileName = `${dataReturn.File.Name}`;
                        const fileUrl = dataReturn.File.UrlFile;
                        const fileId = dataReturn.File.Id;
                        const newFileHtml = `
                            <div class="col-md-12 mb-3">
                                <div class="card border-0 bg-light">
                                    <div class="card-body d-flex justify-content-between align-items-center">
                                        <div>
                                            <a class="text-decoration-none fw-bold" href="data:application/pdf;base64, ${fileUrl}" download="${fileName}">
                                                <i class="fas fa-file-pdf me-2 text-danger"></i>${fileName}
                                            </a>
                                        </div>
                                        <button type="button" class="btn btn-sm btn-outline-danger delete-cvFile" data-fileid="${fileId}" onclick="deleteCVFile(this)">
                                            <i class="fas fa-trash-alt"></i>
                                        </button>
                                   </div>
                               </div>
                           </div>
                        `;
                        $(".rowCVFile").append(newFileHtml);
                        formUploadCV[0].reset();
                    }
                },
                error: function (xhr) {
                    formUploadCV[0].reset();
                    showToast(xhr.responseJSON?.message || 'Upload failed!', 'text-white bg-danger');
                }
            });
        });
    }

    let currentDeleteButton = null;

    window.deleteCVFile = async function (button) {
        $('#formDeleteFileCV')[0].reset();
        const fileId = $(button).attr('data-fileid');
        currentDeleteButton = button;
        $('#deleteFileCVModal').modal('show');
        $('#formDeleteFileCV').find('#deleteCvFile').attr('data-fileid', fileId);
    };

    $('#deleteCvFile').on('click', function () {
        const fileId = $(this).attr('data-fileid');
        const self = $('.delete-cvFile');

        $.ajax({
            url: `/api/upload/fileCV/${fileId}`,
            type: 'PUT',
            success: function (response) {
                showToast(response?.Message);
                changeStatusModal('deleteFileCVModal', 'close')

                if (currentDeleteButton) {
                    $(currentDeleteButton).closest('.col-md-12').remove();
                    currentDeleteButton = null;
                }
            },
            error: function (xhr, status, error) {
                showToast(xhr.responseJSON?.Message);
            }
        })
    });


    $('#btnAddNewSkillModal').on('click', async function () {
        const userId = $(this).attr('data-userId');
        $('#formUpdateUserSkill').attr('data-userId', userId);
        const response = await fetch(`/api/applicants/${userId}/skills`);
        const groupSkills = await response.json();
        let selectSkill = $('<select id="selectUpdateUserSkills" name="skills" multiple></select>');

        groupSkills.forEach(groupSkill => {
            const optGroup = $(`<optgroup label="${groupSkill.Name}"></optgroup>`);
            groupSkill.Skills.forEach(skill => {
                optGroup.append(`<option value="${skill.Id}" ${skill.IsSelected ? 'selected' : ''}>${skill.Name}</option>`);
            })
            selectSkill.append(optGroup);
        });

        selectSkill = selectSkill
            .addClass('selectpicker form-control')
            .attr('name', 'skills')
            .attr('data-live-search', 'true')
            .attr('data-actions-box', 'true')
            .attr('data-selected-text-format', 'count > 5');
        replaceTag('#selectAddNewSkill', selectSkill);

        $('.selectpicker').selectpicker();

    });

    $('#formUpdateUserSkill').submit(function (e) {
        e.preventDefault();

        const userId = $(this).attr('data-userId');
        const skills = $(this).find('select[name="skills"]').val();


        const data = {
            userId: userId,
            skills: skills
        };

        $.ajax({
            url: `/api/applicants/${userId}/skills`,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function (dataReturn) {
                window.location.reload();
            },
            error: function (xhr) {
                showToast(xhr.responseJSON?.Message);
            }
        });
    });

    $('#btnAddNewLevelModal').on('click', async function () {
        const userId = $(this).attr('data-userId');
        $('#formUpdateUserLevel').attr('data-userId', userId);
        const response = await fetch(`/api/applicants/${userId}/levels`);
        const levels = await response.json();
        let selectLevel = $('<select id="selectUpdateUserLevels" name="levels" multiple></select>');

        levels.forEach(level => {
            selectLevel.append(`<option value="${level.Id}" ${level.IsSelected ? 'selected' : ''}>${level.Name}</option>`)
        });

        selectLevel = selectLevel
            .addClass('selectpicker form-control')
            .attr('name', 'levels')
            .attr('data-live-search', 'true')
            .attr('data-actions-box', 'true')
            .attr('data-selected-text-format', 'count > 5');
        replaceTag('#selectAddNewLevel', selectLevel);

        $('.selectpicker').selectpicker();

    });

    $('#formUpdateUserLevel').submit(function (e) {
        e.preventDefault();

        const userId = $(this).attr('data-userId');
        const levels = $(this).find('select[name="levels"]').val();


        const data = {
            userId: userId,
            levels: levels
        };

        $.ajax({
            url: `/api/applicants/${userId}/levels`,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function (dataReturn) {
                window.location.reload();
            },
            error: function (xhr) {
                showToast(xhr.responseJSON?.Message);
            }
        });
    });

    $('#btnAddNewProvinceModal').on('click', async function () {
        const userId = $(this).attr('data-userId');
        $('#formUpdateUserProvince').attr('data-userId', userId);
        const response = await fetch(`/api/applicants/${userId}/provinces`);
        const provinces = await response.json();
        let selectProvince = $('<select id="selectUpdateUserProvinces" name="provinces" multiple></select>');

        provinces.forEach(province => {
            selectProvince.append(`<option value="${province.Id}" ${province.IsSelected ? 'selected' : ''}>${province.Name}</option>`)
        });

        selectProvince = selectProvince
            .addClass('selectpicker form-control')
            .attr('name', 'provinces')
            .attr('data-live-search', 'true')
            .attr('data-actions-box', 'true')
            .attr('data-selected-text-format', 'count > 5');
        replaceTag('#selectAddNewProvince', selectProvince);

        $('.selectpicker').selectpicker();

    });

    $('#formUpdateUserProvince').submit(function (e) {
        e.preventDefault();

        const userId = $(this).attr('data-userId');
        const provinces = $(this).find('select[name="provinces"]').val();


        const data = {
            userId: userId,
            provinces: provinces
        };

        $.ajax({
            url: `/api/applicants/${userId}/provinces`,
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function (dataReturn) {
                window.location.reload();
            },
            error: function (xhr) {
                showToast(xhr.responseJSON?.Message);
            }
        });
    });
});