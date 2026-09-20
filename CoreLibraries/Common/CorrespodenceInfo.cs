using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp.Core.Libraries
{
   public class CorrespodenceInfo
    {
        public DataSet CorrespondenceInfo { get; set; }
        public int CorrespondenceResultCount { get; set; }

        public DataSet ConvertedCorrespondenceInfo { get; set; }
        public int ConvertedCorrespondenceResultCount { get; set; }
    }
}
