using System.Collections.Generic;
using System.Data;

namespace Models.Data.Interfaces
{
    public interface ITaxInfo
    {
	    Dictionary<string, string> CreateParameterList(TaxInfo taxInfo);
	    void Load(DataRow row);
	    int Insert(TaxInfo taxInfo, Dictionary<string, string> parms);
	    void Update(TaxInfo taxInfo, Dictionary<string, string> parms);
    }
}
