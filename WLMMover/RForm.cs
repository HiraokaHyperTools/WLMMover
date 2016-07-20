using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using WLMMailRulesMover.OLEXPDbx;
using System.Diagnostics;
using Microsoft.Win32;
using Microsoft.Isam.Esent.Interop;

namespace WLMMover {
    public partial class RForm : Form {
        public RForm() {
            InitializeComponent();
        }

        private void RForm_Load(object sender, EventArgs e) {
            Text += " " + Application.ProductVersion;

            llDetExp_LinkClicked(sender, null);
            llDetWLM_LinkClicked(sender, null);

            if (!Directory.Exists(tbDirExp.Text)) tbDirExp.Text = String.Empty;
            if (!Directory.Exists(tbDirWLM.Text)) tbDirWLM.Text = String.Empty;
        }

        class AH : IDisposable {
            Cursor prev;

            public AH() {
                prev = Cursor.Current;
                Cursor.Current = Cursors.WaitCursor;
            }

            #region IDisposable メンバ

            public void Dispose() { Cursor.Current = prev; }

            #endregion
        }

        class FUt {
            public static IEnumerable<FolderExpress> GetFolders(DbxFH fh) {
                foreach (DbxTreent fe in fh.RootNodeAll.Entries) {
                    yield return new FolderExpress(fe.IndexedInfo);
                }
            }
        }

        private void bRefExp_Click(object sender, EventArgs e) {
            EP.SetError(bRefExp, null);

            if (Directory.Exists(tbDirExp.Text))
                fbdExp.SelectedPath = tbDirExp.Text;
            if (fbdExp.ShowDialog(this) == DialogResult.OK) {
                tbDirExp.Text = fbdExp.SelectedPath;
            }
        }

        private void bReadExp_Click(object sender, EventArgs e) {
            EP.SetError(bReadExp, null);

            using (AH ah = new AH()) {
                MemoryStream si;
                try {
                    si = new MemoryStream(File.ReadAllBytes(Path.Combine(tbDirExp.Text, "Folders.dbx")));
                }
                catch (FileNotFoundException) {
                    EP.SetError(bRefExp, "Folders.dbxが、見つかりませんでした。\n\n申し訳ございません。\n\n自身の手で" + label1.Text + "を特定してください。");
                    return;
                }
                BinaryReader br = new BinaryReader(si);
                DbxFH fh = new DbxFH(br, 0);
                List<FolderExpress> fes = new List<FolderExpress>(FUt.GetFolders(fh));
                tvE.Nodes.Clear();
                WalkExp(fes, tvE.Nodes, 1);
            }
        }

        private void WalkExp(List<FolderExpress> fes, TreeNodeCollection tnc, int iParent) {
            foreach (FolderExpress f in fes) {
                if (f.ParentKey == iParent) {
                    TreeNode tn = tnc.Add(f.Disp);
                    tn.Name = tn.Text;
                    tn.ImageKey = "D";
                    tn.Tag = f;
                    WalkExp(fes, tn.Nodes, f.Key);
                    tn.Expand();
                }
            }
        }
        private void WalkW(List<FolderW> fes, TreeNodeCollection tnc, Int64 iParent) {
            foreach (FolderW f in fes) {
                if (f.FLDCOL_PARENT == iParent) {
                    TreeNode tn = tnc.Add(f.FLDCOL_NAME);
                    tn.Name = tn.Text;
                    //tn.Text += String.Format(" {1:X2} {0:X8}", f.FLDCOL_FLAGS, f.FLDCOL_TYPE);
                    if (0 == (f.FLDCOL_FLAGS & 0x00040000)) {
                        tn.ImageKey = "B";
                        tn.SelectedImageKey = "B1";
                    }
                    else {
                        tn.ImageKey = "D";
                        tn.SelectedImageKey = "D";
                    }
                    if (0 != (f.FLDCOL_FLAGS & 0x20000000)) {
                        tn.BackColor = Color.Red;
                        tn.ForeColor = Color.Black;
                    }
                    tn.Tag = f;
                    WalkW(fes, tn.Nodes, f.FLDCOL_ID);
                    if (0 != (f.FLDCOL_FLAGS & 0x00800000)) tn.Expand();

                    if (0 != (f.FLDCOL_FLAGS & 1))
                        if (0 != (f.FLDCOL_FLAGS & 8))
                            if (0 != (f.FLDCOL_FLAGS & 0x40000))
                                cbWAcc.Items.Add(tn);

                    //if (tn.Text.Contains("Test ")) tvW.SelectedNode = tn;
                }
            }
        }


        private void label4_Click(object sender, EventArgs e) {

        }


        private void bReadWLM_Click(object sender, EventArgs e) {
            EP.SetError(bReadWLM, null);

            using (AH ah = new AH()) {
                using (WDb wdb = new WDb()) {
                    try {
                        wdb.Open(tbDirWLM.Text, true);
                    }
                    catch (EsentFileNotFoundException) {
                        EP.SetError(bRefWLM, "必要なファイルが見つかりませんでした。\n\n申し訳ございません。\n\n自身の手で" + label3.Text + "を特定してください。");
                        return;
                    }
                    catch (EsentInvalidPathException) {
                        EP.SetError(bRefWLM, "必要なファイルが見つかりませんでした。\n\n申し訳ございません。\n\n自身の手で" + label3.Text + "を特定してください。");
                        return;
                    }
                    List<FolderW> al = wdb.ReadFolders();
                    cbWAcc.Items.Clear();
                    WalkW(al, tvW.Nodes, -1);
                    if (cbWAcc.Items.Count != 0)
                        cbWAcc.SelectedIndex = 0;
                }
            }
        }

        private void bRefWLM_Click(object sender, EventArgs e) {
            EP.SetError(bRefWLM, null);

            if (Directory.Exists(tbDirWLM.Text))
                fbdWLM.SelectedPath = tbDirWLM.Text;
            if (fbdWLM.ShowDialog(this) == DialogResult.OK) {
                tbDirWLM.Text = fbdWLM.SelectedPath;
            }
        }

        private void llDetWLM_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            RegistryKey rkW = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows Live Mail", false);
            if (rkW != null) {
                String StoreRoot = Convert.ToString(rkW.GetValue("Store Root"));
                if (StoreRoot != null && Directory.Exists(StoreRoot)) {
                    String fp = Path.Combine(StoreRoot, "Mail.MSMessageStore");
                    if (File.Exists(fp)) {
                        tbDirWLM.Text = StoreRoot;
                    }
                }
                else if (e != null) MessageBox.Show(this, "検出に失敗しました。");
            }
            else if (e != null) MessageBox.Show(this, "検出に失敗しました。");
        }

        private void llDetExp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            RegistryKey rk50 = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Outlook Express\5.0", false);
            if (rk50 != null) {
                String StoreRoot = Convert.ToString(rk50.GetValue("Store Root"));
                if (StoreRoot != null && Directory.Exists(StoreRoot)) {
                    String fp = Path.Combine(StoreRoot, "Mail.MSMessageStore");
                    if (File.Exists(fp)) {
                        tbDirWLM.Text = StoreRoot;
                    }
                }
                else if (e != null) MessageBox.Show(this, "検出に失敗しました。");
            }
            else if (e != null) MessageBox.Show(this, "検出に失敗しました。");
        }

        private void bMig_Click(object sender, EventArgs e) {
            if (ds1.Mig.Select().Length == 0) {
                EP.SetError(bMake, "移行計画が空っぽのようです。\n\nまずは、「計画」をクリックしてください");
                return;
            }
            if (MessageBox.Show(this, "本当に、実行しますか？", Application.ProductName, MessageBoxButtons.YesNo) != DialogResult.Yes)
                return;
            List<MigReq> al = new List<MigReq>();
            ds1.AcceptChanges();
            foreach (DS1.MigRow row in ds1.Mig.Select()) {
                TreeNode tn = null;
                foreach (String key in row.Exp.Split('\\')) {
                    tn = ((tn == null) ? tvE.Nodes : tn.Nodes)[key];
                    if (tn == null) break;
                }
                if (tn == null) continue;
                MigReq r = new MigReq();
                r.fpdbx = Path.Combine(tbDirExp.Text, ((FolderExpress)tn.Tag).FileName);
                r.disp = ((FolderExpress)tn.Tag).Disp;
                r.wlm = row.Wlm;
                al.Add(r);
            }
            MigForm form = new MigForm(al.ToArray(), tbDirWLM.Text);
            form.ShowDialog(this);
        }

        private void AddWalk(TreeNodeCollection tncExp, TreeNode tnW, Int64 parent, String reldir, WDb wdb) {
            foreach (TreeNode tnE in tncExp) {
                String baseDir = Path.Combine(tbDirWLM.Text, reldir);
                String udir = tnE.Text;
                for (int x = 0; Directory.Exists(Path.Combine(baseDir, udir)); x++) {
                    udir = tnE.Text + "~" + x;
                }
                Directory.CreateDirectory(Path.Combine(baseDir, udir));

                Int64 child = wdb.NewFolder(tnE.Text, parent, Path.Combine(reldir, udir));
                TreeNode tnWc = tnW.Nodes.Add(tnE.Text);
                tvW.SelectedNode = tnWc;

                FolderExpress fe = (FolderExpress)tnE.Tag;

                Update();
                String fpdbx = Path.Combine(tbDirExp.Text, fe.FileName);
                if (File.Exists(fpdbx)) UtExplodeDbx.ExplodeDbx2(fpdbx, Path.Combine(baseDir, udir));
                Update();

                AddWalk(tnE.Nodes, tnWc, child, Path.Combine(reldir, udir), wdb);
            }
        }

        private void bDeleteOrphan_Click(object sender, EventArgs e) {
            TreeNode tnSel = tvW.SelectedNode;
            if (tnSel == null) {
                MessageBox.Show(this, "右からフォルダを選択されたいです。");
                return;
            }

            FolderW w = (FolderW)tnSel.Tag;
            if (0 == (w.FLDCOL_FLAGS & 1)) { MessageBox.Show(this, "フォルダではございません。"); return; }
            if (0 != (w.FLDCOL_FLAGS & 8)) { MessageBox.Show(this, "そちらはご遠慮願います。"); return; }

            if (MessageBox.Show(this, "本当に実施しても良いんですか？", Application.ProductName, MessageBoxButtons.YesNo) == DialogResult.Yes) {
                using (WDb wdb = new WDb()) {
                    wdb.Open(tbDirWLM.Text, false);
                    WDb.CORep rep = wdb.CollectOrphan(tbDirWLM.Text, w.FLDCOL_ID);
                    MessageBox.Show(this, "終わりました。\n\n収容したフォルダ数:\n" + rep.cFolders.ToString("#,##0") + "\n\n" + "収容したメール数:\n" + rep.cMails.ToString("#,##0"));
                }
            }
        }

        private void bRescExp_Click(object sender, EventArgs e) {
            TreeNode tnE = tvE.SelectedNode;
            if (tnE != null) {
                FolderExpress fe = (FolderExpress)tnE.Tag;
                if (fe.FileName.Length != 0) {
                    ofddbx.FileName = Path.Combine(tbDirExp.Text, fe.FileName);
                }
            }

            if (ofddbx.ShowDialog(this) != DialogResult.OK)
                return;

            if (fbdRescExp.ShowDialog(this) == DialogResult.OK) {
                RescForm form = new RescForm(ofddbx.FileName, fbdRescExp.SelectedPath);
                form.Show();
                form.Run();
            }
        }

        private void bMake_Click(object sender, EventArgs e) {
            EP.SetError(bMake, null);
            if (cbWAcc.SelectedIndex < 0) {
                EP.SetError(bReadWLM, "移行先アカウントが空っぽのようです。\n\n「読み込み」をクリックしてください。");
                return;
            }

            TreeNode tnW = (TreeNode)cbWAcc.SelectedItem;

            WalkMake(tvE.Nodes, tnW, 0);

            if (ds1.Mig.Select().Length == 0) {
                EP.SetError(bReadExp, "移行計画が一件も出て来ません。\n\n「読み込み」をクリックしてください。");
                return;
            }
        }

        private void WalkMake(TreeNodeCollection tncE, TreeNode tnW, int lv) {
            foreach (TreeNode tnE in tncE) {
                if (lv == 0 && tnE.Text == "受信トレイ") AddMake(tnE.FullPath, Path.Combine(tnW.FullPath, "受信トレイ"));
                else if (lv == 0 && tnE.Text == "下書き") AddMake(tnE.FullPath, Path.Combine(tnW.FullPath, "下書き"));
                else if (lv == 0 && tnE.Text == "送信済みアイテム") AddMake(tnE.FullPath, Path.Combine(tnW.FullPath, "送信済みアイテム"));
                else if (lv == 0 && tnE.Text == "ごみ箱") AddMake(tnE.FullPath, Path.Combine(tnW.FullPath, "ごみ箱"));
                else AddMake(tnE.FullPath, Path.Combine(tnW.FullPath, (tnE.FullPath)));
                WalkMake(tnE.Nodes, tnW, lv + 1);
            }
        }

        private void AddMake(string lv, string rv) {
            if (ds1.Mig.Select("Exp=" + DB.E(lv) + "").Length != 0) return;
            if (ds1.Mig.Select("Wlm=" + DB.E(rv) + "").Length != 0) return;
            ds1.Mig.AddMigRow(lv, rv);
        }

        class DB {
            internal static string E(string s) {
                return (s == null) ? "null" : "'" + s.Replace("'", "''") + "'";
            }
        }

        private void bRescExpDirect_Click(object sender, EventArgs e) {
            if (ofddbx.ShowDialog(this) != DialogResult.OK)
                return;

            if (fbdRescExp.ShowDialog(this) == DialogResult.OK) {
                RescForm form = new RescForm(ofddbx.FileName, fbdRescExp.SelectedPath);
                form.Show();
                form.Run();
            }
        }
    }

    class FolderExpress { // folder tree OLEXP6
        DbxII ii;

        public FolderExpress(DbxII ii) {
            this.ii = ii;
        }

        public virtual int Key {
            get {
                DbxIF f = Array.Find<DbxIF>(ii.Fields, delegate(DbxIF tar) { return tar.Index == 0; });
                return (f != null) ? f.Int32Value : 0;
            }
        }

        public virtual int ParentKey {
            get {
                DbxIF f = Array.Find<DbxIF>(ii.Fields, delegate(DbxIF tar) { return tar.Index == 1; });
                return (f != null) ? f.Int32Value : -1;
            }
        }

        public virtual String Disp {
            get {
                DbxIF f = Array.Find<DbxIF>(ii.Fields, delegate(DbxIF tar) { return tar.Index == 2; });
                return (f != null) ? f.StringValue : String.Empty;
            }
        }

        public virtual String FileName {
            get {
                DbxIF f = Array.Find<DbxIF>(ii.Fields, delegate(DbxIF tar) { return tar.Index == 3; });
                return (f != null) ? f.StringValue : String.Empty;
            }
        }

        public override string ToString() {
            return String.Format("({0}, {1}) {2}", ParentKey, Key, FileName);
        }
    }
}