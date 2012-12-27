var bootstrapmvc = {};

var bootstrapmvc.grid = (function () {

    var onPrepareFunction;

    function generateHeaderCell(column) {
        var $th = $('<th />');

        if (column.isSortable) {
            $th.append($('<a />').attr('href', '#').attr('data-grid-column', column.sortableColumnName).text(column.name).prepend($('<i /> ')));
        }
        else {
            $th.text(column.name);
        }

        return $th;
    }

    function getVisibleColumnCount(columns) {
        var count = 0;

        $.each(columns, function (i, e) {
            if (!e.hidden) {
                count++;
            }
        });

        return count;
    }

    function generateTitleRow(data) {
        if (!data.title) {
            return $();
        }

        return $('<tr />').addClass('title').append($('<td />').attr('colspan', getVisibleColumnCount(data.columns)).html(data.title));
    }

    function generateHeaderRow(columns) {
        var ths = [];

        $.each(columns, function (i, e) {
            if (!e.hidden) {
                ths.push(generateHeaderCell(e)[0]);
            }
        });

        return $('<tr />').addClass('header').append($(ths));
    }

    function generateCol(column) {
        var $col = $('<col />');

        if (column.width) {
            $col.attr('width', column.width);
        }

        return $col;
    }

    function generateColGroups(columns) {
        var cols = [];

        $.each(columns, function (i, e) {
            if (!e.hidden) {
                cols.push(generateCol(e)[0]);
            }
        });

        return $('<colgroup />').append($(cols));
    }
    function generateDynamicContent(columns, column, row) {
        var value = column.content;

        for (key in columns) {
            var columnKey = columns[key].key;
            var re = new RegExp('\\~\\(' + columnKey + '\\)', 'g');
            value = value.replace(re, row[columnKey]);
        }

        return value;
    }

    function generateDataCell(columns, column, value, row) {
        var $html;
        
        if (column.content) {
            $html = generateDynamicContent(columns, column, row);
        } else {
            var val = String(value === null ? "" : value);
            $html = $('<span />').attr('title', val).html(val)
        }

        return $('<td />').append($html);
    }

    function generateDataRow(columns, item) {
        var tds = [];

        $.each(columns, function (i, e) {
            if (!e.hidden) {
                tds.push(generateDataCell(columns, e, item[e.key], item)[0]);
            }
        });

        return $('<tr />').append($(tds));
    }

    function generateDataRows(columns, items) {
        var trs = [];

        $.each(items, function (i, e) {

            trs.push(generateDataRow(columns, e)[0]);
        });

        return $(trs);
    }

    function updatePagingCell($grid, response) {
        var $pagingCell = $('tfoot td', $grid);
        $pagingdiv = $('<div />').addClass('pagination');
        $ul = $('<ul />');

        var pageNumber = response.PageNumber;
        var totalPages = response.TotalPages;

        $prev = $('<li>').append($('<a href="#"/>').text('«'));
        $next = $('<li>').append($('<a href="#"/>').text('»'));

        var pageRange = 5;
        var pagingRangeStart = 1;
        var pagingRangeEnd = pageRange;

        if (pageNumber + (pageRange / 2) > totalPages) {
            // we are at the end of the range...
            // show the last X values
            pagingRangeStart = totalPages - pageRange + 1;
            pagingRangeEnd = totalPages;
        }
        else if (pageNumber < pageRange) {
            pagingRangeStart = 1;
            pagingRangeEnd = pageRange;
        }
        else {
            pagingRangeStart = pageNumber - Math.floor(pageRange / 2);
            pagingRangeEnd = pageNumber + Math.floor(pageRange / 2);
        }

        if (pagingRangeEnd > totalPages) {
            pagingRangeEnd = totalPages;
        }

        if (pagingRangeStart <= 0) { pagingRangeStart = 1; }

        if (pageNumber == 1) {
            $prev.addClass('disabled');
        }
        else {
            $('a', $prev).attr('data-grid-page', pageNumber - 1);
        }

        if (pageNumber == totalPages || totalPages == 0) {
            $next.addClass('disabled');
        }
        else {
            $('a', $next).attr('data-grid-page', pageNumber + 1);
        }

        $ul.append($prev);

        if (pagingRangeStart > 1) {
            var $moreStart = $('<li>').addClass('disabled').append($('<a href="#"/>').text('...'));
            $ul.append($moreStart);
        }

        for (var i = pagingRangeStart; i <= pagingRangeEnd; i++) {
            var $li = $('<li />').append($('<a />').attr('data-grid-page', i).attr('href', '#').text(i)).addClass(i == pageNumber ? 'active' : '');
            $ul.append($li);
        }

        if (pagingRangeEnd < totalPages) {
            var $moreEnd = $('<li>').addClass('disabled').append($('<a href="#"/>').text('...'));
            $ul.append($moreEnd);
        }

        $ul.append($next);


        $pagingdiv.append($ul);

        if (response.TotalCount > 0) {
            var from = Math.min(((response.PageNumber - 1) * response.PageSize) + 1, response.TotalCount);
            var to = Math.min((response.PageNumber) * response.PageSize, response.TotalCount);
            $pagingdiv.append($('<span class="paging-text" />').text('Displaying ' + from + ' - ' + to + ' of ' + response.TotalCount));
        }
        $pagingCell.html($pagingdiv);


    }

    function addSerializedValuesToUrl(url, serializedValues) {
        if (url.indexOf('?') == -1) {
            url = url + '?' + serializedValues;
        }
        else {
            url = url + '&' + serializedValues;
        }

        return url;
    }

    function buildUrl($gridTable, data) {
        var url = data.url;

        if (data.filterByFormId) {
            var serializedFormValues = $('#' + data.filterByFormId).serialize();

            url = addSerializedValuesToUrl(url, serializedFormValues);
        }

        if (data.onBuildDataUrl) {

            url = eval(data.onBuildDataUrl + '($gridTable, url);');
        }

        return url;
    }

    function getGridData($grid) {
        return $grid.data('grid');
    }

    function reloadGridData($grid) {
        showLoading($grid);
        var $tbody = $('tbody', $grid);
        var data = getGridData($grid);
        var url = buildUrl($grid, data);
        var gridState = $grid.data('grid-state');

        $.ajax(url, { data: { PageNumber: gridState.pageNumber, PageSize: data.pageSize, SortBy: gridState.sortColumn, SortDirection: gridState.sortDirection },
            dataType: 'json',
            type: 'post',
            success: function (response) {
                $tbody.empty();

                var $dataHtml = generateDataRows(data.columns, response.Items);

                if ($dataHtml.length == 0 && data.emptyContent) {
                    $dataHtml = $('<tr />').append($('<td />').attr('colspan', getVisibleColumnCount(data.columns)).html(data.emptyContent));
                }

                if ($dataHtml.length < data.pageSize && gridState.pageNumber > 1) {
                    $dataHtml = $dataHtml.add(generateFillerRows(data.pageSize - $dataHtml.length, getVisibleColumnCount(data.columns)));
                }

                if (onPrepareFunction) {
                    onPrepareFunction($dataHtml);
                }

                $tbody.append($dataHtml);

                if (data.isPaged) {
                    updatePagingCell($grid, response);
                }

                //execute the callback function
                var fn = window[data.onShownFunctionCallback];
                if (typeof fn === 'function') {
                    fn();
                }
            }
        });
    }

    function showLoading($grid) {
        var data = getGridData($grid);

        if (data.loadingContent) {
            var colspan = getVisibleColumnCount(data.columns);
            var fillerCount = Math.ceil($('tbody tr', $grid).length - 1, 1);

            var filler = generateFillerRows(fillerCount, colspan);
            var $tbody = $('tbody', $grid);
            $tbody.empty().append($('<tr />').append($('<td />').attr('colspan', colspan).html(data.loadingContent))).append(filler);
        }
    }

    function generateFillerRows(rowCount, colspan) {
        var $fillerRows = $();
        for (var i = 0; i < rowCount; i++) {
            $fillerRows = $fillerRows.add($('<tr />').append($('<td />').html('&nbsp;').attr('colspan', colspan)));
        }

        return $fillerRows;
    }

    function changePage($grid, pageNumber) {
        var gridState = $grid.data('grid-state');
        gridState.pageNumber = pageNumber;
        $grid.data('grid-state', gridState);
    }

    function showSortingIcons($grid) {
        var gridState = $grid.data('grid-state');

        // set direction icons
        $('thead a[data-grid-column] i', $grid).removeClass('icon-arrow-down').removeClass('icon-arrow-up');
        $('thead a[data-grid-column="' + gridState.sortColumn + '"] i', $grid).addClass('icon-arrow-' + (gridState.sortDirection == 'ascending' ? 'up' : 'down'));
    }

    function changeSorting($grid, sortColumn, sortDirection) {
        var gridState = $grid.data('grid-state');

        if (gridState.sortColumn === sortColumn) {
            if (!sortDirection) {
                // Alternate sort direction
                gridState.sortDirection = gridState.sortDirection == 'ascending' ? 'descending' : 'ascending';
            } else {
                gridState.sortDirection = sortDirection == 'ascending' ? 'ascending' : 'descending';
            }
        }
        else {
            gridState.sortColumn = sortColumn;
            gridState.sortDirection = sortDirection == 'descending' ? 'descending' : 'ascending';
        }

        showSortingIcons($grid);

        $grid.data('grid-state', gridState);
    }

    function prepare($root) {
        $('table[data-grid]', $root).each(function (i, e) {
            var $grid = $(e);

            var data = getGridData($grid);
            var gridState = {
                pageNumber: 1,
                sortColumn: data.sortColumn,
                sortDirection: data.sortDirection
            };

            if (!data) {
                return;
            }

            $grid.append(generateColGroups(data.columns));

            $grid.append($('<thead />').append(generateTitleRow(data)).append(generateHeaderRow(data.columns)));
            $grid.append($('<tbody />'));

            var $tfoot = null;
            var $pagingCell = null;

            if (data.isPaged) {
                $pagingCell = $('<td />').attr('colspan', getVisibleColumnCount(data.columns));
                $tfoot = $('<tfoot />').append($('<tr />').append($pagingCell));
                $grid.append($tfoot);

                $pagingCell.delegate('a[data-grid-page]', 'click', function (event) {
                    event.preventDefault();
                    event.stopPropagation();
                    changePage($grid, $(this).data('grid-page'));
                    reloadGridData($grid);
                });
            }

            $grid.delegate('thead a[data-grid-column]', 'click', function (event) {
                event.preventDefault();
                event.stopPropagation();
                changeSorting($grid, $(this).data('grid-column'));
                reloadGridData($grid);
            });

            if (data.filterByFormId) {
                $('#' + data.filterByFormId).submit(function (event) {
                    event.preventDefault();
                    changePage($grid, 1);
                    reloadGridData($grid);
                });
            }

            // Save the grid state
            $grid.data('grid-state', gridState);

            // set the sort direction icons
            showSortingIcons($grid);

            if (!data.doNotAutoLoad) {
                reloadGridData($grid);
            }
            else {
                showLoading($grid);
            }
        });

        $('*[data-sort-grid]', $root).bind('click', function () {
            var $grid = $($(this).data('sort-grid'));
            changeSorting($grid, $(this).data('sort-grid-column'), $(this).data('sort-grid-column-direction'));
            reloadGridData($grid);

            ///alert($(this).data('sort-grid-column'));
        });
    }

    function refresh($gridTable, pageNumber) {
        $gridTable.each(function (i, e) {
            reloadGridData($(e));
        });
    }

    function onPrepare(newOnPrepareFunction) {
        onPrepareFunction = newOnPrepareFunction;
    }

    return {
        prepare: prepare,
        addSerializedValuesToUrl: addSerializedValuesToUrl,
        refresh: refresh,
        onPrepare: onPrepare
    };
} ());

var bootstrapmvc.dialog = (function () {

    var onPrepareFunction;

    function showResponse($dialogContent, response) {
        var $response = $('<div />').html(response);

        if (onPrepareFunction) {
            onPrepareFunction($response);
        }

        $createdContent = null;

        if ($('.modal-body', $response).length > 0) {
            $createdContent = $dialogContent.html($response);
        }
        else {
            $createdContent = $('.modal-body', $dialogContent).html(response);
        }

        $('input', $createdContent).first().focus();
    }

    function getInternetExplorerVersion()
        // Returns the version of Windows Internet Explorer or a -1
        // (indicating the use of another browser).
    {
        var rv = -1; // Return value assumes failure.
        if (navigator.appName == 'Microsoft Internet Explorer') {
            var ua = navigator.userAgent;
            var re = new RegExp("MSIE ([0-9]{1,}[\.0-9]{0,})");
            if (re.exec(ua) != null)
                rv = parseFloat(RegExp.$1);
        }
        return rv;
    }
    
    function show(data) {
        
        var $modal = $('<div />').addClass('modal hide');
        if (getInternetExplorerVersion() < 10) {
            $modal.addClass('fade');
        }
        var $modalHeader = $('<div />').addClass('modal-header')
                    .append($('<a />').addClass('close').attr('data-dismiss', 'modal').text('×'))
                    .append($('<h3 />').text(data.title));

        var $dialogContent = $('<div />').addClass('dialog-content');
        var $modalBody = $('<div />').addClass('modal-body');
        var $modalFooter = $('<div />').addClass('modal-footer');

        if (!data.contentUrl) {
            $modalBody.html($('<p />').html(data.content));
        }
        else {
            var loadingContent = data.loadingContent;

            if (!loadingContent) {
                loadingContent = $('<p />').text('Loading...');
            }

            $modalBody.html(loadingContent);
        }

        var buttonsReversed = data.buttons.slice(0).reverse();

        for (var buttonIndex in buttonsReversed) {
            var buttonData = buttonsReversed[buttonIndex];
            $button = $('<a />').addClass('btn').html(buttonData.text);

            if (buttonData.type != 'default') {
                $button.addClass('btn-' + buttonData.type);
            }

            if (buttonData.asPost && buttonData.url) {
                $button.attr('data-post-url', buttonData.url).attr('href', '#');
            }
            else if (buttonData.url) {
                $button.attr('href', buttonData.url);
            }

            if (buttonData.closesDialog) {
                $button.attr('data-dismiss', 'modal');
            }

            $modalFooter.append($button);
        }

        $modal.append($modalHeader).append($dialogContent.append($modalBody).append($modalFooter));

        $modal.delegate('form', 'submit', function (event) {
            var $form = $(this);
            event.preventDefault();
            
            $('input[type=submit],button[type=submit]', $form).attr('disabled', 'disabled');
            
            $.ajax($form.attr('action'), { type: $form.attr('method'),
                data: $form.serialize(),
                success: function (response) {
                    // response received
                    if (typeof (response) == 'string') {
                        showResponse($dialogContent, response);
                    }
                    else if (response.command == 'close') {
                        $modal.modal('hide');
                    }
                    else if (response.command == 'redirect' && response.parameter) {
                        window.location = response.parameter;
                    }
                }
            });
        });

        var options = { backdrop: data.showBackDrop, keyboard: data.closeOnEscape };
        $modal.modal(options);

        // make ajax call for content
        $.ajax(data.contentUrl, {
            type: 'get',
            success: function (response) {
                showResponse($dialogContent, response);
            }
        });
    }

    function onPrepare(prepareFunction) {
        onPrepareFunction = prepareFunction;
    }

    $(function () {
        $(document).on({
            click: function () {
                var data = $(this).data('dialoghelper');
                dialoghelper.show(data);
            }
        }, '*[data-dialoghelper]');
    });

    return { show: show, onPrepare: onPrepare };

} ());