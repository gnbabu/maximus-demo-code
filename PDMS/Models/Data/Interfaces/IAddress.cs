using System.Collections.Generic;
using System.Data;

namespace Models.Data.Interfaces
{
    public interface IAddress
    {
	    Dictionary<string, string> CreateParameterList(Address address);
	    void Load(DataRow addressRow, DataRow provRow = null);
	    int Insert(Address address, Dictionary<string, string> parms);
	    void Update(Address address, Dictionary<string, string> parms);
    }
}
