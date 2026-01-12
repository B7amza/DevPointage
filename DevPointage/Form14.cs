using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DevPointage
{
    public partial class Form14 : DevExpress.XtraEditors.XtraForm
    {
        public Form14()
        {
            InitializeComponent();
            ConnectSqlServer.Initialized();
        }

        private void Form14_Load(object sender, EventArgs e)
        {
            txt_annee.Text = Form2.annee_en_cours;
            ActiveControl = txt_matricule;
        }

        private void load_personnel_details()
        {
            string matricule = txt_matricule.Text.Trim();
            if (string.IsNullOrEmpty(matricule)) return;
            string query = "SELECT nom, prenom , fonction.lib_fonction , affectation.lib_affect  FROM Personnel,fonction,affectation WHERE personnel.id_fonction=fonction.c_fonct and personnel.id_affectation=affectation.c_affect and matricule = @matricule";
            SqlParameter[] parameters = {
              new SqlParameter("@matricule", matricule)
                                        };
            DataTable dt = ConnectSqlServer.ExecuteSelect(query, parameters);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                lb_nom_prenom.Text = row["nom"].ToString()+" "+ row["prenom"].ToString();
               lb_nom_prenom.Visible = true;

            }
            else
            {
                lb_nom_prenom.Text = "";
                lb_nom_prenom.Visible = false;
                MessageBox.Show("المستخدم غير موجود", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void txt_matricule_Leave(object sender, EventArgs e)
        {
            load_personnel_details();
        }

        private void txt_matricule_KeyDown(object sender, KeyEventArgs e)
        {
            // 1. التحقق من ضغط زر Enter
            if (e.KeyCode == Keys.Enter)
            {

                // 2. منادات الداله txt_matricule_Leave
                txt_matricule_Leave(sender, e);

            }
        }
    }
}