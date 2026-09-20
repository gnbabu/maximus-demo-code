using MAXIMUS.Models.Data.PDMS;
using System.Data;

namespace MAXIMUS.Presentation.Interfaces.PDMS
{
    public interface ICategoryOfServiceView : IView<CategoryOfService>
    {

        void GetRegCategoryOfService(DataSet ds);
        //void SetErrorMessages();

    }
}