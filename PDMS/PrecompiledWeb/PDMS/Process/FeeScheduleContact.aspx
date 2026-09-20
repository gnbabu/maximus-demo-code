<%@ page title="Fee Schedule Contact Page" language="C#" masterpagefile="~/MasterPage.master" autoeventwireup="true" inherits="Process_FeeScheduleContact, App_Web_14kfymdt" enableEventValidation="false" stylesheettheme="Default" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <script type="text/javascript">
        function openInNewTab() {
            window.document.forms[0].target = '_blank';
            setTimeout(function () { window.document.forms[0].target = ''; }, 0);
        }
    </script>
    <style>
        .imgFee{
            padding-left: 10px;
            padding-top: 10px;
            font-size:20px;
            margin-top:0px;
            color:white;
        }
        
         .column {
         column-count: 2;
               }
    </style>

    <div class="container">
        <div><h1 class="imgFee">Fee Schedules</h1></div>
        <div>
            <p>
                To access current ODM Fee Schedules, <a href="https://medicaid.ohio.gov/resources-for-providers/billing/fee-schedule-and-rates/fee-schedule-and-rates" target="_blank" aria-label="Ohio Departmnet of Medicaid Fee Scheuldes">Click here</a>.
            </p>
        </div>
    </div>
</asp:Content>
