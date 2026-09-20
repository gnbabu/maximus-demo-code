using System.Collections.Generic;
using System.Data;

namespace Models.Data.Interfaces
{
    public interface IOwner
	{
		Dictionary<string, string> CreateParameterList(OwnerInfo ownerinfo);
		void Load(DataRow row);
		int Insert(OwnerInfo ownerinfo, Dictionary<string, string> parms);
		void Update(OwnerInfo ownerinfo, Dictionary<string, string> parms);
    }
}