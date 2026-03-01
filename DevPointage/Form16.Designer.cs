namespace DevPointage
{
    partial class Form16
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form16));
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.bt_telechargerD = new System.Windows.Forms.Button();
            this.cb_pointeuse_source = new System.Windows.Forms.ComboBox();
            this.bt_connexionP = new System.Windows.Forms.Button();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.bt_transfert = new System.Windows.Forms.Button();
            this.cb_pointeuse_destination = new System.Windows.Forms.ComboBox();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.bt_GererPointeuse = new DevExpress.XtraBars.BarButtonItem();
            this.bt_privilege = new System.Windows.Forms.Button();
            this.bt_AjousterTime = new System.Windows.Forms.Button();
            this.lb_timePointeuse = new DevExpress.XtraEditors.LabelControl();
            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.progressBar1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.ForeColor = System.Drawing.Color.Transparent;
            this.panel1.Location = new System.Drawing.Point(0, 810);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1420, 88);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1420, 39);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupControl2);
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.groupControl1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 39);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1420, 771);
            this.panel3.TabIndex = 2;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.gridControl1);
            this.panel5.Location = new System.Drawing.Point(3, 273);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1414, 492);
            this.panel5.TabIndex = 3;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.lb_timePointeuse);
            this.groupControl1.Controls.Add(this.bt_AjousterTime);
            this.groupControl1.Controls.Add(this.bt_privilege);
            this.groupControl1.Controls.Add(this.bt_telechargerD);
            this.groupControl1.Controls.Add(this.cb_pointeuse_source);
            this.groupControl1.Controls.Add(this.bt_connexionP);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Location = new System.Drawing.Point(3, 6);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(734, 261);
            this.groupControl1.TabIndex = 2;
            this.groupControl1.Text = "Source ...";
            // 
            // bt_telechargerD
            // 
            this.bt_telechargerD.Location = new System.Drawing.Point(16, 145);
            this.bt_telechargerD.Name = "bt_telechargerD";
            this.bt_telechargerD.Size = new System.Drawing.Size(110, 97);
            this.bt_telechargerD.TabIndex = 26;
            this.bt_telechargerD.Text = "Charger";
            this.bt_telechargerD.UseVisualStyleBackColor = true;
            this.bt_telechargerD.Click += new System.EventHandler(this.bt_telechargerD_Click);
            // 
            // cb_pointeuse_source
            // 
            this.cb_pointeuse_source.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_pointeuse_source.FormattingEnabled = true;
            this.cb_pointeuse_source.Location = new System.Drawing.Point(137, 51);
            this.cb_pointeuse_source.Name = "cb_pointeuse_source";
            this.cb_pointeuse_source.Size = new System.Drawing.Size(260, 24);
            this.cb_pointeuse_source.TabIndex = 25;
            // 
            // bt_connexionP
            // 
            this.bt_connexionP.Location = new System.Drawing.Point(403, 46);
            this.bt_connexionP.Name = "bt_connexionP";
            this.bt_connexionP.Size = new System.Drawing.Size(110, 32);
            this.bt_connexionP.TabIndex = 21;
            this.bt_connexionP.Text = "Connect";
            this.bt_connexionP.UseVisualStyleBackColor = true;
            this.bt_connexionP.Click += new System.EventHandler(this.bt_connexionP_Click);
            // 
            // labelControl1
            // 
            this.labelControl1.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl1.Appearance.Options.UseFont = true;
            this.labelControl1.Location = new System.Drawing.Point(16, 51);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(106, 24);
            this.labelControl1.TabIndex = 16;
            this.labelControl1.Text = "Pointeuse  :";
            // 
            // groupControl2
            // 
            this.groupControl2.Controls.Add(this.cb_pointeuse_destination);
            this.groupControl2.Controls.Add(this.bt_transfert);
            this.groupControl2.Controls.Add(this.labelControl2);
            this.groupControl2.Location = new System.Drawing.Point(743, 6);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(667, 259);
            this.groupControl2.TabIndex = 22;
            this.groupControl2.Text = "Destination ...";
            // 
            // bt_transfert
            // 
            this.bt_transfert.Location = new System.Drawing.Point(138, 132);
            this.bt_transfert.Name = "bt_transfert";
            this.bt_transfert.Size = new System.Drawing.Size(297, 110);
            this.bt_transfert.TabIndex = 29;
            this.bt_transfert.Text = "Transferer";
            this.bt_transfert.UseVisualStyleBackColor = true;
            this.bt_transfert.Click += new System.EventHandler(this.bt_transfert_Click);
            // 
            // cb_pointeuse_destination
            // 
            this.cb_pointeuse_destination.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_pointeuse_destination.FormattingEnabled = true;
            this.cb_pointeuse_destination.Location = new System.Drawing.Point(138, 51);
            this.cb_pointeuse_destination.Name = "cb_pointeuse_destination";
            this.cb_pointeuse_destination.Size = new System.Drawing.Size(260, 24);
            this.cb_pointeuse_destination.TabIndex = 28;
            // 
            // labelControl2
            // 
            this.labelControl2.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelControl2.Appearance.Options.UseFont = true;
            this.labelControl2.Location = new System.Drawing.Point(17, 51);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(106, 24);
            this.labelControl2.TabIndex = 26;
            this.labelControl2.Text = "Pointeuse  :";
            // 
            // bt_GererPointeuse
            // 
            this.bt_GererPointeuse.Caption = "Gerer Pointeuse";
            this.bt_GererPointeuse.Id = 8;
            this.bt_GererPointeuse.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("bt_GererPointeuse.ImageOptions.LargeImage")));
            this.bt_GererPointeuse.Name = "bt_GererPointeuse";
            // 
            // bt_privilege
            // 
            this.bt_privilege.Location = new System.Drawing.Point(604, 145);
            this.bt_privilege.Name = "bt_privilege";
            this.bt_privilege.Size = new System.Drawing.Size(110, 97);
            this.bt_privilege.TabIndex = 27;
            this.bt_privilege.Text = "Privilege";
            this.bt_privilege.UseVisualStyleBackColor = true;
            this.bt_privilege.Click += new System.EventHandler(this.bt_privilege_Click);
            // 
            // bt_AjousterTime
            // 
            this.bt_AjousterTime.Location = new System.Drawing.Point(466, 145);
            this.bt_AjousterTime.Name = "bt_AjousterTime";
            this.bt_AjousterTime.Size = new System.Drawing.Size(110, 97);
            this.bt_AjousterTime.TabIndex = 28;
            this.bt_AjousterTime.Text = "Ajouster Time";
            this.bt_AjousterTime.UseVisualStyleBackColor = true;
            this.bt_AjousterTime.Click += new System.EventHandler(this.bt_AjousterTime_Click);
            // 
            // lb_timePointeuse
            // 
            this.lb_timePointeuse.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lb_timePointeuse.Appearance.Options.UseFont = true;
            this.lb_timePointeuse.Location = new System.Drawing.Point(137, 96);
            this.lb_timePointeuse.Name = "lb_timePointeuse";
            this.lb_timePointeuse.Size = new System.Drawing.Size(144, 24);
            this.lb_timePointeuse.TabIndex = 30;
            this.lb_timePointeuse.Text = "Time Pointeuse ";
            // 
            // gridControl1
            // 
            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridControl1.Location = new System.Drawing.Point(0, 0);
            this.gridControl1.MainView = this.gridView1;
            this.gridControl1.Name = "gridControl1";
            this.gridControl1.Size = new System.Drawing.Size(1414, 492);
            this.gridControl1.TabIndex = 0;
            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.gridControl1;
            this.gridView1.Name = "gridView1";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(3, 6);
            this.progressBar1.Maximum = 10;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(1414, 23);
            this.progressBar1.TabIndex = 30;
            // 
            // Form16
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1420, 898);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form16";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gerer Pointeuse ...";
            this.Load += new System.EventHandler(this.Form16_Load);
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel5;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        public System.Windows.Forms.Button bt_connexionP;
        public System.Windows.Forms.ComboBox cb_pointeuse_source;
        public System.Windows.Forms.ComboBox cb_pointeuse_destination;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        public System.Windows.Forms.Button bt_telechargerD;
        public System.Windows.Forms.Button bt_transfert;
        private DevExpress.XtraBars.BarButtonItem bt_GererPointeuse;
        public System.Windows.Forms.Button bt_AjousterTime;
        public System.Windows.Forms.Button bt_privilege;
        private DevExpress.XtraGrid.GridControl gridControl1;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private DevExpress.XtraEditors.LabelControl lb_timePointeuse;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}