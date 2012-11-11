// (c)2007 Eric Lawrence
// This example is provided "AS IS" with no warranties, and confers no rights. 
//
// Prototype content-blocker allows testing of websites when some resources are blocked.
//
using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Text;
using Fiddler;
using System.IO;
using System.Diagnostics;
using Microsoft.Win32;
using System.Reflection;

[assembly: Fiddler.RequiredVersion("2.1.1.3")]
[assembly: AssemblyVersion("2.1.1.3")]
[assembly: AssemblyTitle("ContentBlock")]
[assembly: AssemblyDescription("Block HTTP(S) Requests")]
[assembly: AssemblyCompany("Eric Lawrence")]
[assembly: AssemblyProduct("ContentBlock")]

public class ContentBlocker : IAutoTamper, IHandleExecAction
{
    private List<string> slBlockedHosts;
    private bool bBlockerEnabled = false;
    string sSecret = new Random().Next().ToString();
    private System.Windows.Forms.MenuItem miBlockAHost;
    private System.Windows.Forms.MenuItem mnuContentBlock;
    private System.Windows.Forms.MenuItem miContentBlockEnabled;
    private System.Windows.Forms.MenuItem miEditBlockedHosts;
    private System.Windows.Forms.MenuItem miSplit1;
    private System.Windows.Forms.MenuItem miFlashAlwaysBlock;
    private System.Windows.Forms.MenuItem miBlockXDomainFlash;
    private System.Windows.Forms.MenuItem miLikelyPaths;
    private System.Windows.Forms.MenuItem miAutoTrim;
    private System.Windows.Forms.MenuItem miHideBlockedSessions;
    private System.Windows.Forms.MenuItem miSplit2;
    private System.Windows.Forms.MenuItem miSplit3;

    private void InitializeMenu()
    {
        this.miBlockAHost = new System.Windows.Forms.MenuItem();
        this.miEditBlockedHosts = new System.Windows.Forms.MenuItem();
        this.mnuContentBlock = new System.Windows.Forms.MenuItem();
        this.miContentBlockEnabled = new System.Windows.Forms.MenuItem();
        this.miSplit1 = new System.Windows.Forms.MenuItem();
		this.miFlashAlwaysBlock = new System.Windows.Forms.MenuItem();
		this.miBlockXDomainFlash = new System.Windows.Forms.MenuItem();
		this.miLikelyPaths = new System.Windows.Forms.MenuItem();
		this.miAutoTrim = new System.Windows.Forms.MenuItem();
        this.miHideBlockedSessions = new System.Windows.Forms.MenuItem();
		this.miSplit2 = new System.Windows.Forms.MenuItem();
        this.miSplit3 = new System.Windows.Forms.MenuItem();
		// 
		// mnuContentBlock
		// 
		this.mnuContentBlock.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
                                                                                        this.miContentBlockEnabled,
                                                                                        this.miSplit1,
                                                                                        this.miEditBlockedHosts,
                                                        								this.miLikelyPaths,
                                                                                        this.miSplit2,
                                                                                        this.miFlashAlwaysBlock,
                                                                                        this.miBlockXDomainFlash,
            					                                                        this.miSplit3,                                
											                                            this.miAutoTrim,
			                                                                            this.miHideBlockedSessions,
                                                        });
		this.mnuContentBlock.Text = "&ContentBlock";

        // 
        // miContentBlockEnabled
        // 
        this.miContentBlockEnabled.Index = 0;
        this.miContentBlockEnabled.Text = "&Enabled";
        this.miContentBlockEnabled.Click += new System.EventHandler(this.miBlockRule_Click);
        // 
        // miSplit1
        // 
        this.miSplit1.Index = 1;
        this.miSplit1.Text = "-";
        this.miSplit1.Checked = true;
        // 
        // miLikelyPaths
        // 
        this.miLikelyPaths.Index = 2;
        miLikelyPaths.Enabled = false;
        this.miLikelyPaths.Text = "&Block Paths";
        this.miLikelyPaths.Checked = true;
        this.miLikelyPaths.Click += new System.EventHandler(this.miBlockRule_Click);
        // 
        // miEditBlockedHosts
        // 
        this.miEditBlockedHosts.Index = 3;
        this.miEditBlockedHosts.Enabled = false;
        this.miEditBlockedHosts.Text = "Edit B&locked Hosts...";
        this.miEditBlockedHosts.Click += new System.EventHandler(this.miEditBlockedHosts_Click);
        // 
        // miSplit2
        // 
        this.miSplit2.Index = 4;
        miSplit2.Enabled = false;
        this.miSplit2.Text = "-";
        // 
        // miFlashAlwaysBlock
        // 
        this.miFlashAlwaysBlock.Index = 5;
        miFlashAlwaysBlock.Enabled = false;
        this.miFlashAlwaysBlock.Text = "Always Block &Flash";
        this.miFlashAlwaysBlock.Click += new System.EventHandler(this.miBlockRule_Click);
        // 
        // miBlockXDomainFlash
        // 
        miBlockXDomainFlash.Index = 6;
        miBlockXDomainFlash.Enabled = false;
        miBlockXDomainFlash.Checked = true; 
        miBlockXDomainFlash.Text = "Block &X-Domain Flash";
        miBlockXDomainFlash.Click += new System.EventHandler(this.miBlockRule_Click);
        // 
        // miSplit3
        // 
        this.miSplit3.Index = 7;
        this.miSplit3.Text = "-";
        this.miSplit3.Checked = true;
        // 
        // miAutoTrim
        // 
        this.miAutoTrim.Index = 8;
        miAutoTrim.Enabled = false;
        this.miAutoTrim.Text = "&AutoTrim to 400 sessions";
        this.miAutoTrim.Checked = false;
        this.miAutoTrim.Click += new System.EventHandler(this.miBlockRule_Click);
        // 
        // miHideBlockedSessions
        // 
        this.miHideBlockedSessions.Index = 9;
        miHideBlockedSessions.Enabled = false;
        this.miHideBlockedSessions.Text = "&Hide Blocked Sessions";
        this.miHideBlockedSessions.Checked = false;
        this.miHideBlockedSessions.Click += new System.EventHandler(this.miBlockRule_Click);
        // 
        // miBlockAHost
        // 
        this.miBlockAHost.Text = "Block this Host";
        this.miBlockAHost.Click += new System.EventHandler(this.miBlockAHost_Click);
    }

    public bool BlockAHost(string sHost)
    {
        if (!slBlockedHosts.Contains(sHost)){
            slBlockedHosts.Add(sHost);
        }
        return true;
    }

    private void miBlockAHost_Click(object sender, System.EventArgs e)
    {
        Session[] oSessions = FiddlerApplication.UI.GetSelectedSessions();
        foreach (Session oSession in oSessions)
        {
            try
            {
                BlockAHost(oSession.host.ToLower());
            }
            catch (Exception eX)
            {
                MessageBox.Show(eX.Message, "Cannot block host");
            }
        }
    }

    private void EnsureTransGif()
    {
        if (!File.Exists(CONFIG.GetPath("Responses") + "1pxtrans.dat")){
            try{
                byte[] arrHeaders = System.Text.Encoding.ASCII.GetBytes("HTTP/1.1 200 OK\r\nContentBlock: True\r\nContent-Type: image/gif\r\nConnection: close\r\nContent-Length: 49\r\n\r\n");

            byte[] arrBody = { 0x47, 0x49, 0x46, 0x38, 0x39, 0x61, 0x01, 0x00, 0x01, 0x00, 0x91, 0xFF, 0x00, 
                               0xFF, 0xFF, 0xFF, 0x00, 0x00, 0x00, 0xC0, 0xC0, 0xC0, 0x00, 0x00, 0x00, 0x21,
                               0xF9, 0x04, 0x01, 0x00, 0x00, 0x02, 0x00, 0x2c, 0x00, 0x00, 0x00, 0x00, 0x01,
                               0x00, 0x01, 0x00, 0x00, 0x02, 0x02, 0x54, 0x01, 0x00, 0x3B
						     };

            FileStream oFS = File.Create(CONFIG.GetPath("Responses") + "1pxtrans.dat");
            oFS.Write(arrHeaders, 0, arrHeaders.Length);
            oFS.Write(arrBody, 0, arrBody.Length);
            oFS.Close();
            }
            catch (Exception eX){
                MessageBox.Show(eX.ToString(), "Failed to create transparent gif...");
            }
        }
    }

    private void miEditBlockedHosts_Click(object sender, System.EventArgs e)
    {
        string sNewList = frmPrompt.GetUserString("Edit Blocked Host List", "Enter semi-colon delimited block list.", GetBlockedHostList(), true);
        if (null == sNewList)
        {
            FiddlerApplication.UI.sbpInfo.Text = "Block list left unchanged.";
            return;
        }
        else
        {
            FiddlerApplication.UI.sbpInfo.Text = "Block list updated.";
            slBlockedHosts = new List<string>(sNewList.ToLower().Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
        }
    }

	/// <summary>
	/// The logic for unchecking dependent options in this menu system isn't quite right, but it's fine for now.
	/// </summary>
	/// <param name="sender"></param>
	/// <param name="e"></param>
	private void miBlockRule_Click(object sender, System.EventArgs e)
	{
		MenuItem oSender = (sender as MenuItem);
		oSender.Checked = !oSender.Checked;

        bBlockerEnabled = miContentBlockEnabled.Checked;
        if (bBlockerEnabled)
        {
            EnsureTransGif();
        }

        // Enable menuitems based on overall enabled state.
        miEditBlockedHosts.Enabled = miFlashAlwaysBlock.Enabled = miAutoTrim.Enabled = miLikelyPaths.Enabled = miHideBlockedSessions.Enabled =
        miBlockXDomainFlash.Enabled = miSplit2.Enabled = bBlockerEnabled;
	}

    public ContentBlocker()
    {
        // Open key with Read permissions 
        RegistryKey oReg = Registry.CurrentUser.OpenSubKey(CONFIG.GetRegPath("Root") + @"\ContentBlock\");
        if (null != oReg)
        {
            string sList = oReg.GetValue("BlockHosts", String.Empty) as String;
            if ((sList != null && sList.Length > 0))
            {
                slBlockedHosts = new List<string>(sList.ToLower().Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
            }
            oReg.Close();
        }
        
        if (null == slBlockedHosts)
        {
            slBlockedHosts = new List<string>();
        }

        InitializeMenu();
    }
    
    public void OnLoad() {
        /*
         * NB: You might not get called here until ~after~ one of the AutoTamper methods was called.
         * This is okay for us, because we created our mnuContentBlock in the constructor and its simply not
         * visible anywhere until this method is called and we merge it onto the Fiddler Main menu.
         */
        FiddlerApplication.UI.mnuMain.MenuItems.Add(mnuContentBlock);
        FiddlerApplication.UI.mnuSessionContext.MenuItems.Add(0, miBlockAHost);
    }

    private string GetBlockedHostList()
    {
        StringBuilder sbBlockHosts = new StringBuilder();
        foreach (string s in slBlockedHosts)
        {
            if (s.Trim().Length > 3)
            {
                sbBlockHosts.Append(s.Trim() + ";");
            }
        }
        return sbBlockHosts.ToString();
    }

    public void OnBeforeUnload() {
        string sBlockedHosts = GetBlockedHostList();
        // Open key with Write permissions 
        RegistryKey oReg = Registry.CurrentUser.CreateSubKey(CONFIG.GetRegPath("Root") + @"\ContentBlock\");
        oReg.SetValue("BlockHosts", sBlockedHosts.ToString());
        oReg.Close();
    }

    /// <summary>
    /// Respond to user input from QuickExec box under the session list...
    /// </summary>
    /// <param name="sCommand"></param>
    /// <returns></returns>
    public bool OnExecAction(string sCommand){
        // TODO: Add "BLOCKSITE" and "UNBLOCKSITE" commands
        if (0 == String.Compare(sCommand, "blocklist", true))
        {
            StringBuilder sbBlockHosts = new StringBuilder();
            foreach (string s in slBlockedHosts)
            {
                sbBlockHosts.Append(s + "\n");
            }
            MessageBox.Show(sbBlockHosts.ToString(), "Block List...");
            return true;
        }
        return false; 
    }
	/// <summary>
	/// This function kills known matches early
	/// </summary>
	/// <param name="oSession"></param>
    public void AutoTamperRequestBefore(Session oSession) {
        // Return immediately if no rule is enabled
        if (!bBlockerEnabled) return;

        string oHost = oSession.host.ToLower();

        if ((oHost.StartsWith("ad.") ||
            oHost.StartsWith("ads.") ||
            slBlockedHosts.Contains(oHost))) {      // Consider tailmatch?

            if (miHideBlockedSessions.Checked) { 
                oSession["ui-hide"] = "userblocked"; 
            }
            else
            {
                oSession["ui-strikeout"] = "userblocked";
            }
            oSession["x-replywithfile"] = "1pxtrans.dat";
            return;
        }

        if (miLikelyPaths.Checked)
        {
            if (oSession.uriContains("/ad/") || oSession.uriContains("/ads/") || oSession.uriContains("/advert"))
            {
                if (!oSession.uriContains(sSecret))
                {
                    oSession.oRequest.FailSession(404, "Fiddler - ContentBlock", "Blocked <a href='//"+oSession.url+"?&"+sSecret+"'>Click to see</a>");
                    oSession.state = SessionStates.Done;
                    return;
                }
            }
        }

		// If Always Removing, do it and return immediately
        if (miFlashAlwaysBlock.Checked)
        {
            if (/*oSession.url.EndsWith(".swf") ||*/ oSession.oRequest.headers.Exists("x-flash-version"))
            {
                oSession.oRequest.FailSession(404, "Fiddler - ContentBlock", "Blocked Flash");
                oSession.state = SessionStates.Done;
                return;
            }
        }
        else if (miBlockXDomainFlash.Checked)
        {
            // Issue: We don't want to block a .SWF's x-domain request for data, but we do want to block the .SWF if it's xDomain.  Hrm.
            if (oSession.uriContains(".swf"))// || oSession.oRequest.headers.Exists("x-flash-version"))
            {
                bool bBlock = false;
                string sReferer = oSession.oRequest["Referer"];

                // Allow if referer was not sent.  Note, this is a hole.
                if (sReferer == String.Empty) return;

                // Block if Referer was from another domain
                if (!bBlock)
                {
		            Uri sFromURI;
		            Uri sToURI;
		            if ((Uri.TryCreate(sReferer, UriKind.Absolute, out sFromURI)) && (Uri.TryCreate("http://" + oSession.url, UriKind.Absolute, out sToURI)))
		            {
			            bBlock = (0 != Uri.Compare(sFromURI, sToURI, UriComponents.Host, UriFormat.Unescaped, StringComparison.InvariantCultureIgnoreCase));
		            }
                }

                if (bBlock)
                {
                    oSession.oRequest.FailSession(404, "Fiddler - ContentBlock", "Blocked Flash");
                    oSession.state = SessionStates.Done;
                }
                return;
            }
        }
	}

	public void AutoTamperRequestAfter(Session oSession){ /*noop*/ }
    public void AutoTamperResponseBefore(Session oSession) {/*noop*/}
    public void AutoTamperResponseAfter(Session oSession) {
	if (!bBlockerEnabled) return;

        if (miFlashAlwaysBlock.Checked && oSession.oResponse.headers.ExistsAndContains("Content-Type", "application/x-shockwave-flash"))
        {
            oSession.responseCode=404;
            oSession.utilSetResponseBody("Fiddler.ContentBlocked");
        }

        if (miAutoTrim.Checked && 0 == (oSession.id % 10))
        {
            FiddlerApplication.UI.TrimSessionList(400);
        }
    }
    public void OnBeforeReturningError(Session oSession) {/*noop*/}
}
