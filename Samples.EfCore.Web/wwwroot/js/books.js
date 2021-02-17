// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {

    var testTable = $("#dt-authors").DataTable({
        "dom": "Bfrtip",
        "processing": true,
        "serverSide": true,
        "filter": true,
        "orderMulti": false,
        "pageLength": 10,
        "ajax": {
            "url": "/Datatable/GetAll",
            "type": "POST",
            "datatype": "json"
        },
        "columns": [
            { "data": "Name" },
            { "data": "Country" },
            { "data": "Age" },
            { "data": "BooksPublished" }
        ],
        "language": {
            "lengthMenu": "_MENU_",
            "infoFiltered": "",
        },
    });
});