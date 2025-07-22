$(document).ready(function () {
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

    $('a.pagination-user.page-link').click(function () {
        const page = $(this).attr('data-page');
        const url = `${pathName}?page=${page}${query ? `&${query}` : ''}`;
        window.location.href = url;
    })
})