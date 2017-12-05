var RegisterUserPageModule = (function (module) {

    var moduleProfilePictureFieldName;
    var moduleProfilePictureUrl;
    var moduleProfilePictureWidth;
    var moduleProfilePictureHeight;
    var moduleProfilePictureMaxLength;

    module.initializeModule = function (profilePictureFieldName,
        profilePictureUrl,
        profilePictureWidth,
        profilePictureHeight,
        profilePictureMaxLength) {

        moduleProfilePictureFieldName = profilePictureFieldName;
        moduleProfilePictureUrl = profilePictureUrl;
        moduleProfilePictureWidth = profilePictureWidth;
        moduleProfilePictureHeight = profilePictureHeight;
        moduleProfilePictureMaxLength = profilePictureMaxLength;

        addPageFunctionality();
    }

    function addPageFunctionality() {
        $(document).ready(function () {
            addProfilePictureValidation();
            changeProfilePicture(document.getElementById("profile-picture-input"));

            $('#profile-picture').click(function () {
                $('#profile-picture-input').trigger('click');
            });

            $("#profile-picture-input").change(function () {
                changeProfilePicture(this);
            });

            function addProfilePictureValidation() {
                jQuery.validator.addMethod("ProfilePicture",
                    function (value, element) {
                        if (element.files && element.files[0]) {
                            var file = element.files[0];
                            var mimeType = file['type'];
                            var mimeSize = file['size'];
                            if (mimeType.split('/')[0] !== 'image' || mimeSize > moduleProfilePictureMaxLength) {
                                return false;
                            }
                        }
                        return true;
                    },
                    'Invalid file. Please upload an image with size less than 2MB.');
                $("input[name=" + moduleProfilePictureFieldName + "]").rules('add',
                    {
                        ProfilePicture: true
                    });

                //$("input[name=" + moduleProfilePictureFieldName + "]").validate({
                //    rules: {
                //        validUpload: { ProfilePicture: true }
                //    }
                //});
            }

            function changeProfilePicture(input) {
                if (input.files && input.files[0]) {
                    var file = input.files[0];
                    var mimeType = file['type'];
                    var mimeSize = file['size'];
                    if (mimeType.split('/')[0] !== 'image' || mimeSize > moduleProfilePictureMaxLength) {
                        $('#profile-picture').attr('src', moduleProfilePictureUrl);
                        return;
                    }

                    var reader = new FileReader();

                    reader.onload = function (e) {
                        var base64Image = e.target.result;
                        resizeBase64Img(base64Image,
                            moduleProfilePictureWidth,
                            moduleProfilePictureHeight).then(function (result) {
                                $('#profile-picture').attr('src', result[0].src);
                            });
                    }

                    reader.readAsDataURL(file);
                }
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