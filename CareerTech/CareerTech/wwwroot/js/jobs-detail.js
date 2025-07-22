$(document).ready(function () {
    window.handleApplyClick = async function (button) {
        const isLogin = $(button).attr('is-login') === 'true';
        if (isLogin) {
            const userId = $(button).attr('data-userid');
            const jobPostId = $(button).attr('data-jobPostid');
            let files = [];
            const response = await fetch(`/api/applicants/${userId}/cv-files`);
            files = await response.json();
            renderCvOptions(files);
            $('#applyJobModal').modal('show');
            $('#formApplyJob').attr('data-userid', userId);
            $("#formApplyJob").attr('data-jobpostid', jobPostId);
        } else {
            const returnUrl = encodeURIComponent(window.location.pathname);
            window.location.href = `/login?returnUrl=${returnUrl}`;
        }
    }

    function renderCvOptions(files) {
        const container = $('#applyJobModal .container-fluid');
        container.find('.form-group').remove();
        const hasFiles = Array.isArray(files) && files.length > 0;

        const existingOption = `
            <div class="form-check">
                <input class="form-check-input" type="radio" name="cvOption" id="cvOptionExisting" value="existing" ${hasFiles ? "" : "disabled"}>
                <label class="form-check-label" for="cvOptionExisting">
                    ${stringLocalizer.SelectUploadedCV}
                </label>
            </div>
            <div id="existingFileList" class="pl-4 mt-2 ${hasFiles ? "" : "d-none"}">
                ${hasFiles ? files.map(file => `
                    <div class="form-check">
                        <input class="form-check-input" type="radio" name="selectedFile" value="${file.Id}" id="file_${file.Id}">
                        <label class="form-check-label" for="file_${file.Id}">${file.Name}</label>
                        <a href="#" onclick="openPDF('${file.UrlFile}'); return false;" class="text-secondary ms-2 ml-2" title="Xem CV">
                            <i class="fas fa-eye"></i>
                        </a>
                    </div>
                    `).join('') : `<small class="text-muted">${stringLocalizer.NoFilesAvailable}</small>`}
            </div>`;

        const newUploadOption = `
        <div class="form-check mt-3">
            <input class="form-check-input" type="radio" name="cvOption" id="cvOptionNew" value="new" ${hasFiles ? "" : "checked"}>
            <label class="form-check-label" for="cvOptionNew">
                ${stringLocalizer.UploadFromDevice}
            </label>
        </div>
        <div class="mt-2">
            <input type="file" class="form-control-file" name="cvUpload" accept=".pdf"/>
        </div>`;

        const description = `
            <div class="mt-3">
                <label for="description" class="form-label">${stringLocalizer.Description}</label>
                <textarea class="form-control" name="description" rows="4"></textarea>
            </div>
        `

        const spanError = `
            <div class="mt-3">
                <span class="text-center error-message d-none" id="appyJob-error"></span>
            </div>
        `

        container.html(`
        <div class="form-group">
            <label><strong>${stringLocalizer.SelectFile}</strong></label>
            ${existingOption}
            ${newUploadOption}
            ${description}
            ${spanError}
        </div>`);
        bindCvOptionEvents();
    }

    window.openPDF = function (base64Data) {
        const byteCharacters = atob(base64Data);
        const byteNumbers = new Array(byteCharacters.length);
        for (let i = 0; i < byteCharacters.length; i++) {
            byteNumbers[i] = byteCharacters.charCodeAt(i);
        }
        const byteArray = new Uint8Array(byteNumbers);
        const blob = new Blob([byteArray], { type: 'application/pdf' });

        const blobUrl = URL.createObjectURL(blob);
        window.open(blobUrl, '_blank');

        setTimeout(() => URL.revokeObjectURL(blobUrl), 5000);
    }

    function bindCvOptionEvents() {
        $('#cvOptionExisting').on('change', function () {
            if (this.checked) {
                const hasChecked = $('#existingFileList input[type=radio]:checked').length > 0;
                if (!hasChecked) {
                    const firstFile = $('#existingFileList input[type=radio]').first();
                    if (firstFile.length) {
                        firstFile.prop('checked', true);
                    }
                }
                $('input[name="cvUpload"]').val('');
            }
        });

        $('#cvOptionNew').on('change', function () {
            if (this.checked) {
                $('#existingFileList input[type=radio]').prop('checked', false);
            }
        });

        $('#existingFileList').on('change', 'input[name="selectedFile"]', function () {
            $('#cvOptionExisting').prop('checked', true).trigger('change');
        });

        $('input[name="cvUpload"]').on('click', function (e) {
            if (!$('#cvOptionNew').is(':checked')) {
                $('#cvOptionNew').prop('checked', true).trigger('change');
            }
        });
    }

    window.handleSaveJob = function (button) {
        const isLogin = $(button).attr('is-login') === 'true';

        if (!isLogin) {
            const returnUrl = encodeURIComponent(window.location.pathname);
            window.location.href = `/login?returnUrl=${returnUrl}`;
            return;
        }

        const $icon = $(button).find("i");
        const isSaved = $(button).attr("data-issaved") === "true";
        const jobPostId = $(button).attr("data-jobPostid");
        const userId = $(button).attr("data-userid");

        const data = {
            isSaved: isSaved,
            userId: parseInt(userId),
            jobPostId: parseInt(jobPostId)
        };


        $.ajax({
            url: `/api/applicants/saved-jobs`,
            method: "POST",
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function () {
                if (isSaved) {
                    $icon.removeClass("text-danger fas").addClass("far");
                    $(button).attr("data-issaved", "false");
                } else {
                    $icon.removeClass("far").addClass("text-danger fas");
                    $(button).attr("data-issaved", "true");
                }
            },
            error: function (xhr) {
                showToast(xhr.responseJSON?.Message, 'text-white bg-danger');
            }
        });
    };


    $('#formApplyJob').submit(function (e) {
        $('.error-message').text('');
        e.preventDefault();
        const userId = $(this).attr('data-userid');
        const jobPostId = $(this).attr('data-jobpostid');
        const description = $(this).find('textarea[name="description"]').val() || null;

        const cvOption = $('input[name="cvOption"]:checked').val();
        let file = null;
        let fileId = 0;

        if (cvOption === 'existing') {
            fileId = $('input[name="selectedFile"]:checked').val() || 0;
            file = null;
        } else if (cvOption === 'new') {
            const fileInput = $('input[name="cvUpload"]')[0];
            file = fileInput.files[0] || null;
            fileId = 0;
        }

        const formData = new FormData();

        formData.append('userId', userId);
        formData.append('jobPostId', jobPostId);
        formData.append('description', description);
        formData.append('fileId', fileId);
        formData.append('fileCV', file);

        $.ajax({
            url: `/api/applicants/apply-job`,
            type: 'POST',
            data: formData,
            processData: false,
            contentType: false,
            success: function (dataReturn) {
                changeStatusModal('applyJobModal', 'close');
                showToast(dataReturn.Message, 'text-white bg-success');
                setTimeout(function () {
                    window.location.reload();
                }, 2000);
            },
            error: function (xhr, textStatus) {
                const errors = xhr.responseJSON;

                if (errors && Array.isArray(errors)) {
                    errors.forEach(function (error) {
                        $(`#${error.Name}-error`).text(error.Error).removeClass('d-none');
                    });
                } else {
                    showToast(xhr.responseJSON.Message);
                }
            }
        })
        
    });


});