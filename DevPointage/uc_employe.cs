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
using DevExpress.XtraEditors;

namespace DevPointage
{
    public partial class uc_employe : DevExpress.XtraEditors.XtraUserControl
    {
        public uc_employe()
        {
            InitializeComponent();
        }
        private void load_personnel_data()
        {
            // Load personnel data into the grid control gridControl1.DataSource = dt; 
            string connectionString = "Server=.\\SQLEXPRESS;Database=DEVCORE_PN;Integrated Security=True;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT matricule, nom, prenom, sexe, adress, tel, id_fonction,fonction.lib_fonction as fonction, id_affectation,affectation.lib_affect as affectation, type_agent, poinatge FROM Personnel,affectation,fonction where personnel.id_affectation=affectation.c_affect and personnel.id_fonction=fonction.c_fonct";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    gridControl1.DataSource = dt; // Assuming you have a grid control to display the data
                }
            }


        }
        private void bt_ajouter_Click(object sender, EventArgs e)
        {
            Form2.fn_personnel = new Form10();
            Form10.insertion_modification = 0; // Insertion
            Form2.fn_personnel.bt_ajouter.Enabled = true;
            Form2.fn_personnel.ShowDialog();
            Form2.fn_personnel.Dispose();
            load_personnel_data();

        }

        private void uc_employe_Load(object sender, EventArgs e)
        {
            load_personnel_data();
        }

        private void bt_modifier_Click(object sender, EventArgs e)
        {
            // Check if a row is selected in the grid control
            var gridView = gridControl1.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
            if (gridView == null || gridView.SelectedRowsCount == 0)
            {
                XtraMessageBox.Show("Veuillez sélectionner un employé à modifier.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            Form2.fn_personnel = new Form10();
            Form10.insertion_modification = 1; // Modification
            Form2.fn_personnel.bt_ajouter.Enabled = false; // Disable the add button
            Form2.fn_personnel.bt_modifier.Enabled = true; // Enable the modify button
            // Get the selected row data
            int selectedRowHandle = gridView.GetSelectedRows()[0];
            DataRow selectedRow = gridView.GetDataRow(selectedRowHandle);
            if (selectedRow != null)
            {
                Form2.fn_personnel.txt_matricule.Text = selectedRow["matricule"].ToString(); // Set the matricule field
                Form2.fn_personnel.txt_nom.Text = selectedRow["nom"].ToString(); // Set the nom field
                Form2.fn_personnel.txt_prenom.Text = selectedRow["prenom"].ToString(); // Set the prenom field
                Form2.fn_personnel.cb_sexe.Text = selectedRow["sexe"].ToString(); // Set the sexe field
                Form2.fn_personnel.txt_adresse.Text = selectedRow["adress"].ToString(); // Set the adress field
                Form2.fn_personnel.txt_tel.Text = selectedRow["tel"].ToString(); // Set the tel field
                Form2.fn_personnel.cb_fonction.SelectedValue = Convert.ToInt32(selectedRow["id_fonction"]); 
                Form10.var_fonction = selectedRow["id_fonction"].ToString(); // Store the selected function ID for later use
                Form2.fn_personnel.cb_affectation.SelectedValue = Convert.ToInt32(selectedRow["id_affectation"]); // Set the affectation field
                Form10.var_affectation = selectedRow["id_affectation"].ToString(); // Store the selected affectation ID for later use
                Form2.fn_personnel.cb_type.Text = selectedRow["type_agent"].ToString(); // Set the type agent field
                Form2.fn_personnel.cb_pointage.Text = selectedRow["poinatge"].ToString(); // Set the poinatge field
            }
            Form2.fn_personnel.ShowDialog(); // Show the personnel form for modification
            Form2.fn_personnel.Dispose(); // Dispose the form after closing
            load_personnel_data();
        }
        
    }
}
