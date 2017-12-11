﻿Array.prototype.contains = function (obj) {
    var i = this.length;
    while (i--) {
        if (this[i] === obj) {
            return true;
        }
    }
    return false;
}

$(".fade").fadeTo(2000, 500).slideUp(500, function () {
    $(".fade").slideUp(500);
});

var NavbarSearchModule = (function(module) {
    module.Initialize = function() {
        var options = [];

        $('ul.dropdown-menu > li > a').on('click',
            function(event) {

                var $target = $(event.currentTarget),
                    val = $target.attr('data-value'),
                    $inp = $target.find('input'),
                    idx;

                if ((idx = options.indexOf(val)) > -1) {
                    options.splice(idx, 1);
                    setTimeout(function() { $inp.prop('checked', false) }, 0);
                } else {
                    options.push(val);
                    setTimeout(function() { $inp.prop('checked', true) }, 0);
                }

                $(event.target).blur();

                console.log(options);
                return false;
            });
    }

    return module;
}({}));

var DeleteItemDialogs = (function (module) {
    module.InitializeDialogs = function (selector, itemName) {
        $(selector).each(function () {
            var self = this
            $(this).confirm({
                title: 'Delete this ' + itemName,
                content: 'Are you sure want to delete this ' + itemName + '?',
                buttons: {
                    yes: {
                        action: function () {
                            $(self).parents('form:first').submit()
                        }
                    },
                    no: {
                        action: function () {
                        }
                    }
                },
                closeIcon: true,
                type: 'red'
            });
        })
    }

    return module;
}({}))

var RegisterUserPageModule = (function (module) {
    var moduleProfilePictureFieldName;
    var moduleProfilePictureUrl;
    var moduleProfilePictureWidth;
    var moduleProfilePictureHeight;
    var moduleProfilePictureMaxLength;
    var moduleProfilePictureErrorMessage;
    var moduleSupportedImageTypes;

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