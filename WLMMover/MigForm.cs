using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using WLMMover.Properties;
using System.IO;
using System.Threading;
using Microsoft.Isam.Esent.Interop;

namespace WLMMover {
    public partial class MigForm : Form {
        String dirWLM;
        MigReq[] al;
        SynchronizationContext Sync = SynchronizationContext.Current;

        public MigForm(MigReq[] al, String dirWLM) {
            this.al = al;
            this.dirWLM = dirWLM;
            InitializeComponent();
        }

        private void MigForm_Load(object sender, EventArgs e) {
            foreach (MigReq r in al) {
                FlowLayoutPanel flp2 = new FlowLayoutPanel();
                flp2.AutoSize = true;
                flp2.AutoSizeMode = AutoSizeMode.GrowAndShrink;
                PictureBox pb = new PictureBox();
                {
                    pb.Image = null;
                    pb.SizeMode = PictureBoxSizeMode.AutoSize;
                    pb.Size = new Size(32, 32);
                    flp2.Controls.Add(pb);
                }
                {
                    Label la = new Label();
                    la.AutoSize = true;
                    la.Anchor = AnchorStyles.Left;
                    la.Text = r.disp;
                    la.ForeColor = Color.FromKnownColor(KnownColor.HotTrack);
                    flp2.Controls.Add(la);
                }
                {
                    Label la = new Label();
                    la.AutoSize = true;
                    la.Anchor = AnchorStyles.Left;
                    la.Text = "から";
                    flp2.Controls.Add(la);
                }
                foreach (String k in Path.GetFileName(r.wlm).Split('\\')) {
                    {
                        PictureBox pb2 = new PictureBox();
                        pb2.Anchor = AnchorStyles.Left;
                        pb2.Image = Resources.Book_angleHS;
                        pb2.SizeMode = PictureBoxSizeMode.AutoSize;
                        pb2.Margin = new Padding(0);
                        flp2.Controls.Add(pb2);
                    }
                    {
                        Label la = new Label();
                        la.AutoSize = true;
                        la.Anchor = AnchorStyles.Left;
                        la.Text = k;
                        la.ForeColor = Color.FromKnownColor(KnownColor.HotTrack);
                        la.Margin = new Padding(0);
                        flp2.Controls.Add(la);
                    }
                }
                {
                    Label la = new Label();
                    la.AutoSize = true;
                    la.Anchor = AnchorStyles.Left;
                    la.Text = "へ、移行致します。";
                    flp2.Controls.Add(la);
                }
                {
                    Label la = new Label();
                    la.AutoSize = true;
                    la.Anchor = AnchorStyles.Left;
                    la.Text = "";
                    la.Font = new Font(la.Font, FontStyle.Bold);
                    flp2.Controls.Add(la);

                    LinkLabel llErrm = new LinkLabel();
                    llErrm.AutoSize = true;
                    llErrm.Anchor = AnchorStyles.Left;
                    llErrm.Text = "エラー情報";
                    llErrm.LinkClicked += new LinkLabelLinkClickedEventHandler(llErrm_LinkClicked);
                    llErrm.Visible = false;
                    flp2.Controls.Add(llErrm);

                    r.Progress += delegate(int pos) {
                        Sync.Send(delegate {
                            if (pos == 0) {
                                pb.Image = Resources.wip;
                                la.Text = "始めました。";
                            }
                            else if (pos == 100) {
                                pb.Image = Resources.info;
                                la.Text = "終わりました。";
                            }
                            else if (pos == -1) {
                                pb.Image = Resources.warn;
                                la.Text = "失敗しました。";
                            }
                            else {
                                la.Text = pos + "%";
                            }
                        }, null);
                    };
                    r.Failed += delegate(Exception err) {
                        Sync.Send(delegate {
                            llErrm.Tag = err;
                            llErrm.Show();
                        }, null);
                    };
                }

                flpwip.Controls.Add(flp2);
                flpwip.SetFlowBreak(flp2, true);
            }
            Refresh();
            bwMig.RunWorkerAsync();
        }

        void llErrm_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            MessageBox.Show(this, ((Exception)((LinkLabel)sender).Tag).Message);
        }

        private void bwMig_DoWork(object sender, DoWorkEventArgs e) {
            using (WDb wdb = new WDb()) {
                wdb.Open(dirWLM, false);
                foreach (MigReq r in al) {
                    r.Progress(0);
                    try {
                        Int64 parent = wdb.MakeDir(r.wlm.Split('\\'));
                        String dirEMLExp = wdb.GetFolderPath(parent, true);

                        if (File.Exists(r.fpdbx)) {
                            int fno = 1;
                            UtExplodeDbx.ExplodeDbx3(r.fpdbx,
                                delegate(UtExplodeDbx.Stat3 s3) {
                                    Sync.Send(delegate {
                                        if (bwMig.CancellationPending) throw new ApplicationException();
                                        {
                                            //String s = "";
                                            if (s3.x < 100) {
                                                //s += String.Format("解析({0}%)", s3.x);
                                                r.Progress(s3.x / 2);
                                            }
                                            else {
                                                //s += String.Format("解析完了。抽出({0}%)、個数{1:#,##0}", s3.z, s3.emailcnt);
                                                r.Progress(s3.z / 2 + 50);
                                            }
                                        }
                                    }, null);
                                },
                                delegate() {
                                    for (int x = 0; ; x++) {
                                        if (bwMig.CancellationPending) throw new ApplicationException();
                                        String fp = Path.Combine(dirEMLExp, String.Format("{0:000000}.eml", fno++));
                                        if (!File.Exists(fp))
                                            return File.Create(fp);
                                    }
                                });

                        }
                        r.Progress(100);
                    }
                    catch (Exception err) {
                        r.Failed(err);
                        r.Progress(-1);
                    }
                }
            }
        }

        private void MigForm_FormClosing(object sender, FormClosingEventArgs e) {
            bwMig.CancelAsync();
        }
    }

    public class MigReq {
        public String fpdbx, disp;
        public String wlm;

        public Action<int> Progress;
        public Action<Exception> Failed;
    }
}