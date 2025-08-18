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
    public partial class Form10 : DevExpress.XtraEditors.XtraForm
    {
        public Form10()
        {
            InitializeComponent();
        }
        static public int insertion_modification; // 0 for insertion, 1 for modification
        static public string var_affectation, var_fonction;
        private void bt_fermer_Click(object sender, EventArgs e)
        {
            Close();

        }

        private void Form10_Load(object sender, EventArgs e)
        {
            // Load existing affectations and fonctions from the database into comboBoxEdit controls
            load_affectation_data();
            load_fonction_data();
            if (insertion_modification == 1) 
            {
                cb_affectation.SelectedValue = var_affectation; // Set the selected value for affectation
                cb_fonction.SelectedValue = var_fonction; // Set the selected value for fonction

            }


        }
        private void load_affectation_data()
        {
            // Load existing affectations from database  query = "SELECT c_affect, lib_affect FROM Affectation"; into comboBoxEdit cb_affectation
            string connectionString = "Server=.\\SQLEXPRESS;Database=DEVCORE_PN;Integrated Security=True;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT c_affect, lib_affect FROM Affectation";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string codeAffectation = reader["c_affect"].ToString();
                            string libAffectation = reader["lib_affect"].ToString();
                            cb_affectation1.Properties.Items.Add($"{codeAffectation} - {libAffectation}");
                        }
                    }
                }
            }
            // Load existing affectations from database  query = "SELECT c_affect, lib_affect FROM Affectation"; into comboBox cb_affectation
            string connectionString1 = "Server=.\\SQLEXPRESS;Database=DEVCORE_PN;Integrated Security=True;";
            using (SqlConnection conn = new SqlConnection(connectionString1))
            {
                conn.Open();
                string query = "SELECT c_affect, lib_affect FROM Affectation";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {


                        DataTable dt = new DataTable();
                        dt.Load(reader);

                        cb_affectation.DisplayMember = "lib_affect"; // ما يظهر للمستخدم
                        cb_affectation.ValueMember = "c_affect";     // القيمة الداخلية
                        cb_affectation.DataSource = dt;


                    }
                }
            }

        }
        private void load_fonction_data()
        {
            // Load existing fonctions from database into comboBoxEdit cb_fonction
            string connectionString1 = "Server=.\\SQLEXPRESS;Database=DEVCORE_PN;Integrated Security=True;";
            using (SqlConnection conn = new SqlConnection(connectionString1))
            {
                conn.Open();
                string query = "SELECT c_fonct, lib_fonction FROM Fonction";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string codeFonction = reader["c_fonct"].ToString();
                            string libFonction = reader["lib_fonction"].ToString();
                            cb_fonction1.Properties.Items.Add($"{codeFonction} - {libFonction}");
                        }
                    }
                }
            }
            // Load existing fonctions from database into comboBox cb_fonction
            string connectionString = "Server=.\\SQLEXPRESS;Database=DEVCORE_PN;Integrated Security=True;";
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT c_fonct, lib_fonction FROM Fonction";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        DataTable dt = new DataTable();
                        dt.Load(reader);
                        cb_fonction.DisplayMember = "lib_fonction"; // ما يظهر للمستخدم
                        cb_fonction.ValueMember = "c_fonct";         // القيمة الداخلية
                        cb_fonction.DataSource = dt;
                    }
                }
            }
        }

        private void bt_ajouter_Click(object sender, EventArgs e)
        {
            insertion_modification = 0; // Set to 0 for insertion
            //define the connection string
            string connectionString = "Server=.\\SQLEXPRESS;Database=DEVCORE_PN;Integrated Security=True;";
            // Insert the data "personnel" into the database
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT ISNULL(MAX(matricule), '00000') AS matricule FROM Personnel";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Execute the command and get the result
                    object result = cmd.ExecuteScalar();
                    if (result != null && result != DBNull.Value)
                    {
                        // Convert the result to string and increment it
                        string maxCode = result.ToString();
                        int nextCode = Convert.ToInt32(maxCode) + 1;
                        txt_matricule.Text = nextCode.ToString("D5"); // Format as two digits
                    }
                    else
                    {
                        txt_matricule.Text = "00001"; // Default value if no records exist
                    }
                }
            }
            // Set the state of the controls for insertion
            etat_champs(0); // 0 for insertion state



        }
        private void etat_champs(int etat)
        {
            // Enable or disable controls based on the state
            if (etat == 0) // Insertion
            {
                txt_matricule.Enabled = false; // Matricule is not editable in modification

                txt_nom.Enabled = true;
                txt_prenom.Enabled = true;
                txt_adresse.Enabled = true;
                txt_tel.Enabled = true;
                cb_sexe.Enabled = true;
                cb_sexe.SelectedIndex = -1; // Reset selection
                cb_type.Enabled = true;
                cb_type.SelectedIndex = -1; // Reset selection
                cb_pointage.Enabled = true;
                cb_pointage.SelectedIndex = -1; // Reset selection


                cb_fonction.Enabled = true;
                cb_fonction.SelectedIndex = -1; // Reset selection
                cb_affectation.Enabled = true;
                cb_affectation.SelectedIndex = -1; // Reset selection
                cb_affectation1.Enabled = true;
                cb_fonction1.Enabled = true;

                bt_ajouter.Enabled = false;
                bt_modifier.Enabled = false;
                bt_enregistrer.Enabled = true;
                bt_fermer.Enabled = true;
                bt_fermer.Text = "Fermer";
                Text = "Insertion d'un nouvel employé";

            }
            else if (etat == 1) // Modification
            {
                txt_matricule.Enabled = false; // Matricule is not editable in modification
                txt_nom.Enabled = true;
                txt_prenom.Enabled = true;
                txt_adresse.Enabled = true;
                txt_tel.Enabled = true;
                cb_sexe.Enabled = true;
                
                cb_type.Enabled = true;
                
                cb_pointage.Enabled = true;
                


                cb_fonction.Enabled = true;
                
                cb_affectation.Enabled = true;
                
                cb_affectation1.Enabled = true;
                cb_fonction1.Enabled = true;

                bt_ajouter.Enabled = false;
                bt_modifier.Enabled = false;
                bt_enregistrer.Enabled = true;
                bt_fermer.Enabled = true;
                bt_fermer.Text = "Fermer";
                Text = "Modification de l'employé";


            }
            else if (etat == 2) //default
            {
                txt_matricule.Enabled = false;
                txt_nom.Enabled = false;
                txt_prenom.Enabled = false;
                txt_adresse.Enabled = false;
                txt_tel.Enabled = false;
                cb_sexe.Enabled = false;
                cb_sexe.SelectedIndex = -1; // Reset selection
                cb_type.Enabled = false;
                cb_type.SelectedIndex = -1; // Reset selection
                cb_pointage.Enabled = false;
                cb_pointage.SelectedIndex = -1;

                cb_fonction.Enabled = false;
                cb_fonction.SelectedIndex = -1; // Reset selection
                cb_affectation.Enabled = false;
                cb_affectation.SelectedIndex = -1; // Reset selection
                cb_affectation1.Enabled = false;
                cb_fonction1.Enabled = false;

                bt_ajouter.Enabled = true;
                bt_modifier.Enabled = false;
                bt_enregistrer.Enabled = false;
                bt_fermer.Enabled = true;
                bt_fermer.Text = "Fermer";
                Text = "Employé";
            }
        }

        private void bt_modifier_Click(object sender, EventArgs e)
        {
            insertion_modification = 1; // Set to 1 for modification
            // Set the state of the controls for modification
            etat_champs(1); // 1 for modification state

        }

        private void bt_enregistrer_Click(object sender, EventArgs e)
        {
            // Validate the input fields before saving
            if (string.IsNullOrWhiteSpace(txt_matricule.Text) ||
                string.IsNullOrWhiteSpace(txt_nom.Text) ||
                string.IsNullOrWhiteSpace(txt_prenom.Text) ||
                cb_fonction.SelectedIndex == -1 ||
                cb_affectation.SelectedIndex == -1)
            {
                XtraMessageBox.Show("Veuillez remplir tous les champs obligatoires.",
                                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Define the connection string
            string connectionString = "Server=.\\SQLEXPRESS;Database=DEVCORE_PN;Integrated Security=True;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                if (insertion_modification == 0) // Insertion
                {
                    string query = @"INSERT INTO Personnel 
                         (matricule, nom, prenom, sexe, adress, tel, id_fonction, id_affectation, type_agent, poinatge) 
                         VALUES (@matricule, @nom, @prenom, @sexe, @adress, @tel, @c_fonct, @c_affect, @type_agent, @poinatge)";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@matricule", txt_matricule.Text.Trim());
                        cmd.Parameters.AddWithValue("@nom", txt_nom.Text.Trim());
                        cmd.Parameters.AddWithValue("@prenom", txt_prenom.Text.Trim());

                        cmd.Parameters.AddWithValue("@sexe", cb_sexe.Text.Trim());
                        cmd.Parameters.AddWithValue("@adress", txt_adresse.Text.Trim());
                        cmd.Parameters.AddWithValue("@tel", txt_tel.Text.Trim());
                        cmd.Parameters.AddWithValue("@type_agent", cb_type.Text.Trim());
                        cmd.Parameters.AddWithValue("@poinatge", cb_pointage.Text.Trim());

                        cmd.Parameters.AddWithValue("@c_fonct", cb_fonction.SelectedValue);
                        cmd.Parameters.AddWithValue("@c_affect", cb_affectation.SelectedValue);

                        cmd.ExecuteNonQuery();
                    }
                }
                else if (insertion_modification == 1) // Modification
                {
                    string query = @"UPDATE Personnel 
                         SET nom = @nom, prenom = @prenom, sexe=@sexe, adress=@adress, tel=@tel,
                             id_fonction = @c_fonct, id_affectation = @c_affect, type_agent=@type_agent, poinatge=@poinatge
                         WHERE matricule = @matricule";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@matricule", txt_matricule.Text.Trim());
                        cmd.Parameters.AddWithValue("@nom", txt_nom.Text.Trim());
                        cmd.Parameters.AddWithValue("@prenom", txt_prenom.Text.Trim());

                        cmd.Parameters.AddWithValue("@sexe", cb_sexe.Text.Trim());
                        cmd.Parameters.AddWithValue("@adress", txt_adresse.Text.Trim());
                        cmd.Parameters.AddWithValue("@tel", txt_tel.Text.Trim());
                        cmd.Parameters.AddWithValue("@type_agent", cb_type.Text.Trim());
                        cmd.Parameters.AddWithValue("@poinatge", cb_pointage.Text.Trim());

                        cmd.Parameters.AddWithValue("@c_fonct", cb_fonction.SelectedValue);
                        cmd.Parameters.AddWithValue("@c_affect", cb_affectation.SelectedValue);

                        cmd.ExecuteNonQuery();
                    }
                }

                XtraMessageBox.Show("Enregistrement effectué avec succès.",
                                    "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                etat_champs(2); // Default state
                //Close();
            }


        }
    }
}