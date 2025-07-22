async function changeLanguage(select) {
    const lang = $(select).val();
    var res = await fetch(`/language?lang=${lang}`);
    if (res.ok) {
        window.location.reload();
    }
    else {
        var data = await res.json();
        showToast(data?.Message ?? "Language do not support", 'text-white bg-danger');
    }
}

function changeStatusLoading(status) {
    if (status == 'loading') {
        $('.loading').removeClass().addClass('spinner-border loading');
    } else {
        $('.loading').removeClass().addClass('loading');
    }
}

function showToast(message, classAddOn = '') {
    const content = `
  <div class="toast mt-3 text-white w-25" role="alert" aria-live="assertive" aria-atomic="true" data-delay="2000">
    <div class="toast-body bg-success d-flex justify-content-between align-items-center">
      ${message}
      <button type="button" class="close mb-1 ml-2" data-dismiss="toast" aria-label="Close">
        <span aria-hidden="true">&times;</span>
      </button>
    </div>
  </div>`;

    $('body').append(content);

    const $toast = $('.toast:last').toast({
        animation: true,
        delay: 2000
    }).toast('show');

    $('.toast:last').on('hidden.bs.toast', function () {
        $(this).remove();
    })
}

function replaceTag(querySelector, newTag) {
    const tag = $(querySelector);
    tag.empty();
    tag.append(newTag);
}

function previewImage(input) {
    return new Promise((resolve, reject) => {
        if (input.files && input.files[0]) {
            const reader = new FileReader();

            reader.onload = function (e) {
                resolve(e.target.result);
            }

            reader.readAsDataURL(input.files[0]);
        } else {
            reject('No file selected');
        }
    });
}

function changeStatusModal(id, status) {
    const modalElement = $(`#${id}`);

    if (status === 'close') {
        modalElement.modal('hide');
    } else {
        modalElement.modal('show');
    }
}

function convertDateTimeToString(dateTime, format = "yyyy/MM/dd") {
    if (!dateTime) return "";
    const date = new Date(dateTime);

    const map = {
        yyyy: date.getFullYear(),
        MM: ('0' + (date.getMonth() + 1)).slice(-2),
        dd: ('0' + date.getDate()).slice(-2),
        HH: ('0' + date.getHours()).slice(-2),
        mm: ('0' + date.getMinutes()).slice(-2),
        ss: ('0' + date.getSeconds()).slice(-2)
    };

    return format.replace(/yyyy|MM|dd|HH|mm|ss/g, matched => map[matched]);
}

function formatDateTime(datetimeStr) {
    const date = new Date(datetimeStr);

    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, '0'); 
    const day = String(date.getDate()).padStart(2, '0');

    const hours = String(date.getHours()).padStart(2, '0');
    const minutes = String(date.getMinutes()).padStart(2, '0');
    const seconds = String(date.getSeconds()).padStart(2, '0');

    return `${year}/${month}/${day} ${hours}:${minutes}:${seconds}`;
}
