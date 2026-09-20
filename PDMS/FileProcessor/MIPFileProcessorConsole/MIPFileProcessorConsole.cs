using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

using FileProcessorCore;
using MIPFileProcessor;



namespace MIPFileProcessorConsole
{
	public class MIPFileProcessorConsole
	{
		
	

		/*
		 * The file processor (MMISImportFileProcessor) work by inheriting from abstract classes that take care of file searching, reading into lines, 
		 * separating lines into Dictionaries with property names for keywords, type validating, and more based on the XML template defined within the 
		 * custom sub-class. The only code required in the sub-class is completing abstract methods that perform insert, update, and delete operations,
		 * and the methods that determine if a record is an insert, update, or delete record.  In the MMISImportFileProcessor case, they are all inserts
		 * since they all go into the staging table.
		 * 
		 * After the file processor is run, a new database processor (MMISProviderUpdateProcessor) is then run, fetching unprocessed records from the
		 * above staging table and updating or inserting into the MMISProviderEligibility table. Again most of the legwork is done in the base class
		 * DatabaseProcessor, all that is required in the sub class is to perform the actual operations after all of the database records have been
		 * split into Dictionaries.  The MMISProviderUpdateProcessor processor calls a stored procedure to either update or insert a records in the
		 * MMISProviderEligbility table based on the ProviderNumber value.  It will then flag the staging record it just read in as Processed and move on.
		 * 
		 * */

		static void Main(string[] args)
		{
			MMISProviderEligibilityProcessor.ImportAndProcessFile(args);
		}

		
	}
}
