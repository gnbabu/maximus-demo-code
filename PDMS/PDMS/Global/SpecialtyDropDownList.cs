using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI.WebControls;

namespace CustomControls
{
    public class SpecialityDropDownList : DropDownList
    {
        public SpecialityDropDownList()
        {
        }

        public string SPECIALTY_TYPE_ID { get; set; }
        public string TAXONOMY_TYPE_ID { get; set; }

        public string SelectedSpecialtyTypeID
        {
            get
            {
                return ViewState["SPECIALTY_TYPE_ID" + this.SelectedIndex] as string;
            }
        }
        public string SelectedTaxonomyTypeID
        {
            get
            {
                return ViewState["TAXONOMY_TYPE_ID" + this.SelectedIndex] as string;
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            int i = 0;
            if (this.DataSource != null)
            {
                foreach (var item in this.DataSource as IEnumerable)
                {
                    //if have loaded the first item with an empty value, set its values to empty strings then load the values for this item into the next index (i).
                    if (i == 0 && this.Items[i].Value == string.Empty)
                    {
                        this.Items[i].Attributes.Add(SPECIALTY_TYPE_ID, string.Empty);
                        ViewState["SPECIALTY_TYPE_ID" + i] = string.Empty;
                        this.Items[i].Attributes.Add(TAXONOMY_TYPE_ID, string.Empty);
                        ViewState["TAXONOMY_TYPE_ID" + i] = string.Empty;
                    }
                    else
                    {
                        PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(item);
                        PropertyDescriptor pdSelectedSpecialtyTypeID = properties.Find(SPECIALTY_TYPE_ID, true);
                        PropertyDescriptor pdSelectedSelectedTaxonomyTypeID = properties.Find(TAXONOMY_TYPE_ID, true);

                        this.Items[i].Attributes.Add(SPECIALTY_TYPE_ID, pdSelectedSpecialtyTypeID.GetValue(item).ToString());
                        ViewState["SPECIALTY_TYPE_ID" + i] = pdSelectedSpecialtyTypeID.GetValue(item).ToString();

                        this.Items[i].Attributes.Add(TAXONOMY_TYPE_ID, pdSelectedSelectedTaxonomyTypeID.GetValue(item).ToString());
                        ViewState["TAXONOMY_TYPE_ID" + i] = pdSelectedSelectedTaxonomyTypeID.GetValue(item).ToString();

                    }
                    i++;
                }
            }
        }

        protected override object SaveViewState()
        {
            // create object array for Item count + 1
            object[] allStates = new object[this.Items.Count + 1];

            // the +1 is to hold the base info
            object baseState = base.SaveViewState();
            allStates[0] = baseState;

            Int32 i = 1;
            // now loop through and save each Style attribute for the List
            foreach (ListItem li in this.Items)
            {
                Int32 j = 0;
                string[][] attributes = new string[li.Attributes.Count][];
                foreach (string attribute in li.Attributes.Keys)
                {
                    attributes[j++] = new string[] { attribute, li.Attributes[attribute] };
                }
                allStates[i++] = attributes;
            }
            return allStates;
        }

        protected override void LoadViewState(object savedState)
        {
            if (savedState != null)
            {
                object[] myState = (object[])savedState;

                // restore base first
                if (myState[0] != null)
                    base.LoadViewState(myState[0]);

                Int32 i = 1;
                foreach (ListItem li in this.Items)
                {
                    // loop through and restore each style attribute
                    foreach (string[] attribute in (string[][])myState[i++])
                    {
                        li.Attributes[attribute[0]] = attribute[1];
                    }
                }
            }
        }
    }
}