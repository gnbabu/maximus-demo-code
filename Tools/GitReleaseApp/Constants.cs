using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitReleaseApp
{
    public class Constants
    {
        public const string createTableAppInfo = "CREATE TABLE AppInformation(\r\n   ID INTEGER PRIMARY KEY AUTOINCREMENT,\r\n   REPOSITORY TEXT NOT NULL,\r\n   START_DATE TEXT NOT NULL,\r\n   START_DATE_TIME TEXT NOT NULL,\r\n   END_DATE TEXT NOT NULL,\r\n   END_DATE_TIME TEXT NOT NULL,\r\n   UAT_BRANCH TEXT NULL,\r\n   INITIAL_TAG TEXT  NULL,\r\n   FINAL_TAG TEXT  NULL,\r\n   TOKEN TEXT NOT NULL,\r\n   P3_REPOPATH TEXT NOT NULL,\r\n   P4_REPOPATH TEXT NOT NULL,\r\n   DOC_PATH TEXT NOT NULL,\r\n   NONPROD_CHNL TEXT NOT NULL,\r\n   PROD_CHNL TEXT NOT NULL,\r\n   E2EP3_HEADER TEXT  NULL,\r\n   E2EP3_DESCP TEXT  NULL,\r\n   E2EP4_HEADER TEXT  NULL,\r\n   E2EP4_DESCP TEXT  NULL,\r\n   UAT_HEADER TEXT  NULL,\r\n   UAT_DESCP TEXT  NULL,\r\n   PROD_HEADER TEXT NULL,\r\n   PROD_DESCP TEXT NULL\r\n);";

        public const string insertTableRecordP3 = "Insert into AppInformation (REPOSITORY, START_DATE, START_DATE_TIME, END_DATE, END_DATE_TIME, TOKEN, P3_REPOPATH, P4_REPOPATH, DOC_PATH, NONPROD_CHNL, PROD_CHNL) \r\nvalues \r\n('P3', '2024-02-28', '08:00', '2024-03-06', '08:00', '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', 'C:\\Users\\CA_OHPNM_DEV_11\\source\\repos\\p3\\', 'C:\\Users\\CA_OHPNM_DEV_11\\source\\repos\\p4\\', \r\n'C:\\Users\\CA_OHPNM_DEV_11\\source\\repos\\CommitHistory\\', \r\n'https://maximus365.webhook.office.com/webhookb2/903b3829-13aa-4287-adac-fc174eb3338d@b699068d-c14b-454b-a0bc-10918cf075d3/IncomingWebhook/acae91e680134d4cb2f39a61b38d564f/8adb92f9-0650-4596-a132-4d0a50684e85',\r\n'https://maximus365.webhook.office.com/webhookb2/903b3829-13aa-4287-adac-fc174eb3338d@b699068d-c14b-454b-a0bc-10918cf075d3/IncomingWebhook/acae91e680134d4cb2f39a61b38d564f/8adb92f9-0650-4596-a132-4d0a50684e85')\r\n";

        public const string insertTableRecordP4 = "Insert into AppInformation (REPOSITORY, START_DATE, START_DATE_TIME, END_DATE, END_DATE_TIME, TOKEN, P3_REPOPATH, P4_REPOPATH, DOC_PATH, NONPROD_CHNL, PROD_CHNL) \r\nvalues \r\n('P4', '2024-03-03', '08:00', '2024-03-06', '08:00', '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', 'C:\\Users\\CA_OHPNM_DEV_11\\source\\repos\\p3\\', 'C:\\Users\\CA_OHPNM_DEV_11\\source\\repos\\p4\\', \r\n'C:\\Users\\CA_OHPNM_DEV_11\\source\\repos\\CommitHistory\\', \r\n'https://maximus365.webhook.office.com/webhookb2/903b3829-13aa-4287-adac-fc174eb3338d@b699068d-c14b-454b-a0bc-10918cf075d3/IncomingWebhook/acae91e680134d4cb2f39a61b38d564f/8adb92f9-0650-4596-a132-4d0a50684e85',\r\n'https://maximus365.webhook.office.com/webhookb2/903b3829-13aa-4287-adac-fc174eb3338d@b699068d-c14b-454b-a0bc-10918cf075d3/IncomingWebhook/acae91e680134d4cb2f39a61b38d564f/8adb92f9-0650-4596-a132-4d0a50684e85')\r\n";

        public static class RosterStatusType
        {
            public const string Active = "Active";
            public const string Inactive = "Inactive";
        }
    }
}
