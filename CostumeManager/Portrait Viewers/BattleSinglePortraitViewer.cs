using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using BrawlLib.SSBB.ResourceNodes;
using System.IO;

namespace BrawlCostumeManager
{
    public class BattleSinglePortraitViewer : SinglePortraitViewer
    {
        public override int PortraitWidth
        {
            get { return 48; }
        }
        public override int PortraitHeight
        {
            get { return 56; }
        }

        public override ResourceNode PortraitRootFor(int charNum, int costumeNum)
        {

            string tex_number;
            int index;
            if (costumeNum < 0) return null;
            int charID = 0;
            if (charNum == 0) { charID = 0000; }
            if (charNum == 1) { charID = 0050; }
            if (charNum == 2) { charID = 0100; }
            if (charNum == 3) { charID = 0150; }
            if (charNum == 4) { charID = 0200; }
            if (charNum == 5) { charID = 0250; }
            if (charNum == 6) { charID = 0300; }
            if (charNum == 7) { charID = 0350; }
            if (charNum == 8) { charID = 0400; }
            if (charNum == 9) { charID = 0450; }
            if (charNum == 10) { charID = 0500; }
            if (charNum == 11) { charID = 0550; }
            if (charNum == 12) { charID = 0600; }
            if (charNum == 13) { charID = 0650; }
            if (charNum == 14) { charID = 0700; }
            if (charNum == 15) { charID = 0750; }
            if (charNum == 16) { charID = 0800; }
            if (charNum == 17) { charID = 0850; }
            if (charNum == 18) { charID = 0900; }
            if (charNum == 19) { charID = 0950; }
            if (charNum == 21) { charID = 1050; }
            if (charNum == 22) { charID = 1100; }
            if (charNum == 23) { charID = 1150; }
            if (charNum == 24) { charID = 1200; }
            if (charNum == 25) { charID = 1250; }
            if (charNum == 26) { charID = 1300; }
            if (charNum == 27) { charID = 1350; }
            if (charNum == 28) { charID = 1400; }
            if (charNum == 29) { charID = 1450; }
            if (charNum == 30) { charID = 1500; }
            if (charNum == 31) { charID = 1550; }
            if (charNum == 32) { charID = 1600; }
            if (charNum == 33) { charID = 1650; }
            if (charNum == 34) { charID = 1700; }
            if (charNum == 36) { charID = 1800; }
            if (charNum == 37) { charID = 1850; }
            if (charNum == 39) { charID = 1950; }
            if (charNum == 40) { charID = 2000; }
            if (charNum == 43) { charID = 2150; }
            if (charNum == 45) { charID = 2250; }
            if (charNum == 46) { charID = 2300; }
            
            if (costumeNum < 12)
            {
                 tex_number = (charID + costumeNum + 1).ToString("D4");
                 index = (charID + costumeNum + 1);
            }
            else
            {
                tex_number = (charID + costumeNum).ToString("D4");
                index = (charID + costumeNum);
            }
            
            
            ResourceNode bres;
            if (!bres_cache.TryGetValue(index, out bres))
            {
                string f = "../info/portrite/InfFace" + tex_number + ".brres";
                if (new FileInfo(f).Exists)
                {
                    bres_cache[index] = bres = (BRRESNode)NodeFactory.FromFile(null, f);
                }

                if (bres == null)
                {
                    label1.Text = "InfFace" + tex_number + ".brres: not found";
                    return null;
                }
            }
            return bres;
        }
        
        /*
        public override ResourceNode PortraitRootFor(int charNum, int costumeNum)
        {
            if (costumeNum < 0) return null;

            string tex_number = (charNum * 10 + costumeNum + 1).ToString("D3");
            int index = charNum * 10 + costumeNum + 1;
            ResourceNode bres;
            if (!bres_cache.TryGetValue(index, out bres))
            {
                string f = "../info/portrite/InfFace" + tex_number + ".brres";
                if (new FileInfo(f).Exists)
                {
                    bres_cache[index] = bres = (BRRESNode)NodeFactory.FromFile(null, f);
                }

                if (bres == null)
                {
                    label1.Text = "InfFace" + tex_number + ".brres: not found";
                    return null;
                }
            }
            return bres;
        }
        */
        public override ResourceNode MainTEX0For(ResourceNode node, int charNum, int costumeNum)
        {
            return node.FindChild("Textures(NW4R)", false).Children[0];
        }

        private Dictionary<int, ResourceNode> bres_cache;

        public BattleSinglePortraitViewer() : base()
        {
            UpdateDirectory();
        }

        public override void UpdateDirectory()
        {
            if (bres_cache != null)
            {
                foreach (ResourceNode node in bres_cache.Values)
                {
                    if (node != null) node.Dispose();
                }
            }
            bres_cache = new Dictionary<int, ResourceNode>();
        }

        protected override void saveButton_Click(object sender, EventArgs e)
        {
            foreach (int i in bres_cache.Keys)
            {
                if (bres_cache[i] != null && bres_cache[i].IsDirty)
                {
                    bres_cache[i].Merge();
                    bres_cache[i].Export("../info/portrite/InfFace" + i.ToString("D4") + ".brres");
                }
            }
        }
    }
}