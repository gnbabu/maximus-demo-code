using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataModels
{
    public class LearningCategoryDocs
    {
        public LearningCategoryDocs()
        {
            this.Documents = new LearningDoc[] { };
            this.ChildCategories = new LearningCategoryDocs[] { };
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public int ParentCatId { get; set; }
        public LearningDoc[] Documents { get; set; }
        public LearningCategoryDocs[] ChildCategories { get; set; }
    }
    public class LearningDoc
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string FileName { get; set; }
        public int CatId { get; set; }
        public int Order { get; set; }
    }

}
