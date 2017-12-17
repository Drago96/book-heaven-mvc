Array.prototype.contains = function (obj) {
    var i = this.length;
    while (i--) {
        if (this[i] === obj) {
            return true;
        }
    }
    return false;
}

function getFormData(form) {
    var unindexed_array = $(form).serializeArray();
    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });

    return indexed_array;
}

$(".fade").fadeTo(2000, 500).slideUp(500, function () {
    $(".fade").slideUp(500);
});

var AdminHomePageModule = (function (module) {
    module.Initialize = function (locations, visits) {
        new Chart(document.querySelector('canvas.visits-chart'), {
            type: 'doughnut',
            data: {
                labels: locations,
                datasets: [
                    {
                        label: "Most visits",
                        backgroundColor: ["#3e95cd", "#8e5ea2", "#3cba9f", "#e8c3b9", "#c45850"],
                        data: visits
                    }
                ]
            },
            options: {
                title: {
                    display: true,
                    text: 'Most site visits by country',
                    fontSize: 30
                }
            }
        });
    }

    return module;
}({}))

var AdminUsersListModule = (function (module) {
    module.Initialize = function () {
        $('.ui.search')
            .search({
                apiSettings: {
                    url: '/api/users?searchTerm={query}',
                    onResponse(response) {
                        var result = []
                        for (var key in response) {
                            if (key != undefined && key != 'contains') {
                                result.push(response[key])
                            }
                        }
                        return {
                            results: result
                        }
                    }
                },
                fields: {
                    title: 'name',
                    description: 'email',
                },
                minCharacters: 3,
                onSelect: function (result) {
                    $('#search-field').val(result.name)
                    $('#search-form').submit();
                },
            });
    }

    return module;
}({}))

var AllCategoriesModule = (function (module) {
    module.Initialize = function () {
        addCategoriesFilter();
        modifyEditButtons();
        modifyCreateButton();
    }

    function addCategoriesFilter() {
        $('#categories-filter').keyup(function () {
            var text = $(this).val();
            $('div .category-item').each(function () {
                var category = $(this).find('.category-label').text()
                if (category.indexOf(text) === -1) {
                    $(this).hide();
                } else {
                    $(this).show();
                }
            });
        });
    }

    function modifyCreateButton() {
        $('#create-category-button').confirm({
            title: 'Create category',
            content: '' +
            '<form method="post" action="" class="category-create-form">' +
            '<div class="form-group">' +
            '<label>Name</label>' +
            '<input name="name" type="text" value="" class="name form-control" required />' +
            '<span class="text-danger" style="display:none" id="category-error"></span>' +
            '</div>' +
            '</form>',
            buttons: {
                formSubmit: {
                    text: 'Create',
                    btnClass: 'btn-success',
                    action: function () {
                        createCategory(this)
                        return false;
                    }
                },
                cancel: function () {
                },
            },
            onContentReady: function () {
                var jc = this;
                this.$content.find('form').on('submit', function (e) {
                    e.preventDefault();
                    jc.$$formSubmit.trigger('click');
                });
            }
        });

        function createCategory(dialog) {
            $.ajax({
                url: '/api/categories/',
                contentType: 'application/json',
                type: 'Post',
                data: JSON.stringify(getFormData('.category-create-form')),
                success: function () {
                    dialog.close()
                    $.alert({
                        title: 'Category created successfully!',
                        content: '',
                        buttons: {
                            ok: {
                                action: function () {
                                    this.close()
                                    window.location.replace("/admin/categories/all")
                                }
                            }
                        }
                    });
                },
                error: function (response) {
                    var responseText
                    if (response.responseJSON) {
                        responseText = response.responseJSON['Name']
                    } else {
                        responseText = response.responseText
                    }

                    $('#category-error').text(responseText);
                    $('#category-error').show();
                }
            });
        }
    }

    function modifyEditButtons() {
        $('.edit-icon').each(function () {
            var categoryName = $(this).attr('category-name')
            var categoryId = $(this).attr('category-id')
            $(this).confirm({
                title: 'Edit category',
                content: '' +
                '<form method="post" action="" class="category-edit-form">' +
                '<div class="form-group">' +
                '<label>Name</label>' +
                '<input name="name" type="text" value="' + categoryName + '" class="name form-control" required />' +
                '<span class="text-danger" style="display:none" id="category-error"></span>' +
                '</div>' +
                '</form>',
                buttons: {
                    formSubmit: {
                        text: 'Edit',
                        btnClass: 'btn-warning',
                        action: function () {
                            updateCategory(categoryId, this)
                            return false;
                        }
                    },
                    cancel: function () {
                    },
                },
                onContentReady: function () {
                    var jc = this;
                    this.$content.find('form').on('submit', function (e) {
                        e.preventDefault();
                        jc.$$formSubmit.trigger('click');
                    });
                }
            });
        });

        function updateCategory(categoryId, dialog) {
            $.ajax({
                url: '/api/categories/' + categoryId,
                contentType: 'application/json',
                type: 'PUT',
                data: JSON.stringify(getFormData('.category-edit-form')),
                success: function () {
                    dialog.close()
                    $.alert({
                        title: 'Category edited successfully!',
                        content: '',
                        buttons: {
                            ok: {
                                action: function () {
                                    this.close()
                                    window.location.replace("/admin/categories/all")
                                }
                            }
                        }
                    });
                },
                error: function (response) {
                    var responseText
                    if (response.responseJSON) {
                        responseText = response.responseJSON['Name']
                    } else {
                        responseText = response.responseText
                    }

                    $('#category-error').text(responseText);
                    $('#category-error').show();
                }
            });
        }
    }

    return module;
}({}));

var DeleteItemDialogs = (function(module) {
    module.InitializeDialogs = function(selector, itemName) {
        $(selector).each(function() {
            var self = this;
            $(this).confirm({
                title: 'Delete this ' + itemName,
                content: 'Are you sure want to delete this ' + itemName + '?',
                buttons: {
                    yes: {
                        action: function() {
                            $(self).parents('form:first').submit()
                        }
                    },
                    no: {
                        action: function() {
                        }
                    }
                },
                closeIcon: true,
                type: 'red'
            });
        });
    }

    return module;
}({}));

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

var UserFormModule = (function(module) {
    var moduleProfilePictureFieldName;
    var moduleProfilePictureUrl;
    var moduleProfilePictureWidth;
    var moduleProfilePictureHeight;
    var moduleProfilePictureMaxLength;
    var moduleProfilePictureErrorMessage;
    var moduleSupportedImageTypes;

    module.initializeModule = function(profilePictureFieldName,
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
        $(document).ready(function() {
            $('#profile-picture').click(function() {
                $('#profile-picture-input').trigger('click');
            });

            $('#profile-picture-input').change(function() {
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
                reader.onload = function(e) {
                    var base64Image = e.target.result;
                    resizeBase64Img(base64Image,
                        moduleProfilePictureWidth,
                        moduleProfilePictureHeight).then(function(result) {
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
                $("<img/>").attr("src", base64).load(function() {
                    context.scale(width / this.width, height / this.height);
                    context.drawImage(this, 0, 0);
                    deferred.resolve($("<img/>").attr("src", canvas.toDataURL()));
                });
                return deferred.promise();
            }
        });
    }

    return module;
}({}));
