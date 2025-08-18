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
    public partial class uc_structure : DevExpress.XtraEditors.XtraUserControl
    {
        public uc_structure()
        {
            InitializeComponent();
        }

        private void bt_ajouter_Click(object sender, EventArgs e)
        {
            Form2.fn_affectation = new Form11();

            Form11.insertion_modification=0; // Insertion
            Form2.fn_affectation.txt_c_affectation.Enabled = false;
            Form2.fn_affectation.txt_lib_affectation.Enabled = true;
            Form2.fn_affectation.txt_c_affectation.Text = string.Empty;   // c_fonction remplir automatic
            Form2.fn_affectation.txt_lib_affectation.Text = string.Empty;
            Form2.fn_affectation.bt_ajouter.Enabled = false;
            Form2.fn_affectation.bt_modifier.Enabled = false;
            Form2.fn_affectation.bt_enregistrer.Enabled = true;
            Form2.fn_affectation.bt_fermer.Enabled = true; // Enable the close button
            Form2.fn_affectation.bt_fermer.Text = "Fermer"; // Change the text of the close button
            Form2.fn_affectation.Text = "Ajouter une affectation"; // Change the title of the form

            //////
            ///
            // Define the connection string
            string connectionString = "Server=.\\SQLEXPRESS;Database=DEVCORE_PN;Integrated Security=True;";

            // Insert the data into the database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ISNULL(MAX(c_affect), '00') AS c_affect FROM AFFECTATION";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Execute the command and get the result
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        // Convert the result to string and increment it
                        string maxCode = result.ToString();
                        int nextCode = Convert.ToInt32(maxCode) + 1;
                        Form2.fn_affectation.txt_c_affectation.Text = nextCode.ToString("D2"); // Format as two digits
                    }
                    else
                    {
                        Form2.fn_affectation.txt_c_affectation.Text = "01"; // Default value if no records exist
                    }
                }



            }

            /////////////////////////////////////////
            Form2.fn_affectation.ShowDialog();
            Form2.fn_affectation.Dispose();
            load_affectation_data(); // Reload the affectation data to reflect changes


        }

        private void uc_structure_Load(object sender, EventArgs e)
        {
            // Initialize the user control, load data, etc.
            // For example, you might want to load existing affectations into a grid or list.
            load_affectation_data();

        }
        private void load_affectation_data()
        {
            // Load existing affectations into the grid control
            string connectionString = "Server=.\\SQLEXPRESS;Database=DEVCORE_PN;Integrated Security=True;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT c_affect, lib_affect FROM Affectation";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    gridControl1.DataSource = dt; // Assuming you have a grid control to display the data
                }
            }
        }

        private void bt_modifier_Click(object sender, EventArgs e)
        {
            Form11.insertion_modification = 1; // Modification

            // Ensure the MainView is a GridView to access selected rows
            var gridView = gridControl1.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
            if (gridView == null || gridView.SelectedRowsCount == 0)
            {
                XtraMessageBox.Show("Veuillez sélectionner une affectation à modifier.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            Form2.fn_affectation = new Form11();
            Form2.fn_affectation.txt_c_affectation.Enabled = false; // Disable the code field
            Form2.fn_affectation.txt_lib_affectation.Enabled = true; // Enable the libelle field

            // Get the selected row data
            int selectedRowHandle = gridView.GetSelectedRows()[0];
            DataRow selectedRow = gridView.GetDataRow(selectedRowHandle);
            if (selectedRow != null)
            {
                Form2.fn_affectation.txt_c_affectation.Text = selectedRow["c_affect"].ToString(); // Set the code field
                Form2.fn_affectation.txt_lib_affectation.Text = selectedRow["lib_affect"].ToString(); // Set the libelle field
            }

            Form2.fn_affectation.bt_ajouter.Enabled = false; // Disable the add button
            Form2.fn_affectation.bt_modifier.Enabled = false; // Disable the modify button
            Form2.fn_affectation.bt_enregistrer.Enabled = true; // Enable the save button
            Form2.fn_affectation.bt_fermer.Enabled = true; // Enable the close button
            Form2.fn_affectation.bt_fermer.Text = "Fermer"; // Change the text of the close button
            Form2.fn_affectation.Text = "Modifier une affectation"; // Change the title of the form
            Form2.fn_affectation.ShowDialog(); // Show the form as a dialog
            Form2.fn_affectation.Dispose(); // Dispose of the form after closing
            load_affectation_data(); // Reload the affectation data to reflect changes
        }
    }
}
