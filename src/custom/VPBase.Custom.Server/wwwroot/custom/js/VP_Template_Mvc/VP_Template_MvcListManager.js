var VP_Template_MvcListManager = function () {
    var _dataTable;

    function init() {

        ColumnManager.Init("VP_Template_MvcList_Columns");
        var chosenColumns = ColumnManager.GetColumnsToShow();
        var columnIndex = 0;
        var columnDefs = [];

        columnDefs.push({
            "targets": columnIndex++, "responsivePriority": 1, "data": "title", "className": "filter-column",
            "render": function (data, type, row, meta) {
                return '<div class="no-wrap">' + DataTableHelper.AddColumnDataFilterHandler(data, meta.col) + '</div>';
            }
        });
        $('.data-table-container thead tr:eq(0)').append('<td></td>');
        $('.data-table-container thead tr:eq(1)').append('<th>Title</th>');

        // ----

        if (ColumnManager.ContainsColumn(chosenColumns, 'column-category')) {
            columnDefs.push({
                "targets": columnIndex++, "responsivePriority": 1, "data": "category", "className": "filter-column", "width": "100px",
                "render": function (data, type, row, meta) {
                    return '<div class="no-wrap">' + DataTableHelper.AddColumnDataFilterHandler(data, meta.col) + '</div>';
                }
            });
            $('.data-table-container thead tr:eq(0)').append('<td></td>');
            $('.data-table-container thead tr:eq(1)').append('<th>Category</th>');
        }

        // ---

        if (ColumnManager.ContainsColumn(chosenColumns, 'column-description')) {
            columnDefs.push({
                "targets": columnIndex++, "responsivePriority": 1, "data": "description", "className": "filter-column",
                "render": function (data, type, row, meta) {
                    return data;
                }
            });
            $('.data-table-container thead tr:eq(0)').append('<td></td>');
            $('.data-table-container thead tr:eq(1)').append('<th>Description</th>');
        }

        // ---

        if (ColumnManager.ContainsColumn(chosenColumns, 'column-internal-id')) {
            columnDefs.push({
                "targets": columnIndex++, "responsivePriority": 1, "data": "vP_Template_MvcId",
                "render": function (data, type, row, meta) {
                    return data;
                }
            });
            $('.data-table-container thead tr:eq(0)').append('<td></td>');
            $('.data-table-container thead tr:eq(1)').append('<th>Internal Id</th>');
        }

        // ---

        if (ColumnManager.ContainsColumn(chosenColumns, 'column-created-utc')) {
            columnDefs.push({
                "targets": columnIndex++, "responsivePriority": 1, "data": "createdUtc", type: 'date',
                "render": function (data, type, row, meta) {
                    return data;
                }
            });
            $('.data-table-container thead tr:eq(0)').append('<td></td>');
            $('.data-table-container thead tr:eq(1)').append('<th>Created Utc</th>');
        }

        // ---

        if (ColumnManager.ContainsColumn(chosenColumns, 'column-modified-utc')) {
            columnDefs.push({
                "targets": columnIndex++, "responsivePriority": 1, "data": "modifiedUtc", type: 'date',
                "render": function (data, type, row, meta) {
                    return data;
                }
            });
            $('.data-table-container thead tr:eq(0)').append('<td></td>');
            $('.data-table-container thead tr:eq(1)').append('<th>Modified Utc</th>');
        }

        // ---

        columnDefs.push({
            targets: columnIndex++,
            width: '200px',
            data: null,
            sortable: false,
            searchable: false,
            className: 'js-ignore-column-export',
            render: function (data, type, row, meta) {
                var html = '<div class="fixed-btn-content">';
                html +=
                    '<button data-id="' + row.vP_Template_MvcId + '" data-name="' + row.title + '" class="btn btn-xs btn-danger delete-btn">' +
                    '<i class="fa fa-trash"></i> ' + LocalizationHelper.GetTranslation('Delete') +
                    '</button>' +
                    '<a href="/Custom/VP_Template_Mvc/Show/' + row.vP_Template_MvcId + '" class="btn btn-default btn-xs" >' +
                    '<i class="fa fa-search"></i> Show</a>' +
                    '<a href="/Custom/VP_Template_Mvc/Edit/' + row.vP_Template_MvcId + '" class="btn btn-primary btn-xs" >' +
                    '<i class="fa fa-pencil-square-o"></i> ' + LocalizationHelper.GetTranslation('Edit') +
                    '</a>';

                return html + '</div>';
            }
        });
        $('.data-table-container thead tr:eq(0)').append('<td></td>');
        $('.data-table-container thead tr:eq(1)').append('<th>Title</th>');

        var columns = CustomFieldListColumnManager.ApplyCustomFieldColumns(columnDefs);

        _dataTable = $('.data-table-container table').DataTable(DataTableHelper.GetDefaultOptions({
            columns: columns,
            ajax: {
                url: '/Custom/VP_Template_Mvc/GetVP_Template_MvcListItems',
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
            drawCallback: function () {
                DataTableHelper.FilterLinksBindClickEventInColumns();
            },
            initComplete: function () {
                FilterHelper.ApplyDataTableFilter(_dataTable);

                //CustomFieldListColumnManager.Init(_dataTable);

                DataTableHelper.InitColumnHeaderFilters('.data-table-container table');
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
                title: LocalizationHelper.GetTranslation('Delete') + ' ' + button.data('name') + '?',
                cancelButtonText: LocalizationHelper.GetTranslation('Cancel'),
                confirmButtonText: LocalizationHelper.GetTranslation('Confirm'),
            },
            function () {
                AjaxManager.Send('/Custom/VP_Template_Mvc/Delete?id=' + button.data('id'), {
                    type: 'POST',
                    callback: function (data, success) {
                        if (success === true) {
                            _dataTable.row(button.closest('tr')).remove().draw(false);

                            toastr.success(button.data('name') + ' ' + LocalizationHelper.GetTranslation('deleted'));
                        }
                    }
                });
            });
    }

    return {
        Init: init,
    };
}();

