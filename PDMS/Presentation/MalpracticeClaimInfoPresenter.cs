using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using System;
using System.Collections.Generic;
using System.Data;

namespace MAXIMUS.Presentation.PDMS
{
    public class MalpracticeClaimInfoPresenter : PresenterBase, IPresenter<IMalpracticeClaimView, MalpracticeClaimInfo>
    {
        private IMalpracticeClaimView view = null;

        #region svc
        private PDMSService.PDMSServiceClient _svc;
        private PDMSService.PDMSServiceClient svc
        {
            get
            {
                if (_svc == null)
                {
                    _svc = new PDMSService.PDMSServiceClient();
                }

                return _svc;
            }
        }
        #endregion

        public MalpracticeClaimInfoPresenter(IMalpracticeClaimView view)
		{
			Check.IsNotNull(view, "view cannot be null");

            this.view = view;
        }

        #region View Event Listeners
        public void Init()
        {
            this.view.Model = new MalpracticeClaimInfo();
        }



        public void SaveRegMalpracticeClaimInfo(MalpracticeClaimInfo obj)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", obj.RegID.ToString());


            if (obj.IsMalPractice != null)
            {

                parms.Add("IsMalPractice", obj.IsMalPractice.ToString());
            }

            if (obj.DateOccurance != null)
            {

                parms.Add("DateOccurance", obj.DateOccurance.ToString());
            }

                      
            if (obj.DateClaimFiled != null)
            {

                parms.Add("DateClaimFiled", obj.DateClaimFiled.Value.ToShortDateString());
            }
            else
            {
                parms.Add("DateClaimFiled", null);
            }
            if (!string.IsNullOrEmpty(obj.ProfessionalCarrier))
            {

                parms.Add("ProfessionalCarrier", obj.ProfessionalCarrier.ToString());
            }

            if (obj.DefendentTypeID != null)
            {

                parms.Add("Defendent_Type_ID", obj.DefendentTypeID.ToString());
            }

            if (!string.IsNullOrEmpty(obj.OtherDefendents))
            {

                parms.Add("OtherDefendents", obj.OtherDefendents.ToString());
            }

            if (!string.IsNullOrEmpty(obj.Allegations))
            {

                parms.Add("Allegations", obj.Allegations.ToString());
            }
            if (obj.RegAddressId!=null)
            {

                parms.Add("REG_ADDRESS_ID", obj.RegAddressId.ToString());
            }
            if (!string.IsNullOrEmpty(obj.AllegedInjury))
            {

                parms.Add("AllegedInjury", obj.AllegedInjury.ToString());
            }

            if (obj.IsFiledSuitInCourt != null)
            {

                parms.Add("IsFiledSuitInCourt", obj.IsFiledSuitInCourt.ToString());
            }

            if (!string.IsNullOrWhiteSpace(obj.DateFiled.ToString()) && obj.DateFiled != DateTime.MinValue)
            {

                parms.Add("DateFiled", obj.DateFiled.Value.ToShortDateString());
            }
            else
            {
                parms.Add("DateFiled", null);
            }

            if (!string.IsNullOrEmpty(obj.StateCaseNumber))
            {

                parms.Add("StateCaseNumber", obj.StateCaseNumber.ToString());
            }

            if (!string.IsNullOrEmpty(obj.State))
            {

                parms.Add("State", obj.State.ToString());
            }

            if (!string.IsNullOrEmpty(obj.Country))
            {

                parms.Add("Country", obj.Country.ToString());
            }
            
            if (!string.IsNullOrEmpty(obj.FederalCaseNumber))
            {

                parms.Add("FederalCaseNumber", obj.FederalCaseNumber.ToString());
            }

            if (!string.IsNullOrEmpty(obj.District))
            {

                parms.Add("District", obj.District.ToString());
            }



            if (obj.OtherStatus != null)
            {

                parms.Add("OtherStatus", obj.OtherStatus.ToString());
            }


            if (obj.AdditionalInfo != null)
            {

                parms.Add("AdditionalInfo", obj.AdditionalInfo.ToString());
            }

            if (obj.ClaimStatusID != null)
            {

                parms.Add("Claim_Status_ID", obj.ClaimStatusID.ToString());
            }
            if (obj.PolicyNumber != null)
            {

                parms.Add("POLICYNUMBER", obj.PolicyNumber.ToString());
            }
            if (obj.ISNPDB != null)
            {

                parms.Add("IS_NPDB", obj.ISNPDB.ToString());
            }
            if (obj.MethodOfResolutionId != null)
            {

                parms.Add("METHOD_OF_RESULTION_ID", obj.MethodOfResolutionId.ToString());
            }
            if (obj.NoofOtherDefendents != null)
            {

                parms.Add("NUMBEROFOTHERDEFENDENT", obj.NoofOtherDefendents.ToString());
            }
            if (obj.SettledAmount != null)
            {

                parms.Add("SETTLEMENT_AMOUNT", obj.SettledAmount.ToString());
            }
            if (obj.ResultedInDeath != null)
            {

                parms.Add("RESULTED_IN_DEATH", obj.ResultedInDeath.ToString());
            }
            if (obj.Involvement != null)
            {

                parms.Add("Involvement", obj.Involvement.ToString());
            }

            if (!string.IsNullOrWhiteSpace(obj.DateClaimSettled.ToString()) && obj.DateClaimSettled != DateTime.MinValue)
            {

                parms.Add("CLAIM_SETTLED_DATE", obj.DateClaimSettled.Value.ToShortDateString());
            }
            



            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", obj.UserID.ToString());

            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            if (obj.REG_MALPRACTICE_CLAIM_ID == 0)
            {
                parms.Add("Created_On_Date_Time", DateTime.Now.ToString());
                parms.Add("Created_By_User", string.IsNullOrEmpty(obj.UserID.ToString()) ? null : obj.UserID.ToString());
                svc.InsertRegistrationDataTable("MALPRACTICE_CLAIM", parms);
            }
            else
            {
                parms.Add("REG_MALPRACTICE_CLAIM_ID", obj.REG_MALPRACTICE_CLAIM_ID.ToString());
                svc.UpdateRegistrationDataTable("MALPRACTICE_CLAIM", parms);
            }
        }



        public DataSet GetMalpracticeClaimByID(int MalpracticeClaimID)
        {
            DataSet ds;
            ds = svc.SelectMalpracticeClaimByID(MalpracticeClaimID);
            return ds;
        }



        public void DeleteMalpracticeClaimByID(int MalpracticeClaimID)
        {
            svc.DeleteMalpracticeClaimByID(MalpracticeClaimID);
            
        }

        public DataSet GetDefendentType()
        {
            return svc.SelectDefendentType();
        }

        public DataSet GetClaimStatus()
        {
            return svc.SelectClaimStatus();
        }

        public DataSet GetCountiesByState(string stateAbbreviation)
        {
            return svc.SelectCountiesByStateAbbreviation(stateAbbreviation);
        }
		#endregion
    
    }
}
