using MAXIMUS.Presentation.Interfaces.PDMS;
using MAXIMUS.Presentation.PDMS;
using Models.Data;
using Presentation.Interfaces;
using System.Collections.Generic;

namespace Presentation
{
    public class OwnerInfoPresenter : PresenterBase, IPresenter<IOwnerInfoView, OwnerInfo>
    {
        private IOwnerInfoView view = null;

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

        public OwnerInfoPresenter(IOwnerInfoView view)
        {
            Check.IsNotNull(view, "view cannot be null");
            this.view = view;
        }

        public void Init()
        {
            this.view.OwnerInfo = new OwnerInfo();
        }

        public int Insert(OwnerInfo ownerInfo, Dictionary<string, string> parms)
        {
            if (view.OwnerInfo == null)
            {
                view.OwnerInfo = new OwnerInfo();
            }

            view.OwnerInfo = ownerInfo;
            view.OwnerInfo.RegOwnerId = view.OwnerInfo.Insert(ownerInfo, parms);
            return view.OwnerInfo.RegOwnerId;
        }

        public void Update(OwnerInfo ownerInfo, Dictionary<string, string> parms)
        {
            if (view.OwnerInfo == null)
            {
                view.OwnerInfo = new OwnerInfo();
            }

            view.OwnerInfo = ownerInfo;
            view.OwnerInfo.Update(ownerInfo, parms);
        }

        public int Insert(Address address, Dictionary<string, string> parms)
        {
            if (view.OwnerInfo == null)
            {
                view.OwnerInfo = new OwnerInfo();
            }

            view.OwnerAddress = address;
            view.OwnerInfo.AddressId = view.OwnerAddress.Insert(address, parms);
            return view.OwnerAddress.AddressId;
        }

        public void Update(Address address, Dictionary<string, string> parms)
        {
            if (view.OwnerAddress == null)
            {
                view.OwnerAddress = new Address();
            }

            view.OwnerAddress = address;
            view.OwnerAddress.Update(address, parms);
        }
    }
}