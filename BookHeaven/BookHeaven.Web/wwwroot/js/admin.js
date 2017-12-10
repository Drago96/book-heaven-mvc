function getFormData(form) {
    var unindexed_array = $(form).serializeArray();
    var indexed_array = {};

    $.map(unindexed_array, function (n, i) {
        indexed_array[n['name']] = n['value'];
    });

    return indexed_array;
}

var AllCategoriesModule = (function (module) {

    module.Initialize = function () {
        addCategoriesFilter();
        modifyEditButtons();
        modifyCreateButton();
    }

    function addCategoriesFilter() {
        $('#categories-filter').keyup(function () {
            var text = $(this).val()
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

    function modifyCreateButton()   {
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
                        content:'',
                        buttons: {
                            ok : {
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
                        return {
                            results: response
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
                }
            });
    }

    return module;
}({}))
