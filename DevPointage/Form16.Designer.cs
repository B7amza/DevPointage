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
            this.panel4 = new System.Windows.Forms.Panel();
            this.groupControl1 = new DevExpress.XtraEditors.GroupControl();
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.groupControl2 = new DevExpress.XtraEditors.GroupControl();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel6 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.bt_recherche = new System.Windows.Forms.Button();
            this.cb_pointeuse_source = new System.Windows.Forms.ComboBox();
            this.cb_pointeuse_destination = new System.Windows.Forms.ComboBox();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.bt_GererPointeuse = new DevExpress.XtraBars.BarButtonItem();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).BeginInit();
            this.groupControl1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).BeginInit();
            this.groupControl2.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
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
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.groupControl1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 39);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(744, 771);
            this.panel3.TabIndex = 2;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.panel6);
            this.panel4.Controls.Add(this.groupControl2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel4.Location = new System.Drawing.Point(743, 39);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(677, 771);
            this.panel4.TabIndex = 3;
            // 
            // groupControl1
            // 
            this.groupControl1.Controls.Add(this.button2);
            this.groupControl1.Controls.Add(this.cb_pointeuse_source);
            this.groupControl1.Controls.Add(this.bt_recherche);
            this.groupControl1.Controls.Add(this.labelControl1);
            this.groupControl1.Location = new System.Drawing.Point(3, 6);
            this.groupControl1.Name = "groupControl1";
            this.groupControl1.Size = new System.Drawing.Size(734, 240);
            this.groupControl1.TabIndex = 2;
            this.groupControl1.Text = "Source ...";
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
            this.groupControl2.Controls.Add(this.button3);
            this.groupControl2.Controls.Add(this.cb_pointeuse_destination);
            this.groupControl2.Controls.Add(this.labelControl2);
            this.groupControl2.Location = new System.Drawing.Point(3, 6);
            this.groupControl2.Name = "groupControl2";
            this.groupControl2.Size = new System.Drawing.Size(667, 235);
            this.groupControl2.TabIndex = 22;
            this.groupControl2.Text = "Destination ...";
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.dataGridView1);
            this.panel5.Location = new System.Drawing.Point(3, 252);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(734, 513);
            this.panel5.TabIndex = 3;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.dataGridView2);
            this.panel6.Location = new System.Drawing.Point(3, 252);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(666, 509);
            this.panel6.TabIndex = 4;
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(734, 513);
            this.dataGridView1.TabIndex = 0;
            // 
            // dataGridView2
            // 
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(0, 0);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RowHeadersWidth = 51;
            this.dataGridView2.RowTemplate.Height = 24;
            this.dataGridView2.Size = new System.Drawing.Size(666, 509);
            this.dataGridView2.TabIndex = 0;
            // 
            // bt_recherche
            // 
            this.bt_recherche.Location = new System.Drawing.Point(403, 46);
            this.bt_recherche.Name = "bt_recherche";
            this.bt_recherche.Size = new System.Drawing.Size(110, 32);
            this.bt_recherche.TabIndex = 21;
            this.bt_recherche.Text = "Connect";
            this.bt_recherche.UseVisualStyleBackColor = true;
            // 
            // cb_pointeuse_source
            // 
            this.cb_pointeuse_source.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_pointeuse_source.Enabled = false;
            this.cb_pointeuse_source.FormattingEnabled = true;
            this.cb_pointeuse_source.Items.AddRange(new object[] {
            "Oui",
            "Non"});
            this.cb_pointeuse_source.Location = new System.Drawing.Point(137, 51);
            this.cb_pointeuse_source.Name = "cb_pointeuse_source";
            this.cb_pointeuse_source.Size = new System.Drawing.Size(260, 24);
            this.cb_pointeuse_source.TabIndex = 25;
            // 
            // cb_pointeuse_destination
            // 
            this.cb_pointeuse_destination.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cb_pointeuse_destination.Enabled = false;
            this.cb_pointeuse_destination.FormattingEnabled = true;
            this.cb_pointeuse_destination.Items.AddRange(new object[] {
            "Oui",
            "Non"});
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
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(16, 113);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(110, 110);
            this.button2.TabIndex = 26;
            this.button2.Text = "Charger";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(17, 113);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(110, 110);
            this.button3.TabIndex = 29;
            this.button3.Text = "Transferer";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // Form16
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1420, 898);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Form16";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Gerer Pointeuse ...";
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.groupControl1)).EndInit();
            this.groupControl1.ResumeLayout(false);
            this.groupControl1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.groupControl2)).EndInit();
            this.groupControl2.ResumeLayout(false);
            this.groupControl2.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel6.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.DataGridView dataGridView1;
        private DevExpress.XtraEditors.GroupControl groupControl1;
        private DevExpress.XtraEditors.LabelControl labelControl1;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.DataGridView dataGridView2;
        private DevExpress.XtraEditors.GroupControl groupControl2;
        public System.Windows.Forms.Button bt_recherche;
        public System.Windows.Forms.ComboBox cb_pointeuse_source;
        public System.Windows.Forms.ComboBox cb_pointeuse_destination;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        public System.Windows.Forms.Button button2;
        public System.Windows.Forms.Button button3;
        private DevExpress.XtraBars.BarButtonItem bt_GererPointeuse;
    }
}