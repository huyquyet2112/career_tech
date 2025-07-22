$(document).ready(function () {
    $('#adminApplicantTable').DataTable({
        lengthMenu: [10, 15, 20, 25, 50, 100],
        stateSave: true,
        stateSaveCallback: function (settings, data) {
            sessionStorage.setItem('DataTables_' + settings.sInstance, JSON.stringify(data));
        },
        stateLoadCallback: function (settings) {
            return JSON.parse(sessionStorage.getItem('DataTables_' + settings.sInstance));
        },
        ajax: {
            url: getUrlAdminApplicants(),
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
            emptyTable: stringAdminApplicant.NoApplicants,
            paginate: {
                previous: "&laquo;",
                next: "&raquo;",
            }
        },
        responsive: true,
        autoWidth: false,
        columnDefs: [
            { className: "text-center align-middle", targets: "_all" },
            { width: "3%", targets: 0 },
            { width: "40%", targets: 1 },
            { width: "22%", targets: 2 },
            { width: "20%", targets: 3 },
            { width: "15%", targets: 4 },
        ],
        dom: "<'row'<'col-md-6 dataTables_filter-wrapper'f><'col-md-6 text-right'l>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-12 text-md-right'pi>>",
        columns: [
            {
                data: null,
                render: function (data, type, row) {
                    if (row.Avatar != null) {
                        return `
                            <div class="d-flex justify-content-center align-items-center">
                                <img src="${row.Avatar}" class="avatar-lg2 border shadow-sm" />
                            </div>
                        `
                    } else {
                        return `
                            <div class="d-flex justify-content-center align-items-center">
                                <div class="avatar-lg2 d-flex justify-content-center align-items-center bg-light border shadow-sm">
                                    <i class="fas fa-building text-secondary fa-2x"></i>
                                </div>
                            </div>
                        `
                    }
                }
            },
            {
                data: null,
                render: function (data, type, row) {
                    return `
                            <p>${row.Name}</p>
                        `
                }
            },
            {
                data: null,
                render: function (data, type, row) {
                    return `
                            <p>${row.Email}</p>
                        `
                }
            },
            {
                data: null,
                render: function (data, type, row) {
                    return `
                            <p>${stringAdminApplicant[row.Status]}</p>
                        `
                }
            },
            {
                data: null,
                render: function (data, type, row) {
                    return `
                        <div class="d-flex justify-content-center align-items-center">
                            <button type="button" class="btn btn-info btn-sm custom-icon-action-datatable me-1 mr-1"
                                onclick="window.location.href='/admin/applicants/${row.UserId}'">
                                <i class="fas fa-info-circle"></i>
                            </button>
                            <button type="button" class="btn btn-secondary border btn-sm custom-icon-action-datatable"
                                onclick="editStatusUser(this)" data-userid="${row.UserId}">
                                <i class="fas fa-cog"></i>
                            </button>
                        </div>
                        `
                }
            }
        ]
    })

    function getUrlAdminApplicants() {
        const baseUrl = '/api/admin/applicants';
        const params = new URLSearchParams();
        const statuss = $('#status').val();
        const name = $('#name').val();

        if (statuss && statuss.length) {
            statuss.forEach(status => params.append('status', status));
        }

        if (name) {
            params.append('name', name);
        }

        return `${baseUrl}?${params.toString()}`;
    }


    window.editStatusUser = async function (button) {
        $('#formEditStatusUser')[0].reset();
        const userId = $(button).attr('data-userid');
        const response = await fetch(`/api/admin/user-status/${userId}`);
        const data = await response.json();



        $('#statusUser > option').each(function (index, option) {
            if (option.value == data.Status) {
                $(option).prop('selected', true);
                return false;
            }
        });
        $('#statusUser').selectpicker('destroy');
        $('#statusUser').selectpicker();

        $('#verifyStatus > option').each(function (index, option) {
            if (option.value == data.VerifyStatus) {
                $(option).prop('selected', true);
                return false;
            }
        });
        $('#verifyStatus').selectpicker('destroy');
        $('#verifyStatus').selectpicker();

        $('#editStatusUserModal').modal('show');
        $('#formEditStatusUser').attr('data-userid', userId);
    }

    $('#formEditStatusUser').submit(function (e) {
        $('.error-message').text('');
        e.preventDefault();
        const userId = $(this).attr('data-userid');
        const status = $(this).find('#statusUser').val();
        const verifyStatus = $(this).find('#verifyStatus').val();

        const data = {
            status: status,
            verifyStatus: verifyStatus
        };

        $.ajax({
            url: `/api/admin/user-status/${userId}`,
            type: 'PUT',
            contentType: 'application/json',
            data: JSON.stringify(data),
            success: function (dataReturn) {
                changeStatusModal('editStatusUserModal', 'close');
                showToast(dataReturn.Message, 'text-white bg-success');
                $('#adminApplicantTable').DataTable().ajax.reload();
            },
            error: function (xhr) {
                $('#editUser-error').text(xhr.responseJSON.Message).removeClass('d-none');
            }
        })

    })
})

document.addEventListener('DOMContentLoaded', function () {
    const filterHeader = document.querySelector('.card-header');
    const filterContent = document.getElementById('filterContent');
    const collapseIcon = filterHeader.querySelector('.collapse-icon');

    filterHeader.addEventListener('click', function () {
        filterContent.classList.toggle('show');

        collapseIcon.classList.toggle('fa-chevron-down');
        collapseIcon.classList.toggle('fa-chevron-up');
    });
});