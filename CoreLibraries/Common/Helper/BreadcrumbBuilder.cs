using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Corp.Core.Libraries.Helper
{
    public class BreadcrumbBuilder
    {
        private List<string> _items = new List<string>();

        public BreadcrumbBuilder Add(string title, string url = null)
        {
            string displayTitle = title;

            if (!string.IsNullOrEmpty(title) && title.Length > 80)
            {
                displayTitle = title.Substring(0, 80) + "...";
            }

            if (!string.IsNullOrEmpty(url))
            {
                _items.Add($"<a class='breadcrumb-link' href='{url}'>{title}</a>");
            }
            else
            {
                _items.Add($"<span class='breadcrumb-current' aria-current='page' title='{title}'>{displayTitle}</span>");
            }

            return this;
        }

        public string Build()
        {
            return string.Join(" &gt; ", _items);
        }
    }

}
