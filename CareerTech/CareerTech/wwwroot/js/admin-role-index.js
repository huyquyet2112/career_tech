$(document).ready(function () {
    $('#adminRoleTable').DataTable({
        lengthMenu: [10, 15, 20, 25, 50, 100],
        stateSave: true,
        stateSaveCallback: function (settings, data) {
            sessionStorage.setItem('DataTables_' + settings.sInstance, JSON.stringify(data));
        },
        stateLoadCallback: function (settings) {
            return JSON.parse(sessionStorage.getItem('DataTables_' + settings.sInstance));
        },
        ajax: {
            url: '/api/admin/roles',
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
            emptyTable: stringAdminRoleLocalized.NoRole,
            paginate: {
                previous: "&laquo;",
                next: "&raquo;",
            }
        },
        responsive: true,
        autoWidth: false,
        columnDefs: [
            { className: "text-center align-middle", targets: "_all" },
            { width: "10%", targets: 0 },
            { width: "75%", targets: 1 },
            { width: "15%", targets: 2 },
        ],
        dom: "<'row'<'col-md-6 dataTables_filter-wrapper'f><'col-md-6 text-right'l>>" +
            "<'row'<'col-sm-12'tr>>" +
            "<'row'<'col-sm-12 text-md-right'pi>>",
        columns: [
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
                            <p>${row.Description}</p>
                        `
                }
            },
            {
                data: null,
                render: function (data, type, row) {
                    return `
                        <div class="d-flex justify-content-center align-items-center">
                            <button type="button" class="btn btn-info btn-sm custom-icon-action-datatable me-1 mr-1"
                                onclick="window.location.href='/admin/roles/${row.Id}'">
                                <i class="fas fa-info-circle"></i>
                            </button>
                        </div>
                        `
                }
            }
        ]
    })
})