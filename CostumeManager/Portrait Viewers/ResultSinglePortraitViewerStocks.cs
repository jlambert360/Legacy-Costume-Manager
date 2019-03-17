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
    public class ResultSinglePortraitViewerStocks : SinglePortraitViewer
    {
        public override int PortraitWidth
        {
            get { return 32; }
        }
        public override int PortraitHeight
        {
            get { return 32; }
        }

        public override ResourceNode PortraitRootFor(int charNum, int costumeNum)
        {
            ResourceNode bres;

            if (!bres_cache1.TryGetValue(charNum, out bres))
            {

                string f = "../menu/common/StockFaceTex.brres";
                if (new FileInfo(f).Exists)
                {
                    bres_cache1[charNum] = bres = (BRRESNode)NodeFactory.FromFile(null, f);
                }
                
				if (bres == null) {
					label1.Text = "StockFaceTex not found";
					return null;
				}
            }
            return bres;
        }

        /*/Textures(NW4R)/InfStc." + (i * 50 + (j <= 12 ? j + 1 : j)).ToString("D4")) };*/
        //StockFaceTex.brres
        //(costumeNum<=12 ? costumeNum + 1 : costumeNum)
        public override ResourceNode MainTEX0For(ResourceNode brres, int charNum, int costumeNum)
        {
            string path = "Textures(NW4R)/InfStc." + (charNum == 37 || charNum == 19 || charNum == 34 || charNum == 40 || charNum == 46 ? (charNum * 50 + (costumeNum <= 12 ? costumeNum + 1 : costumeNum)).ToString("D4") : (charNum * 50 + (costumeNum + 1)).ToString("D4"));
            return brres.FindChild(path, true);
        }

        private Dictionary<int, ResourceNode> bres_cache1;

        public ResultSinglePortraitViewerStocks() : base()
        {
            UpdateDirectory();
        }

        public override void UpdateDirectory()
        {
            if (bres_cache1 != null)
            {
                foreach (ResourceNode node in bres_cache1.Values)
                {
                    if (node != null) node.Dispose();
                }
            }
            bres_cache1 = new Dictionary<int, ResourceNode>();
        }

        protected override void saveButton_Click(object sender, EventArgs e)
        {
            foreach (int i in bres_cache1.Keys)
            {
                if (bres_cache1[i] != null && bres_cache1[i].IsDirty)
                {
                    bres_cache1[i].Merge();
                    bres_cache1[i].Export("../menu/common/StockFaceTex.brres");
                }
            }
        }
    }
}
