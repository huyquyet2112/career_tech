$(document).ready(function () {
    if ($('#formCRUJdPost').length) {
        const formCRUJd = $('#formCRUJdPost');

        function getJdData(createJd = true) {
            const userId = formCRUJd.find('input[name="userId"]').val();
            const jdPostId = formCRUJd.find('input[name="jdPostId"]').val();
            const title = formCRUJd.find('textarea[name="title"]').val() || null;
            const domainId = formCRUJd.find('select[name="domainId"]').val();
            const levels = formCRUJd.find('select[name="levels"]').val();
            const workingType = formCRUJd.find('select[name="workingType"]').val();
            const provinces = formCRUJd.find('select[name="provinces"]').val();
            const skills = formCRUJd.find('select[name="skills"]').val();
            const status = formCRUJd.find('select[name="status"]').val();
            const minSalary = formCRUJd.find('input[name="minSalary"]').val() || null;
            const maxSalary = formCRUJd.find('input[name="maxSalary"]').val() || null;
            const currencySalary = formCRUJd.find('select[name="currencySalary"]').val() || null;
            const endDate = formCRUJd.find('input[name="endDate"]').val() || null;
            const quantity = formCRUJd.find('input[name="quantity"]').val() || null;
            const experienceYear = formCRUJd.find('select[name="experienceYear"]').val() || null;
            const description = editorDatas['description'].getData() || null;
            const requirement = editorDatas['requirement'].getData() || null;
            const benefit = editorDatas['benefit'].getData() || null;
            const workLocation = editorDatas['workLocation'].getData() || null;
            const workingHour = editorDatas['workingHour'].getData() || null;

            const jdData = {
                title: title,
                domainId: domainId,
                levels: levels,
                workingType: workingType,
                provinces: provinces,
                skills: skills,
                status: status,
                minSalary: minSalary,
                maxSalary: maxSalary,
                currencySalary: currencySalary,
                endDate: endDate,
                quantity: quantity,
                experienceYear: experienceYear,
                description: description,
                requirement: requirement,
                benefit: benefit,
                workLocation: workLocation,
                workingHour: workingHour,
                userId: userId
            }

            if (createJd == false) {
                jdData.jdPostId = jdPostId;
            }

            return jdData;
        }

        if ($('#submitCreate').length) {
            $('#submitCreate').on('click', function () {
                $('.error-message').text('');
                jdData = getJdData(true);
                const userId = jdData.userId;

                $.ajax({
                    url: `/api/recruiters/job-posts`,
                    type: 'POST',
                    contentType: 'application/json',
                    data: JSON.stringify(jdData),
                    success: function () {
                        window.location.href = `/recruiters/${userId}/job-posts`
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
        }

        if ($('#submitUpdate').length) {
            $('#submitUpdate').on('click', function (e) {
                $('.error-message').text('');
                const jdData = getJdData(false);
                const userId = jdData.userId;
                const jobPostId = jdData.jdPostId;

                $.ajax({
                    url: `/api/recruiters/job-posts/${jobPostId}`,
                    type: 'PUT',
                    contentType: 'application/json',
                    data: JSON.stringify(jdData),
                    success: function () {
                        window.location.href = `/recruiters/${userId}/job-posts`
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
        }
    }
});