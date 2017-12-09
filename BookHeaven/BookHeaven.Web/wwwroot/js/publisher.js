Array.prototype.contains = function (obj) {
    var i = this.length;
    while (i--) {
        if (this[i] === obj) {
            return true;
        }
    }
    return false;
}

var PublishBookModule = (function (module) {
    var moduleBookFieldName;
    var moduleBookPictureUrl;
    var moduleBookPictureWidth;
    var moduleBookPictureHeight;
    var moduleBookPictureMaxLength;
    var moduleBookPictureErrorMessage;
    var moduleSupportedImageTypes;

    module.initializeModule = function (bookFieldName,
        bookPictureUrl,
        bookPictureWidth,
        bookPictureHeight,
        bookPictureMaxLength,
        bookPictureErrorMessage,
        supportedImageTypes) {
        moduleBookFieldName = bookFieldName;
        moduleBookPictureUrl = bookPictureUrl;
        moduleBookPictureWidth = bookPictureWidth;
        moduleBookPictureHeight = bookPictureHeight;
        moduleBookPictureMaxLength = bookPictureMaxLength;
        moduleBookPictureErrorMessage = bookPictureErrorMessage;
        moduleSupportedImageTypes = supportedImageTypes;

        addPageFunctionality();
    }

    function addPageFunctionality() {
        $(document).ready(function () {
            $('#picture-select').click(function () {
                $('#book-picture-input').trigger('click');
            });

            $("#book-picture-input").change(function () {
                changeBookPicture(this);
            });

            function changeBookPicture(input) {
                if (input.files && input.files[0]) {
                    var file = input.files[0];
                    var pictureValid = isPictureValid(file);

                    if (!pictureValid) {
                        resetBookPicture();
                    } else {
                        setNewBookPicture(file);
                    }

                }
            }

            function resetBookPicture() {
                $('#book-picture').attr('src', moduleBookPictureUrl);
                $('#book-picture-input').val('');
                $('#book-picture-validation').text(moduleBookPictureErrorMessage);
            }

            function setNewBookPicture(file) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    var base64Image = e.target.result;
                    resizeBase64Img(base64Image,
                        moduleBookPictureWidth,
                        moduleBookPictureHeight).then(function (result) {
                            $('#book-picture').attr('src', result[0].src);
                            $('#book-picture-validation').text('');
                        });
                }
                reader.readAsDataURL(file);
            }

            function isPictureValid(file) {
                var mimeType = file['type'];
                var mimeSize = file['size'];
                if (!moduleSupportedImageTypes.contains(mimeType) || mimeSize > moduleBookPictureMaxLength) {
                    return false;
                }
                return true;
            }

            function resizeBase64Img(base64, width, height) {
                var canvas = document.createElement("canvas");
                canvas.width = width;
                canvas.height = height;
                var context = canvas.getContext("2d");
                var deferred = $.Deferred();
                $("<img/>").attr("src", base64).load(function () {
                    context.scale(width / this.width, height / this.height);
                    context.drawImage(this, 0, 0);
                    deferred.resolve($("<img/>").attr("src", canvas.toDataURL()));
                });
                return deferred.promise();
            }
        });
    }

    return module;
}({}))