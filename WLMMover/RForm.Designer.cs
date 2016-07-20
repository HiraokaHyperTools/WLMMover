namespace WLMMover {
    partial class RForm {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RForm));
            this.label1 = new System.Windows.Forms.Label();
            this.tbDirExp = new System.Windows.Forms.TextBox();
            this.bRefExp = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tvE = new System.Windows.Forms.TreeView();
            this.il16 = new System.Windows.Forms.ImageList(this.components);
            this.bReadExp = new System.Windows.Forms.Button();
            this.fbdExp = new System.Windows.Forms.FolderBrowserDialog();
            this.label3 = new System.Windows.Forms.Label();
            this.tbDirWLM = new System.Windows.Forms.TextBox();
            this.bRefWLM = new System.Windows.Forms.Button();
            this.bReadWLM = new System.Windows.Forms.Button();
            this.fbdWLM = new System.Windows.Forms.FolderBrowserDialog();
            this.label4 = new System.Windows.Forms.Label();
            this.tvW = new System.Windows.Forms.TreeView();
            this.llDetWLM = new System.Windows.Forms.LinkLabel();
            this.bMig = new System.Windows.Forms.Button();
            this.bDeleteOrphan = new System.Windows.Forms.Button();
            this.bRescExp = new System.Windows.Forms.Button();
            this.fbdRescExp = new System.Windows.Forms.FolderBrowserDialog();
            this.label5 = new System.Windows.Forms.Label();
            this.gvm = new System.Windows.Forms.DataGridView();
            this.expDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.wlmDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.bindingSourceMig = new System.Windows.Forms.BindingSource(this.components);
            this.ds1 = new WLMMover.DS1();
            this.bMake = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.cbWAcc = new System.Windows.Forms.ComboBox();
            this.ofddbx = new System.Windows.Forms.OpenFileDialog();
            this.llDetExp = new System.Windows.Forms.LinkLabel();
            this.EP = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.gvm)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceMig)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ds1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.EP)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(186, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "Outlook Express 6 メールストアのパス";
            // 
            // tbDirExp
            // 
            this.tbDirExp.Location = new System.Drawing.Point(12, 24);
            this.tbDirExp.Name = "tbDirExp";
            this.tbDirExp.Size = new System.Drawing.Size(263, 19);
            this.tbDirExp.TabIndex = 1;
            this.tbDirExp.Text = "C:\\A\\Outlook Express";
            // 
            // bRefExp
            // 
            this.bRefExp.Location = new System.Drawing.Point(281, 22);
            this.bRefExp.Name = "bRefExp";
            this.bRefExp.Size = new System.Drawing.Size(75, 23);
            this.bRefExp.TabIndex = 2;
            this.bRefExp.Text = "参照...";
            this.bRefExp.UseVisualStyleBackColor = true;
            this.bRefExp.Click += new System.EventHandler(this.bRefExp_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(134, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "Folders.dbxより解析内容：";
            // 
            // tvE
            // 
            this.tvE.ImageIndex = 0;
            this.tvE.ImageList = this.il16;
            this.tvE.Location = new System.Drawing.Point(12, 61);
            this.tvE.Name = "tvE";
            this.tvE.SelectedImageIndex = 0;
            this.tvE.Size = new System.Drawing.Size(263, 271);
            this.tvE.TabIndex = 5;
            // 
            // il16
            // 
            this.il16.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("il16.ImageStream")));
            this.il16.TransparentColor = System.Drawing.Color.Transparent;
            this.il16.Images.SetKeyName(0, "D");
            this.il16.Images.SetKeyName(1, "B");
            this.il16.Images.SetKeyName(2, "B1");
            this.il16.Images.SetKeyName(3, "O");
            // 
            // bReadExp
            // 
            this.bReadExp.Location = new System.Drawing.Point(281, 51);
            this.bReadExp.Name = "bReadExp";
            this.bReadExp.Size = new System.Drawing.Size(75, 23);
            this.bReadExp.TabIndex = 3;
            this.bReadExp.Text = "読み込む";
            this.bReadExp.UseVisualStyleBackColor = true;
            this.bReadExp.Click += new System.EventHandler(this.bReadExp_Click);
            // 
            // fbdExp
            // 
            this.fbdExp.Description = "Outlook Express 6 メールストアのパス";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(378, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(159, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "Windows Live メール ストア パス";
            // 
            // tbDirWLM
            // 
            this.tbDirWLM.Location = new System.Drawing.Point(380, 24);
            this.tbDirWLM.Name = "tbDirWLM";
            this.tbDirWLM.Size = new System.Drawing.Size(263, 19);
            this.tbDirWLM.TabIndex = 10;
            // 
            // bRefWLM
            // 
            this.bRefWLM.Location = new System.Drawing.Point(649, 22);
            this.bRefWLM.Name = "bRefWLM";
            this.bRefWLM.Size = new System.Drawing.Size(75, 23);
            this.bRefWLM.TabIndex = 11;
            this.bRefWLM.Text = "参照...";
            this.bRefWLM.UseVisualStyleBackColor = true;
            this.bRefWLM.Click += new System.EventHandler(this.bRefWLM_Click);
            // 
            // bReadWLM
            // 
            this.bReadWLM.Location = new System.Drawing.Point(649, 51);
            this.bReadWLM.Name = "bReadWLM";
            this.bReadWLM.Size = new System.Drawing.Size(75, 23);
            this.bReadWLM.TabIndex = 12;
            this.bReadWLM.Text = "読み込む";
            this.bReadWLM.UseVisualStyleBackColor = true;
            this.bReadWLM.Click += new System.EventHandler(this.bReadWLM_Click);
            // 
            // fbdWLM
            // 
            this.fbdWLM.Description = "Windows Live メール ストア パス";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(378, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "Foldersテーブルよりの解析：";
            this.label4.Click += new System.EventHandler(this.label4_Click);
            // 
            // tvW
            // 
            this.tvW.ImageIndex = 0;
            this.tvW.ImageList = this.il16;
            this.tvW.Location = new System.Drawing.Point(380, 61);
            this.tvW.Name = "tvW";
            this.tvW.SelectedImageIndex = 0;
            this.tvW.Size = new System.Drawing.Size(263, 271);
            this.tvW.TabIndex = 14;
            // 
            // llDetWLM
            // 
            this.llDetWLM.AutoSize = true;
            this.llDetWLM.Location = new System.Drawing.Point(587, 9);
            this.llDetWLM.Name = "llDetWLM";
            this.llDetWLM.Size = new System.Drawing.Size(56, 12);
            this.llDetWLM.TabIndex = 9;
            this.llDetWLM.TabStop = true;
            this.llDetWLM.Text = "(検出する)";
            this.llDetWLM.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llDetWLM_LinkClicked);
            // 
            // bMig
            // 
            this.bMig.BackColor = System.Drawing.SystemColors.Info;
            this.bMig.ForeColor = System.Drawing.SystemColors.InfoText;
            this.bMig.Location = new System.Drawing.Point(649, 388);
            this.bMig.Name = "bMig";
            this.bMig.Size = new System.Drawing.Size(75, 46);
            this.bMig.TabIndex = 21;
            this.bMig.Text = "実行";
            this.bMig.UseVisualStyleBackColor = false;
            this.bMig.Click += new System.EventHandler(this.bMig_Click);
            // 
            // bDeleteOrphan
            // 
            this.bDeleteOrphan.Location = new System.Drawing.Point(649, 93);
            this.bDeleteOrphan.Name = "bDeleteOrphan";
            this.bDeleteOrphan.Size = new System.Drawing.Size(75, 46);
            this.bDeleteOrphan.TabIndex = 15;
            this.bDeleteOrphan.Text = "孤児をここに収容";
            this.bDeleteOrphan.UseVisualStyleBackColor = true;
            this.bDeleteOrphan.Click += new System.EventHandler(this.bDeleteOrphan_Click);
            // 
            // bRescExp
            // 
            this.bRescExp.Location = new System.Drawing.Point(281, 93);
            this.bRescExp.Name = "bRescExp";
            this.bRescExp.Size = new System.Drawing.Size(75, 46);
            this.bRescExp.TabIndex = 6;
            this.bRescExp.Text = "DbxRescueを模した物";
            this.bRescExp.UseVisualStyleBackColor = true;
            this.bRescExp.Click += new System.EventHandler(this.bRescExp_Click);
            // 
            // fbdRescExp
            // 
            this.fbdRescExp.Description = "救出したEMLファイルを保存するフォルダ？";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 373);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(59, 12);
            this.label5.TabIndex = 19;
            this.label5.Text = "移行計画：";
            // 
            // gvm
            // 
            this.gvm.AutoGenerateColumns = false;
            this.gvm.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.expDataGridViewTextBoxColumn,
            this.wlmDataGridViewTextBoxColumn});
            this.gvm.DataSource = this.bindingSourceMig;
            this.gvm.Location = new System.Drawing.Point(12, 388);
            this.gvm.Name = "gvm";
            this.gvm.RowTemplate.Height = 21;
            this.gvm.Size = new System.Drawing.Size(631, 194);
            this.gvm.TabIndex = 20;
            // 
            // expDataGridViewTextBoxColumn
            // 
            this.expDataGridViewTextBoxColumn.DataPropertyName = "Exp";
            this.expDataGridViewTextBoxColumn.HeaderText = "元";
            this.expDataGridViewTextBoxColumn.Name = "expDataGridViewTextBoxColumn";
            this.expDataGridViewTextBoxColumn.Width = 222;
            // 
            // wlmDataGridViewTextBoxColumn
            // 
            this.wlmDataGridViewTextBoxColumn.DataPropertyName = "Wlm";
            this.wlmDataGridViewTextBoxColumn.HeaderText = "先";
            this.wlmDataGridViewTextBoxColumn.Name = "wlmDataGridViewTextBoxColumn";
            this.wlmDataGridViewTextBoxColumn.Width = 330;
            // 
            // bindingSourceMig
            // 
            this.bindingSourceMig.DataMember = "Mig";
            this.bindingSourceMig.DataSource = this.ds1;
            // 
            // ds1
            // 
            this.ds1.DataSetName = "DS1";
            this.ds1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // bMake
            // 
            this.bMake.Location = new System.Drawing.Point(649, 336);
            this.bMake.Name = "bMake";
            this.bMake.Size = new System.Drawing.Size(75, 46);
            this.bMake.TabIndex = 18;
            this.bMake.Text = "計画";
            this.bMake.UseVisualStyleBackColor = true;
            this.bMake.Click += new System.EventHandler(this.bMake_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(378, 335);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 12);
            this.label6.TabIndex = 16;
            this.label6.Text = "移行先アカウント：";
            // 
            // cbWAcc
            // 
            this.cbWAcc.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWAcc.FormattingEnabled = true;
            this.cbWAcc.Location = new System.Drawing.Point(380, 350);
            this.cbWAcc.Name = "cbWAcc";
            this.cbWAcc.Size = new System.Drawing.Size(263, 20);
            this.cbWAcc.TabIndex = 17;
            // 
            // ofddbx
            // 
            this.ofddbx.DefaultExt = "dbx";
            this.ofddbx.Filter = "*.dbx|*.dbx";
            // 
            // llDetExp
            // 
            this.llDetExp.AutoSize = true;
            this.llDetExp.Location = new System.Drawing.Point(219, 9);
            this.llDetExp.Name = "llDetExp";
            this.llDetExp.Size = new System.Drawing.Size(56, 12);
            this.llDetExp.TabIndex = 22;
            this.llDetExp.TabStop = true;
            this.llDetExp.Text = "(検出する)";
            this.llDetExp.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llDetExp_LinkClicked);
            // 
            // EP
            // 
            this.EP.ContainerControl = this;
            // 
            // RForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 594);
            this.Controls.Add(this.llDetExp);
            this.Controls.Add(this.cbWAcc);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.bMake);
            this.Controls.Add(this.gvm);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.bRescExp);
            this.Controls.Add(this.bDeleteOrphan);
            this.Controls.Add(this.bMig);
            this.Controls.Add(this.llDetWLM);
            this.Controls.Add(this.tvW);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.bReadWLM);
            this.Controls.Add(this.bRefWLM);
            this.Controls.Add(this.tbDirWLM);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.bReadExp);
            this.Controls.Add(this.tvE);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.bRefExp);
            this.Controls.Add(this.tbDirExp);
            this.Controls.Add(this.label1);
            this.Name = "RForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "WLM Mover";
            this.Load += new System.EventHandler(this.RForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gvm)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceMig)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ds1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.EP)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbDirExp;
        private System.Windows.Forms.Button bRefExp;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TreeView tvE;
        private System.Windows.Forms.ImageList il16;
        private System.Windows.Forms.Button bReadExp;
        private System.Windows.Forms.FolderBrowserDialog fbdExp;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbDirWLM;
        private System.Windows.Forms.Button bRefWLM;
        private System.Windows.Forms.Button bReadWLM;
        private System.Windows.Forms.FolderBrowserDialog fbdWLM;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TreeView tvW;
        private System.Windows.Forms.LinkLabel llDetWLM;
        private System.Windows.Forms.Button bMig;
        private System.Windows.Forms.Button bDeleteOrphan;
        private System.Windows.Forms.Button bRescExp;
        private System.Windows.Forms.FolderBrowserDialog fbdRescExp;
        private System.Windows.Forms.Label label5;
        private DS1 ds1;
        private System.Windows.Forms.DataGridView gvm;
        private System.Windows.Forms.Button bMake;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cbWAcc;
        private System.Windows.Forms.OpenFileDialog ofddbx;
        private System.Windows.Forms.LinkLabel llDetExp;
        private System.Windows.Forms.ErrorProvider EP;
        private System.Windows.Forms.BindingSource bindingSourceMig;
        private System.Windows.Forms.DataGridViewTextBoxColumn expDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn wlmDataGridViewTextBoxColumn;
    }
}

