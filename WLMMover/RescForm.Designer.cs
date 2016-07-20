namespace WLMMover {
    partial class RescForm {
        /// <summary>
        /// 必要なデザイナ変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナで生成されたコード

        /// <summary>
        /// デザイナ サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディタで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            this.bwResc = new System.ComponentModel.BackgroundWorker();
            this.llVisit = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.pb1 = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.pb2 = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.l2 = new System.Windows.Forms.Label();
            this.l1 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.flpOk = new System.Windows.Forms.FlowLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label4 = new System.Windows.Forms.Label();
            this.flpFail = new System.Windows.Forms.FlowLayoutPanel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tberr = new System.Windows.Forms.TextBox();
            this.flpOk.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.flpFail.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // bwResc
            // 
            this.bwResc.WorkerSupportsCancellation = true;
            this.bwResc.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bwResc_RunWorkerCompleted);
            // 
            // llVisit
            // 
            this.llVisit.AutoSize = true;
            this.llVisit.LinkArea = new System.Windows.Forms.LinkArea(0, 54);
            this.llVisit.Location = new System.Drawing.Point(12, 192);
            this.llVisit.Name = "llVisit";
            this.llVisit.Size = new System.Drawing.Size(291, 12);
            this.llVisit.TabIndex = 11;
            this.llVisit.TabStop = true;
            this.llVisit.Text = "http://www.geocities.co.jp/SiliconValley-Oakland/3664/";
            this.llVisit.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llVisit_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "解析状況：";
            // 
            // pb1
            // 
            this.pb1.Location = new System.Drawing.Point(12, 24);
            this.pb1.Name = "pb1";
            this.pb1.Size = new System.Drawing.Size(438, 23);
            this.pb1.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 66);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "抽出状況：";
            // 
            // pb2
            // 
            this.pb2.Location = new System.Drawing.Point(12, 81);
            this.pb2.Name = "pb2";
            this.pb2.Size = new System.Drawing.Size(438, 23);
            this.pb2.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 171);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(277, 12);
            this.label3.TabIndex = 10;
            this.label3.Text = "Dbxファイルの救出プログラムは、次のサイトを参考に開発：";
            // 
            // l2
            // 
            this.l2.AutoSize = true;
            this.l2.Location = new System.Drawing.Point(77, 66);
            this.l2.Name = "l2";
            this.l2.Size = new System.Drawing.Size(11, 12);
            this.l2.TabIndex = 6;
            this.l2.Text = "...";
            // 
            // l1
            // 
            this.l1.AutoSize = true;
            this.l1.Location = new System.Drawing.Point(77, 9);
            this.l1.Name = "l1";
            this.l1.Size = new System.Drawing.Size(11, 12);
            this.l1.TabIndex = 2;
            this.l1.Text = "...";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // flpOk
            // 
            this.flpOk.AutoSize = true;
            this.flpOk.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpOk.Controls.Add(this.pictureBox1);
            this.flpOk.Controls.Add(this.label4);
            this.flpOk.Location = new System.Drawing.Point(12, 110);
            this.flpOk.Name = "flpOk";
            this.flpOk.Size = new System.Drawing.Size(114, 38);
            this.flpOk.TabIndex = 8;
            this.flpOk.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.pictureBox1.Image = global::WLMMover.Properties.Resources.info;
            this.pictureBox1.Location = new System.Drawing.Point(3, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(32, 32);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label4
            // 
            this.label4.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(41, 13);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "終わりました。";
            // 
            // flpFail
            // 
            this.flpFail.AutoSize = true;
            this.flpFail.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.flpFail.Controls.Add(this.pictureBox2);
            this.flpFail.Controls.Add(this.label5);
            this.flpFail.Controls.Add(this.tberr);
            this.flpFail.Location = new System.Drawing.Point(132, 110);
            this.flpFail.Name = "flpFail";
            this.flpFail.Size = new System.Drawing.Size(273, 38);
            this.flpFail.TabIndex = 9;
            this.flpFail.Visible = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.pictureBox2.Image = global::WLMMover.Properties.Resources.warn;
            this.pictureBox2.Location = new System.Drawing.Point(3, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(32, 32);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 0;
            this.pictureBox2.TabStop = false;
            // 
            // label5
            // 
            this.label5.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(41, 13);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 12);
            this.label5.TabIndex = 1;
            this.label5.Text = "失敗しました。";
            // 
            // tberr
            // 
            this.tberr.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.tberr.Location = new System.Drawing.Point(120, 3);
            this.tberr.Multiline = true;
            this.tberr.Name = "tberr";
            this.tberr.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tberr.Size = new System.Drawing.Size(150, 32);
            this.tberr.TabIndex = 2;
            // 
            // RescForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(462, 213);
            this.Controls.Add(this.flpFail);
            this.Controls.Add(this.flpOk);
            this.Controls.Add(this.l1);
            this.Controls.Add(this.l2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.pb2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pb1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.llVisit);
            this.Name = "RescForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "DbxRescueを模した物";
            this.Load += new System.EventHandler(this.RescForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.RescForm_FormClosing);
            this.flpOk.ResumeLayout(false);
            this.flpOk.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.flpFail.ResumeLayout(false);
            this.flpFail.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker bwResc;
        private System.Windows.Forms.LinkLabel llVisit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar pb1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar pb2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label l2;
        private System.Windows.Forms.Label l1;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.FlowLayoutPanel flpOk;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.FlowLayoutPanel flpFail;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tberr;
    }
}