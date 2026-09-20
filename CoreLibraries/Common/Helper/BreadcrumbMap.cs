using System.Collections.Generic;

namespace Corp.Core.Libraries.Helper
{

    public static class BreadcrumbMap
    {
        public static string Current(string pageKey, string dynamicValue = null)
        {
            var home = NavigationHelper.GetHome();

            switch (pageKey)
            {
                case "Exception":
                    return Build(home, "CustomException.aspx",
                        ("Exception", null)); // null = current page

                case "CVOQueue":
                    return Build(home, "CVOQueue.aspx",
                        ("CVO Queue", null)); // null = current page

                case "ReportViewer":
                    return Build(home, "Reports.aspx",
                        ("Report Management", "Reports.aspx"),
                        (dynamicValue, null));

                case "ReportEditor":
                    return Build(home, "Reports.aspx",
                        ("Report Management", "Reports.aspx"),
                        (dynamicValue, null));

                case "WorkBenchQueue":
                    return Build(home, "CvoWorkbench.aspx",
                        ("CVO Workbench", null));

                case "Policies":
                    return Build(home, "Policies.aspx",
                        ("Policies", null));

                case "ProviderInformationBar":
                    return Build(home, "ProviderInformationBar.aspx",
                        ("Provider Credentialing Details", null));

                case "ProviderVerificationResults":
                    return Build(home, "ProviderVerificationResults.aspx",
                        ("Provider Credentialing Details", null));

                case "ProviderRegistration":
                    return Build(home, "ProviderRegistration.aspx",
                        ("Provider Credentialing Details", null));

                case "Reports":
                    return Build(home, "Reports.aspx",
                        ("Report Management", null));

                case "ReportQueryDesigner":
                    return Build(home, "Reports.aspx",
                        ("Report Management", "Reports.aspx"),
                        ("Ad-hoc Query", null));

                case "ContactUs":
                    return Build(home, "ContactUs.aspx",
                        ("Contact Us", null));

                case "FrequentlyAskedQuestions":
                    return Build(home, "FrequentlyAskedQuestions.aspx",
                        ("Frequently Asked Questions", null));

                case "Learning":
                    return Build(home, "Learning.aspx",
                        ("Learning", null));

                case "SecurityPrivacyAccessibility":
                    return Build(home, "SecurityPrivacyAccessibility.aspx",
                        ("Security Privacy & Accessibility", null));

                case "Sitemap":
                    return Build(home, "Sitemap.aspx",
                        ("Sitemap", null));

                case "Correspondence":
                    return Build(home, "Correspondence.aspx",
                        ("Correspondence", null));

                case "ConfigurationData":
                    return Build(home, "ConfigurationData.aspx",
                        ("Configuration Data", null));

                case "UserAccounts":
                    return Build(home, "User.aspx",
                        ("User Accounts", null));

                case "AdminUserAccounts":
                    return Build(home, "AdminUserAccounts.aspx",
                        ("User Accounts", null));

                case "ContentManagement":
                    return Build(home, "DesignStaticPages.aspx",
                        ("Content Management", null));

                case "ReferenceData":
                    return Build(home, "ReferenceData.aspx",
                        ("Reference Data", null));

                case "ChairQueue":
                    return Build(home, "CommitteeQueue.aspx",
                        ("Chair Queue", null));

                case "CommitteeQueue":
                    return Build(home, "CommitteeQueue.aspx",
                        ("Committee Queue", null));

                case "ProviderDetails":
                    return Build(home, "Providers.aspx",
                        ("Providers", "Providers.aspx"),
                        (dynamicValue, null)); // dynamic provider name

                case "ProviderCredentialingHistory":
                    return Build(home, "ProviderCredentialingHistory.aspx",
                        ("Provider Credentialing Details", $"ProviderInformationBar.aspx?regId={dynamicValue}"),
                        ("Provider Credential History", null));

                case "ProviderVerificationResultsHistory":
                    return Build(home, "ProviderCredentialingHistory.aspx",
                        ("Provider Credentialing Details", $"ProviderInformationBar.aspx?regId={dynamicValue}"),
                        ("Provider Verification Results History", null));

                default:
                    return home.Title;
            }
        }

        private static string Build(
            (string Title, string Url) home,
            string currentPageUrl,
            params (string Title, string Url)[] nodes)
        {
            var bc = new BreadcrumbBuilder()
                .Add(home.Title, home.Url);

            // If current page IS home → stop
            //if (home.Url.Contains(currentPageUrl))
            //    return bc.Build();

            foreach (var node in nodes)
            {
                bc.Add(node.Title, node.Url);
            }

            return bc.Build();
        }
    }
}
