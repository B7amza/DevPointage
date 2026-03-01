namespace DevPointage
{
    partial class Form2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            this.ribbon = new DevExpress.XtraBars.Ribbon.RibbonControl();
            this.bt_employe = new DevExpress.XtraBars.BarButtonItem();
            this.bt_affectation = new DevExpress.XtraBars.BarButtonItem();
            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
            this.bt_telecharger = new DevExpress.XtraBars.BarButtonItem();
            this.bt_horaireT = new DevExpress.XtraBars.BarButtonItem();
            this.bt_pointage = new DevExpress.XtraBars.BarButtonItem();
            this.bt_absence = new DevExpress.XtraBars.BarButtonItem();
            this.bt_GererPointeuse = new DevExpress.XtraBars.BarButtonItem();
            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPage2 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup2 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonPage3 = new DevExpress.XtraBars.Ribbon.RibbonPage();
            this.ribbonPageGroup3 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
            this.ribbonStatusBar = new DevExpress.XtraBars.Ribbon.RibbonStatusBar();
            this.pn_continer = new System.Windows.Forms.Panel();
            this.bt_PointageJR = new DevExpress.XtraBars.BarButtonItem();
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).BeginInit();
            this.SuspendLayout();
            // 
            // ribbon
            // 
            this.ribbon.ExpandCollapseItem.Id = 0;
            this.ribbon.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.ribbon.ExpandCollapseItem,
            this.bt_employe,
            this.bt_affectation,
            this.barButtonItem1,
            this.bt_telecharger,
            this.bt_horaireT,
            this.bt_pointage,
            this.bt_absence,
            this.bt_GererPointeuse,
            this.bt_PointageJR});
            this.ribbon.Location = new System.Drawing.Point(0, 0);
            this.ribbon.MaxItemId = 10;
            this.ribbon.Name = "ribbon";
            this.ribbon.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
            this.ribbonPage1,
            this.ribbonPage2,
            this.ribbonPage3});
            this.ribbon.Size = new System.Drawing.Size(948, 193);
            this.ribbon.StatusBar = this.ribbonStatusBar;
            // 
            // bt_employe
            // 
            this.bt_employe.Caption = "Employee";
            this.bt_employe.Id = 1;
            this.bt_employe.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("bt_employe.ImageOptions.LargeImage")));
            this.bt_employe.Name = "bt_employe";
            this.bt_employe.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bt_employe_ItemClick);
            // 
            // bt_affectation
            // 
            this.bt_affectation.Caption = "Structure";
            this.bt_affectation.Id = 2;
            this.bt_affectation.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("bt_affectation.ImageOptions.LargeImage")));
            this.bt_affectation.Name = "bt_affectation";
            this.bt_affectation.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bt_affectation_ItemClick);
            // 
            // barButtonItem1
            // 
            this.barButtonItem1.Caption = "Fonction";
            this.barButtonItem1.Id = 3;
            this.barButtonItem1.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonItem1.ImageOptions.LargeImage")));
            this.barButtonItem1.Name = "barButtonItem1";
            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // bt_telecharger
            // 
            this.bt_telecharger.Caption = "Telecharger";
            this.bt_telecharger.Id = 4;
            this.bt_telecharger.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("bt_telecharger.ImageOptions.LargeImage")));
            this.bt_telecharger.Name = "bt_telecharger";
            this.bt_telecharger.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bt_telecharger_ItemClick);
            // 
            // bt_horaireT
            // 
            this.bt_horaireT.Caption = "Horaire Travail";
            this.bt_horaireT.Id = 5;
            this.bt_horaireT.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("bt_horaireT.ImageOptions.LargeImage")));
            this.bt_horaireT.Name = "bt_horaireT";
            this.bt_horaireT.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bt_horaireT_ItemClick);
            // 
            // bt_pointage
            // 
            this.bt_pointage.Caption = "Pointage";
            this.bt_pointage.Id = 6;
            this.bt_pointage.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("bt_pointage.ImageOptions.LargeImage")));
            this.bt_pointage.Name = "bt_pointage";
            this.bt_pointage.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bt_pointage_ItemClick);
            // 
            // bt_absence
            // 
            this.bt_absence.Caption = "Absence";
            this.bt_absence.Id = 7;
            this.bt_absence.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("bt_absence.ImageOptions.LargeImage")));
            this.bt_absence.Name = "bt_absence";
            this.bt_absence.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bt_absence_ItemClick);
            // 
            // bt_GererPointeuse
            // 
            this.bt_GererPointeuse.Caption = "Gerer Pointeuse";
            this.bt_GererPointeuse.Id = 8;
            this.bt_GererPointeuse.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("bt_GererPointeuse.ImageOptions.LargeImage")));
            this.bt_GererPointeuse.Name = "bt_GererPointeuse";
            this.bt_GererPointeuse.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bt_GererPointeuse_ItemClick);
            // 
            // ribbonPage1
            // 
            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup1});
            this.ribbonPage1.Name = "ribbonPage1";
            this.ribbonPage1.Text = "Personnel";
            // 
            // ribbonPageGroup1
            // 
            this.ribbonPageGroup1.ItemLinks.Add(this.bt_employe);
            this.ribbonPageGroup1.ItemLinks.Add(this.bt_horaireT);
            this.ribbonPageGroup1.ItemLinks.Add(this.bt_pointage);
            this.ribbonPageGroup1.ItemLinks.Add(this.bt_absence);
            this.ribbonPageGroup1.ItemLinks.Add(this.bt_PointageJR);
            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
            this.ribbonPageGroup1.Text = "ribbonPageGroup1";
            // 
            // ribbonPage2
            // 
            this.ribbonPage2.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup2});
            this.ribbonPage2.Name = "ribbonPage2";
            this.ribbonPage2.Text = "Base";
            // 
            // ribbonPageGroup2
            // 
            this.ribbonPageGroup2.ItemLinks.Add(this.bt_affectation);
            this.ribbonPageGroup2.ItemLinks.Add(this.barButtonItem1);
            this.ribbonPageGroup2.Name = "ribbonPageGroup2";
            this.ribbonPageGroup2.Text = "ribbonPageGroup2";
            // 
            // ribbonPage3
            // 
            this.ribbonPage3.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
            this.ribbonPageGroup3});
            this.ribbonPage3.Name = "ribbonPage3";
            this.ribbonPage3.Text = "Pointage";
            // 
            // ribbonPageGroup3
            // 
            this.ribbonPageGroup3.ItemLinks.Add(this.bt_telecharger);
            this.ribbonPageGroup3.ItemLinks.Add(this.bt_GererPointeuse);
            this.ribbonPageGroup3.Name = "ribbonPageGroup3";
            this.ribbonPageGroup3.Text = "ribbonPageGroup3";
            // 
            // ribbonStatusBar
            // 
            this.ribbonStatusBar.Location = new System.Drawing.Point(0, 618);
            this.ribbonStatusBar.Name = "ribbonStatusBar";
            this.ribbonStatusBar.Ribbon = this.ribbon;
            this.ribbonStatusBar.Size = new System.Drawing.Size(948, 30);
            // 
            // pn_continer
            // 
            this.pn_continer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pn_continer.Location = new System.Drawing.Point(0, 193);
            this.pn_continer.Name = "pn_continer";
            this.pn_continer.Size = new System.Drawing.Size(948, 425);
            this.pn_continer.TabIndex = 2;
            // 
            // bt_PointageJR
            // 
            this.bt_PointageJR.Caption = "Pointage Journalier";
            this.bt_PointageJR.Id = 9;
            this.bt_PointageJR.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("barButtonItem2.ImageOptions.LargeImage")));
            this.bt_PointageJR.Name = "bt_PointageJR";
            this.bt_PointageJR.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.bt_PointageJR_ItemClick);
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(948, 648);
            this.Controls.Add(this.pn_continer);
            this.Controls.Add(this.ribbonStatusBar);
            this.Controls.Add(this.ribbon);
            this.Name = "Form2";
            this.Ribbon = this.ribbon;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.StatusBar = this.ribbonStatusBar;
            this.Text = "Dev Pointage ...";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form2_FormClosed);
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ribbon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraBars.Ribbon.RibbonControl ribbon;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
        private DevExpress.XtraBars.Ribbon.RibbonStatusBar ribbonStatusBar;
        private DevExpress.XtraBars.BarButtonItem bt_employe;
        private System.Windows.Forms.Panel pn_continer;
        private DevExpress.XtraBars.BarButtonItem bt_affectation;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage2;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup2;
        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
        private DevExpress.XtraBars.BarButtonItem bt_telecharger;
        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage3;
        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup3;
        private DevExpress.XtraBars.BarButtonItem bt_horaireT;
        private DevExpress.XtraBars.BarButtonItem bt_pointage;
        private DevExpress.XtraBars.BarButtonItem bt_absence;
        private DevExpress.XtraBars.BarButtonItem bt_GererPointeuse;
        private DevExpress.XtraBars.BarButtonItem bt_PointageJR;
    }
}