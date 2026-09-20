using MAXIMUS.Models.Data.PDMS;
using MAXIMUS.Presentation.Interfaces.PDMS;
using System;
using System.Collections.Generic;
using System.Data;

namespace MAXIMUS.Presentation.PDMS
{
    public class BoardCertificationsPresenter : PresenterBase, IPresenter<IBoardCertificationsView, BoardCertifications>
    {
        private IBoardCertificationsView view = null;

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

        public BoardCertificationsPresenter(IBoardCertificationsView view)
		{
			Check.IsNotNull(view, "view cannot be null");

            this.view = view;
        }

        #region View Event Listeners
        public void Init()
        {
            this.view.Model = new BoardCertifications();
        }



        public void SaveRegBoardCertification(BoardCertifications Board)
        {
            Dictionary<string, string> parms = new Dictionary<string, string>();
            parms.Add("REG_ID", Board.RegID.ToString());
            



            if (Board.BoardCertificationID > 0)
            {

                parms.Add("Board_Certification_id", Board.BoardCertificationID.ToString());
            }
            if (Board.IsPrimary)
            {

                parms.Add("IsPrimary", Board.IsPrimary.ToString());
            }

            if (Board.BoardSpecialtyID > 0)
            {

                parms.Add("Board_Specialty_id", Board.BoardSpecialtyID.ToString());
            }

            if (!string.IsNullOrWhiteSpace(Board.EffectiveDate.ToString()) && Board.EffectiveDate != DateTime.MinValue)
            {

                parms.Add("Effective_Date", Board.EffectiveDate.ToShortDateString());
            }
            else
            {
                parms.Add("Effective_Date", null);
            }
            if (!string.IsNullOrWhiteSpace(Board.ExpirationDate.ToString()) && Board.ExpirationDate != DateTime.MinValue)
            {

                parms.Add("Expiration_Date", Board.ExpirationDate.ToShortDateString());
            }
            else
            {
                parms.Add("Expiration_Date", null);
            }
            if (!string.IsNullOrWhiteSpace(Board.CertificationNumber.ToString()))
            {

                parms.Add("CERTIFICATION_NUMBER", Board.CertificationNumber);
            }
            else
            {
                parms.Add("CERTIFICATION_NUMBER", null);
            }
            if (!string.IsNullOrWhiteSpace(Board.BoardSpecialty.ToString()))
            {

                parms.Add("BOARD_SPECIALTY", Board.BoardSpecialty);
            }
            else
            {
                parms.Add("BOARD_SPECIALTY", null);
            }

            parms.Add("LAST_MODIFIED_DATE_TIME", DateTime.Now.ToString());
            parms.Add("LAST_MODIFIED_USER", Board.UserID.ToString());

            PDMSService.PDMSServiceClient svc = new PDMSService.PDMSServiceClient();
            if (Board.RegBoardCertificationID == 0)
            {
                parms.Add("CREATED_ON_DATE_TIME", DateTime.Now.ToString());
                parms.Add("CREATED_BY_USER", Board.CreatedUserId.ToString());
            
                svc.InsertRegistrationDataTable("Board_CERTIFICATION", parms);
            }
            else
            {
                parms.Add("REG_Board_CERTIFICATION_ID", Board.RegBoardCertificationID.ToString());
                svc.UpdateRegistrationDataTable("Board_CERTIFICATIONCustom", parms);
            }
        }



        public DataSet GetBoardCertificationByID(int BoardCertificationID)
        {
            DataSet ds;
            ds = svc.SelectBoardCertificationByID(BoardCertificationID);
            return ds;
        }





		#endregion
    
    }
}
