using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraBars;

namespace DevPointage
{
    public partial class Form2 : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        static public string annee_en_cours;
        static public int plage_hrs=5;
        static public int plage_retards = 15;
        static public int nbr_machine = 8;
        public Form2()
        {
            InitializeComponent();
            LoadPrincipaleUC();
        }

        static public Form10 fn_personnel;
        static public Form11 fn_affectation;
        static public Form12 fn_fonction;
        static public Form4 fn_telechargement;
        static public Form13 fn_horaire_travail;
        static public Form14 fn_pointage_mensuelle;
        static public Form15 fn_gestion_absence;
        static public Form16 fn_gerer_pointeuse;
        static public Form17 fn_pointage_journalier;
        static public Form_imprission fn_imprission;



        uc_accueil uc_accueil1 = new uc_accueil();
        uc_employe uc_employe1 = new uc_employe();
        uc_structure uc_structure1 = new uc_structure();
        uc_fonction uc_fonction1 = new uc_fonction();
        uc_typeconge uc_typeconge1 = new uc_typeconge();


        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            annee_en_cours = DateTime.Now.Year.ToString();
        }
        private void LoadUC(DevExpress.XtraEditors.XtraUserControl Page_UControle)
        {
            try
            {
                pn_continer.Controls.Clear();
                Page_UControle.Dock = DockStyle.Fill;
                pn_continer.Controls.Add(Page_UControle);

            }
            catch
            {

            }
        }
        private void LoadPrincipaleUC()
        {

            LoadUC(uc_accueil1);

        }

        private void bt_employe_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadUC(uc_employe1);
        }

        private void bt_affectation_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadUC(uc_structure1);
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadUC(uc_fonction1);
        }

        private void bt_telecharger_ItemClick(object sender, ItemClickEventArgs e)
        {
            fn_telechargement = new Form4();
            fn_telechargement.ShowDialog();
            fn_telechargement.Dispose();
        }

        private void bt_horaireT_ItemClick(object sender, ItemClickEventArgs e)
        {
            fn_horaire_travail = new Form13();
            fn_horaire_travail.ShowDialog();
            fn_horaire_travail.Dispose();

        }

        private void bt_pointage_ItemClick(object sender, ItemClickEventArgs e)
        {
            fn_pointage_mensuelle = new Form14();
            fn_pointage_mensuelle.ShowDialog();
            fn_pointage_mensuelle.Dispose();
        }

        private void bt_absence_ItemClick(object sender, ItemClickEventArgs e)
        {
            fn_gestion_absence = new Form15();
            fn_gestion_absence.ShowDialog();
            fn_gestion_absence.Dispose();
        }

        private void bt_GererPointeuse_ItemClick(object sender, ItemClickEventArgs e)
        {
            fn_gerer_pointeuse = new Form16();
            fn_gerer_pointeuse.ShowDialog();
            fn_gerer_pointeuse.Dispose();
        }

        private void bt_PointageJR_ItemClick(object sender, ItemClickEventArgs e)
        {
            fn_pointage_journalier = new Form17();
            fn_pointage_journalier.ShowDialog();
            fn_pointage_journalier.Dispose();

        }

        private void bt_typeconge_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadUC(uc_typeconge1);
        }

        private void bt_ferie_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void bt_principale_ItemClick(object sender, ItemClickEventArgs e)
        {
            LoadUC(uc_accueil1);
        }
    }
}