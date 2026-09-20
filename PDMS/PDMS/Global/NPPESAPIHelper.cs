using MAXIMUS.Core.Libraries;
using System;
using System.Data;
using System.Linq;

public static class NPPESAPIHelper
    {
        public static string ErrorMessageNoTaxonomy = "* No available taxonomy codes for this provider type were found in NPPES." +
                                       " Please return to NPPES and add the correct taxonomy code.";

        public static DataTable LoadTaxonomyFromNPPES(Result rs, DataTable dtFilterTaxonomyType)
        {
            Boolean NeedToFilter = false;
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("TaxonomyName");
            dt.Columns.Add("TaxonomyCode");
            dt.Columns.Add("TaxonomyNameWithCode");

            //some taxonomies by provider type is filtered CR 63: Remove Taxonomy Code 251E00000X for EPD Waiver = dtFilterTaxonomyType
            if (Methods.HasRows(dtFilterTaxonomyType))
                NeedToFilter = true;

            foreach (Taxonomy t in rs.taxonomies)
            {
                bool contains = false;

                if (NeedToFilter)
                    contains = dtFilterTaxonomyType.AsEnumerable().Any(row => t.code == row.Field<String>("TAXONOMY_CODE"));

                if (!contains)
                {
                    DataRow tax = dt.NewRow();
                    tax["TaxonomyNameWithCode"] = t.desc + " (" + t.code + ")";
                    tax["TaxonomyName"] = t.desc;
                    tax["TaxonomyCode"] = t.code;
                    dt.Rows.Add(tax);

                    //if taxonomy group exists and is not duplicated then add
                    if (!string.IsNullOrEmpty(t.taxonomy_group))
                    {
                        if (t.taxonomy_group.Contains(" ") && dt.Select("TaxonomyNameWithCode = '" + t.taxonomy_group + "'").Count() == 0)
                        {
                            tax = dt.NewRow();
                            
                            string taxName = t.taxonomy_group.Remove(0, t.taxonomy_group.IndexOf(' ') + 1).Trim();

                            tax["TaxonomyName"] = taxName;
                            tax["TaxonomyCode"] = t.taxonomy_group.Replace(taxName, "");
                            tax["TaxonomyNameWithCode"] = tax["TaxonomyName"] + " (" + tax["TaxonomyCode"] + ")";
                            dt.Rows.Add(tax);
                        }
                    }
                }
            }
            return dt.DefaultView.ToTable(true, "TaxonomyNameWithCode", "TaxonomyName", "TaxonomyCode");  // JIRA 2926
    }
 
    }
 
