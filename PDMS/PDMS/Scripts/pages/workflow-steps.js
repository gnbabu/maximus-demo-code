$(function () {  
    loadWorkflowSteps();
});

function loadWorkflowSteps() {

    $("#workflowStepsAccordion").maximusAccordion({
        allowMultiple: false,
        defaultOpen: 0
    });

    ProviderService.selectWorkflowStepsByRegID(function (response) {
        renderWorkflowStepsGrid(response);
    });
}

function renderWorkflowStepsGrid(data) {

    $('#workflow-steps-grid-container')
        .removeClass('d-none')
        .dataGrid({

            data: data,

            columns: [
                { key: 'TaskName', title: 'Task Name', type: 'text', sortable: true },
                { key: 'UserName', title: 'User Name', type: 'text', sortable: true },
                { key: 'StartDate', title: 'Start Date', type: 'date', sortable: true },
                { key: 'EndDate', title: 'End Date', type: 'date', sortable: true }
            ],

            tableClass: 'maximus-base-table',
            gridTitle: '',
            noDataMessage: 'No Workflow found.',
            idProperty: 'StepID',  // unique property

            enableAllColumnSearch: false,
            enableColumnFilters: true,
            enableSorting: true,

            dateFormat: 'MM-DD-YYYY HH:mm:ss',
            includeTime: false
        });

}
