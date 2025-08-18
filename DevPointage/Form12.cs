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
    public partial class Form12 : DevExpress.XtraEditors.XtraForm
    {
        public Form12()
        {
            InitializeComponent();
        }
        static public int insertion_modification ; // 0 pour insertion, 1 pour modification
        private void bt_fermer_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bt_ajouter_Click(object sender, EventArgs e)
        {
            insertion_modification = 0; // Insertion

            txt_c_fonction.Enabled = false; // Disable c_fonction field for automatic filling
            txt_lib_fonction.Enabled = true;  // Enable lib_fonction field for input
            txt_c_fonction.Text = string.Empty;   // c_fonction remplir automatic
            txt_lib_fonction.Text = string.Empty; // Clear lib_fonction field
            bt_ajouter.Enabled = false; // Disable add button
            bt_modifier.Enabled = false; // Disable modify button
            bt_enregistrer.Enabled = true; // Enable save button
            bt_fermer.Enabled = true; // Enable close button
            bt_fermer.Text = "Fermer"; // Change the text of the close button
            Text = "Ajouter une fonction"; // Change the title of the form
            // Define the connection string
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
                        txt_c_fonction.Text = nextCode.ToString("D2"); // Format as two digits
                    }
                    else
                    {
                        txt_c_fonction.Text = "01"; // Default value if no records exist
                    }
                }

            }
        }

        private void bt_modifier_Click(object sender, EventArgs e)
        {
            insertion_modification = 1; // Modification
            // verify if the code is not empty
            if (string.IsNullOrWhiteSpace(txt_c_fonction.Text))
            {
                XtraMessageBox.Show("Veuillez saisir le code de la fonction.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            txt_c_fonction.Enabled = false; // Disable c_fonction field for automatic filling
            txt_lib_fonction.Enabled = true;  // Enable lib_fonction field for input
            bt_ajouter.Enabled = false; // Disable add button
            bt_modifier.Enabled = false; // Disable modify button
            bt_enregistrer.Enabled = true; // Enable save button
            bt_fermer.Enabled = true; // Enable close button
            bt_fermer.Text = "Fermer"; // Change the text of the close button
            Text = "Modifier une fonction"; // Change the title of the form
            

        }

        private void bt_enregistrer_Click(object sender, EventArgs e)
        {
            // Verify if the lib_fonction is not empty
            if (string.IsNullOrWhiteSpace(txt_lib_fonction.Text))
            {
                XtraMessageBox.Show("Veuillez saisir le libellé de la fonction.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // Verify if the c_fonction is not empty
            if (string.IsNullOrWhiteSpace(txt_c_fonction.Text))
            {
                XtraMessageBox.Show("Veuillez saisir le code de la fonction.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            // Check if the code already exists in insertion mode
            if (insertion_modification == 0) // Insertion
            {
                string connectionString1 = "Server=.\\SQLEXPRESS;Database=DEVCORE_PN;Integrated Security=True;";
                using (SqlConnection conn = new SqlConnection(connectionString1))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM FONCTION WHERE c_fonct = @c_fonct";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@c_fonct", txt_c_fonction.Text);
                        int count = (int)cmd.ExecuteScalar();
                        if (count > 0)
                        {
                            XtraMessageBox.Show("Le code de la fonction existe déjà.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                }
            }

            // Define the connection string
            string connectionString = "Server=.\\SQLEXPRESS;Database=DEVCORE_PN;Integrated Security=True;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query;
                if (insertion_modification == 0) // Insertion
                {
                    query = "INSERT INTO FONCTION (c_fonct, lib_fonction) VALUES (@c_fonct, @lib_fonction)";
                }
                else // Modification
                {
                    query = "UPDATE FONCTION SET lib_fonction = @lib_fonction WHERE c_fonct = @c_fonct";
                }
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@c_fonct", txt_c_fonction.Text);
                    cmd.Parameters.AddWithValue("@lib_fonction", txt_lib_fonction.Text);
                    cmd.ExecuteNonQuery();
                }
            }
            if (insertion_modification == 0) // Insertion
            {
                XtraMessageBox.Show("Fonction ajoutée avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else // Modification
            {
                XtraMessageBox.Show("Fonction modifiée avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            // Clear the text boxes after insertion or modification
            txt_c_fonction.Text = string.Empty;
            txt_lib_fonction.Text = string.Empty;
            bt_ajouter.Enabled = true; // Enable add button
            bt_modifier.Enabled = false; // Disable modify button
            bt_enregistrer.Enabled = false; // Disable save button
            bt_fermer.Text = "Fermer"; // Change the text of the close button
            Text = "Ajouter une fonction"; // Change the title of the form
            insertion_modification = 0; // Reset to Insertion mode

        }
    
    }
}