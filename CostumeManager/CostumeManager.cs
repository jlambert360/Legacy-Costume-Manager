﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrawlManagerLib;
using System.Globalization;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Deployment.Application;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Net;
using System.Diagnostics;
using BrawlLib.SSBB.ResourceNodes;

namespace BrawlCostumeManager
{
    public partial class CostumeManager : Form
    {
        private static string DASH = "-";
        private static string TITLE = "Legacy Costume Manager";

        private List<PortraitViewer> portraitViewers;
        private PortraitMap pmap;

        public bool Swap_Wario;
        
        public CostumeManager()
        {
            InitializeComponent();
            isUpdateAvailable();

            pmap = new PortraitMap();

            try
            {
                Icon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
            }
            catch (Exception) { }
            portraitViewers = new List<PortraitViewer> { cssPortraitViewer1, resultPortraitViewer1, resultSinglePortraitViewerStocks1, battlePortraitViewer1, infoStockIconViewer1, infoStockIconViewer1 };

            if (!new DirectoryInfo("fighter").Exists)
            {
                if (new DirectoryInfo("/private/wii/app/RSBE/pf/fighter").Exists)
                {
                    Environment.CurrentDirectory = "/private/wii/app/RSBE/pf";
                }
                else if (new DirectoryInfo("/projectm/pf/fighter").Exists)
                {
                    Environment.CurrentDirectory = "/projectm/pf";
                }
            }

            cssPortraitViewer1.NamePortraitPreview = nameportraitPreviewToolStripMenuItem.Checked;
            modelManager1.ZoomOut = defaultZoomLevelToolStripMenuItem.Checked;

            readDir();
        }

        private void readDir()
        {
            if (!Directory.Exists("mario"))
            {
                foreach (string path in new[] {
                    "/private/wii/app/RSBE/pf/fighter",
                    "/projectm/pf/fighter",
                    "/fighter"
                })
                {
                    if (Directory.Exists(Environment.CurrentDirectory + path))
                    {
                        Environment.CurrentDirectory += path;
                        readDir();
                        return;
                    }
                }

                string findFighterFolder(string dir)
                {
                    if (dir.EndsWith("\\fighter")) return dir;
                    try
                    {
                        foreach (string subdir in Directory.EnumerateDirectories(dir))
                        {
                            string possible = findFighterFolder(subdir);
                            if (Directory.Exists(possible + "\\mario"))
                            {
                                return possible;
                            }
                        }
                    }
                    catch (Exception) { }
                    return null;
                }

                string fighterDir = findFighterFolder(Environment.CurrentDirectory);
                if (fighterDir != null)
                {
                    Environment.CurrentDirectory = Path.GetDirectoryName(fighterDir);
                    readDir();
                    return;
                }
            }

            Text = TITLE + " - " + System.Environment.CurrentDirectory;

            pmap.ClearAll();
            pmap.BrawlExScan("../BrawlEx");

            int selectedIndex = listBox1.SelectedIndex;
            listBox1.Items.Clear();
            listBox1.Items.Add(DASH);
            foreach (string charname in pmap.GetKnownFighterNames())
            {
                if (charname != null) listBox1.Items.Add(charname);
            }

            foreach (PortraitViewer p in portraitViewers)
            {
                p.UpdateDirectory();
            }
            if (selectedIndex >= 0)
            {
                listBox1.SelectedIndex = selectedIndex;
            }
        }

        public void LoadFile(string path)
        {
            modelManager1.LoadFile(path);
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            FighterFile ff = (FighterFile)listBox2.SelectedItem;
            string path = ff.FullName;
            LoadFile(path);
            RefreshPortraits();
        }

        public void RefreshPortraits()
        {
            FighterFile ff = (FighterFile)listBox2.SelectedItem;
            if (ff == null) return;

            int portraitNum = ff.CostumeNum;
            bool confident = false;

            if (pmap.ContainsMapping(ff.CharNum))
            {
                int[] mappings = pmap.GetPortraitMappings(ff.CharNum);
                int index = Array.IndexOf(mappings, ff.CostumeNum);
                if (index >= 0)
                {
                    portraitNum = index;
                    confident = true;
                }
            }
            if (Swap_Wario && ff.CharNum == pmap.CharBustTexFor("wario"))
            {
                portraitNum = (portraitNum + 6) % 12;
            }
            foreach (PortraitViewer p in portraitViewers)
            {
                p.UpdateImage(ff.CharNum, portraitNum);
            }
            costumeNumberLabel.UpdateImage(ff.CharNum, portraitNum, confident);
        }

        public void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateCostumeSelectionPane();
        }

        public void updateCostumeSelectionPane()
        {
            int selectedIndex = listBox2.SelectedIndex;

            string charname = listBox1.SelectedItem.ToString();
            listBox2.Items.Clear();
            if (charname == DASH)
            {
                foreach (FileInfo f in new DirectoryInfo(".").EnumerateFiles())
                {
                    string name = f.Name.ToLower();
                    if (name.EndsWith(".pac")/* || name.EndsWith(".pcs")*/)
                    {
                        listBox2.Items.Add(new FighterFile(f.Name, 1, 1));
                    }
                }
            }
            else
            {
                int charNum = pmap.CharBustTexFor(charname);
                int upperBound = 51;

                string pathNoExtAltZ = charname + "/fit" + charname + ("AltZ"), pathNoExtAltR = charname + "/fit" + charname + ("AltR");
                listBox2.Items.Add(new FighterFile(pathNoExtAltZ + ".pac", charNum, 0));
                listBox2.Items.Add(new FighterFile(pathNoExtAltR + ".pac", charNum, 0));

                for (int i = 0; i < upperBound; i++)
                {
                    if (i != 12)
                    {
                        string pathNoExt = charname + "/fit" + charname + i.ToString("D2");
                        listBox2.Items.Add(new FighterFile(pathNoExt + ".pac", charNum, i));
                        //listBox2.Items.Add(new FighterFile(pathNoExt + ".pcs", charNum, i));
                        if (charname.ToLower() == "kirby")
                        {
                            foreach (string hatchar in PortraitMap.KirbyHats)
                            {
                                listBox2.Items.Add(new FighterFile("kirby/fitkirby" + hatchar + i.ToString("D2") + ".pac", charNum, i));
                            }
                        }
                    }
                }

                listBox2.SelectedIndex = (selectedIndex < listBox2.Items.Count) ? selectedIndex : 0;
            }
        }

        private void changeDirectory_Click(object sender, EventArgs e)
        {
            var openFolder = new CommonOpenFileDialog();
            openFolder.AllowNonFileSystemItems = false;
            openFolder.Multiselect = false;
            openFolder.IsFolderPicker = true;
            openFolder.Title = "Select Folder";
            openFolder.InitialDirectory = "::{20D04FE0-3AEA-1069-A2D8-08002B30309D}";

            if (openFolder.ShowDialog() != CommonFileDialogResult.Ok)
            {
                MessageBox.Show("No Folder selected");
                return;
            }
            else
            {
                openFolder.Dispose();
                System.Environment.CurrentDirectory = openFolder.FileName;
                readDir();
            }
        }

        private void hidePolygonsCheckbox_Click(object sender, EventArgs e)
        {
            modelManager1.UseExceptions = hidePolygonsCheckbox.Checked;
            modelManager1.RefreshModel();
        }

        private void cBlissCheckbox_Click(object sender, EventArgs e)
        {
            projectMCheckbox.Checked = false;
            pmap = cBlissCheckbox.Checked
                ? new PortraitMap.CBliss()
                : new PortraitMap();
            pmap.BrawlExScan("../BrawlEx");
            foreach (PortraitViewer p in portraitViewers)
            {
                RefreshPortraits();
            }
        }

        private void projectMCheckbox_Click(object sender, EventArgs e)
        {
            cBlissCheckbox.Checked = false;
            pmap = projectMCheckbox.Checked
                ? new PortraitMap.Brawl()
                : new PortraitMap();
            pmap.BrawlExScan("../BrawlEx");
            foreach (PortraitViewer p in portraitViewers)
            {
                RefreshPortraits();
            }
        }

        private void swapPortraitsForWarioStylesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Swap_Wario = swapPortraitsForWarioStylesToolStripMenuItem.Checked;
            foreach (PortraitViewer p in portraitViewers)
            {
                RefreshPortraits();
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            listBox2.SelectedIndex = listBox2.IndexFromPoint(listBox2.PointToClient(Cursor.Position));
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string toDelete = (listBox2.SelectedItem as FighterFile).FullName;
            if (Path.HasExtension(toDelete))
            {
                toDelete = toDelete.Substring(0, toDelete.LastIndexOf('.'));
            }
            FileInfo pac = new FileInfo(toDelete + ".pac");
            FileInfo pcs = new FileInfo(toDelete + ".pcs");
            if (DialogResult.Yes == MessageBox.Show(
                "Are you sure you want to delete " + pac.Name + "/" + pcs.Name + "?",
                "Confirm", MessageBoxButtons.YesNo))
            {
                modelManager1.LoadFile(null);
                if (pac.Exists) pac.Delete();
                if (pcs.Exists) pcs.Delete();
                updateCostumeSelectionPane();
            }
        }

        private void copyToToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.Filter = "PAC Archive (*.pac)|*.pac|" +
                    "Compressed PAC Archive (*.pcs)|*.pcs|" +
                    "Archive Pair (*.pair)|*.pair";
                if (dlg.ShowDialog(this) == DialogResult.OK)
                {
                    modelManager1.WorkingRoot.Export(dlg.FileName);
                }
            }
        }

        private void copyToOtherPacpcsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string charfile = ((FighterFile)listBox2.SelectedItem).FullName;
            if (charfile.EndsWith(".pac", StringComparison.InvariantCultureIgnoreCase))
            {
                updateCostumeSelectionPane();
            }
            else
            {
                MessageBox.Show("Not a .pac file");
            }

        }

        private void nameportraitPreviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cssPortraitViewer1.NamePortraitPreview = nameportraitPreviewToolStripMenuItem.Checked;
        }

        private void backgroundColorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ColorDialog cd = new ColorDialog())
            {
                cd.Color = cssPortraitViewer1.BackColor;
                if (cd.ShowDialog() == DialogResult.OK)
                {
                    foreach (var pv in portraitViewers)
                    {
                        pv.BackColor = cd.Color;
                    }
                }
            }
        }

        private void screenshotPortraitsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bitmap screenshot = modelManager1.GrabScreenshot(true);

            int size = Math.Min(screenshot.Width, screenshot.Height);
            Bitmap rect = new Bitmap(size, (int)(size * 160.0 / 128.0));
            using (Graphics g = Graphics.FromImage(rect))
            {
                int x = (screenshot.Width - rect.Width) / -2;
                int y = (screenshot.Height - rect.Height) / -2;
                g.DrawImage(screenshot, x, y);
            }

            string iconFile = System.IO.Path.GetTempPath() + Guid.NewGuid().ToString() + ".png";

            BitmapUtilities.Resize(rect, new Size(128, 160)).Save(iconFile);
            cssPortraitViewer1.ReplaceMain(iconFile, false);

            try
            {
                File.Delete(iconFile);
            }
            catch (Exception)
            {
                Console.WriteLine("Could not delete temporary file " + iconFile);
            }
        }

        private void limitModelViewerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            modelManager1.ModelPreviewSize = limitModelViewerToolStripMenuItem.Checked
                ? (Size?)new Size(256, 320)
                : null;
        }

        private void defaultZoomLevelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            modelManager1.ZoomOut = defaultZoomLevelToolStripMenuItem.Checked;
            modelManager1.RefreshModel();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            new AboutBSM(Icon, System.Reflection.Assembly.GetExecutingAssembly()).ShowDialog(this);
        }

        private void updateMewtwoHatForCurrentKirbyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string kirby, hat;

            FighterFile ff = (FighterFile)listBox2.SelectedItem;
            if (pmap.CharBustTexFor("kirby") != ff.CharNum)
            {
                MessageBox.Show(this, "Select a Kirby costume before using this feature.");
                return;
            }

            string p = ff.FullName.ToLower();
            string nn = ff.CostumeNum.ToString("D2");
            if (p.Contains("fitkirbymewtwo"))
            {
                kirby = "kirby/FitKirby" + nn + ".pcs";
                hat = ff.FullName;
            }
            else
            {
                kirby = ff.FullName;
                hat = "kirby/FitKirbyMewtwo" + nn + ".pac";
            }
            if (!File.Exists(kirby))
            {
                MessageBox.Show(this, "Could not find file: " + kirby);
                return;
            }
            if (!File.Exists(hat))
            {
                MessageBox.Show(this, "Could not find file: " + hat);
                return;
            }

            if (DialogResult.OK == MessageBox.Show(this, "Copy from " + kirby + " to " + hat + "?", Text, MessageBoxButtons.OKCancel))
            {
                KirbyCopy.Copy(kirby, hat);
            }
        }

        private void use16ptFontToolStripMenuItem_Click(object sender, EventArgs e)
        {
            float? size = use16ptFontToolStripMenuItem.Checked
                ? 16f
                : (float?)null;

            this.Font = new Font(this.Font.FontFamily, size ?? 8.25f, this.Font.Style);
            toolStrip1.Font = new Font(toolStrip1.Font.FontFamily, size ?? 9f, toolStrip1.Font.Style);
        }

        private void updateCheck_Click(object sender, EventArgs e)
        {
            if (isUpdateAvailable() == false)
            {
                MessageBox.Show("Costume Manager is up to date!", "Update");
            }
        }

        public bool isUpdateAvailable()
        {
            Assembly b = Assembly.GetAssembly(this.GetType());

            string version = b.GetName().Version.ToString();

            System.Net.WebClient wc = new System.Net.WebClient();
            byte[] raw = wc.DownloadData("https://docs.google.com/document/d/19R1Tx1CFHCjBaw_aJ4pot44SqQOaSgZFQCaaqnP8DyA/edit");

            string webData = System.Text.Encoding.UTF8.GetString(raw);

            if (!webData.Contains(version))
            {
                if (MessageBox.Show("An update is available! Would you like to download the update?", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Asterisk) == DialogResult.Yes)
                {
                    System.Diagnostics.Process.Start("https://github.com/Birdthulu/Legacy-Costume-Manager/releases");
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        private void copyStockIconsToResultScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string stgResult_path = null;
            string singlePlayerStocks_path = null;

            //Check for files
            if (File.Exists("../stage/melee/STGRESULT.pac"))
            {
                string pathResult = "../stage/melee/STGRESULT.pac";
                stgResult_path = pathResult;
            }
            if (File.Exists("../menu/common/StockFaceTex.brres"))
            {
                string path = "../menu/common/StockFaceTex.brres";
                singlePlayerStocks_path = path;
            }
            //Send error if files are missing
            if (stgResult_path == null)
            {
                MessageBox.Show("STGRESULT not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (singlePlayerStocks_path == null)
            {
                MessageBox.Show("StockFaceTex.brres not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            //Confirm you want to merge files
            else if (DialogResult.OK == MessageBox.Show("Overwrite the current STGRESULT?", "Overwrite File", MessageBoxButtons.OKCancel))
            {
                //Generate temp files
                string tempFile = Path.GetTempFileName();
                string stockFaceTextmp = Path.GetTempFileName();

                //Copy files into a temp file that is to be overwritten
                File.Copy(stgResult_path, tempFile, true);
                File.Copy(singlePlayerStocks_path, stockFaceTextmp, true);

                //Search for STGRESULT.pac/2.pac/Misc Data [120]
                ResourceNode stgResult = NodeFactory.FromFile(null, stgResult_path);
                ARCNode resultStocks = stgResult.FindChild("2", true) as ARCNode;
                BRRESNode resultStockIcons = resultStocks.FindChild("Misc Data [120]", true) as BRRESNode;

                //Send error if files are missing
                if (resultStocks == null)
                {
                    MessageBox.Show(this.ParentForm, "The STGRESULT file does not appear to have a 2.pac.",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (resultStockIcons == null)
                {
                    MessageBox.Show(this.ParentForm, "The 2.pac file does not appear to have a Misc Data [120].",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    //Replace STGRESULT.pac/2.pac/Misc Data [120] with StockFaceTex.brres
                    resultStockIcons.Replace(stockFaceTextmp);
                    //Merge the files
                    stgResult.Merge();
                    //Export STGRESULT.pac
                    stgResult.Export(stgResult_path);
                }
                //Delete generated temp files
                File.Delete(stockFaceTextmp);
                File.Delete(tempFile);
            }
        }
    }
}
