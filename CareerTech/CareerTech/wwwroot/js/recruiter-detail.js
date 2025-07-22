$(document).ready(function () {
    $('#iconButton').click(function () {
        $('#card-body-content').slideToggle();
        let icon = $('#toggleIcon');
        if (icon.attr('icon') == 'bxs:chevron-up') {
            icon.attr('icon', 'bxs:chevron-down');
        } else {
            icon.attr('icon', 'bxs:chevron-up');
        }
    });

    //Event Search in Menu
    $('#searchInput').focus(function () {
        $(this).addClass('custom-border');
    });

    $('#searchInput').blur(function () {
        $(this).removeClass('custom-border');
    });

    //Active Button Menu Content
    $('#menu-option-detail .list-option').on('click', function (event) {
        var selectedText = $(this).text();
        $('#text-content-detail').text(selectedText);
        $('#menu-option-detail .list-option').removeClass('active');
        $(this).addClass('active');
        var target = $(this).attr('href');
        $('.tab-pane').removeClass('show active');
        $(target).addClass('show active');
    });

    var activeOption = $('#menu-option-detail .list-option.active');
    if (activeOption.length) {
        var selectedText = activeOption.text();
        $('#text-content-detail').text(selectedText);
    }

    // Update Basic Info Recruitment.
    if ($("#formUpdateBasicInfoRecruitment").length) {
        const formUpdateBasicInforRecruitment = $('#formUpdateBasicInfoRecruitment');
        const errorMessageContainer = $('<div>').addClass('alert alert-danger d-none').attr('id', 'errorMessage');
        formUpdateBasicInforRecruitment.prepend(errorMessageContainer);
        let originalDataHtml = '';
        let originalData = {};

        function resetForm() {
            formUpdateBasicInforRecruitment.find('.editButton').removeClass('d-none');
            formUpdateBasicInforRecruitment.find('.saveButton').addClass('d-none');
            formUpdateBasicInforRecruitment.find('.cancelButton').addClass('d-none');
            formUpdateBasicInforRecruitment.find('.remove-disabled').prop('disabled', true);
            formUpdateBasicInforRecruitment.find('.remove-readonly').prop('readonly', true);
            formUpdateBasicInforRecruitment.find('#companySize').selectpicker('destroy');
            formUpdateBasicInforRecruitment.find('#companySize').selectpicker();
            formUpdateBasicInforRecruitment.find('#workSchedule').selectpicker('destroy');
            formUpdateBasicInforRecruitment.find('#workSchedule').selectpicker();
        }

        formUpdateBasicInforRecruitment.find('.editButton').click(function () {
            formUpdateBasicInforRecruitment.find('.editButton').addClass('d-none');
            formUpdateBasicInforRecruitment.find('.saveButton').removeClass('d-none');
            formUpdateBasicInforRecruitment.find('.cancelButton').removeClass('d-none');
            formUpdateBasicInforRecruitment.find('.remove-disabled').prop('disabled', false);
            formUpdateBasicInforRecruitment.find('.remove-readonly').prop('readonly', false);
            formUpdateBasicInforRecruitment.find('#companySize').selectpicker('destroy');
            formUpdateBasicInforRecruitment.find('#companySize').selectpicker();
            formUpdateBasicInforRecruitment.find('#workSchedule').selectpicker('destroy');
            formUpdateBasicInforRecruitment.find('#workSchedule').selectpicker();
            originalDataHtml = formUpdateBasicInforRecruitment.find('#imageAvatar').html();
            formUpdateBasicInforRecruitment.find('input, select').each(function () {
                originalData[$(this).attr('name')] = $(this).val();
            });
        });

        formUpdateBasicInforRecruitment.find('.cancelButton').click(function () {
            $.each(originalData, function (name, value) {
                $(`select[name="${name}"], input[name="${name}"]`).val(value).trigger('change');
            });

            formUpdateBasicInforRecruitment.find('#imageAvatar').html(originalDataHtml);
            resetForm();
        });

        formUpdateBasicInforRecruitment.submit(function (e) {
            $('.error-message').text('');
            e.preventDefault();
            const userId = $(this).attr('data-userid');
            const avatar = formUpdateBasicInforRecruitment.find('input[name="avatar"]').val() || null;
            const oldAvatar = formUpdateBasicInforRecruitment.find('input[name="oldAvatar"]').val() || null;
            const name = formUpdateBasicInforRecruitment.find('input[name="name"]').val() || null;
            const phoneNumber = formUpdateBasicInforRecruitment.find('input[name="phoneNumber"]').val() || null;
            const address = formUpdateBasicInforRecruitment.find('textarea[name="address"]').val() || null;
            const establishedDate = formUpdateBasicInforRecruitment.find('input[name="establishedDate"]').val() || null;
            const linkWeb = formUpdateBasicInforRecruitment.find('input[name="linkWeb"]').val() || null;
            const description = formUpdateBasicInforRecruitment.find('textarea[name="description"]').val() || null;
            const locationMap = formUpdateBasicInforRecruitment.find('textarea[name="locationMap"]').val() || null;
            const companySize = formUpdateBasicInforRecruitment.find('select[name="companySize"]').val();
            const workSchedule = formUpdateBasicInforRecruitment.find('select[name="workSchedule"]').val();

            const data = {
                avatar: avatar,
                oldAvatar: oldAvatar,
                name: name,
                phoneNumber: phoneNumber,
                address: address,
                establishedDate: establishedDate,
                linkWeb: linkWeb,
                description: description,
                locationMap: locationMap,
                companySize: companySize,
                workSchedule: workSchedule,
            }

            $.ajax({
                url: `/api/recruiters/${userId}/basic-info`,
                type: 'PUT',
                contentType: 'application/json',
                data: JSON.stringify(data),
                success: function (dataReturn) {
                    errorMessageContainer.addClass('d-none').text('');
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

    // Update Recruitment Group Domain.
    if ($('#formUpdateRecruitmentGroupDomain').length) {
        let originData = {};
        const formUpdateRecruitmentGroupDomain = $('#formUpdateRecruitmentGroupDomain');
        const errorMessageContainer = $('<div>').addClass('alert alert-danger d-none').attr('id', 'errorMessage');
        formUpdateRecruitmentGroupDomain.prepend(errorMessageContainer);

        function resetForm() {
            formUpdateRecruitmentGroupDomain.find('.editButton').removeClass('d-none');
            formUpdateRecruitmentGroupDomain.find('.saveButton').addClass('d-none');
            formUpdateRecruitmentGroupDomain.find('.cancelButton').addClass('d-none');
            formUpdateRecruitmentGroupDomain.find('.remove-disabled').prop('disabled', true);
        }

        formUpdateRecruitmentGroupDomain.find('.editButton').click(function () {
            formUpdateRecruitmentGroupDomain.find('.editButton').addClass('d-none');
            formUpdateRecruitmentGroupDomain.find('.saveButton').removeClass('d-none');
            formUpdateRecruitmentGroupDomain.find('.cancelButton').removeClass('d-none');
            formUpdateRecruitmentGroupDomain.find('.remove-disabled').prop('disabled', false);

            formUpdateRecruitmentGroupDomain.find('.recruitmentGroupDomain').each(function () {
                const value = $(this).val();
                originData[value] = $(this).prop('checked');
            });
        });

        formUpdateRecruitmentGroupDomain.find('.cancelButton').click(function () {
            formUpdateRecruitmentGroupDomain.find('.recruitmentGroupDomain').each(function () {
                const value = $(this).val();
                $(this).prop('checked', originData[value]);
            });
            resetForm();
        });

        formUpdateRecruitmentGroupDomain.find('.selectAll').click(function () {
            formUpdateRecruitmentGroupDomain.find('.recruitmentGroupDomain').prop('checked', true);
        });

        formUpdateRecruitmentGroupDomain.find('.deselectAll').click(function () {
            formUpdateRecruitmentGroupDomain.find('.recruitmentGroupDomain').prop('checked', false);
        });

        formUpdateRecruitmentGroupDomain.submit(function (e) {
            e.preventDefault();
            const userId = $(this).attr('data-userid');
            let data = {};
            let groupDomains = [];

            formUpdateRecruitmentGroupDomain.find('.recruitmentGroupDomain:checked').each(function () {
                let groupDomainId = $(this).val();

                if (groupDomainId) {
                    groupDomains.push({
                        groupDomainId: groupDomainId,
                    });
                }
            });

            data = {
                userId: userId,
                groupDomains: groupDomains
            };

            $.ajax({
                url: `/api/recruiters/group-domain`,
                type: 'PUT',
                contentType: 'application/json',
                data: JSON.stringify(data),
                success: function (dataReturn) {
                    showToast(dataReturn.Message.Message, 'text-white bg-success');
                    errorMessageContainer.addClass('d-none').text('');
                    let groupDomainIds = dataReturn.RecruitmentGroupDomains.map(us => us.GroupDomainId);
                    formUpdateRecruitmentGroupDomain.find('.recruitmentGroupDomain').each(function () {
                        let groupDomainId = parseInt($(this).val());
                        $(this).prop('checked', groupDomainIds.includes(groupDomainId));
                    });
                },
                error: function (dataReturn) {
                    errorMessageContainer.removeClass('d-none').text(dataReturn.responseJSON?.Message);
                }
            });

            resetForm();
        });
    }
});