$(document).ready(function () {
    const jobPostId = $('#jobPostApplicantsTable').attr('data-jobPostid');
    $('#jobPostApplicantsTable').DataTable({
        lengthMenu: [10, 15, 20, 25, 50, 100],
        stateSave: true,
        stateSaveCallback: function (settings, data) {
            sessionStorage.setItem('DataTables_' + settings.sInstance, JSON.stringify(data));
        },
        stateLoadCallback: function (settings) {
            return JSON.parse(sessionStorage.getItem('DataTables_' + settings.sInstance));
        },
        ajax: {
            url: `/api/recruiters/job-posts/${jobPostId}/applicants`,
            method: 'GET',
            contentType: 'application/json',
            dataSrc: function (data) {
                if (!data) {
                    return [];
                }

                return data;
            },
        },
        language: {
            emptyTable: stringLocalizer.NoApplicants,
            paginate: {
                previous: "&laquo;",
                next: "&raquo;",
            }
        },
        responsive: true,
        autoWidth: false,
        columnDefs: [
            { className: "text-center align-middle", targets: "_all" },
        ],
        dom: "<'row'<'col-md-6 dataTables_filter-wrapper'f><'col-md-6 text-right'l>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-12 text-md-right'pi>>",
        columns: [
            {
                data: null,
                render: function (data, type, row) {
                    return `
                        <a href="/recruiters/public-applicants/${row.UserId}">
                            <span>${row.NameApplicant}</span>
                        </a>
                            
                        `
                }
            },
            {
                data: null,
                render: function (data, type, row) {
                    return `
                        <a href="#" onclick="openPDF('${row.UrlFile}', this)" data-applyid="${row.ApplyId}" class="text-secondary ms-2 ml-2" title="Xem CV">
                            <span>${row.FileName}</span> <i class="fas fa-eye"></i>
                        </a>
                    `
                }
            },
            {
                data: null,
                render: function (data, type, row) {
                    return `
                        <span>${row.Status}</span>
                    `
                }
            },
            {
                data: null,
                render: function (data, type, row) {
                    if (row.FitApplyJob != null) {
                        return `
                            <span>${stringLocalizer[row.FitApplyJob]}</span>
                        `
                    }
                }
            },
            {
                data: null,
                render: function (data, type, row) {
                    return `
                        <span>${formatDateTime(row.CreatedAt)}</span>
                    `
                }
            },
            {
                data: null,
                render: function (data, type, row) {
                    return `
                        <div class="d-flex flex-column gap-2 align-items-center">
                            <button type="button" class="btn btn-primary btn-sm text-nowrap px-3 py-2"
                                style="width: 130px;"
                                onclick="window.location.href='/recruiters/public-applicants/${row.UserId}'">
                                    ${stringLocalizer.ViewApplicant}
                            </button>
                            <button type="button" class="btn btn-secondary btn-sm text-nowrap px-3 py-2 mt-1"
                                style="width: 130px;"
                                onclick="editFitStatus(this)" data-applyid="${row.ApplyId}">
                                    ${stringLocalizer.Edit}
                            </button>
                        </div>
                    `
                }
            }
        ]
    })

    window.openPDF = function (base64Data, a) {
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

        const applyId = $(a).attr('data-applyid');

        fetch(`/api/recruiters/job-posts/applications/${applyId}/status`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
        }).then(data => {
            $('#jobPostApplicantsTable').DataTable().ajax.reload();
        }).catch(console.error);
    }

    window.editFitStatus = async function (button) {
        $('#formEditJobPostApplication')[0].reset();
        const applyId = $(button).attr('data-applyid');

        const response = await fetch(`/api/recruiters/applications/${applyId}`);
        const data = await response.json();



        $(`#fitStatusApplication > option`).each(function (index, option) {
            if (option.value == data.FitStatus) {
                $(option).prop('selected', true);
                return false;
            }
        });

        $('#fitStatusApplication').selectpicker('destroy');
        $('#fitStatusApplication').selectpicker();

        $('#jobPostTitle').val(data.JobTitle);
        $('#nameApplicant').val(data.NameApplicant);
        $('#description').val(data.Description);
        $('#formEditJobPostApplication').attr('data-applyid', applyId);
        $('#editJobPostApplicationModal').modal('show');
    }

    $('#formEditJobPostApplication').submit(function (e) {
        e.preventDefault();
        const applyId = $(this).attr('data-applyid');
        const fitStatus = $(this).find('select[name="fitStatus"]').val();
        
        const data = {
            fitStatus: fitStatus
        };


        $.ajax({
            url: `/api/recruiters/job-posts/applications/${applyId}/fit-status`,
            type: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function (data) {
                changeStatusModal('editJobPostApplicationModal', 'close');
                showToast(data.Message, 'text-white bg-success');
                setTimeout(function () {
                    $('#jobPostApplicantsTable').DataTable().ajax.reload();
                }, 2000);
            },
            error: function (xhr, textStatus) {
                showToast(xhr.responseJSON.Message);
            }
        });
    });
})