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
    public partial class uc_fonction : DevExpress.XtraEditors.XtraUserControl
    {
        public uc_fonction()
        {
            InitializeComponent();
        }

        private void bt_ajouter_Click(object sender, EventArgs e)
        {
            Form2.fn_fonction = new Form12();
            // Set the insertion mode
            Form12.insertion_modification = 0; // Set to insertion mode

            Form2.fn_fonction.Text = "Ajouter une fonction";
            
            Form2.fn_fonction.txt_c_fonction.Enabled = false; // Disable c_fonction field for automatic filling
            Form2.fn_fonction.txt_lib_fonction.Enabled = true; // Enable lib_fonction field for input
            // Clear the fields
            Form2.fn_fonction.txt_c_fonction.Text = string.Empty; // Clear c_fonction field
            Form2.fn_fonction.txt_lib_fonction.Text = string.Empty; // Clear lib_fonction field

            Form2.fn_fonction.bt_fermer.Enabled = true; // Enable close button
            Form2.fn_fonction.bt_enregistrer.Enabled = true; // Enable save button
            Form2.fn_fonction.bt_modifier.Enabled = false; // Disable modify button
            Form2.fn_fonction.bt_ajouter.Enabled = false; // Disable add button


           

            // define the connection string
            string connectionString = "Server=.\\SQLEXPRESS;Database=DEVCORE_PN;Integrated Security=True;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                // Get the next c_fonction value
                string query = "SELECT ISNULL(MAX(c_fonct), '00')  FROM FONCTION";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Execute the command and get the result
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        // Convert the result to string and increment it
                        string maxCode = result.ToString();
                        int nextCode = Convert.ToInt32(maxCode) + 1;
                        Form2.fn_fonction.txt_c_fonction.Text = nextCode.ToString("D2"); // Format as two digits
                    }
                    else
                    {
                        Form2.fn_fonction.txt_c_fonction.Text = "01"; // Default value if no records exist
                    }
                }




            }
            
            




            Form2.fn_fonction.ShowDialog();
            Form2.fn_fonction.Dispose();
            load_fonction_data(); // Reload the fonction data to reflect changes
        }

        private void uc_fonction_Load(object sender, EventArgs e)
        {
            load_fonction_data();
        }

        private void load_fonction_data()
        {
            // Load existing fonction into the grid control
            string connectionString = "Server=.\\SQLEXPRESS;Database=DEVCORE_PN;Integrated Security=True;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT c_fonct, lib_fonction FROM FONCTION";
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
            // Set the modification mode
            Form12.insertion_modification = 1; // Set to modification mode
            Form2.fn_fonction = new Form12();
            Form2.fn_fonction.Text = "Modifier une fonction";
            Form2.fn_fonction.txt_c_fonction.Enabled = false; // Disable c_fonction field for editing
            Form2.fn_fonction.txt_lib_fonction.Enabled = true; // Enable lib_fonction field for input
            // Ensure the MainView is a GridView to access selected rows
            var gridView = gridControl1.MainView as DevExpress.XtraGrid.Views.Grid.GridView;
            if (gridView == null || gridView.SelectedRowsCount == 0)
            {
                XtraMessageBox.Show("Veuillez sélectionner une fonction à modifier.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // Get the selected row data
            int selectedRowHandle = gridView.GetSelectedRows()[0];
            DataRow selectedRow = gridView.GetDataRow(selectedRowHandle);
            if (selectedRow != null)
            {
                Form2.fn_fonction.txt_c_fonction.Text = selectedRow["c_fonct"].ToString(); // Set the c_fonction field
                Form2.fn_fonction.txt_lib_fonction.Text = selectedRow["lib_fonction"].ToString(); // Set the lib_fonction field
            }
            Form2.fn_fonction.bt_ajouter.Enabled = false; // Disable the add button
            Form2.fn_fonction.bt_modifier.Enabled = false; // Disable the modify button
            Form2.fn_fonction.bt_enregistrer.Enabled = true; // Enable the save button
            Form2.fn_fonction.bt_fermer.Enabled = true; // Enable the close button
            Form2.fn_fonction.bt_fermer.Text = "Fermer"; // Change the text of the close button
            Form2.fn_fonction.ShowDialog();
            Form2.fn_fonction.Dispose();
            load_fonction_data(); // Reload the fonction data to reflect changes

        }
    }
}
