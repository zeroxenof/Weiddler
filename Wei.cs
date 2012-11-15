using System;
using System.Windows.Forms;
using Fiddler;
using System.Collections.Generic;
using System.Drawing;

[assembly: Fiddler.RequiredVersion("2.3.6.7")]

public class Wei : IAutoTamper    // Ensure class is public, or Fiddler won't see it!
{

    private System.Windows.Forms.MenuItem mnuColors;
    string defaultColor = "yellowgreen";

    public Wei()
    {
        /* NOTE: It's possible that Fiddler UI isn't fully loaded yet, so don't add any UI in the constructor.

           But it's also possible that AutoTamper* methods are called before OnLoad (below), so be
           sure any needed data structures are initialized to safe values here in this constructor */


        //List<MenuItem> list = new List<MenuItem>();
        //list.Add(new MenuItem() { Text = Color.GreenYellow.Name });
        //list.Add(new MenuItem() { Text = Color.Red.Name });
        //list.Add(new MenuItem() { Text = Color.Blue.Name });
        //list.Add(new MenuItem() { Text = Color.LightSkyBlue.Name });
        //list.Add(new MenuItem() { Text = Color.Green.Name });
        //foreach (MenuItem mi in list)
        //{
        //    mi.Click += new EventHandler(mi_Click);
        //}
        //mnuColors = new MenuItem();
        //mnuColors.MenuItems.AddRange(list.ToArray());
        //this.mnuColors.Text = "Highlight&Color";
#if DEBUG
        Log("Weiddler creating.......");
#endif
    }

    //void mi_Click(object sender, EventArgs e)
    //{
    //    defaultColor = (sender as MenuItem).Text.ToLower();
    //}



    public void OnLoad()
    {

        /* Load your UI here */
        FiddlerScript fs = new FiddlerScript();

        try
        {
            fs.UI.lvSessions.AddBoundColumn("HostIP", 100, "x-hostIP");  //Add host IP
            //Change HostIP column index, in the left of Host column
            int idx = 0;
            for (int i = 0; i < fs.UI.lvSessions.Columns.Count; i++)
            {
                if (fs.UI.lvSessions.Columns[i].Text.Equals("Host", StringComparison.OrdinalIgnoreCase))
                {
                    idx = i;
                    break;
                }
            }
            fs.UI.lvSessions.SetColumnOrderAndWidth("HostIP", idx, 100);
            fs.ReloadScript();
        }
        catch (Exception ex)
        {
            Log(ex.ToString());
        }

#if DEBUG
        Log("Weiddler OnLoad.......");
#endif

        TabPage oPage = new TabPage("Weiddler");
        oPage.ImageIndex = (int)Fiddler.SessionIcons.Silverlight;  //This sets the Icon image used in the tab
        Weiddler.ucWei oView = new Weiddler.ucWei(); //UserControl1 is a Windows Forms UserControl class
        try
        {
            oView.InitSQLiteData();
        }
        catch (Exception ex)
        {
            Log(ex.ToString());
        }
        oView.Dock = DockStyle.Fill;
        oPage.Controls.Add(oView);
        FiddlerApplication.UI.tabsViews.TabPages.Add(oPage);


    }
    public void OnBeforeUnload() { }
    public void AutoTamperRequestBefore(Session oSession) { }
    public void AutoTamperRequestAfter(Session oSession) { }
    public void AutoTamperResponseBefore(Session oSession)
    {
        if (Weiddler.ucWei.UrlColorMapping != null)
        {
            try
            {
                if (Weiddler.ucWei.UrlColorMapping.Keys.Count > 0)
                {
                    if (Weiddler.ucWei.UrlColorMapping.ContainsKey(oSession.hostname.ToLower()))
                    {
                        Color sessionColor = Weiddler.ucWei.UrlColorMapping[oSession.hostname.ToLower()];
                        oSession["ui-backcolor"] = Weiddler.ColorHelper.ColorToHex(sessionColor);
                        Color antColor = Color.FromArgb(0xff - sessionColor.R, 0xff - sessionColor.G, 0xff - sessionColor.B);
                        oSession["ui-color"] = Weiddler.ColorHelper.ColorToHex(antColor);
                    }
                }
            }
            catch (Exception ex)
            {
                FiddlerApplication.Log.LogString(ex.Message);
            }

        }
    }
    public void AutoTamperResponseAfter(Session oSession)
    {
    }
    public void OnBeforeReturningError(Session oSession) { }

    public void Log(string msg)
    {
        FiddlerApplication.Log.LogString(msg);
    }
}