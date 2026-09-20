using System;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Data.SQLite;
using System.Net;
using System.Security.Policy;

namespace GitReleaseApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            createInitialDB();
            InitializeRecords("P3");
        }

        private void mainMethod(string type)
        {
            string path = System.IO.Directory.GetCurrentDirectory() + @"\ShellScripts\";
            string Outputpath = System.IO.Directory.GetCurrentDirectory() + @"\ShellScripts\GitScriptMainOutput.txt";
            string startDate = txt_StartDate.Text;
            string endDate = txt_EndDate.Text;
            string startTime = txt_StartTime.Text;
            string endTime = txt_EndTime.Text;
            string token = txt_Token.Text;
            string p3RepoPath = txt_P3RepoPath.Text;
            string p4RepoPath = txt_P4RepoPath.Text;
            string documentationPath = txt_DocumentationPath.Text;
            string p3p4 = (rbd_P3.IsChecked == true) ? "P3" : "P4";
            string uatBranchName = txt_UATBranchName.Text;
            string initialTagName = txt_InitialTagName.Text;
            string finalTagName = txt_FinalTagName.Text;

            System.Diagnostics.Process p = new System.Diagnostics.Process();
            p.StartInfo.FileName = System.IO.Directory.GetCurrentDirectory() + "/ShellScripts/GitScriptMain.sh";
            p.StartInfo.UseShellExecute = true;
            p.StartInfo.Arguments = path + " " + Outputpath + " " + p3p4 + " " + type + " " + p3RepoPath + " " + p4RepoPath + " " + startDate + " " + startTime + " " + endDate + " " + endTime + " " + token + " " + documentationPath + " " + uatBranchName + " " + initialTagName + " " + finalTagName;
            p.Start();
            p.WaitForExit();

            string readText = File.ReadAllText(Outputpath);

            consoleTXT.AppendText(readText);
        }

        private void btn_CheckOutDev(object sender, RoutedEventArgs e)
        {
            mainMethod("CheckoutDEV");
            consoleTXT.Text = consoleTXT.Text + "\n" + "CheckoutDEV Clicked";
        }

        private void btn_CreateDocumentation(object sender, RoutedEventArgs e)
        {
            mainMethod("CreateDocumentation");
            consoleTXT.Text = consoleTXT.Text + "\n" + "CreateDocumentation Clicked";
        }

        private void btn_CreateUATBranch(object sender, RoutedEventArgs e)
        {
            mainMethod("CreateBranchUAT");
            consoleTXT.Text = consoleTXT.Text + "\n" + "CreateBranchUAT Clicked";
        }
        private void btn_CheckOutUAT(object sender, RoutedEventArgs e)
        {
            mainMethod("CheckoutUAT");
            consoleTXT.Text = consoleTXT.Text + "\n" + "CheckoutUAT Clicked";
        }
        private void btn_CreateInitialTag(object sender, RoutedEventArgs e)
        {
            mainMethod("CreateInitialTag");
            consoleTXT.Text = consoleTXT.Text + "\n" + "CreateInitialTag Clicked";
        }
        private void btn_CreateFinalTag(object sender, RoutedEventArgs e)
        {
            mainMethod("CreateFinalTag"); 
            consoleTXT.Text = consoleTXT.Text + "\n" + "CreateFinalTag Clicked";
        }

        private void rbd_P4_Checked(object sender, RoutedEventArgs e)
        {
            lbl_RepoSelected.Content = "P4";
            InitializeRecords("P4");
            InitializeComboBox("P4");
        }

        private void rbd_P3_Checked(object sender, RoutedEventArgs e)
        {
            lbl_RepoSelected.Content = "P3";
            InitializeRecords("P3");
            InitializeComboBox("P3");
        }
        private void btn_Non(object sender, RoutedEventArgs e)
        {
            lbl_RepoSelected.Content = "P3";
        }

        private void btn_VersionChange(object sender, RoutedEventArgs e)
        {
            mainMethod("CheckoutUAT");

            if (rbd_P3.IsChecked == true)
            {
                //string text = File.ReadAllText(txt_P3RepoPath.Text + @"\\ohpnm-ssrs-p3\\app.config");
                //text = text.Replace("$BuildVersion$", txt_FinalTagName.Text);
                //File.WriteAllText(txt_P3RepoPath.Text + @"\\ohpnm-ssrs-p3\\app.config", text);
                //text = text.Replace("$BuildDate$", DateTime.Now.ToString("yyyy-MM-dd"));
                //File.WriteAllText(txt_P3RepoPath.Text + @"\\ohpnm-ssrs-p3\\app.config", text);

                //string text = File.ReadAllText(txt_P3RepoPath.Text + @"\\ohpnm-ssis-p3\\app.config");
                //text = text.Replace("$BuildVersion$", txt_FinalTagName.Text);
                //File.WriteAllText(txt_P3RepoPath.Text + @"\\ohpnm-ssis-p3\\app.config", text);
                //text = text.Replace("$BuildDate$", DateTime.Now.ToString("yyyy-MM-dd"));
                //File.WriteAllText(txt_P3RepoPath.Text + @"\\ohpnm-ssis-p3\\app.config", text);

                //text = File.ReadAllText(txt_P3RepoPath.Text + @"\\ohpnm-si-mq-p3\\app.config");
                //text = text.Replace("$BuildVersion$", txt_FinalTagName.Text);
                //File.WriteAllText(txt_P3RepoPath.Text + @"\\ohpnm-si-mq-p3\\app.config", text);
                //text = text.Replace("$BuildDate$", DateTime.Now.ToString("yyyy-MM-dd"));
                //File.WriteAllText(txt_P3RepoPath.Text + @"\\ohpnm-si-mq-p3\\app.config", text);

                //text = File.ReadAllText(txt_P3RepoPath.Text + @"\\ohpnm-tibco-p3\\app.config");
                //text = text.Replace("$BuildVersion$", txt_FinalTagName.Text);
                //File.WriteAllText(txt_P3RepoPath.Text + @"\\ohpnm-tibco-p3\\app.config", text);
                //text = text.Replace("$BuildDate$", DateTime.Now.ToString("yyyy-MM-dd"));
                //File.WriteAllText(txt_P3RepoPath.Text + @"\\ohpnm-tibco-p3\\app.config", text);

                string text = File.ReadAllText(txt_P3RepoPath.Text + @"\\ohpnm-src-p3\\PDMS\\PDMS\\web.config");
                text = text.Replace("$BuildVersion$", txt_FinalTagName.Text);
                File.WriteAllText(txt_P3RepoPath.Text + @"\\ohpnm-src-p3\\PDMS\\PDMS\\web.config", text);
                text = text.Replace("$BuildDate$", DateTime.Now.ToString("yyyy-MM-dd"));
                File.WriteAllText(txt_P3RepoPath.Text + @"\\ohpnm-src-p3\\PDMS\\PDMS\\web.config", text);
            }
            else
                {
                //    string text = File.ReadAllText(txt_P4RepoPath.Text + @"\\ohpnm-ssis\\app.config");
                //    text = text.Replace("$BuildVersion$", txt_FinalTagName.Text);
                //    File.WriteAllText(txt_P4RepoPath.Text + @"\\ohpnm-ssis\\app.config", text);
                //    text = text.Replace("$BuildDate$", DateTime.Now.ToString("yyyy-MM-dd"));
                //    File.WriteAllText(txt_P4RepoPath.Text + @"\\ohpnm-ssis\\app.config", text);

                string text = File.ReadAllText(txt_P4RepoPath.Text + @"\\ohpnm-src\\PDMS\\PDMS\\web.config");
                text = text.Replace("$BuildVersion$", txt_FinalTagName.Text);
                File.WriteAllText(txt_P4RepoPath.Text + @"\\ohpnm-src\\PDMS\\PDMS\\web.config", text);
                text = text.Replace("$BuildDate$", DateTime.Now.ToString("yyyy-MM-dd"));
                File.WriteAllText(txt_P4RepoPath.Text + @"\\ohpnm-src\\PDMS\\PDMS\\web.config", text);
            }

            mainMethod("CommitVersionChange");
            consoleTXT.Text = consoleTXT.Text + "\n" + "CommitVersionChange Clicked";
        }

        private async void btn_E2EP3Notification(object sender, RoutedEventArgs e)
        {
            bool retry = true;
            int maxRetry = 5;
            do
            {
                try
                {
                    HttpClientHandler clientHandler = new HttpClientHandler();
                    clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                    HttpClient client = new HttpClient(clientHandler);
                    var request = new HttpRequestMessage(HttpMethod.Post, txt_NonProdNotificationHook.Text);
                    var content = new StringContent("{    \"Title\": \""+txt_E2EP3Header.Text+"\",    \"Text\": \""+txt_E2EP3Descp.Text+"\"   }", null, "application/json");
                    request.Content = content;
                    var response = client.Send(request);
                    response.EnsureSuccessStatusCode();
                    retry = false;
                    maxRetry = 0;
                }
                catch (Exception ex)
                {
                    consoleTXT.Text = consoleTXT.Text + "\n" + ex.Message;
                    retry = true;
                    maxRetry--;
                }
            } while (retry == true && maxRetry > 0);
            consoleTXT.Text = consoleTXT.Text + "\n" + "E2EP3Notification Clicked";
        }

        private async void btn_E2EP4Notification(object sender, RoutedEventArgs e)
        {
            bool retry = true;
            int maxRetry = 5;
            do
            {
                try
                {
                    HttpClientHandler clientHandler = new HttpClientHandler();
                    clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                    HttpClient client = new HttpClient(clientHandler);
                    var request = new HttpRequestMessage(HttpMethod.Post, txt_NonProdNotificationHook.Text);
                    var content = new StringContent("{    \"Title\": \"" + txt_E2EP4Header.Text + "\",    \"Text\": \"" + txt_E2EP4Descp.Text + "\"   }", null, "application/json");
                    request.Content = content;
                    var response = client.Send(request);
                    response.EnsureSuccessStatusCode();
                    retry = false;
                    maxRetry = 0;
                }
                catch (Exception ex)
                {
                    consoleTXT.Text = consoleTXT.Text + "\n" + ex.Message;
                    retry = true;
                    maxRetry--;
                }
            } while (retry == true && maxRetry > 0);
            consoleTXT.Text = consoleTXT.Text + "\n" + "E2EP4Notification Clicked";
        }

        private async void btn_UATP4Notification(object sender, RoutedEventArgs e)
        {
            bool retry = true;
            int maxRetry = 5;
            do
            {
                try
                {
                    HttpClientHandler clientHandler = new HttpClientHandler();
                    clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                    HttpClient client = new HttpClient(clientHandler);
                    var request = new HttpRequestMessage(HttpMethod.Post, txt_NonProdNotificationHook.Text);
                    var content = new StringContent("{    \"Title\": \"" + txt_UATP4Header.Text + "\",    \"Text\": \"" + txt_UATP4Descp.Text + "\"   }", null, "application/json");
                    request.Content = content;
                    var response = client.Send(request);
                    response.EnsureSuccessStatusCode();
                    retry = false;
                    maxRetry = 0;
                }
                catch (Exception ex)
                {
                    consoleTXT.Text = consoleTXT.Text + "\n" + ex.Message;
                    retry = true;
                    maxRetry--;
                }
            } while (retry == true && maxRetry > 0);
            consoleTXT.Text = consoleTXT.Text + "\n" + "UATP4Notification Clicked";
        }

        private void btn_ProdNotification(object sender, RoutedEventArgs e)
        {
            bool retry = true;
            int maxRetry = 5;
            do
            {
                try
                {
                    HttpClientHandler clientHandler = new HttpClientHandler();
                    clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                    HttpClient client = new HttpClient(clientHandler);
                    var request = new HttpRequestMessage(HttpMethod.Post, txt_ProdNotificationHook.Text);
                    var content = new StringContent("{    \"Title\": \"" + txt_PRODHeader.Text + "\",    \"Text\": \"" + txt_PRODDescp.Text + "\"   }", null, "application/json");
                    request.Content = content;
                    var response = client.Send(request);
                    response.EnsureSuccessStatusCode();
                    retry = false;
                    maxRetry = 0;
                }
                catch (Exception ex)
                {
                    consoleTXT.Text = consoleTXT.Text + "\n" + ex.Message;
                    retry = true;
                    maxRetry--;
                }
            } while (retry == true && maxRetry > 0);
            consoleTXT.Text = consoleTXT.Text + "\n" + "ProdNotification Clicked";
        }

        private void InitializeRecords(string p3p4, string? version = null)
        {
            txt_StartDate.Text = fetchLastValue("START_DATE", p3p4, version);
            txt_StartTime.Text = fetchLastValue("START_DATE_TIME", p3p4, version);
            txt_EndDate.Text = fetchLastValue("END_DATE", p3p4, version);
            txt_EndTime.Text = fetchLastValue("END_DATE_TIME", p3p4, version);

            txt_UATBranchName.Text = fetchLastValue("UAT_BRANCH", p3p4, version);
            txt_InitialTagName.Text = fetchLastValue("INITIAL_TAG", p3p4, version);
            txt_FinalTagName.Text = fetchLastValue("FINAL_TAG", p3p4, version);

            txt_E2EP3Header.Text = fetchLastValue("E2EP3_HEADER", p3p4, version);
            txt_E2EP3Descp.Text = fetchLastValue("E2EP3_DESCP", p3p4, version);
            txt_E2EP4Header.Text = fetchLastValue("E2EP4_HEADER", p3p4, version);
            txt_E2EP4Descp.Text = fetchLastValue("E2EP4_DESCP", p3p4, version);
            txt_UATP4Header.Text = fetchLastValue("UAT_HEADER", p3p4, version);
            txt_UATP4Descp.Text = fetchLastValue("UAT_DESCP", p3p4, version);
            txt_PRODHeader.Text = fetchLastValue("PROD_HEADER", p3p4, version);
            txt_PRODDescp.Text = fetchLastValue("PROD_DESCP", p3p4, version);

            txt_Token.Text = fetchLastValue("TOKEN", p3p4, version);
            txt_P3RepoPath.Text = fetchLastValue("P3_REPOPATH", p3p4, version);
            txt_P4RepoPath.Text = fetchLastValue("P4_REPOPATH", p3p4, version);
            txt_DocumentationPath.Text = fetchLastValue("DOC_PATH", p3p4, version);
            txt_NonProdNotificationHook.Text = fetchLastValue("NONPROD_CHNL", p3p4, version);
            txt_ProdNotificationHook.Text = fetchLastValue("PROD_CHNL", p3p4, version);
            if (p3p4.Equals("P3"))
            {
                rbd_P3.IsChecked = true;
                rbd_P4.IsChecked = false;
            }
            else{
                rbd_P4.IsChecked = true;
                rbd_P3.IsChecked = false;
            }
        }

        private string fetchLastValue(string field, string repo, string? version = null)
        {
            string dbLocation = AppDomain.CurrentDomain.BaseDirectory + "Database\\GITDatabase.sqlite";
            string tableName = "AppInformation";
            string connectionString = "Data Source=" + dbLocation + ";Version=3;";
            SQLiteConnection m_dbConnection = new SQLiteConnection(connectionString);
            m_dbConnection.Open();

            string sql = string.Empty;

            if (string.IsNullOrEmpty(version))
            {
                sql = "SELECT REPOSITORY, START_DATE, START_DATE_TIME, END_DATE, END_DATE_TIME, UAT_BRANCH, INITIAL_TAG, FINAL_TAG, TOKEN, P3_REPOPATH, P4_REPOPATH, DOC_PATH, NONPROD_CHNL, PROD_CHNL, E2EP3_HEADER,\r\nE2EP3_DESCP, E2EP4_HEADER, E2EP4_DESCP, UAT_HEADER, UAT_DESCP, PROD_HEADER, PROD_DESCP FROM " + tableName + " WHERE REPOSITORY = '" + repo + "' ORDER BY ROWID DESC LIMIT 1";
            }
            else
            {
                sql = "SELECT REPOSITORY, START_DATE, START_DATE_TIME, END_DATE, END_DATE_TIME, UAT_BRANCH, INITIAL_TAG, FINAL_TAG, TOKEN, P3_REPOPATH, P4_REPOPATH, DOC_PATH, NONPROD_CHNL, PROD_CHNL, E2EP3_HEADER,\r\nE2EP3_DESCP, E2EP4_HEADER, E2EP4_DESCP, UAT_HEADER, UAT_DESCP, PROD_HEADER, PROD_DESCP FROM " + tableName + " WHERE REPOSITORY = '" + repo + "' AND FINAL_TAG = '"+ version + "' ORDER BY ROWID DESC LIMIT 1";
            }

            SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);

            var result = command.ExecuteReader();
            string test = string.Empty;
            while (result.Read())
            {
                test = result[field].ToString();
            }

            return test;
        }


        private void cmb_repoTags_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string p3p4 = (rbd_P3.IsChecked == true) ? "P3" : "P4"; 
            if (rbd_P3.IsChecked == true || rbd_P4.IsChecked == true)
            {
                if (cmb_repoTags.SelectedValue != null)
                {
                    InitializeRecords(p3p4, cmb_repoTags.SelectedValue.ToString());
                }                
            }
        }

        private void InitializeComboBox(string repo)
        {
            try
            {
                cmb_repoTags.Items.Clear();
                cmb_repoTags.SelectedIndex = -1;
                string dbLocation = AppDomain.CurrentDomain.BaseDirectory + "Database\\GITDatabase.sqlite";
                string tableName = "AppInformation";
                string connectionString = "Data Source=" + dbLocation + ";Version=3;";
                SQLiteConnection m_dbConnection = new SQLiteConnection(connectionString);
                m_dbConnection.Open();

                string sql = "SELECT DISTINCT ID, FINAL_TAG FROM " + tableName + " WHERE REPOSITORY = '" + repo + "' AND FINAL_TAG <> '' ORDER BY ROWID DESC LIMIT 5";
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);

                var result = command.ExecuteReader();
                string test = string.Empty;
                while (result.Read())
                {
                    cmb_repoTags.Items.Add(result["FINAL_TAG"].ToString());
                }
                m_dbConnection.Close();
            }catch(Exception ex)
            {
                throw ex;
            }
        }


        private void createInitialDB()
        {
            string dbLocation = AppDomain.CurrentDomain.BaseDirectory + "Database\\GITDatabase.sqlite";
            if (!File.Exists(dbLocation))
            {
                SQLiteConnection.CreateFile(dbLocation);

                string connectionString = "Data Source="+dbLocation+";Version=3;";
                SQLiteConnection m_dbConnection = new SQLiteConnection(connectionString);
                m_dbConnection.Open();

                string sql = Constants.createTableAppInfo;
                SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                command.ExecuteNonQuery();

                if (tableAlreadyExists(m_dbConnection, "AppInformation"))
                {
                    sql = Constants.insertTableRecordP3;
                    command = new SQLiteCommand(sql, m_dbConnection);
                    command.ExecuteNonQuery();

                    sql = Constants.insertTableRecordP4;
                    command = new SQLiteCommand(sql, m_dbConnection);
                    command.ExecuteNonQuery();
                }
                m_dbConnection.Close();
            }
            
        }

        private void insertRecord(string p3p4)
        {
            string dbLocation = AppDomain.CurrentDomain.BaseDirectory + "Database\\GITDatabase.sqlite";
            if (File.Exists(dbLocation))
            {
                string connectionString = "Data Source=" + dbLocation + ";Version=3;";
                SQLiteConnection m_dbConnection = new SQLiteConnection(connectionString);
                m_dbConnection.Open();

                if (tableAlreadyExists(m_dbConnection, "AppInformation"))
                {
                    string sql = "Insert into AppInformation (REPOSITORY, START_DATE, START_DATE_TIME, END_DATE, END_DATE_TIME, UAT_BRANCH, INITIAL_TAG, FINAL_TAG, TOKEN, P3_REPOPATH, P4_REPOPATH, DOC_PATH, NONPROD_CHNL, PROD_CHNL, E2EP3_HEADER,\r\nE2EP3_DESCP, E2EP4_HEADER, E2EP4_DESCP, UAT_HEADER, UAT_DESCP, PROD_HEADER, PROD_DESCP) \r\nvalues \r\n('"+p3p4+"', '"+txt_StartDate.Text+"', '"+txt_StartTime.Text+"', '"+txt_EndDate.Text+"', '"+txt_EndTime.Text+"', '"+txt_UATBranchName.Text+"', '"+txt_InitialTagName.Text+"', '"+txt_FinalTagName.Text+"', '"+txt_Token.Text+"', '"+txt_P3RepoPath.Text+"', \r\n'"+txt_P4RepoPath.Text+"', '"+txt_DocumentationPath.Text+"', '"+txt_NonProdNotificationHook.Text+"', '"+txt_ProdNotificationHook.Text+"', '"+txt_E2EP3Header.Text+"', '"+txt_E2EP3Descp.Text+"', \r\n'"+txt_E2EP4Header.Text+"', '"+txt_E2EP4Descp.Text+"', '"+txt_UATP4Header.Text+"', '"+txt_UATP4Descp.Text+"', '"+txt_PRODHeader.Text+"', '"+txt_PRODDescp.Text+"')";
                    SQLiteCommand command = new SQLiteCommand(sql, m_dbConnection);
                    command.ExecuteNonQuery();
                }
                m_dbConnection.Close();
            }

        }

        public static bool tableAlreadyExists(SQLiteConnection connection, string tableName)
        {
            string sql = "SELECT name FROM sqlite_master WHERE type='table' AND name='"+ tableName + "';";
            SQLiteCommand command = new SQLiteCommand(sql, connection);

            try
            {
                var result = command.ExecuteReader();

                if (result.HasRows)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        private void btn_SaveClick(object sender, RoutedEventArgs e)
        {
            txt_Token.IsReadOnly = true;
            txt_P3RepoPath.IsReadOnly = true;
            txt_P4RepoPath.IsReadOnly = true;
            txt_DocumentationPath.IsReadOnly = true;
            txt_NonProdNotificationHook.IsReadOnly = true;
            txt_ProdNotificationHook.IsReadOnly = true;

            

            if (rbd_P3.IsChecked == true || rbd_P4.IsChecked == true)
            {
                string p3p4 = (rbd_P3.IsChecked == true) ? "P3" : "P4";
                insertRecord(p3p4);
            }
            consoleTXT.Text = consoleTXT.Text + "\n" + "Save Clicked";
        }

        private void btn_EditClick(object sender, RoutedEventArgs e)
        {
            txt_Token.IsReadOnly = false;
            txt_P3RepoPath.IsReadOnly = false;
            txt_P4RepoPath.IsReadOnly = false;
            txt_DocumentationPath.IsReadOnly = false;
            txt_NonProdNotificationHook.IsReadOnly = false;
            txt_ProdNotificationHook.IsReadOnly = false;
            consoleTXT.Text = consoleTXT.Text + "\n" + "Edit Clicked";
        }

    }
}
