using System.Collections.Generic;
using System.Data;

namespace Models.Data.Interfaces
{
    public interface IForm1099
	{
		Dictionary<string, string> CreateParameterList(Form1099 taxForm);
		void Load(DataRow row);
		int Insert(Form1099 taxForm, Dictionary<string, string> parms);
		void Update(Form1099 taxForm, Dictionary<string, string> parms);
	}
}