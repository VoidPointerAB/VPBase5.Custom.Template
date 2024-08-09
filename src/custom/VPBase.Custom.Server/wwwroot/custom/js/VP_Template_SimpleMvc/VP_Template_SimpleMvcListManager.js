var VP_Template_SimpleMvcListManager = function () {
    var _dataTable;

    function init() {
        _dataTable = $('.data-table-container table').DataTable(DataTableHelper.GetDefaultOptions({
            buttons: DataTableHelper.GetDefaultButtons("Simple_VP_Template_MVC"),
            columns: [
                { title: 'Title', data: 'title' },
                {
                    width: '120px',
                    data: null,
                    sortable: false,
                    searchable: false,
                    className: 'js-ignore-column-export',
                    render: function (data, type, row, meta) {
                        var html = '<div class="fixed-btn-content">';
                        html +=
                            '<button data-id="' + row.vP_Template_SimpleMvcId + '" data-name="' + row.title + '" class="btn btn-xs btn-danger delete-btn">' +
                                '<i class="fa fa-trash"></i> Delete' +
                            '</button>' +
                            '<a href="/Custom/VP_Template_SimpleMvc/Edit/' + row.vP_Template_SimpleMvcId + '" class="btn btn-primary btn-xs" >' +
                                '<i class="fa fa-pencil-square-o"></i> Edit' +
                            '</a>';

                        return html + '</div>';
                    }
                }
            ],
            ajax: {
                url: '/Custom/VP_Template_SimpleMvc/GetVP_Template_SimpleMvcListItems',
                data: function () { // Parameters that are sent in the request
                    return FilterHelper.GetFilter();
                },
                //dataSrc: function (json) { // Modify incoming data
                //    var data = json.data;
                //    for (var i = 0, ien = data.length; i < ien; i++) {
                //        data[i].title = data[i].title + ' more data';
                //    }
                //    return data;
                //}
            },
            initComplete: function () {
                FilterHelper.ApplyDataTableFilter(_dataTable);
            },
        }));

        $('#load-button').click(function () {
            FilterHelper.SaveFilter();
            _dataTable.ajax.reload();
        });

        $('.data-table-container table').on('click', '.delete-btn', deleteItem);
    }

    function deleteItem(e) {
        var button = $(e.currentTarget);

        AlertHelper.Confirm(
            {
                title: 'Delete ' + button.data('name') + '?',
                cancelButtonText: 'Cancel',
                confirmButtonText: 'Confirm',
            },
            function () {
                AjaxManager.Send('/Custom/VP_Template_SimpleMvc/Delete?id=' + button.data('id'), {
                    type: 'POST',
                    callback: function (data, success) {
                        if (success === true) {
                            _dataTable.row(button.closest('tr')).remove().draw(false);

                            toastr.success(button.data('name') + ' deleted');
                        }
                    }
                });
            });
    }

    return {
        Init: init,
    };
}();
