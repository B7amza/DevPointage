using DevExpress.XtraEditors;
using DevExpress.XtraGrid.Views.Grid;
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
    public partial class Form13 : DevExpress.XtraEditors.XtraForm
    {
        static public int insertion_modification; // 0: insertion , 1: modification
        public Form13()
        {
            InitializeComponent();
            ConnectSqlServer.Initialized();
        }

        private void labelControl11_Click(object sender, EventArgs e)
        {

        }

        private void cb_horaire_travail_SelectedIndexChanged(object sender, EventArgs e)
        {
            dt_du.Enabled = true;
            dt_au.Enabled = true;
        }
        private void load_personnel_emploi_temps_data()
        {
            // 1. التأكد من وجود نص في خانة الماتريكول قبل البحث
            string matricule = txt_matricule.Text.Trim();
            if (string.IsNullOrEmpty(matricule)) return;

            // 2. تعريف الاستعلام
            string query = "SELECT matri, code, date_du, date_au FROM Personnel_emploi_temps WHERE matri = @matricule";

            // 3. تجهيز البارامترات
            SqlParameter[] parameters = {
              new SqlParameter("@matricule", matricule)
                                        };

            // 4. استخدام ExecuteSelect من الكلاس الخاص بك (يغنيك عن SqlConnection و DataAdapter)
            DataTable dt = ConnectSqlServer.ExecuteSelect(query, parameters);

            // 5. ربط النتيجة بالـ GridControl
            gridControl1.DataSource = dt;
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
                txt_nom_prenom.Text = row["nom"].ToString() +" "+ row["prenom"].ToString(); 
                txt_fonction.Text = row["lib_fonction"].ToString();
                txt_affectation.Text = row["lib_affect"].ToString();

            }
            else
            {
                txt_nom_prenom.Text = "";
                txt_fonction.Text = "";
                txt_affectation.Text = "";

            }
        }

        private void load_horaire_travail_data()
        {
            string query = "SELECT code, designation FROM Horaire_travail";
            DataTable dt = ConnectSqlServer.ExecuteSelect(query);
            cb_horaire_travail.DataSource = dt;
            cb_horaire_travail.DisplayMember = "designation";
            cb_horaire_travail.ValueMember = "code";
        }
        private void txt_matricule_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                txt_matricule_Leave(sender, e);
            }
        }

        private void txt_matricule_Leave(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txt_matricule.Text.Trim()))
            {
                load_personnel_details();
                load_personnel_emploi_temps_data();
            }
        }

        private void bt_ajouter_Click(object sender, EventArgs e)
        {
            insertion_modification= 0; // Insertion
            dt_du.Enabled = true;
            dt_au.Enabled = true;

            cb_horaire_travail.SelectedIndex = -1;
            cb_horaire_travail.Enabled = true;
            bt_enregistrer.Enabled = true;

            txt_matricule.Enabled = true;
        }

        private void bt_fermer_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bt_modifier_Click(object sender, EventArgs e)
        {
            insertion_modification = 1; // Modification


            var gridView = gridControl1.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
            if (gridView == null || gridView.SelectedRowsCount == 0)
            {
                XtraMessageBox.Show("Veuillez sélectionner une periode  à modifier.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // Get the selected row data
            int selectedRowHandle = gridView.GetSelectedRows()[0];
            DataRow selectedRow = gridView.GetDataRow(selectedRowHandle);
            if (selectedRow != null)
            {
                cb_horaire_travail.SelectedValue = selectedRow["code"].ToString();
                dt_du.Value = Convert.ToDateTime( selectedRow["date_du"]);
                dt_au.Value = Convert.ToDateTime( selectedRow["date_au"]);
            }

            cb_horaire_travail.Enabled = true;
            dt_du.Enabled = true;
            dt_au.Enabled = true;
        }

        private void Form13_Load(object sender, EventArgs e)
        {
            load_horaire_travail_data();
        }

        private void bt_enregistrer_Click(object sender, EventArgs e)
        {
            // Validation
            if (string.IsNullOrEmpty(txt_matricule.Text.Trim()))
            {
                XtraMessageBox.Show("Veuillez entrer le matricule.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (cb_horaire_travail.SelectedIndex == -1)
            {
                XtraMessageBox.Show("Veuillez sélectionner un horaire de travail.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            // Insert or Update in personnel_emploi_temps
            string matricule = txt_matricule.Text.Trim();
            string code = cb_horaire_travail.SelectedValue.ToString();
            DateTime date_du = dt_du.Value;
            DateTime date_au = dt_au.Value;


            if (insertion_modification == 1)
            {
                // Update
                string updateQuery = "UPDATE Personnel_emploi_temps SET code = @code WHERE matri = @matricule and date_du = @date_du and date_au = @date_au";
                SqlParameter[] updateParameters = {
                new SqlParameter("@code", code),
                new SqlParameter("@date_du", date_du),
                new SqlParameter("@date_au", date_au),
                new SqlParameter("@matricule", matricule)
                                                   };
                int rowsUpdated = ConnectSqlServer.ExecuteNonQuery(updateQuery, updateParameters);
                if (rowsUpdated > 0)
                {
                    XtraMessageBox.Show("Mise à jour réussie.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //load_personnel_emploi_temps_data();
                    cb_horaire_travail.Enabled = false;
                    dt_du.Enabled = false;
                    dt_au.Enabled = false;
                    bt_ajouter.Enabled = true;
                    bt_modifier.Enabled = true;
                    bt_enregistrer.Enabled = false;
                    return;
                }
                else
                {
                    XtraMessageBox.Show("Échec de la mise à jour.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            // Insert
            else
            {
                if (insertion_modification == 0)

                {
                    string query = "INSERT INTO Personnel_emploi_temps (matri, code, date_du, date_au) VALUES (@matricule, @code, @date_du, @date_au)";
                    SqlParameter[] parameters = {
                new SqlParameter("@matricule", matricule),
                new SqlParameter("@code", code),
                new SqlParameter("@date_du", date_du),
                new SqlParameter("@date_au", date_au)
                                                };
                    int rowsAffected = ConnectSqlServer.ExecuteNonQuery(query, parameters);
                    if (rowsAffected > 0)
                    {
                        XtraMessageBox.Show("Enregistrement réussi.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //load_personnel_emploi_temps_data();
                        cb_horaire_travail.Enabled = false;
                        dt_du.Enabled = false;
                        dt_au.Enabled = false;
                        bt_ajouter.Enabled = true;
                        bt_modifier.Enabled = true;
                        bt_enregistrer.Enabled = false;
                    }
                    else
                    {
                        XtraMessageBox.Show("Échec de l'enregistrement.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);


                    }
                }
            }
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
            bt_modifier.Enabled = true;
        }

        
    }
}