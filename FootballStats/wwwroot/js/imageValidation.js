function handleImageInputChange(inputId, errorMessage, maxFileSizeBytes, allowedMimeTypes) {
    var imageInput = document.getElementById(inputId);
    var errorMessageElement = document.getElementById(errorMessage);

    if (imageInput.files.length > 0) {
        var file = imageInput.files[0];


        if (!allowedMimeTypes.includes(file.type)) {
            errorMessageElement.textContent = 'Only certain types of images are allowed.';
            imageInput.value = '';
            return;
        }


        if (file.size > maxFileSizeBytes) {
            errorMessageElement.textContent = 'The image size is too large.';
            imageInput.value = '';
            return;
        }


        errorMessageElement.textContent = '';
    }
}

document.getElementById('Logo').addEventListener('change', function (event) {
    handleImageInputChange('Logo', 'errorLogo', 1 * 1024 * 1024, ['image/jpeg', 'image/png', 'image/gif']);
});

document.getElementById('LogoStadium').addEventListener('change', function (event) {
    handleImageInputChange('LogoStadium', 'errorLogoStadium', 1 * 1024 * 1024, ['image/jpeg', 'image/png', 'image/gif']);
});