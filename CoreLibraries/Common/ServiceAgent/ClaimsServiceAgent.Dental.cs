using System.Collections.Generic;

namespace Corp.Core.Libraries
{
    public partial class ClaimsServiceAgent
    {
        public List<string> ValidateDentalData()
        {
            List<string> errorList = new List<string>();

            errorList.AddRange(ValidateOtherPayers());
            errorList.AddRange(ValidateDentalServiceDetails());
            errorList.AddRange(ValidateAdditionalProviders());
            errorList.AddRange(ValidateOtherPayerPaidAmount());
            

            return errorList;
        }
        public List<DentalServiceDetail> DentalServiceDetails
        {
            get
            {
                return ViewState["DentalServiceDetail"] == null ? new List<DentalServiceDetail>() : (List<DentalServiceDetail>)ViewState["DentalServiceDetail"];
            }
            set
            {
                ViewState["DentalServiceDetail"] = value;
            }

        }
        public List<string> ValidateDentalServiceDetails()
        {
            List<string> errorList = new List<string>();

            // Service Details

            foreach (DentalServiceDetail row in DentalServiceDetails)
            {

                if (string.IsNullOrEmpty(row.cde_proc))
                {
                    errorList.Add("Procedure code missing for detail " + row.num_dtl_total);
                }

                if (string.IsNullOrEmpty(row.qty_billed))
                {
                    errorList.Add("Billed unit not reported for detail  " + row.num_dtl_total);
                }

                if (string.IsNullOrEmpty(row.dte_first_svc))
                {
                    errorList.Add("Date of Service not reported for detail  " + row.num_dtl_total);
                }
            }

            return errorList;
        }


       
        public List<ToothQuadrantInfo> ToothQuadrants
        {
            get
            {
                return ViewState["toothquadrant"] == null ? new List<ToothQuadrantInfo>() : (List<ToothQuadrantInfo>)ViewState["toothquadrant"];
            }
            set
            {
                ViewState["toothquadrant"] = value;
            }

        }

        public List<OtherPayerInformation> OtherPayer
        {
            get
            {
                return ViewState["OtherPayerInfo"] == null ? new List<OtherPayerInformation>() : (List<OtherPayerInformation>)ViewState["OtherPayerInfo"];
            }
            set
            {
                ViewState["OtherPayerInfo"] = value;
            }

        }
        public List<string> ValidateOtherPayers()
        {
            List<string> errorList = new List<string>();
            bool primaryNotFound = true;
            bool rowsFound = false;

            foreach (OtherPayerInformation pay in OtherPayer)
            {
                rowsFound = true;
                //Other provider
                if (!string.IsNullOrEmpty(pay.cde_insured_group_number) ||
                    !string.IsNullOrEmpty(pay.nam_first) ||
                    !string.IsNullOrEmpty(pay.nam_last) ||
                    !string.IsNullOrEmpty(pay.cde_ind_relationship) ||
                    !string.IsNullOrEmpty(pay.employer_name) ||
                    !string.IsNullOrEmpty(pay.cde_claim_filing_ind) ||
                    !string.IsNullOrEmpty(pay.cde_payer_responib) ||
                    !string.IsNullOrEmpty(pay.dte_clm_adjudication) ||
                    !string.IsNullOrEmpty(pay.amt_monetary))
                {
                    if (string.IsNullOrEmpty(pay.named_insured_group))
                    {
                        errorList.Add("Other Payer Name is required");
                    }

                }
                if (!string.IsNullOrEmpty(pay.named_insured_group))
                {


                    // Health Plan Id
                    if (string.IsNullOrEmpty(pay.cde_insured_group_number))
                    {
                        errorList.Add("Health Plan ID is required");

                    }

                    // Patient Relationship
                    if (string.IsNullOrEmpty(pay.cde_ind_relationship))
                    {
                        errorList.Add("Patient relationship missing");

                    }

                    // Claim Filing Indicator
                    if (string.IsNullOrEmpty(pay.cde_claim_filing_ind))
                    {
                        errorList.Add("Claim Filing Indicator missing");

                    }

                    if (string.IsNullOrEmpty(pay.cde_payer_responib))
                    {
                        errorList.Add("Payer sequence missing");

                    }
                    else
                    {
                        if (pay.cde_payer_responib.Equals("P"))
                        {
                            primaryNotFound = false;
                        }
                    }

                    if (string.IsNullOrEmpty(pay.dte_clm_adjudication))
                    {
                        errorList.Add("Paid date is required");

                    }

                    if (string.IsNullOrEmpty(pay.amt_monetary))
                    {
                        errorList.Add("Paid Amount is required");

                    }

                    // Need to add adjument group validation
                };

            }
            if (rowsFound && primaryNotFound)
            {
                errorList.Add("Primary payer missing");
            }
            return errorList;
        }


    }

   



}
