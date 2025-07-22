$(document).ready(function () {

    $('.groupController').click(function () {
        let groupController = $(this);
        let row = groupController.closest('.rowRolePermission');
        let permission = row.find('.permission');
        let isChecked = groupController.prop('checked');
        permission.prop('checked', isChecked);
    });

    $('#formUpdateRolePermissions .selectAll').click(function () {
        $('#formUpdateRolePermissions .permission, .groupController').prop('checked', true);
    });

    $('#formUpdateRolePermissions .deselectAll').click(function () {
        $('#formUpdateRolePermissions .permission, .groupController').prop('checked', false);
    });

    if ($('#formUpdateRolePermissions').length) {
        let originData = {}
        const formUpdateRolePermissions = $('#formUpdateRolePermissions');

        function resetForm() {
            formUpdateRolePermissions.find('.editButton').removeClass('d-none');
            formUpdateRolePermissions.find('.saveButton').addClass('d-none');
            formUpdateRolePermissions.find('.cancelButton').addClass('d-none');
            formUpdateRolePermissions.find('.remove-disabled').prop('disabled', true);
        }

        formUpdateRolePermissions.find('.editButton').click(function () {
            formUpdateRolePermissions.find('.editButton').addClass('d-none');
            formUpdateRolePermissions.find('.saveButton').removeClass('d-none');
            formUpdateRolePermissions.find('.cancelButton').removeClass('d-none');
            formUpdateRolePermissions.find('.remove-disabled').prop('disabled', false);

            formUpdateRolePermissions.find('.permission, .groupController').each(function () {
                const value = $(this).val();
                originData[value] = $(this).prop('checked');
            })
        });
        formUpdateRolePermissions.find('.cancelButton').click(function () {
            formUpdateRolePermissions.find('.permission, .groupController').each(function () {
                const value = $(this).val();
                $(this).prop('checked', originData[value]);
            })


            resetForm();
        });

        $('#formUpdateRolePermissions').submit(function (e) {
            e.preventDefault();
            const roleId = $(this).attr('data-roleid');
            let data = {};
            let listPermission = [];

            $('#formUpdateRolePermissions .permission:checked').each(function () {
                let permissionId = $(this).val();

                if (permissionId) {
                    listPermission.push({
                        Id: permissionId,
                    });
                }
            });

            data = {
                RoleId: roleId,
                Permissions: listPermission,
            };

            $.ajax({
                url: '/api/admin/role-permissions',
                type: 'PUT',
                contentType: 'application/json',
                data: JSON.stringify(data),
                success: function (data) {
                    showToast(data.Message.Message, 'text-white bg-success');
                    let permissionIds = data.RolePermissions.map(p => p.PermissionId);
                    $('#formUpdateRolePermissions .permission').each(function () {
                        let permissionId = parseInt($(this).val());
                        $(this).prop('checked', permissionIds.includes(permissionId));
                    });

                    $('#formUpdateRolePermissions .rowRolePermission').each(function () {
                        let row = $(this);
                        let groupController = row.find('.groupController');
                        let permission = row.find('.permission');

                        let allChecked = permission.length > 0 &&
                            permission.filter((_, el) => $(el).prop('checked')).length === permission.length;

                        groupController.prop('checked', allChecked);
                    })
                },
                error: function (xhr) {
                    showToast(xhr.responseJSON?.Message, 'text-danger bg-success');
                }
            });

            resetForm();
        })
    }
});