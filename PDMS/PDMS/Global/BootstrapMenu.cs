//-----------------------------------------------------------------------
// <copyright file="BootstrapMenu.cs">
//     Copyright (c) Jeremy Knight. All rights reserved.
//     This source is subject to The MIT License (MIT).
//     For more information, see https://github.com/knight0323/aspnet-forms-bootstrap-menu
// </copyright>
// <author>Jeremy Knight</author>
//-----------------------------------------------------------------------

using System;
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CON = MAXIMUS.Core.Libraries.Constants;

namespace JK.BootstrapControls
{
    [ControlValueProperty("SelectedValue")]
    [DefaultEvent("MenuItemClick")]
    [SupportsEventValidation]
    [ToolboxData("<{0}:BootstrapMenu runat=\"server\"></{0}:BootstrapMenu>")]
    public sealed class BootstrapMenu : Menu
    {
        private const string hightlightActiveKey = "HighlightActive";

        /// <summary>
        /// Gets or sets the header text over the left list box.
        /// </summary>
        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [DefaultValue(false)]
        [DisplayName("HighlightActive")]
        public bool HighlightActive
        {
            get { return this.ViewState[hightlightActiveKey] != null && Convert.ToBoolean(this.ViewState[hightlightActiveKey]); }
            set { this.ViewState[hightlightActiveKey] = value; }
        }

        public override void RenderBeginTag(HtmlTextWriter writer)
        {
            // don't call base.RenderBeginTag()
        }

        public override void RenderEndTag(HtmlTextWriter writer)
        {
            // don't call base.RenderEndTag()
        }

        protected override void OnPreRender(EventArgs e)
        {
            // don't call base.OnPreRender(e);
            this.EnsureDataBound();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            this.BuildItems(writer, this.Items, true);
        }

        protected override void EnsureDataBound()
        {
            base.EnsureDataBound();
        }

        private void BuildItems(HtmlTextWriter writer, MenuItemCollection items, bool isRoot = false)
        {
            if (items.Count <= 0)
            {
                return;
            }

            string cssClass = "dropdown-menu";

            if (isRoot)
            {
                cssClass = "nav sidebar-nav";
                if (!string.IsNullOrEmpty(this.CssClass))
                {
                    cssClass += " " + this.CssClass;
                }
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Class, cssClass);
            writer.RenderBeginTag(HtmlTextWriterTag.Ul);

            if (SessionVarRetriever.IsOhID)
            {  
                foreach (MenuItem item in items)
                {
                    bool isProfileItem = item.NavigateUrl.Contains("MyProfile.aspx");
                    if (isProfileItem)
                    {
                        items.Remove(item);
                        break;
                    }
                }
                if (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ProviderAgent))
                {
                    foreach (MenuItem item1 in items)
                    {
                        // OHPNM-18384 -do not show Alternative Payment Model Information link to Provider Agent in left menu.
                        bool isAPMItem = item1.NavigateUrl.Contains("PaymentInnovationReports.aspx");
                        if (isAPMItem)
                        {
                            items.Remove(item1);
                            break;
                        }

                    }
                    // SAM604
                    if(!Helper.IsUserInSubRoles(0, Helper.GetUserId(HttpContext.Current.User.Identity.Name).ToString(), CON.SpecialtySearchSubRoles))
                    {
                        foreach (MenuItem item1 in items)
                        {                           
                            if (item1.NavigateUrl.Contains("SpecialtySearchMenu.aspx"))
                            {
                                items.Remove(item1);
                                break;
                            }

                        }
                    }
                }
            }           

            foreach (MenuItem item in items)
            {
                this.BuildItem(writer, item);
            }

            writer.RenderEndTag(); // </ul>
        }

        private void BuildItem(HtmlTextWriter writer, MenuItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            if (item.ChildItems.Count > 0)
            {
                writer.AddAttribute(HtmlTextWriterAttribute.Class, "sidebar-brand");    
            }
            
            if (this.IsLink(item))
            {
                if (this.HighlightActive && this.ResolveLinkUrl(item.NavigateUrl) == this.Page.Request.Url.AbsolutePath)
                {
                    writer.AddAttribute("class", "active");
                }

                writer.RenderBeginTag(HtmlTextWriterTag.Li);
                this.RenderLink(writer, item);
            }
            else if (this.HasChildren(item))
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Li);
                this.RenderDropDown(writer, item);
            }
            else
            {
                writer.RenderBeginTag(HtmlTextWriterTag.Li);
                writer.RenderBeginTag(HtmlTextWriterTag.A);
                writer.Write(item.Text);
                writer.RenderEndTag();
            }

            writer.RenderEndTag(); // </li>
        }

        private bool HasChildren(MenuItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            return item.ChildItems.Count > 0;
        }

        private bool IsLink(MenuItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            return item.Enabled && !string.IsNullOrEmpty(item.NavigateUrl);
        }

        private void RenderLink(HtmlTextWriter writer, MenuItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            string href = !string.IsNullOrEmpty(item.NavigateUrl)
                    ? this.Page.Server.HtmlEncode(this.ResolveLinkUrl(item.NavigateUrl))
                    : this.Page.ClientScript.GetPostBackClientHyperlink(
                        this,
                        "b" + item.ValuePath.Replace(this.PathSeparator.ToString(), "\\"),
                        true);
            writer.AddAttribute(HtmlTextWriterAttribute.Href, href);
            if (!string.IsNullOrEmpty(href) && HttpContext.Current.User.Identity.IsAuthenticated 
                && (Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ProviderAdministrator)
                    || Helper.IsUserInRole(HttpContext.Current.User.Identity.Name, CON.UserRole.ProviderAgent)))
            {
                if (href.IndexOf('/') != 0)
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "return BeforeLeaving(event, '" + href + "', 'new');", false);
                else
                    writer.AddAttribute(HtmlTextWriterAttribute.Onclick, "return BeforeLeaving(event, '" + href.Replace("~", "") + "', '');", false);
            }

            if (href.Contains("target=_blank").ToString() == "True")
                 writer.AddAttribute(HtmlTextWriterAttribute.Target, "target=_blank");

            string toolTip = !string.IsNullOrEmpty(item.ToolTip)
                ? item.ToolTip
                : item.Text;
            writer.AddAttribute(HtmlTextWriterAttribute.Title, toolTip);

            writer.RenderBeginTag(HtmlTextWriterTag.A);
            writer.Write(item.Text);
            writer.RenderEndTag(); // </a>
        }

        private void RenderDropDown(HtmlTextWriter writer, MenuItem item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("item");
            }

            if (writer == null)
            {
                throw new ArgumentNullException("writer");
            }

            writer.AddAttribute(HtmlTextWriterAttribute.Href, "#");
            writer.AddAttribute(HtmlTextWriterAttribute.Class, "dropdown-toggle");
            writer.AddAttribute("data-toggle", "sidebar-brand");
            writer.RenderBeginTag(HtmlTextWriterTag.A);

            string anchorValue = item.Text + "&nbsp;";
            writer.Write(anchorValue);

            writer.AddAttribute(HtmlTextWriterAttribute.Class, "caret");
            writer.RenderBeginTag(HtmlTextWriterTag.B);
            writer.RenderEndTag(); // </b>          

            writer.RenderEndTag(); // </a>

            this.BuildItems(writer, item.ChildItems);
        }

        /// <summary>
        /// resolve virtual path to client url with support for handling hash tag only href's.
        /// When an anchor href only contains a hash tag value it will be appended to the current url by the browser 
        /// without causing a page reload.
        /// </summary>
        /// <param name="href"></param>
        /// <returns></returns>
        private string ResolveLinkUrl(string href)
        {
            if (string.IsNullOrWhiteSpace(href)) return string.Empty;
            var url = href.StartsWith("/#") ? href.Replace("/#", "#") : this.ResolveClientUrl(href); ;
            return url;
        }
    }
}
