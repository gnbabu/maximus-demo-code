$(document).ready(function () {
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg_txtMedicaidBillingNumber',
        function () {
           
            $('#ctl00_MainContent_uc5SubmitClaim_txtMedicaidBillingNumber').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtMedicaidBillingNumber').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg_rev_txtMedicaidBillingNumber',
        function () {

            $('#ctl00_MainContent_uc5SubmitClaim_txtMedicaidBillingNumber').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtMedicaidBillingNumber').focus();

        });


    $('#ctl00_MainContent_uc5SubmitClaim_txtMedicaidBillingNumber').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_revMedicaidBillingNumber').is(':hidden') &&
                $('#ctl00_MainContent_uc5SubmitClaim_rfvMEDBillNum').is(':hidden')) {

                $('#ctl00_MainContent_uc5SubmitClaim_txtMedicaidBillingNumber').css('background-color', 'transparent');
            }

        });


    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg1_txtBirthDate',
        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtBirthDate').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtBirthDate').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg1_rev_txtBirthDate',
        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtBirthDate').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtBirthDate').focus();

        });
    
    $('#ctl00_MainContent_uc5SubmitClaim_txtBirthDate').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_revBirthdate').is(':hidden') &&
                $('#ctl00_MainContent_uc5SubmitClaim_rfvBirthdate').is(':hidden')) {

                $('#ctl00_MainContent_uc5SubmitClaim_txtBirthDate').css('background-color', 'transparent');
            }

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg2_txtPatientControlNumber',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtPatientControlNumber').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtPatientControlNumber').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg2_rev_txtPatientControlNumber',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtPatientControlNumber').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtPatientControlNumber').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_txtPatientControlNumber').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_revPatientControlNumber').is(':hidden') &&
                $('#ctl00_MainContent_uc5SubmitClaim_reqPatientControlNumber').is(':hidden')) {
                $('#ctl00_MainContent_uc5SubmitClaim_txtPatientControlNumber').css('background-color', 'transparent');
            }

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg3_ddlDentalReleaseofInfo',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_ddlDentalReleaseofInfo').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_ddlDentalReleaseofInfo').focus();

        });
  
    $('#ctl00_MainContent_uc5SubmitClaim_ddlDentalReleaseofInfo').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_rfvDentalReleaseofInfo').is(':hidden')) {
                $('#ctl00_MainContent_uc5SubmitClaim_ddlDentalReleaseofInfo').css('background-color', 'transparent');
            }

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg4_txtPlaceofService',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtPlaceofService').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtPlaceofService').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg4_rev_txtPlaceofService',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtPlaceofService').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtPlaceofService').focus();

        });
    
    $('#ctl00_MainContent_uc5SubmitClaim_txtPlaceofService').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_revPlaceofservice').is(':hidden') &&
                $('#ctl00_MainContent_uc5SubmitClaim_RequiredFieldValidator10').is(':hidden')) {
                $('#ctl00_MainContent_uc5SubmitClaim_txtPlaceofService').css('background-color', 'transparent');
            }

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg5_ddlAccidentrelatedto',
        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_ddlAccidentrelatedto').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_ddlAccidentrelatedto').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_ddlAccidentrelatedto').blur(
        function () {

            if (
                $('#ctl00_MainContent_uc5SubmitClaim_rfvAccidentrelatedto').is(':hidden')) {
                $('#ctl00_MainContent_uc5SubmitClaim_ddlAccidentrelatedto').css('background-color', 'transparent');
            }

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg6_txtPriorAuthNumber',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtPriorAuthNumber').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtPriorAuthNumber').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg6_rev_txtPriorAuthNumber',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtPriorAuthNumber').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtPriorAuthNumber').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_txtPriorAuthNumber').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_revPriorAuthNumber').is(':hidden') &&
                $('#ctl00_MainContent_uc5SubmitClaim_reqPriorAuthNumber').is(':hidden')) {
                $('#ctl00_MainContent_uc5SubmitClaim_txtPriorAuthNumber').css('background-color', 'transparent');
            }

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg7_txtReferringProviderNPI',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtReferringProviderNPI').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtReferringProviderNPI').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg7_rev_txtReferringProviderNPI',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtReferringProviderNPI').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtReferringProviderNPI').focus();

        });
    
    $('#ctl00_MainContent_uc5SubmitClaim_txtReferringProviderNPI').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_revNPI').is(':hidden') &&
                $('#ctl00_MainContent_uc5SubmitClaim_rfvReferringProviderNPI').is(':hidden')) {
                $('#ctl00_MainContent_uc5SubmitClaim_txtReferringProviderNPI').css('background-color', 'transparent');
            }

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg8_txtRenderingProvNPI',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtRenderingProvNPI').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtRenderingProvNPI').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg8_rev_txtRenderingProvNPI',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtRenderingProvNPI').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtRenderingProvNPI').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_txtRenderingProvNPI').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_revRefNPI').is(':hidden') &&
                $('#ctl00_MainContent_uc5SubmitClaim_rfvRenderingProvNPI').is(':hidden')) {
                $('#ctl00_MainContent_uc5SubmitClaim_txtRenderingProvNPI').css('background-color', 'transparent');
            }

        });

    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg9_ddlAccidentrelatedto',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_ddlAccidentrelatedto').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_ddlAccidentrelatedto').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_ddlAccidentrelatedto').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_rfvAccidentrelatedto').is(':hidden')) {
                $('#ctl00_MainContent_uc5SubmitClaim_ddlAccidentrelatedto').css('background-color', 'transparent');
            }

        });


    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg10_txtSupervisingProviderNPI',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtSupervisingProviderNPI').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtSupervisingProviderNPI').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg10_rev_txtSupervisingProviderNPI',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtSupervisingProviderNPI').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtSupervisingProviderNPI').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_txtSupervisingProviderNPI').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_revSupervisingProviderNPI').is(':hidden') &&
                $('#ctl00_MainContent_uc5SubmitClaim_rfvSupervisingProviderNPI').is(':hidden')) {
                $('#ctl00_MainContent_uc5SubmitClaim_txtSupervisingProviderNPI').css('background-color', 'transparent');
            }

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg11_txtOtherPayerInformation',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtOtherPayerInformation').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtOtherPayerInformation').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg11_rev_txtOtherPayerInformation',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtOtherPayerInformation').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtOtherPayerInformation').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_txtOtherPayerInformation').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_revOtherPayerInFormation').is(':hidden') &&
                $('#ctl00_MainContent_uc5SubmitClaim_rfvOtherPayerInformation').is(':hidden')) {
                $('#ctl00_MainContent_uc5SubmitClaim_txtOtherPayerInformation').css('background-color', 'transparent');
            }

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg12_txtHealthPlanID',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtHealthPlanID').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtHealthPlanID').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg12_rev_txtHealthPlanID',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtHealthPlanID').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtHealthPlanID').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_txtHealthPlanID').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_revHealthPlanID').is(':hidden') &&
                $('#ctl00_MainContent_uc5SubmitClaim_rfvHealthPlanID').is(':hidden')) {
                $('#ctl00_MainContent_uc5SubmitClaim_txtHealthPlanID').css('background-color', 'transparent');
            }

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg13_ddlPatientRelationship',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_ddlPatientRelationship').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_ddlPatientRelationship').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_ddlPatientRelationship').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_rfvPatientRelationship').is(':hidden')) {
                $('#ctl00_MainContent_uc5SubmitClaim_ddlPatientRelationship').css('background-color', 'transparent');
            }

        });

    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg14_ddlClaimFilingIndicator',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_ddlClaimFilingIndicator').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_ddlClaimFilingIndicator').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_ddlClaimFilingIndicator').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_rfvClaimFilingIndicator').is(':hidden')) {
                $('#ctl00_MainContent_uc5SubmitClaim_ddlClaimFilingIndicator').css('background-color', 'transparent');
            }

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg15_ddlPayerSequence',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_ddlPayerSequence').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_ddlPayerSequence').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_ddlPayerSequence').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_rfvPayerSequence').is(':hidden')) {
                $('#ctl00_MainContent_uc5SubmitClaim_ddlPayerSequence').css('background-color', 'transparent');
            }

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg16_txtPaidDate',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtPaidDate').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtPaidDate').focus();

        }); 
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg16_rev_txtPaidDate',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtPaidDate').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtPaidDate').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_txtPaidDate').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_revPaidDate').is(':hidden') &&
                $('#ctl00_MainContent_uc5SubmitClaim_cvPaidDate').is(':hidden') &&
                $('#ctl00_MainContent_uc5SubmitClaim_rfvPaidDate').is(':hidden')) {
                $('#ctl00_MainContent_uc5SubmitClaim_txtPaidDate').css('background-color', 'transparent');
            }

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg34_txtPaidAmount1',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtPaidAmount1').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtPaidAmount1').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg34_rev_txtPaidAmount1',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtPaidAmount1').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtPaidAmount1').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_txtPaidAmount1').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_revPaidAmount').is(':hidden') &&
               
                $('#ctl00_MainContent_uc5SubmitClaim_rfPaidAmount1').is(':hidden')) {
                $('#ctl00_MainContent_uc5SubmitClaim_txtPaidAmount1').css('background-color', 'transparent');
            }

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg17_ddlAdjustmentGroup',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_ddlAdjustmentGroup').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_ddlAdjustmentGroup').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_ddlAdjustmentGroup').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_rfvAdjustmentGroup').is(':hidden')) {
                $('#ctl00_MainContent_uc5SubmitClaim_ddlAdjustmentGroup').css('background-color', 'transparent');
            }

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg18_txtReasonCode',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtReasonCode').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtReasonCode').focus();

        }); 
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg18_rev_txtReasonCode',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtReasonCode').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtReasonCode').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_txtReasonCode').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_revReasonCode').is(':hidden') &&

                $('#ctl00_MainContent_uc5SubmitClaim_rfvReasonCode').is(':hidden')) {
                $('#ctl00_MainContent_uc5SubmitClaim_txtReasonCode').css('background-color', 'transparent');
            }

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg19_txtAmount',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtAmount').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtAmount').focus();

        }); 
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg19_rev_txtAmount',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtAmount').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtAmount').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_txtAmount').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_revAmount').is(':hidden') &&

                $('#ctl00_MainContent_uc5SubmitClaim_rfvAmount').is(':hidden')) {
                $('#ctl00_MainContent_uc5SubmitClaim_txtAmount').css('background-color', 'transparent');
            }

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg20_txtProcedureCode',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtProcedureCode').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtProcedureCode').focus();

        }); 
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg20_rev_txtProcedureCode',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtProcedureCode').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtProcedureCode').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_txtProcedureCode').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_revProcedureCode').is(':hidden') &&

                $('#ctl00_MainContent_uc5SubmitClaim_rfvdentalprocedurecode').is(':hidden')) {
                $('#ctl00_MainContent_uc5SubmitClaim_txtProcedureCode').css('background-color', 'transparent');
            }

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg21_txtBilledUnits',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtBilledUnits').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtBilledUnits').focus();

        }); 
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg21_rev_txtBilledUnits',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtBilledUnits').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtBilledUnits').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_txtBilledUnits').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_revBillunits').is(':hidden') &&

                $('#ctl00_MainContent_uc5SubmitClaim_rfvBilledUnit').is(':hidden')) {
                $('#ctl00_MainContent_uc5SubmitClaim_txtBilledUnits').css('background-color', 'transparent');
            }

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg22_txtdateofservice',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtdateofservice').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtdateofservice').focus();

        }); 
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg22_rev_txtdateofservice',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtdateofservice').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtdateofservice').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_txtdateofservice').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_revdateofservice').is(':hidden') &&
                $('#ctl00_MainContent_uc5SubmitClaim_cvdateofservice').is(':hidden') &&

                $('#ctl00_MainContent_uc5SubmitClaim_rfvdateofservice').is(':hidden')) {
                $('#ctl00_MainContent_uc5SubmitClaim_txtdateofservice').css('background-color', 'transparent');
            }

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click','#msg23_ddlProviderType',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_ddlProviderType').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_ddlProviderType').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_ddlProviderType').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_rvProviderType').is(':hidden')) {
                $('#ctl00_MainContent_uc5SubmitClaim_ddlProviderType').css('background-color', 'transparent');
            }

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg24_txtProviderNPI',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtProviderNPI').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtProviderNPI').focus();

        }); 
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg24_rev_txtProviderNPI',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtProviderNPI').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtProviderNPI').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_ddlProviderType').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_rfvProviderNPI').is(':hidden')&&
                $('#ctl00_MainContent_uc5SubmitClaim_revProviderNPI').is(':hidden') &&
                $('#ctl00_MainContent_uc5SubmitClaim_cvProviderNPI').is(':hidden')) {
                $('#ctl00_MainContent_uc5SubmitClaim_txtProviderNPI').css('background-color', 'transparent');
            }

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg25_ddlDetailId',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_ddlDetailId').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_ddlDetailId').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_ddlDetailId').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_rfvDetailId').is(':hidden')) {
                $('#ctl00_MainContent_uc5SubmitClaim_ddlDetailId').css('background-color', 'transparent');
            }

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg26_ddlOPPHealhPlanId',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_ddlOPPHealhPlanId').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_ddlOPPHealhPlanId').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_ddlOPHealhPlanId').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_rfvOPPHealthPlanId').is(':hidden')) {
                $('#ctl00_MainContent_uc5SubmitClaim_ddlOPPHealhPlanId').css('background-color', 'transparent');
            }

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg27_txtOPPAmount',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtOPPAmount').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtOPPAmount').focus();

        }); 
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg27_rev_txtOPPAmount',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtOPPAmount').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtOPPAmount').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_txtOPPAmount').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_rfvOPPAmount').is(':hidden') &&
                $('#ctl00_MainContent_uc5SubmitClaim_revOPPAmount').is(':hidden')) {
                
                $('#ctl00_MainContent_uc5SubmitClaim_txtOPPAmount').css('background-color', 'transparent');
            }

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg28_ddlOPPAdjustmentGroup',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_ddlOPPAdjustmentGroup').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_ddlOPPAdjustmentGroup').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_ddlOPPAdjustmentGroup').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_rfvOPPAdjustmentGroup').is(':hidden')) {
                $('#ctl00_MainContent_uc5SubmitClaim_ddlOPPAdjustmentGroup').css('background-color', 'transparent');
            }

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg29_ddlOPPReasonCode',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_ddlOPPReasonCode').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_ddlOPPReasonCode').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_ddlOPPReasonCode').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_rfvOPPReasonCode').is(':hidden')) {
                $('#ctl00_MainContent_uc5SubmitClaim_ddlOPPReasonCode').css('background-color', 'transparent');
            }

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg30_txtOPPAdjAmount',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtOPPAdjAmount').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtOPPAdjAmount').focus();

        }); 
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg30_rev_txtOPPAdjAmount',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_txtOPPAdjAmount').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_txtOPPAdjAmount').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_txtOPPAdjAmount').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_rfvOPPAdjAmount').is(':hidden') &&
                $('#ctl00_MainContent_uc5SubmitClaim_revOPPAdjAmount').is(':hidden')) {

                $('#ctl00_MainContent_uc5SubmitClaim_txtOPPAdjAmount').css('background-color', 'transparent');
            }

        });

    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg31_ddlDocumentTypeclaims',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_dlDocumentTypeclaims').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_dlDocumentTypeclaims').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_dlDocumentTypeclaims').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_rfvDocumentTypeclaims').is(':hidden')) {
                $('#ctl00_MainContent_uc5SubmitClaim_dlDocumentTypeclaims').css('background-color', 'transparent');
            }

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg32_PriorAttachmentUpload',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_PriorAttachmentUpload').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_PriorAttachmentUpload').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg32_rev_PriorAttachmentUpload',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_PriorAttachmentUpload').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_PriorAttachmentUpload').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_PriorAttachmentUpload').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_rfvPriorAttachmentUpload').is(':hidden') &&
                $('#ctl00_MainContent_uc5SubmitClaim_revPriorAttachmentUpload').is(':hidden')) {

                $('#ctl00_MainContent_uc5SubmitClaim_PriorAttachmentUpload').css('background-color', 'transparent');
            }

        });
    $('#ctl00_MainContent_uc5SubmitClaim_valerrormess').on('click', '#msg33_ddlDoctype',

        function () {
            $('#ctl00_MainContent_uc5SubmitClaim_ddlDoctype').css('background-color', 'yellow');
            $('#ctl00_MainContent_uc5SubmitClaim_ddlDoctype').focus();

        });
    $('#ctl00_MainContent_uc5SubmitClaim_ddlDoctype').blur(
        function () {

            if ($('#ctl00_MainContent_uc5SubmitClaim_rfvDoctype').is(':hidden')) {
                $('#ctl00_MainContent_uc5SubmitClaim_ddlDoctype').css('background-color', 'transparent');
            }

        });
});



