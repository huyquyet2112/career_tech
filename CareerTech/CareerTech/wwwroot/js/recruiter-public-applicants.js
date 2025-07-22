$(document).ready(function () {
    $('#recruiterApplicantPublicTable').DataTable({
        lengthMenu: [10, 15, 20, 25, 50, 100],
        stateSave: true,
        stateSaveCallback: function (settings, data) {
            sessionStorage.setItem('DataTables_' + settings.sInstance, JSON.stringify(data));
        },
        stateLoadCallback: function (settings) {
            return JSON.parse(sessionStorage.getItem('DataTables_' + settings.sInstance));
        },
        ajax: {
            url: getUrlApplicantsPublic(),
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
            emptyTable: stringRecruiterApplicantPublic.NoApplicants,
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
            { width: "45%", targets: 1 },
            { width: "37%", targets: 2 },
            { width: "15%", targets: 3 },
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
                            <a href="recruiters/public-applicants/${row.UserId}">
                                <div class="d-flex justify-content-center align-items-center">
                                    <img src="${row.Avatar}" class="avatar-lg2 border shadow-sm" />
                                </div>
                            </a>`
                    } else {
                        return `
                            <a href="recruiters/public-applicants/${row.UserId}">
                                <div class="d-flex justify-content-center align-items-center">
                                    <div class="avatar-lg2 d-flex justify-content-center align-items-center bg-light border shadow-sm">
                                        <i class="fas fa-building text-secondary fa-2x"></i>
                                    </div>
                                </div>
                            </a>
                            
                        `
                    }
                }
            },
            {
                data: null,
                render: function (data, type, row) {
                    return `
                        <a href="recruiters/public-applicants/${row.UserId}">
                            <span>${row.Name}</span>
                        </a>
                        `
                }
            },
            {
                data: null,
                render: function (data, type, row) {
                    return `
                            <span>${row.Email}</span>
                        `
                }
            },
            {
                data: null,
                render: function (data, type, row) {
                    return `
                        <div class="d-flex justify-content-center align-items-center">
                            <button type="button" class="btn btn-info btn-sm custom-icon-action-datatable me-1 mr-1"
                                onclick="window.location.href='/recruiters/public-applicants/${row.UserId}'">
                                <i class="fas fa-info-circle"></i>
                            </button>
                        </div>
                        `
                }
            }
        ]
    })


    function getUrlApplicantsPublic() {
        const baseUrl = '/api/recruiters/public-applicants';
        const params = new URLSearchParams();
        const name = $('#name').val();
        const levels = $('#levels').val();
        const provinces = $('#provinces').val();
        const skills = $('#skills').val();

        if (name) {
            params.append('name', name);
        }

        if (skills && skills.length) {
            skills.forEach(skill => params.append('skills', skill));
        }

        if (levels && levels.length) {
            levels.forEach(level => params.append('levels', level));
        }

        if (provinces && provinces.length) {
            provinces.forEach(province => params.append('provinces', province));
        }

        return `${baseUrl}?${params.toString()}`;
    }
});

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

