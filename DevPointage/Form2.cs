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



        uc_accueil uc_accueil1 = new uc_accueil();
        uc_employe uc_employe1 = new uc_employe();
        uc_structure uc_structure1 = new uc_structure();
        uc_fonction uc_fonction1 = new uc_fonction();


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
    }
}