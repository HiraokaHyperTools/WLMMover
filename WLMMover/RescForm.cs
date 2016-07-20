using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using System.Diagnostics;

namespace WLMMover {
    public partial class RescForm : Form {
        public RescForm(string fpdbx, string dir) {
            InitializeComponent();

            bwResc.DoWork += delegate(object sender, DoWorkEventArgs e) {
                UtExplodeDbx.ExplodeDbx3(fpdbx,
                    delegate(UtExplodeDbx.Stat3 s3) {
                        Sync.Send(delegate {
                            if (bwResc.CancellationPending) throw new ApplicationException();
                            s3last = s3;
                            {
                                l1.Text = String.Format("{0:0}%", s3last.x);
                                l2.Text = String.Format("{0:0}% {1:#,##0}個", s3last.z, s3last.emailcnt);
                                pb1.Value = s3last.x;
                                pb2.Value = s3last.z;
                                s3last = null;
                            }
                        }, null);
                    },
                    delegate() {
                        for (int x = 0; ; x++) {
                            if (bwResc.CancellationPending) throw new ApplicationException();
                            String fp = Path.Combine(dir, String.Format("{0:000000}.eml", fno++));
                            if (!File.Exists(fp))
                                return File.Create(fp);
                        }
                    }
                    );
            };
        }

        UtExplodeDbx.Stat3 s3last = null;

        int fno = 1;

        SynchronizationContext Sync;

        public void Run() {
            Sync = SynchronizationContext.Current;
            bwResc.RunWorkerAsync();
        }

        private void llVisit_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start("http://www.geocities.co.jp/SiliconValley-Oakland/3664/");
        }

        private void RescForm_Load(object sender, EventArgs e) {

        }

        private void timer1_Tick(object sender, EventArgs e) {
            if (s3last != null) {
                l1.Text = String.Format("{0:0}%", s3last.x);
                l2.Text = String.Format("{0:0}% {1:#,##0}個", s3last.z, s3last.emailcnt);
                pb1.Value = s3last.x;
                pb2.Value = s3last.z;
                s3last = null;
            }
        }

        private void RescForm_FormClosing(object sender, FormClosingEventArgs e) {
            if (bwResc.IsBusy)
                bwResc.CancelAsync();
        }

        private void bwResc_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if (e.Error == null) {
                flpOk.Show();
            }
            else {
                tberr.Text = e.Error.ToString();
                flpFail.Show();
            }
        }
    }
}