
let refModal = '';

$(function () {
    loadReferenceDataTableItems();

});

function setModalmessage(message){
    $("#refDataModal").find("#msg-body").text(message);
}

function saveDatafixTable() {

    let isValid = true;
    $("#form-entry-datafix").find(":input:visible").each(function () {
        if (!$(this).valid()){
            isValid = false;
        }
    });

    if (!isValid) return false;

    let dto = {

        ID: $('#hdnDataFixTableID').val(),
        TABLE_VALUE: $('#TABLE_VALUE').val(),
        TABLE_NAME: $('#TABLE_NAME').val(),
        TABLE_TYPE: $('#TABLE_TYPE').val(),
        IS_VISIBLE: $('#IS_VISIBLE').prop('checked'),
        PK_COLUMN: $('#PK_COLUMN').val(),
        COLUMNS_HIDE: $('#COLUMNS_HIDE').val(),
        READONLY_COLUMNS: $('#READONLY_COLUMNS').val()
    };

    ConfigurationService.saveRowItem(dto, function (response) {
        
        showModal({
            title: "Save",
            message: response.message,
            buttons: [
                { label: "Ok", class: "btn btn-secondary popup-button" }
            ]
        });

       
        if (!response.success) return false;

        loadReferenceDataTableItems();
        clearFormVals();
        
        return true; //success operation
    });
}

function deleteDataFixTableItem(id) {

    showModal({
        title: "Deleting Item",
        message: "WARNING: This will permanently delete this row and cannot be undone. Would you like to continue?",
        buttons: [
            { label: "Cancel", class: "btn btn-secondary popup-button" },
            { label: "Delete", class: "btn btn-primary popup-button", callback: ()=> { _deleteRow(id) } }
        ]
    });
}

function _deleteRow(id) {

    ConfigurationService.deleteRowItemById(id, function (response) {
        console.log(response);

        if (typeof response === "string") {
            response = JSON.parse(response);
        }

        showModal({
            title: "Delete",
            message: response.message,
            buttons: [
                { label: "Ok", class: "btn btn-secondary popup-button" }
            ]
        });

        if (response.success) {
            loadReferenceDataTableItems();
        }

        //success operation
        return true;
    });

    return false; //stop propegatioons
}

function editDataFixTableItem(id) {
    ConfigurationService.getDataFixDataItemForEdit(id, function (response) {

        $('#hdnDataFixTableID').val(response.ID);
        $('#TABLE_VALUE').val(response.TABLE_VALUE);
        $('#TABLE_NAME').val(response.TABLE_NAME);
        $('#TABLE_TYPE').val(response.TABLE_TYPE);
        $('#IS_VISIBLE').prop('checked', response.IS_VISIBLE);
        $('#PK_COLUMN').val(response.PK_COLUMN);
        $('#COLUMNS_HIDE').val(response.COLUMNS_HIDE);
        $('#READONLY_COLUMNS').val(response.READONLY_COLUMNS);
        $('#modify-form').collapse('show');
    });
}

/* REGION : Grid Functions */
function loadReferenceDataTableItems() {
    ConfigurationService.getDataFixTableList(function (response) {
        renderDataFixTableGrid(response);
    });
}

function clearFormVals() {

    $('#modify-form').collapse('hide');

    $("#modify-form").find("input[type=checkbox]")
        .prop("checked", false);

    // Optionally clear textareas
    $("#modify-form").find("input[type=text]").val("");
    $("#modify-form").find("input[type=hidden]").val("0");

    clearAllValidationErrorsForContainer("#modify-form"); //clears errors || can also call ForForm function at form level validation

    return true;
}

function renderDataFixTableGrid(data) {
    $('#datafix-tables-grid-container')
        .removeClass('d-none')
        .dataGrid({

            data: data,
            columns: [

                { key: 'TABLE_NAME', title: 'Table Name', type: 'text', sortable: true },
                { key: 'TABLE_VALUE', title: 'Table Value', type: 'text', sortable: true },
                { key: 'TABLE_TYPE', title: 'Type', type: 'text', sortable: true },
                { key: 'IS_VISIBLE', title: 'Is Visible', type: 'text', sortable: true },
                { key: 'PK_COLUMN', title: 'Primary Key Column', type: 'text', sortable: true },
                { key: 'COLUMNS_HIDE', title: 'Columns To Hide', type: 'text', sortable: true },
                { key: 'READONLY_COLUMNS', title: 'Read Only Columns', type: 'text', sortable: true },
                {
                    title: 'Actions',
                    type: 'text',
                    sortable: true,
                    cellTemplate: function (row) {
                        return `
                        <td><div class="dropdown pd-dropdown">
                         <button class="dropdown-button border-0 three-dots-btn"
                                          data-bs-toggle="dropdown"
                                          data-bs-auto-close="outside"
                                          aria-expanded="false"
                                          type="button"
                                          title="Actions">
                                    <span class="material-symbols-outlined">more_vert</span>
                                  </button>
                                  <ul class="dropdown-menu">
                                  <li>
                                      <a href="javascript:void(0)"
                                        class="dropdown-item"
                                        onclick="editDataFixTableItem(${ row.ID })">
                                        Edit
                                    </a>
                                    </li>
                                     <li>
                                      <a href="javascript:void(0)"
                                        class="dropdown-item"
                                        onclick="deleteDataFixTableItem(${row.ID})">
                                        Delete
                                    </a>
                                    </li>
                        </ul></div>`;
                    }
                },
            ],

            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No rows found.',
            idProperty: 'tblDataFixTablesList',

            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,

            dateFormat: 'MM-DD-YYYY',
            includeTime: false
        });

}
