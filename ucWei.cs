using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using Fiddler;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Telnet;

namespace Weiddler
{
    public partial class ucWei : UserControl
    {

        public ucWei()
        {
#if DEBUG
            Log("Weiddler Tab Loading...");
#endif
            InitializeComponent();
            try
            {
                TryGetSoftwarePath("fiddler2", out dbFile);
            }
            catch (Exception ex)
            {
                Log(ex.ToString());
            }
            if (string.IsNullOrEmpty(dbFile))
            {
                Log("fiddler not found!");
                //return;
            }
            else
            {
                Log("dbFile:\t" + dbFile);
                try
                {
                    dbFile = Path.GetDirectoryName(dbFile);
                }
                catch (Exception ex)
                {
                    Log(ex.ToString());
                    dbFile = dbFile.ToLower().Replace("fiddler.exe", "");
                }
                dbFile = Path.Combine(dbFile, @"Scripts\weiddler.db");

                Log("dbFile:\t" + dbFile);
            }

            string connStr = "Data Source=" + dbFile + ";Pooling=true;password=";
#if DEBUG
            Log("connStr:\t" + connStr);
#endif
            SQLiteHelper.SetConnectionString(connStr);
        }

        private void NSButtonsChange(bool status)
        {
            btnPing.Enabled = status;
            btnNSLookup.Enabled = status;
            btnTracert.Enabled = status;
            btnTelnet.Enabled = status;
        }
        private string dbFile = @"D:\Program Files (x86)\Fiddler2\Scripts\weiddler.db";


        public static Dictionary<string, Color> UrlColorMapping = new Dictionary<string, Color>();
        public Dictionary<string, bool> nuSwitcher = new Dictionary<string, bool>();

        private void txtColor_MouseClick(object sender, MouseEventArgs e)
        {
            if (colorDialogUrl.ShowDialog() != DialogResult.Cancel)
            {
                txtColor.BackColor = colorDialogUrl.Color;

            }
        }

        private void Log(string msg)
        {
            FiddlerApplication.Log.LogString(msg);
        }

        private void btnSaveUrlColor_Click(object sender, EventArgs e)
        {
            string color = ColorHelper.ColorToHex(txtColor.BackColor);
            string url = txtUrl.Text;
            Log(string.Format("Color:{0};Url:{1}", color, url));
            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("Url cannot be empty!");
                return;
            }
            try
            {

                SQLiteConnectionStringBuilder connBuild = new SQLiteConnectionStringBuilder();
                connBuild.DataSource = dbFile;
                connBuild.Pooling = true;

                connBuild.Password = "";
                string connStr = connBuild.ConnectionString;
                if (!File.Exists(dbFile))
                {
                    SQLiteConnection.CreateFile(dbFile);
                    using (SQLiteConnection conn = new SQLiteConnection(connStr))
                    {
                        conn.Open();
                        string cmdStr = string.Format(@"CREATE TABLE [T_Config] (
                                                                        [AutoID] INTEGER  NOT NULL PRIMARY KEY,
                                                                        [Url] NVARCHAR(50)  UNIQUE NULL,
                                                                        [Color] NVARCHAR(10)  NULL
                                                                        )");
                        SQLiteCommand cmd = new SQLiteCommand(conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = cmdStr;
                        cmd.ExecuteNonQuery();
                    }
                }


                //string connStr = @"Data Source=D:\Program Files\Fiddler2\Scripts\weddler.db;Pooling=true;";

                using (SQLiteConnection conn = new SQLiteConnection(connStr))
                {
                    conn.Open();
                    string cmdStr = string.Format(@"update T_CONFIG set color='{0}' where url='{1}'", color, url);
                    SQLiteCommand cmd = new SQLiteCommand(conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = cmdStr;
                    if (cmd.ExecuteNonQuery() == 0)
                    {
                        cmdStr = string.Format("insert into T_CONFIG (url,color) values('{0}','{1}')", url, color);
                        cmd.CommandText = cmdStr;

                        cmd.ExecuteNonQuery();
                    }
                }
                LoadSQLiteData();
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                Log(ex.StackTrace);
                MessageBox.Show("SQLite Error! " + ex.Message);

            }
        }

        private void ucWei_Load(object sender, EventArgs e)
        {
            InitSQLiteData();
            //LoadSQLiteData();
        }

        private void LoadSQLiteData()
        {
            DataSet ds = SQLiteHelper.ExecuteDataSet("select * from t_config");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dgUrlColors.DataSource = ds.Tables[0];
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    string colorName = dr["color"].ToString();
                    if (!string.IsNullOrEmpty(colorName))
                    {
                        UrlColorMapping[dr["url"].ToString()] = ColorHelper.HexToColor(dr["color"].ToString());
                    }
                }
            }
        }
        public void InitSQLiteData()
        {

            if (UrlColorMapping != null && UrlColorMapping.Count == 0)
            {
                LoadSQLiteData();
            }
        }

        private void dgUrlColors_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            if (grid != null)
            {
                DataGridViewRow rc = grid.Rows[e.RowIndex];
                if (rc != null && !rc.IsNewRow)
                {
                    txtUrl.Text = rc.Cells[1].Value.ToString();
                    txtColor.BackColor = ColorHelper.HexToColor(rc.Cells[2].Value.ToString());
                }
            }
        }
        private void dgUrlColors_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0 && e.ColumnIndex == 2 && e.RowIndex <= dgUrlColors.Rows.Count)
                {

                    DataGridView grid = sender as DataGridView;
                    if (grid != null)
                    {
                        DataGridViewRow rc = grid.Rows[e.RowIndex];
                        if (rc != null && !rc.IsNewRow)
                        {
                            if (rc.Cells[2] != null && !string.IsNullOrEmpty(rc.Cells[2].ToString()))
                            {
                                rc.Cells[2].Style.BackColor = ColorHelper.HexToColor(rc.Cells[2].Value.ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString(ex.ToString());
            }

        }



        private void PingByDotNet(string host)
        {
            try
            {
                using (Ping ping = new Ping())
                {
                    PingReply reply = ping.Send(host, 3000);
                    rtbNU.AppendText(string.Format("Reply from {0} :bytes={1} time={2}ms Status={3} TTL={4} DontFragment={5}" + Environment.NewLine,
                        reply.Address.ToString(), reply.Buffer.Length, reply.RoundtripTime.ToString(),
                        reply.Status.ToString(), reply.Options.Ttl.ToString(), reply.Options.DontFragment.ToString()));

                    //async method
                    //ping.SendAsync(host, null);
                    //ping.PingCompleted += ping_PingCompleted;

                }
            }
            catch (Exception ex)
            {
                rtbNU.AppendText("ping " + host + " error," + ex.Message + Environment.NewLine);
            }
            finally
            {
                Application.DoEvents();
            }
        }

        void ping_PingCompleted(object sender, PingCompletedEventArgs e)
        {

            PingReply reply = e.Reply;

            rtbNU.AppendText(string.Format("Reply from {0} :bytes={1} time={2}ms Status={3} TTL={4} DontFragment={5}" + Environment.NewLine,
                            reply.Address.ToString(), reply.Buffer.Length, reply.RoundtripTime.ToString(),
                            reply.Status.ToString(), reply.Options.Ttl.ToString(), reply.Options.DontFragment.ToString()));

        }


        private void CmdExec(string cmdName, string strIp)
        {

            Process p = new Process();
            p.StartInfo.FileName = "cmd.exe";
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.Start();
            p.StandardInput.WriteLine(cmdName + " " + strIp);
            p.StandardInput.WriteLine("exit");
            string result = p.StandardOutput.ReadToEnd();
            p.Close();

            rtbNU.AppendText(result + Environment.NewLine);

        }
        private void CmdExec(object cmdName)
        {
            if (cmdName == null || string.IsNullOrEmpty(cmdName.ToString()))
            {
                throw new ArgumentException("invaild cmdname");
            }
            NSButtonsChange(false);
            using (Process p = new Process())
            {
                p.StartInfo.FileName = "cmd.exe";
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardInput = true;
                p.StartInfo.RedirectStandardOutput = true;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.CreateNoWindow = true;
                p.Start();
                string result = "";
                if (!cmdName.ToString().Equals("telnet", StringComparison.OrdinalIgnoreCase))
                {
                    p.StandardInput.WriteLine(cmdName + " " + txtHostOrIP.Text);
                    p.StandardInput.WriteLine("exit");
                }
                else
                {
                    p.StandardInput.WriteLine(cmdName + " " + txtHostOrIP.Text + " " + txtPort.Text);
                    //p.StandardInput.WriteLine("I am zwei");
                    //p.StandardInput.WriteLine("exit");
                }



                rtbNU.Clear();

                StringBuilder sb = new StringBuilder();
                int igRow = 4;
                int rowIdx = 0;
                result = p.StandardOutput.ReadLine();

                using (StreamReader reader = p.StandardOutput)
                {
                    string line = reader.ReadLine();//每次读取一行
                    while (!reader.EndOfStream)
                    {
                        rowIdx++;
                        if (rowIdx > igRow)
                        {
                            rtbNU.AppendText(line + Environment.NewLine);
                        }
                        line = reader.ReadLine();
                    }
                    reader.Close();
                }

                using (StreamReader reader = p.StandardError)
                {
                    try
                    {
                        rtbNU.ForeColor = Color.Red;
                        string line = reader.ReadLine();//每次读取一行
                        while (!reader.EndOfStream)
                        {
                            rowIdx++;
                            if (rowIdx > igRow)
                            {
                                rtbNU.AppendText(line + Environment.NewLine);
                            }
                            line = reader.ReadLine();
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        rtbNU.AppendText(ex.Message + Environment.NewLine);
                    }
                    finally
                    {
                        rtbNU.ForeColor = Color.Black;
                    }
                }



                //while (result != null)
                //{
                //    //FiddlerApplication.Log.LogString(result);
                //    rowIdx++;
                //    if (rowIdx > igRow)
                //    {
                //        sb.Append(result + Environment.NewLine);
                //    }
                //    result = p.StandardOutput.ReadLine();

                //}

                p.Close();
                NSButtonsChange(true);
                //rtbNU.AppendText(regex.Replace(sb.ToString(), ""));
            }

        }
        static Regex regex = new Regex(@"^[^exit].+exit", RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Multiline);

        private void txtPing_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.btnPing_Click(null, null);
            }
        }

        private void btnPing_Click(object sender, EventArgs e)
        {

            ThreadPool.QueueUserWorkItem(new WaitCallback(CmdExec), "ping");

        }
        private void btnNSLookup_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(CmdExec), "nslookup");
        }
        private void btnTracert_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(CmdExec), "tracert");
        }

        private void btnTelnet_Click(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(new WaitCallback(SocketTelnet));
        }

        private void SocketTelnet(object state)
        {
            NSButtonsChange(false);
            rtbNU.Clear();
            int port = 0;
            if (int.TryParse(txtPort.Text, out port))
            {
                using (Terminal term = new Terminal(txtHostOrIP.Text, port, 3000, 0, 0))
                {
                    string msg = string.Format("telnet {0}:{1} ", txtHostOrIP.Text, txtPort.Text);
                    if (term.Connect())
                    {
                        rtbNU.AppendText(msg + " Connected");
                    }
                    else
                    {
                        rtbNU.AppendText(msg + " Disconnected");
                    }
                }
            }
            else
            {
                MessageBox.Show("invalid Port");
            }
            NSButtonsChange(true);
        }

        #region DelegateMethods

        delegate void SetDelegateText(Control ctrl, string text);


        void SetText(Control ctrl, string text)
        {
            if (ctrl.InvokeRequired)
            {
                SetDelegateText sdt = new SetDelegateText(SetControlText);
                ctrl.Invoke(sdt, ctrl, text);
            }
            else
            {
                ctrl.Text = text;
            }
        }
        void SetControlText(Control ctrl, string text)
        {
            ctrl.Text = text;
        }


        #endregion

        private void btnRemove_Click(object sender, EventArgs e)
        {
            string url = txtUrl.Text;
            if (string.IsNullOrEmpty(url))
            {
                MessageBox.Show("Url cannot be empty!");
                return;
            }
            try
            {

                SQLiteConnectionStringBuilder connBuild = new SQLiteConnectionStringBuilder();
                connBuild.DataSource = dbFile;
                connBuild.Pooling = true;

                connBuild.Password = "";
                string connStr = connBuild.ConnectionString;
                if (!File.Exists(dbFile))
                {
                    SQLiteConnection.CreateFile(dbFile);
                    using (SQLiteConnection conn = new SQLiteConnection(connStr))
                    {
                        conn.Open();
                        string cmdStr = string.Format(@"CREATE TABLE [T_Config] (
                                                                        [AutoID] INTEGER  NOT NULL PRIMARY KEY,
                                                                        [Url] NVARCHAR(50)  UNIQUE NULL,
                                                                        [Color] NVARCHAR(10)  NULL
                                                                        )");
                        SQLiteCommand cmd = new SQLiteCommand(conn);
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandText = cmdStr;
                        cmd.ExecuteNonQuery();
                    }
                }
                using (SQLiteConnection conn = new SQLiteConnection(connStr))
                {
                    conn.Open();
                    string cmdStr = string.Format(@"delete from T_CONFIG  where url='{0}'", url);
                    SQLiteCommand cmd = new SQLiteCommand(conn);
                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = cmdStr;
                    cmd.ExecuteNonQuery();
                }
                LoadSQLiteData();
            }
            catch (Exception ex)
            {
                Log(ex.Message);
                Log(ex.StackTrace);
                MessageBox.Show("SQLite Error! " + ex.Message);

            }
        }


        public bool TryGetSoftwarePath(string softName, out string path)
        {
            string strPathResult = string.Empty;
            string strKeyName = "";     //"(Default)" key, which contains the intalled path 
            object objResult = null;

            Microsoft.Win32.RegistryValueKind regValueKind;
            Microsoft.Win32.RegistryKey regKey = null;
            Microsoft.Win32.RegistryKey regSubKey = null;

            try
            {
                //Read the key 
                regKey = Microsoft.Win32.Registry.LocalMachine;
                regSubKey = regKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\" + softName.ToString() + ".exe", false);

                //Read the path 
                objResult = regSubKey.GetValue(strKeyName);
                regValueKind = regSubKey.GetValueKind(strKeyName);

                //Set the path 
                if (regValueKind == Microsoft.Win32.RegistryValueKind.String)
                {
                    strPathResult = objResult.ToString();
                }
            }
            catch (System.Security.SecurityException ex)
            {
                Log("You have no right to read the registry!" + Environment.NewLine + ex.Message);
                throw new System.Security.SecurityException("You have no right to read the registry!", ex);

            }
            catch (Exception ex)
            {
                Log("Reading registry error!" + Environment.NewLine + ex.Message);
                throw new Exception("Reading registry error!", ex);

            }
            finally
            {

                if (regKey != null)
                {
                    regKey.Close();
                    regKey = null;
                }

                if (regSubKey != null)
                {
                    regSubKey.Close();
                    regSubKey = null;
                }
            }

            if (strPathResult != string.Empty)
            {
                //Found 
                path = strPathResult.Replace("\"", "");
                return true;
            }
            else
            {
                //Not found 
                path = null;
                return false;
            }
        }
    }



    public class UrlColor
    {
        public string Url { get; set; }
        public string Color { get; set; }
    }
}
