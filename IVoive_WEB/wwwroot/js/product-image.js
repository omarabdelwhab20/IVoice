$(document).ready(function () {
    $('#ImageFile').on('change', function () {
        // Use the id attribute to target the img element
        $('#previewImage').attr('src', window.URL.createObjectURL(this.files[0])).removeClass('d-none');
    });
});