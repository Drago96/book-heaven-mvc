Array.prototype.contains = function (obj) {
    var i = this.length;
    while (i--) {
        if (this[i] === obj) {
            return true;
        }
    }
    return false;
}

var RegisterUserPageModule = (function (module) {
    var moduleProfilePictureFieldName;
    var moduleProfilePictureUrl;
    var moduleProfilePictureWidth;
    var moduleProfilePictureHeight;
    var moduleProfilePictureMaxLength;
    var moduleProfilePictureErrorMessage;
    var moduleSupportedImageTypes

    module.initializeModule = function (profilePictureFieldName,
        profilePictureUrl,
        profilePictureWidth,
        profilePictureHeight,
        profilePictureMaxLength,
        profilePictureErrorMessage,
        supportedImageTypes) {
        moduleProfilePictureFieldName = profilePictureFieldName;
        moduleProfilePictureUrl = profilePictureUrl;
        moduleProfilePictureWidth = profilePictureWidth;
        moduleProfilePictureHeight = profilePictureHeight;
        moduleProfilePictureMaxLength = profilePictureMaxLength;
        moduleProfilePictureErrorMessage = profilePictureErrorMessage;
        moduleSupportedImageTypes = supportedImageTypes

        addPageFunctionality();
    }

    function addPageFunctionality() {
        $(document).ready(function () {
            $('#profile-picture').click(function () {
                $('#profile-picture-input').trigger('click');
            });

            $("#profile-picture-input").change(function () {
                changeProfilePicture(this);
            });

            function changeProfilePicture(input) {
                if (input.files && input.files[0]) {
                    var file = input.files[0];
                    var pictureValid = isPictureValid(file);

                    if (!pictureValid) {
                        resetProfileImage();
                    } else {
                        setNewProfilePicture(file);    
                    }
                    
                }
            }

            function resetProfileImage() {
                $('#profile-picture').attr('src', moduleProfilePictureUrl);
                $('#profile-picture-input').val('');
                $('#profile-picture-validation').text(moduleProfilePictureErrorMessage);
            }

            function setNewProfilePicture(file) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    var base64Image = e.target.result;
                    resizeBase64Img(base64Image,
                        moduleProfilePictureWidth,
                        moduleProfilePictureHeight).then(function (result) {
                            $('#profile-picture').attr('src', result[0].src);
                            $('#profile-picture-validation').text('');
                        });
                }
                reader.readAsDataURL(file);
            }

            function isPictureValid(file) {
                var mimeType = file['type'];
                var mimeSize = file['size'];
                if (!moduleSupportedImageTypes.contains(mimeType) || mimeSize > moduleProfilePictureMaxLength) {
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