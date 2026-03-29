using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DevPointage
{
    public partial class Form18 : DevExpress.XtraEditors.XtraForm
    {
        public Form18()
        {
            InitializeComponent();
            ConnectSqlServer.Initialized();
        }
        static public int insertion_modification; // 0: Insertion, 1: Modification
        private void bt_fermer_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bt_ajouter_Click(object sender, EventArgs e)
        {
            txt_ip.Enabled = true;
            txt_ip.Text = string.Empty;
            txt_ip.Focus();

            txt_lib.Enabled = true;
            txt_lib.Text = string.Empty;
        }

        private void Form18_Load(object sender, EventArgs e)
        {
            if (insertion_modification == 0)
            {
                // إعدادات لعملية الإدراج
                this.Text = "Ajouter une adresse IP";
                txt_ip.Enabled = true;
                txt_lib.Enabled = true;

                bt_ajouter.Enabled = true;
                bt_modifier.Enabled = false;
                bt_enregistrer.Enabled = true;
            }
            else if (insertion_modification == 1)
            {
                // إعدادات لعملية التعديل
                this.Text = "Modifier une adresse IP";
                // يمكنك تحميل بيانات العنوان الحالي إلى الحقول هنا إذا لزم الأمر
            }
        }

        private void bt_enregistrer_Click(object sender, EventArgs e)
        {
            // 1. التحقق من صحة الإدخال
            if (string.IsNullOrWhiteSpace(txt_ip.Text) || string.IsNullOrWhiteSpace(txt_lib.Text))
            {
                MessageBox.Show("Veuillez remplir tous les champs obligatoires.", "Champs manquants", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txt_ip.Focus();
                return;
            }

            try
            {
                string query = "";
                SqlParameter[] parameters = {
            new SqlParameter("@ip", txt_ip.Text.Trim()),
            new SqlParameter("@lib", txt_lib.Text.Trim())
            
        };

                if (insertion_modification == 0)
                {
                    // --- عملية الإضافة ---
                    query = "INSERT INTO POINTEUSE (adresse_ip, lib_pointeuse, N_PORT, N_MACHINE, ETAT) " +
                            "VALUES (@ip, @lib, '4370', '1', 'O')";
                }
                else
                {
                    // --- عملية التعديل ---
                    // تأكد من تمرير المعرف الصحيح للسجل (مثلاً من متغير اسمه currentId)
                    query = "UPDATE POINTEUSE SET adresse_ip = @ip, lib_pointeuse = @lib WHERE adresse_ip = @ip";

                    
                }

                // تنفيذ الاستعلام
                ConnectSqlServer.ExecuteNonQuery(query, parameters);

                string message = (insertion_modification == 0) ? "ajoutée" : "modifiée";
                MessageBox.Show($"Adresse IP {message} avec succès.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // تنظيف الواجهة بعد العملية
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors de l'opération : " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // دالة مساعدة لتنظيف الحقول
        private void ClearFields()
        {
            txt_ip.Clear();
            txt_lib.Clear();
            insertion_modification = 0; // العودة لوضع الإضافة الافتراضي
        }
    }
}