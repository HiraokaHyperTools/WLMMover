namespace WLMMover {
    partial class MigForm {
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
            this.label1 = new System.Windows.Forms.Label();
            this.bwMig = new System.ComponentModel.BackgroundWorker();
            this.flpwip = new System.Windows.Forms.FlowLayoutPanel();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "移行状況：";
            // 
            // bwMig
            // 
            this.bwMig.WorkerSupportsCancellation = true;
            this.bwMig.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bwMig_DoWork);
            // 
            // flpwip
            // 
            this.flpwip.AutoScroll = true;
            this.flpwip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flpwip.Location = new System.Drawing.Point(0, 12);
            this.flpwip.Name = "flpwip";
            this.flpwip.Size = new System.Drawing.Size(730, 426);
            this.flpwip.TabIndex = 1;
            // 
            // MigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(730, 438);
            this.Controls.Add(this.flpwip);
            this.Controls.Add(this.label1);
            this.Name = "MigForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MigForm";
            this.Load += new System.EventHandler(this.MigForm_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MigForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel flpwip;
        private System.ComponentModel.BackgroundWorker bwMig;
    }
}