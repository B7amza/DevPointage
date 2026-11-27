using Microsoft.VisualBasic; // لاستعمال GetSetting / SaveSetting
using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace DevPointage
{
    public static class ConnectSqlServer
    {
        public static SqlConnection myConn = new SqlConnection();
        public static string serveur, instance, username, password, constring, port;
        public static string DATABASE_NAME = "DEVCORE_PN";

        public static bool Initialized()
        {
            string AppName = Application.ProductName;

            DATABASE_NAME = Interaction.GetSetting(AppName, "DBSection", "DB_Name", "DEVCORE_PN");
            serveur = Interaction.GetSetting(AppName, "DBSection", "DB_IP", @".\SQLEXPRESS");
            username = Interaction.GetSetting(AppName, "DBSection", "DB_User", "sa");
            password = Interaction.GetSetting(AppName, "DBSection", "DB_Password", "123");
            port = Interaction.GetSetting(AppName, "DBSection", "DB_Port", "1433");

            // SQL Authentication
            // constring = $"Server={serveur},{port};Database={DATABASE_NAME};User Id={username};Password={password};";

            // Windows Authentication
            constring = $"Server={serveur};Database={DATABASE_NAME};Trusted_Connection=True;";

            return true;
        }

        // -------------------------
        // فتح الاتصال
        // -------------------------
        public static bool OpenConnection()
        {
            try
            {
                if (myConn.State == ConnectionState.Open)
                    myConn.Close();

                myConn.ConnectionString = constring;
                myConn.Open();
                return true;
            }
            catch (SqlException ex)
            {
                MessageBox.Show("Erreur connexion SQL Server : " + ex.Message,
                                "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        // -------------------------
        // إغلاق الاتصال
        // -------------------------
        public static void CloseConnection()
        {
            if (myConn != null && myConn.State == ConnectionState.Open)
                myConn.Close();
        }

        // -------------------------
        // SELECT → يرجع DataTable
        // -------------------------
        public static DataTable ExecuteSelect(string query, params SqlParameter[] parameters)
        {
            DataTable dt = new DataTable();
            try
            {
                if (OpenConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query, myConn))
                    {
                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters);

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur SELECT : " + ex.Message,
                                "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CloseConnection();
            }
            return dt;
        }

        // -------------------------
        // INSERT / UPDATE / DELETE
        // -------------------------
        public static int ExecuteNonQuery(string query, params SqlParameter[] parameters)
        {
            int rows = 0;
            try
            {
                if (OpenConnection())
                {
                    using (SqlCommand cmd = new SqlCommand(query, myConn))
                    {
                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters);

                        rows = cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur NonQuery : " + ex.Message,
                                "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                CloseConnection();
            }
            return rows;
        }

        public static bool TryExecuteNonQuery(string query, SqlParameter[] parameters,
                                      out int rows, out string error)
        {
            rows = 0;
            error = null;
            try
            {
                if (!OpenConnection())
                {
                    error = "Connexion à la base de données impossible.";
                    return false;
                }

                using (SqlCommand cmd = new SqlCommand(query, myConn))
                {
                    if (parameters != null)
                        cmd.Parameters.AddRange(parameters);

                    rows = cmd.ExecuteNonQuery();
                }
                return true;
            }
            catch (SqlException ex) when (ex.Number == 2627 || ex.Number == 2601) // مفتاح مكرر
            {
                error = "Cet enregistrement existe déjà (numéro de bon en double). Veuillez réessayer.";
                return false;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
            finally
            {
                CloseConnection();
            }
        }
        // -------------------------
        // حفظ بيانات الاتصال
        // -------------------------
        public static void SaveData()
        {
            string AppName = Application.ProductName;

            Interaction.SaveSetting(AppName, "DBSection", "DB_Name", DATABASE_NAME);
            Interaction.SaveSetting(AppName, "DBSection", "DB_IP", serveur);
            Interaction.SaveSetting(AppName, "DBSection", "DB_Port", port);
            Interaction.SaveSetting(AppName, "DBSection", "DB_User", username);
            Interaction.SaveSetting(AppName, "DBSection", "DB_Password", password);

            Interaction.MsgBox("Les paramètres de connexion à SQL Server sont enregistrés.", MsgBoxStyle.Information);
        }
    }
}
