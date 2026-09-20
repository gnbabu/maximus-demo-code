
//holds the model for edit or insert
let confDataModal = '';
//holds the rules form the reference data entry. Hidden columns, read only etc. This is the data from prio
//reference data page row object of this table
let configurationTableProps = {};
/* holds the schema and data for the current context table

    EXAMPLE: 
    {
        Table: 'TABLE_NAME'
        Columns: [Array of column schema such as name, datatype, nullable etc]
        TableObj: [data rows for table]
    }
*/
let tableTabaModel = '';
let userId = '';
let hideColumns = [];
let readOnlyColumns = [];

//utility
function escapeHtml(val) {

    if (typeof val !== "string") return val;
    
    return val
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;")
        .replace(/'/g, "&#39;");
}

function setModalmessage(message) {
    $("#confDataModal").find("#msg-body").text(message);
}

//load dropdown options
function loadConfigurationDataDropDownTableOptions() {
    console.log("Calling dropdown load");
    ConfigurationDataService.loadConfigurationDataDropDownItems(function (data) {

        let menu = $('#ddlConfigurationTables');
        menu.empty();


        if (!data || data.length < 1) {
            menu.append(`<option>No Results Found</option>`);
        }
        else {
            menu.append(`<option selected disabled>Select One</option>`);

            data.forEach(function (item) {
                menu.append(
                    `<option value="${item["TABLE_NAME"]}">${item["TABLE_VALUE"]}</option>`);
            });
        }
    });
}

function buildFormJson() {
    const returnObj = {};

    tableTabaModel.Columns.forEach(col => {
        const field = document.querySelector(`[name="${col.Name}"]`); //maps field name to the column name
        returnObj[col.Name] = field ? field.value : null;
    });

    return returnObj;
}

function saveConfigurationData() {

    let isValid = true;
    $("#form-entry-configurationdata-rows").find(":input:visible").each(function () {
        if (!$(this).valid()) {
            isValid = false;
        }
    });

    let primaryKey = tableTabaModel.Columns.filter(col => col.IsPrimaryKey)[0].Name;
    let dataModel = buildFormJson();

    let dtoModel = {
        Id: dataModel[primaryKey],
        PrimaryKeyColumn: primaryKey,
        TableName: tableTabaModel.Table,
        JsonRow: JSON.stringify(dataModel),
        Action: $("#Action").val()
    };

    if (!isValid) return false;


    ConfigurationDataService.saveConfigurationDataItem(dtoModel, function (response) {

        showModal({
            title: "Save",
            message: response.message,
            buttons: [
                { label: "Ok", class: "btn btn-secondary popup-button" }
            ]
        });


        if (!response.success) return false;

        let selectedTable = $("#ddlConfigurationTables").val();

        loadConfigurationDataTableRecords(selectedTable);
        clearFormVals();

        return true; //success operation
    });
}

function deleteConfigurationDataRowItemById(row) {

    showModal({
        title: "Deleting Item",
        message: "WARNING: This will permanently delete this row and cannot be undone. Would you like to continue?",
        buttons: [
            { label: "Cancel", class: "btn btn-secondary popup-button" },
            { label: "Delete", class: "btn btn-primary popup-button", callback: () => { _deleteRow(row) } }
        ]
    });
}

function _deleteRow(data) {
    let row = JSON.parse(data.dataset.row);
    let primaryKey = tableTabaModel.Columns.filter(col => col.IsPrimaryKey)[0].Name;
    
    let dtoModel = {
        Id: row[primaryKey].toString(),
        PrimaryKeyColumn: primaryKey,
        TableName: tableTabaModel.Table,
        JsonRow: JSON.stringify(row),
        Action: 'delete'
    };

    ConfigurationDataService.saveConfigurationDataItem(dtoModel, function (response) {

        showModal({
            title: "Delete",
            message: response.message,
            buttons: [
                { label: "Ok", class: "btn btn-secondary popup-button" }
            ]
        });


        if (response.success) loadConfigurationDataTableRecords(tableTabaModel.Table);

        //success operation
        return true;
    });

    return false; //stop propegatioons
}

function clearFormVals() {

    toggleEditForm(false);

    $("#modify-form").find("input[type=checkbox]")
        .prop("checked", false);

    // Optionally clear textareas
    $("#modify-form").find("input[type=text]").val("");
    $("#modify-form").find("input[type=hidden]").val("0");

    clearAllValidationErrorsForContainer("#modify-form"); //clears errors || can also call ForForm function at form level validation

    return true;
}

function loadConfigurationDataTableRowsAndModel(data) {
    $('#configuration-data-table-grid-container')
        .removeClass('d-none')
        .dataGrid({
            //dynamic grid does not define
            data: data,
            autoGenerateActions: true,
            callBackEditFunction: 'editConfigurationDataTableItem',
            callBackDeleteFunction: 'deleteConfigurationDataRowItemById', 
            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No rows found.',
         
            enableColumnFilters: true,
            enableSorting: true,
            dateFormat: 'MM-DD-YYYY',
            includeTime: false
        });

    //apply configuration rules
    let columnsToHide = configurationTableProps.COLUMNS_HIDE?.toUpperCase().split(",") ?? [];
    if(columnsToHide.length > 0) hideColumnsByHeaders(columnsToHide);

}

function hideColumnsByHeaders(headersToHide) {

    let table = document.getElementsByClassName('maximus-base-table')[0];

    if (table == null) return;

    let headerCells = table.querySelectorAll("th");
    let targets = headersToHide.map(h => h.trim().toLowerCase());
    let columnIndexes = [];

    headerCells.forEach((th, index) => {
        
        let span = th.querySelector('[data-key]');
        if (span && targets.includes(span.dataset.key.toLowerCase())) {
            th.style.display = 'none';

            for (let row of table.rows) {
                if (row.cells[index]) {
                    row.cells[index].style.display = "none";
                }
            }

        }

        
    });

}


//this handles building a dynamic form BASED
//on what ever the data schema is that comes into the grid.
function editConfigurationDataTableItem(data) {
    let row = JSON.parse(data.dataset.row);

    //build form
    $("#dynamic-form").empty();

    $("#dynamic-form").append("<div class='row' id='hdnInputs></div>");

    $("#dynamic-form").append(buildHtmlFormForDataRowforEdit(row));

    let rules = buildRules(tableTabaModel.Columns); 
    let messages = buildMessages(tableTabaModel.Columns); 

    //apply a reinit of validator with new settings for a dynmamic form
    $("#aspnetForm").validate({
        errorClass: "text-danger",
        onfocusout: function (element) {  
            this.element(element);
        },
        errorPlacement: function (error, element) {
            // Find the existing span next to the input and set its text
            element.siblings(".error-message").text(error.text());
        },
        success: function (label, element) {
            // Clear the error text when valid
            $(element).siblings(".error-message").text("");
        },
        rules: rules,
        messages: messages,
        highlight: function (element, errorClass) {
            $(element).removeClass(errorClass); //since resetting validator, by default it highlights the related input. We only want the error span
        }
    });

    //now load the dynamic validations for the form inputs
    Object.keys(rules).forEach(function (colName) {
        $("[name='" + colName + "']").rules("add", rules[colName]);
    });

    toggleEditForm(true);
}

/*
Build validation rules for a dynamic form

*/
function buildRules(columns) {
    const rules = {};
    columns.forEach(c => {
        rules[c.Name] = {
            required: !c.IsNullable,
            maxlength: (c.MaxLength === -1 || c.MaxLength == null) ? undefined : c.MaxLength, //needed because sql shema reports back a -1 for MAX
            number: c.DataType === "int" || c.DataType === "decimal" || c.DataType === "float",
        };
    });
    return rules;
}

/*
Build validation error messages for a dynamic form.
*/
function buildMessages(columns) {
    const messages = {};

    columns.forEach(col => {
   
        // Required rule
        if (!col.IsNullable) {
            messages[col.Name] = { required: `${col.Name} is required` };
        }

        // Max length rule
        if (col.MaxLength && col.MaxLength > 0) {
            messages[col.Name] = messages[col.Name] || {};
            messages[col.Name].maxlength = `${col.Name} max length is ${col.MaxLength}`;
        }

        // Numeric rule
        if (col.DataType === "int" || col.DataType === "decimal" || col.DataType === "float") {
            messages[col.Name] = messages[col.Name] || {};
            messages[col.Name].number = `${col.Name} must be numeric`;
        }
    });

    return messages;
}

/*
   Builds dynamic form based on the row data being edited
*/
function buildHtmlFormForDataRowforEdit(row) {
    let returnHtml = '';
    let counter = 0;
    let hdnHtml = '';


    //now loop each field to build the form
    Object.keys(row).forEach((key) => {
        let val = row[key];
        let colRules = tableTabaModel.Columns.filter(col => col.Name === key)[0]; //get rules key

        let isDisabled = (readOnlyColumns.includes(key) || colRules.IsIdentity || colRules.IsPrimaryKey_) ? 'disabled' : ''; 

        hdnHtml += `<input type="hidden" id="Action" name="Action" class="form-control" value = "edit" />`;

        //hide as hidden input if set to hide
        if (hideColumns.some( k => k.toUpperCase() == key)) {
            hdnHtml += `<input type="hidden" id="${key}" name="${key}" class="form-control" value = "${val ?? ""}" />`;
        }
        else {

            if (counter % 2 === 0) returnHtml += `<div class="form-row mb-5">`;
            
            if (colRules.DataType === 'datetime') {
                let uiDate = new Date(val);
                returnHtml += `<div class="col-md-6">
                                 <label for="${key}" class="form-label">${key} </label>
                                 <input type="date" id="${key}" name="${key}" ${isDisabled} class="form-control" value="${uiDate.toISOString().split("T")[0] ?? ""}">
                                  <!-- error span -->
                                  <span class="text-danger field-validation-valid d-inline-block min-h-10 error-message" data-valmsg-for="${key}" data-valmsg-replace="true"></span>
                            </div>`;
            }
            else {             
                let encodedValue = (val !== null && val !== undefined) ? escapeHtml(val.toString()) : "";

                returnHtml += `<div class="col-md-6">
                                 <label for="${key}" class="form-label">${key} </label>
                                 <input type="text" id="${key}" name="${key}" ${isDisabled} class="form-control" value="${encodedValue}">
                                 <!-- error span -->
                                 <span class="text-danger field-validation-valid d-inline-block min-h-10 error-message" data-valmsg-for="${key}" data-valmsg-replace="true"></span>
                                 </div>`;
            }

            counter += 1;
        }

        if (counter % 2 === 0) {
            returnHtml += `</div>`;
        }
    });

    returnHtml += hdnHtml;

    return returnHtml;
}

function buildHtmlFormForDataRowforAdd() {
    let returnHtml = '';
    let hdnHtml = '';
    let counter = 0;
    let schema = tableTabaModel.Columns;

    Object.keys(schema).forEach((key) => {
        let val = schema[key];

        let isDisabled = (readOnlyColumns.includes(val.name) || (val.IsIdentity && val.IsPrimaryKey)) ? 'disabled' : '';

        

        hdnHtml += `<input type="hidden" id="Action" name="Action" class="form-control" value = "insert" />`;

        //hide as hidden input if set to hide
        if (val.IsPrimaryKey && val.IsIdentity) {
            hdnHtml += `<input type="hidden" id="${val.Name}" name="${val.Name}" class="form-control" value = "0" />`;
        }
        else if (hideColumns.some(k => k.toUpperCase() == key)) {
            hdnHtml += `<input type="hidden" id="${val.Name}" name="${val.Name}" class="form-control" value = "" />`;
        }
        //else if (val.Name == "CREATED_BY_USER" || val.Name == "LAST_MODIFIED_USER") {
        //    hdnHtml += `<input type="hidden" id="${val.Name}" name="${val.Name}" class="form-control" value = "${userId}" />`;
        //}
        else {

            if (counter % 2 === 0) returnHtml += `<div class="form-row mb-5">`;

            if (val.DataType === 'datetime') {
                let uiDate = getDateToday();
                returnHtml += `<div class=" col-md-6">
                                 <label for="${val.Name}" class="form-label">${val.Name} </label>
                                 <input type="date" id="${val.Name}" name="${val.Name}" disabled class="form-control" value="${uiDate}">
                                  <!-- error span -->
                                  <span class="text-danger field-validation-valid d-inline-block min-h-10 error-message" data-valmsg-for="${val.Name}" data-valmsg-replace="true"></span>
                            </div>`;
                counter += 1;

            }
            else {

                returnHtml += `<div class=" col-md-6">
                                 <label for="${val.Name}" class="form-label">${val.Name} </label>
                                 <input type="text" id="${val.Name}" name="${val.Name}" ${isDisabled} class="form-control" value="">
                                 <!-- error span -->
                                 <span class="text-danger field-validation-valid d-inline-block min-h-10 error-message" data-valmsg-for="${key}" data-valmsg-replace="true"></span>
                                 </div>`;
                counter += 1;

            }
        }

        if (counter % 2 === 0) {
            returnHtml += `</div>`;
        }

    });


        returnHtml += hdnHtml;
    return returnHtml;
}


function mapSqlTypeToValidatorRules(sqlType) {
    switch (sqlType) {
        case "bool": return { bool: true };
        case "int": return { number: true };
        case "decimal": return { number: true };
        case "float": return { number: true };
        case "varchar": return { maxlength: 255 };
        case "datetime": return { date: true };
        default: return {};
    }
}

function toggleEditForm(showForm) {

    showForm === true ? $('#modify-form').collapse('show') : $('#modify-form').collapse('hide');

}

function showClientLoader() {
    $("#client-loader").removeClass("d-none");
}
function hideClientLoader() {
    $("#client-loader").addClass("d-none");
    $("#client-loader").addClass("d-none"); 
}

function loadConfigurationDataTableRecords(tableName) {
    ConfigurationDataService.getDataFixTablePropertiesByTableName(tableName, function (data) {

        configurationTableProps = data.Table[0];

        //now pull in data fix setting entry and apply, hidden columns, read only etc.
        hideColumns = (configurationTableProps.COLUMNS_HIDE || "")
            .split(",")
            .map(v => v.trim())
            .filter(Boolean);

        //now pull in data fix setting entry and apply, hidden columns, read only etc.
        readOnlyColumns = (configurationTableProps.READONLY_COLUMNS || "")
            .split(",")
            .map(v => v.trim())
            .filter(Boolean);
    });

    $("#editForm").empty();

    toggleEditForm(false);

    $("#main-form-wrapper").removeClass("d-none");

    showClientLoader()

    ConfigurationDataService.getConfigurationTableRows(tableName, function (data) {
        //set schem model for edit / add form

        tableTabaModel = Object.fromEntries(
            Object.entries(data).filter(([key]) => ['Table', 'Columns'].includes(key))
        );

        //set data grid values
        loadConfigurationDataTableRowsAndModel(data.Rows);

        hideClientLoader();
    });
}
//**** jquery sections */

function getDateToday() {
    let now = new Date();
    let day = ("0" + now.getDate()).slice(-2);
    let month = ("0" + (now.getMonth() + 1)).slice(-2);
    let today = now.getFullYear() + "-" + (month) + "-" + (day);
    return today;
}

$(document).on("click", "#cmdAddConfigurationRecord", function () {
    //build form
    $("#dynamic-form").empty();

    $("#dynamic-form").append("<div class='row' id='hdnInputs></div>");

    $("#dynamic-form").append(buildHtmlFormForDataRowforAdd());

    let rules = buildRules(tableTabaModel.Columns);
    let messages = buildMessages(tableTabaModel.Columns);

    //apply a reinit of validator with new settings for a dynmamic form
    $("#aspnetForm").validate({
        errorClass: "text-danger",
        onfocusout: function (element) {
            this.element(element);
        },
        errorPlacement: function (error, element) {
            // Find the existing span next to the input and set its text
            element.siblings(".error-message").text(error.text());
        },
        success: function (label, element) {
            // Clear the error text when valid
            $(element).siblings(".error-message").text("");
        },
        rules: rules,
        messages: messages,
        highlight: function (element, errorClass) {
            $(element).removeClass(errorClass); //since resetting validator, by default it highlights the related input. We only want the error span
        }
    });

    //now load the dynamic validations for the form inputs
    Object.keys(rules).forEach(function (colName) {
        $("[name='" + colName + "']").rules("add", rules[colName]);
    });

    toggleEditForm(true);
});

$(document).on("change", "#ddlConfigurationTables", function () {

    let selectedTableName = $(this).val();

    loadConfigurationDataTableRecords(selectedTableName);

});


$(function () {
    console.log("Loading configuration data table from dropdown list");
    loadConfigurationDataDropDownTableOptions();
    userId = $('#ctl00_MainContent_userId').val();
});