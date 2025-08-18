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
    public partial class Form11 : DevExpress.XtraEditors.XtraForm
    {
        public Form11()
        {
            InitializeComponent();
        }
        static public int insertion_modification ; // 0: Insertion, 1: Modification
        private void bt_fermer_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bt_ajouter_Click(object sender, EventArgs e)
        {
            insertion_modification = 0; // Insertion

            txt_c_affectation.Enabled = false;
            txt_lib_affectation.Enabled = true;

            txt_c_affectation.Text = string.Empty;   // c_fonction remplir automatic
            txt_lib_affectation.Text = string.Empty;

            bt_ajouter.Enabled = false;
            bt_modifier.Enabled = false;
            bt_enregistrer.Enabled = true;

            bt_fermer.Enabled = true; // Enable the close button
            bt_fermer.Text = "Fermer"; // Change the text of the close button
            Text = "Ajouter une affectation"; // Change the title of the form
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
                        txt_c_affectation.Text = nextCode.ToString("D2"); // Format as two digits
                    }
                    else
                    {
                        txt_c_affectation.Text = "01"; // Default value if no records exist
                    }
                }
            }
            // Clear the text boxes after insertion
            txt_lib_affectation.Text = string.Empty; // Clear the designation text box
            //txt_c_affectation.Text = string.Empty; // Clear the code text box
            bt_ajouter.Enabled = false; // Disable the add button
            bt_modifier.Enabled = false; // Disable the modify button
            bt_enregistrer.Enabled = true; // Enable the save button
            bt_fermer.Text = "Fermer"; // Change the text of the close button
            Text = "Ajouter une affectation"; // Change the title of the form

        }

        private void bt_enregistrer_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_lib_affectation.Text))
            {
                MessageBox.Show("Veuillez saisir la désignation de l'affectation.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if ( string.IsNullOrWhiteSpace(txt_c_affectation.Text))
            {
                MessageBox.Show("Veuillez saisir le code de l'affectation.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            // Check if the code already exists
            if (insertion_modification == 0) // Insertion
            {
                string connectionString1 = "Server=.\\SQLEXPRESS;Database=DEVCORE_PN;Integrated Security=True;";
                using (SqlConnection conn = new SqlConnection(connectionString1))
                {
                    conn.Open();
                    string query = "SELECT COUNT(*) FROM Affectation WHERE c_affect = @c_affect";
                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@c_affect", txt_c_affectation.Text.Trim());
                        int count = (int)cmd.ExecuteScalar();
                        if (count > 0)
                        {
                            MessageBox.Show("Le code d'affectation existe déjà.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }

            // Define the connection string
            string connectionString = "Server=.\\SQLEXPRESS;Database=DEVCORE_PN;Integrated Security=True;";
            // Insert or update the data in the database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query;
                if (insertion_modification == 0) // Insertion
                {
                    query = "INSERT INTO Affectation (c_affect, lib_affect) VALUES (@c_affect, @lib_affect)";
                }
                else // Modification
                {
                    query = "UPDATE Affectation SET lib_affect = @lib_affect WHERE c_affect = @c_affect";
                }
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@c_affect", txt_c_affectation.Text.Trim());
                    cmd.Parameters.AddWithValue("@lib_affect", txt_lib_affectation.Text.Trim());
                    cmd.ExecuteNonQuery();
                }
            }
            if (insertion_modification == 0)
            {
                MessageBox.Show("Affectation ajoutée avec succès.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Affectation modifiée avec succès.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            // Clear the text boxes after insertion or modification
            txt_lib_affectation.Text = string.Empty; // Clear the designation text box
            txt_c_affectation.Text = string.Empty; // Clear the code text box
            // Reset the form to its initial state
            txt_c_affectation.Enabled = false; // Disable the code field
            txt_lib_affectation.Enabled = true; // Enable the designation field
            bt_ajouter.Enabled = true; // Enable the add button
            bt_modifier.Enabled = false; // Disable the modify button
            bt_enregistrer.Enabled = false; // Disable the save button
            bt_fermer.Text = "Fermer"; // Change the text of the close button
            Text = "Ajouter une affectation"; // Change the title of the form




        }

        private void bt_modifier_Click(object sender, EventArgs e)
        {
            insertion_modification = 1; // Modification
            // verify if the code is not empty
            if (string.IsNullOrWhiteSpace(txt_c_affectation.Text))
            {
                MessageBox.Show("Veuillez sélectionner une affectation à modifier.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            txt_c_affectation.Enabled = false;
            txt_lib_affectation.Enabled = true;
            bt_ajouter.Enabled = false;
            bt_modifier.Enabled = false;
            bt_enregistrer.Enabled = true;
            bt_fermer.Text = "Fermer"; // Change the text of the close button
            Text = "Modifier une affectation"; // Change the title of the form

        }
    }
}