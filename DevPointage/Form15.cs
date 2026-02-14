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
    public partial class Form15 : DevExpress.XtraEditors.XtraForm
    {
        public Form15()
        {
            InitializeComponent();
            ConnectSqlServer.Initialized();
        }

        static public int insertion_modification;
        public decimal n_jours;
        public decimal temps;
        public Boolean vergule;
        public char Separateur = System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator[0];

        private void bt_ajouter_Click(object sender, EventArgs e)
        {
            insertion_modification = 0;
            

            txt_matricule.Text = "";
            lb_nom_prenom.Text = "Nom et prénom";
                dt_du.Value = DateTime.Now;
                dt_au.Value = DateTime.Now;
            cb_horaire.SelectedIndex = -1;
            cb_type.SelectedIndex = -1;
            txt_nbrABS.Text = "";
            txt_observation.Text = "";

            txt_matricule.Enabled = true;
            dt_du.Enabled = true;
            dt_au.Enabled = true;
            cb_horaire.Enabled = true;
            chk_periodique.Enabled = true;
            cb_type.Enabled = true;
            txt_nbrABS.Enabled = false;
            txt_observation.Enabled = true;

            bt_ajouter.Enabled = false;
            bt_modifier.Enabled = false;
            bt_enregistrer.Enabled = true;


            load_horaire();
            load_type_conge("N");
            chk_periodique_CheckedChanged(sender, e);
            UpdateAbsenceDuration();
            LoadAllAbsences();

            ActiveControl = txt_matricule;

        }

        private void bt_fermer_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bt_apercu_Click(object sender, EventArgs e)
        {

        }

        private void dt_au_ValueChanged(object sender, EventArgs e)
        {
            if (dt_au.Enabled == true)
            {
                n_jours = (dt_au.Value.Date - dt_du.Value.Date).Days + 1;
                txt_nbrABS.Text = n_jours.ToString();
                temps = 0;
            }
            else
            {
                n_jours = 0;
            }
            UpdateAbsenceDuration();
        }

        private void bt_modifier_Click(object sender, EventArgs e)
        {
            insertion_modification = 1; // وضع التعديل

            // تفعيل الحقول للإدخال
            txt_matricule.Enabled = false; // غالباً الماتريكول لا يُعدل لأنه جزء من المفتاح الأساسي
            dt_du.Enabled = false;        // تاريخ البداية أيضاً جزء من المفتاح الأساسي

            // إذا كان إجازة، نفعّل تاريخ النهاية
            dt_au.Enabled = !chk_periodique.Checked;

            cb_horaire.Enabled = true;
            cb_type.Enabled = true;
            txt_nbrABS.Enabled = true;
            txt_observation.Enabled = true;

            // تفعيل أزرار التحكم
            bt_enregistrer.Enabled = true;
            bt_ajouter.Enabled = false;
            bt_modifier.Enabled = false;    

            txt_nbrABS.Focus();
        }

        private void Form15_Load(object sender, EventArgs e)
        {
            txt_annee.Text = Form2.annee_en_cours;
            liste_mois.SelectedIndex = DateTime.Now.Month - 1;
            load_horaire();
            load_type_conge("N");
            LoadAllAbsences();
            ResetUI();
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
                lb_nom_prenom.Text = row["nom"].ToString() + " " + row["prenom"].ToString();
                lb_nom_prenom.Visible = true;

            }
            else
            {
                lb_nom_prenom.Text = "";
                lb_nom_prenom.Visible = false;
                MessageBox.Show("المستخدم غير موجود", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
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

        private void txt_matricule_Leave(object sender, EventArgs e)
        {
            load_personnel_details();
            //vider le gridControl1 
            //gridControl1.DataSource = null;

            //gridControl1.Refresh();
            
        }

        private void load_horaire()
        {
            string query = "SELECT code,designation FROM horaire_travail";
            DataTable dt = ConnectSqlServer.ExecuteSelect(query);
            cb_horaire.DataSource = dt;
            cb_horaire.DisplayMember = "designation";
            cb_horaire.ValueMember = "code";
            cb_horaire.SelectedIndex = -1;
        }
        private void load_type_conge(string prd)
        {
            string query = "SELECT code,lib FROM type_conge where PERIODIQUE=@periodique";

            // 2. تعريف البارامترات
            SqlParameter[] parameters = {
        new SqlParameter("@periodique", prd ?? (object)DBNull.Value)
                                        };
            // 3. تمرير البارامترات للدالة 
            DataTable dt = ConnectSqlServer.ExecuteSelect(query, parameters);


            // 4. ربط البيانات بـ ComboBox
            cb_type.DataSource = dt;
            cb_type.DisplayMember = "lib";
            cb_type.ValueMember = "code";

            // 5. ضبط الاختيار الافتراضي
            cb_type.SelectedIndex = -1;

        }

        private void chk_periodique_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_periodique.Checked)
            {
                load_type_conge("N");
                chk_periodique.Text = "Bon Sortie";
                lb_nbrABS.Text = "Nbr Heurs :";
                dt_au.Enabled = false;
                txt_nbrABS.Enabled = true;
            }
            else
            {
                load_type_conge("O");
                chk_periodique.Text = "Conge";
                lb_nbrABS.Text = "Nbr Jours";
                dt_au.Enabled = true;
                txt_nbrABS.Enabled = false;
            }
            UpdateAbsenceDuration();
            CalculateDays();
        }

        private void txt_nbrABS_Leave(object sender, EventArgs e)
        {
            if (decimal.TryParse(txt_nbrABS.Text, out decimal val))
            {
                if (val < 0 || val > 8)
                {
                    MessageBox.Show("Veuillez vérifier vos données?", "DevPointage", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_nbrABS.Text = "0";
                    txt_nbrABS.Focus(); // أفضل من ActiveControl في هذه الحالة
                    temps = 0;
                    n_jours = 0;
                }
                else
                {
                    temps = val;
                }
            }
            else
            {
                txt_nbrABS.Text = "0";
                temps = 0;
            }
        }

        // دالة محسنة للتحقق من التواريخ وحساب الأيام
        private void CalculateDays()
        {
            if (dt_au.Enabled)
            {
                if (dt_au.Value.Date < dt_du.Value.Date)
                {
                    // منع اختيار تاريخ نهاية قبل تاريخ البداية
                    dt_au.Value = dt_du.Value;
                }
                n_jours = (dt_au.Value.Date - dt_du.Value.Date).Days + 1;
                txt_nbrABS.Text = n_jours.ToString();
            }
            else { 
                n_jours = 0;
                txt_nbrABS.Text = "0";
                dt_au.Value = dt_du.Value; // تأكد من أن تاريخ النهاية يساوي تاريخ البداية في حالة Bon Sortie
            }
        }

        // تعديل حدث تغيير تاريخ البداية
        private void dt_du_ValueChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_matricule.Text)) return;

            string code = get_personnel_emploi_code(txt_matricule.Text, dt_du.Value);

            if (string.IsNullOrWhiteSpace(code))
            {
                cb_horaire.SelectedIndex = -1;
                // يفضل عدم إظهار MessageBox في كل مرة يتغير فيها التاريخ لتجنب إزعاج المستخدم
                // يمكن تغيير لون الكومبوبوكس للأحمر مثلاً كتحذير
            }
            else
            {
                cb_horaire.SelectedValue = code.Trim();
                bt_enregistrer.Enabled = true; // تفعيل الحفظ عند التأكد من وجود جدول توقيت
            }

            CalculateDays();
            UpdateAbsenceDuration();
        }

        private string get_personnel_emploi_code(string matricule, DateTime date_jr)
        {
            //string matricule = txt_matricule.Text.Trim();
            if (string.IsNullOrEmpty(matricule)) return "";

            string query = @"SELECT code FROM Personnel_emploi_temps 
                     WHERE matri = @matricule 
                     AND @date_jr BETWEEN date_du AND date_au";

            SqlParameter[] parameters = {
        new SqlParameter("@matricule", SqlDbType.Char, 5) { Value = matricule },
        new SqlParameter("@date_jr", SqlDbType.Date) { Value = date_jr.Date }
    };

            DataTable dt = ConnectSqlServer.ExecuteSelect(query, parameters);

            return dt.Rows.Count > 0 ? dt.Rows[0]["code"].ToString() : "";
        }

        private void UpdateAbsenceDuration()
        {
            // 1. التحقق من منطقية التواريخ (فقط إذا لم يكن Bon Sortie)
            if (!chk_periodique.Checked)
            {
                if (dt_au.Value.Date < dt_du.Value.Date)
                {
                    MessageBox.Show("تاريخ النهاية لا يمكن أن يكون أصغر من تاريخ البداية",
                                    "خطأ في التاريخ", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    // تصحيح التاريخ تلقائياً
                    dt_au.Value = dt_du.Value;
                }

                // 2. حساب عدد الأيام بعد التأكد من صحة التواريخ
                n_jours = (dt_au.Value.Date - dt_du.Value.Date).Days + 1;
                txt_nbrABS.Text = n_jours.ToString();
                temps = 0;
            }
            else
            {
                // في حالة Bon Sortie
                n_jours = 0;
                // هنا المستخدم يدخل الساعات يدوياً في txt_nbrABS
            }
        }

        private string GetNextNumBon(string matri, DateTime date1)
        {
            string query = "SELECT ISNULL(MAX(CAST(NUM_BON AS INT)), 0) + 1 FROM [ABS] WHERE MATRI = @matri AND DATE1 = @date1";

            SqlParameter[] parameters = {
        new SqlParameter("@matri", matri),
        new SqlParameter("@date1", date1.Date)
    };

            DataTable dt = ConnectSqlServer.ExecuteSelect(query, parameters);
            return dt.Rows[0][0].ToString();
        }

        private void bt_enregistrer_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txt_matricule.Text) || cb_type.SelectedValue == null)
            {
                MessageBox.Show("Veuillez remplir les champs obligatoires", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string numBon;
                string query = "";

                if (insertion_modification == 0) // إضافة
                {
                    // توليد الرقم التالي تلقائياً
                    numBon = GetNextNumBon(txt_matricule.Text.Trim(), dt_du.Value);

                    query = @"INSERT INTO [ABS] 
                      (MATRI, C_CONGE, DATE1, DATE2, JOUR, HEURE, NATURE, OBS, NUM_BON, CLOT) 
                      VALUES 
                      (@matri, @c_conge, @date1, @date2, @jour, @heure, @nature, @obs, @num_bon, 'N')";
                }
                else // تعديل
                {
                    // في التعديل نأخذ الرقم الموجود في الصف المختار من الـ Grid
                    numBon = gridView1.GetFocusedRowCellValue("NUM_BON").ToString();

                    query = @"UPDATE [ABS] SET 
                      C_CONGE=@c_conge, DATE2=@date2, JOUR=@jour, HEURE=@heure, 
                      NATURE=@nature, OBS=@obs 
                      WHERE MATRI=@matri AND DATE1=@date1 AND NUM_BON=@num_bon";
                }

                SqlParameter[] parameters = {
            new SqlParameter("@matri", txt_matricule.Text.Trim()),
            new SqlParameter("@c_conge", cb_type.SelectedValue.ToString()),
            new SqlParameter("@date1", dt_du.Value.Date),
            new SqlParameter("@date2", chk_periodique.Checked ? (object)DBNull.Value : dt_au.Value.Date),
            new SqlParameter("@jour", chk_periodique.Checked ? 0 : Convert.ToDecimal(txt_nbrABS.Text)),
            new SqlParameter("@heure", chk_periodique.Checked ? Convert.ToDecimal(txt_nbrABS.Text) : 0),
            new SqlParameter("@nature", chk_periodique.Checked ? "S" : "C"),
            new SqlParameter("@obs", txt_observation.Text.Trim()),
            new SqlParameter("@num_bon", numBon)
        };

                ConnectSqlServer.ExecuteNonQuery(query, parameters);
                MessageBox.Show($"Enregistré avec succès ! Numéro de bon: {numBon}", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // تحديث الجدول بعد الحفظ
                LoadAllAbsences();
                ResetUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur: " + ex.Message);
            }
        }
        private void LoadAllAbsences()
        {
            try
            {
                // 1. تحويل الشهر المختار إلى رقم (Janvier = 1, etc.)
                int month = liste_mois.SelectedIndex + 1;
                string year = txt_annee.Text.Trim();

                if (string.IsNullOrEmpty(year))
                {
                    MessageBox.Show("Veuillez saisir l'année");
                    return;
                }

                // 2. الاستعلام لجلب البيانات مع اسم الموظف (Join) ليكون الجدول مفهوماً
                string query = @"SELECT A.MATRI, P.NOM + ' ' + P.PRENOM AS [Nom & Prénom], 
                               A.DATE1, A.DATE2, A.NUM_BON, A.C_CONGE, 
                               A.JOUR, A.HEURE, A.NATURE, A.OBS
                        FROM [ABS] A
                        INNER JOIN [PERSONNEL] P ON A.MATRI = P.MATRICULE
                        WHERE MONTH(A.DATE1) = @month 
                        AND YEAR(A.DATE1) = @year
                        ORDER BY A.DATE1 DESC, A.MATRI ASC";

                SqlParameter[] parameters = {
            new SqlParameter("@month", month),
            new SqlParameter("@year", year)
        };

                DataTable dt = ConnectSqlServer.ExecuteSelect(query, parameters);

                // 3. ربط النتيجة بالـ Grid
                gridControl1.DataSource = dt;
               // dataGridView1.DataSource = dt; // إذا كنت تستخدم DataGridView أيضاً

                // تحسين مظهر الأعمدة في DevExpress (اختياري)
                gridView1.BestFitColumns();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Erreur lors du chargement : " + ex.Message);
            }
        }

        private void liste_mois_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadAllAbsences();
        }

        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            ResetUI(); // إعادة تعيين الحقول قبل تحميل البيانات الجديدة

            // التأكد من وجود سطر مختار
            if (gridView1.FocusedRowHandle >= 0)
            {
                DataRow row = gridView1.GetDataRow(gridView1.FocusedRowHandle);

                txt_matricule.Text = row["MATRI"].ToString();
                // استدعاء دالة جلب الاسم التي برمجناها سابقاً
                load_personnel_details();

                dt_du.Value = Convert.ToDateTime(row["DATE1"]);

                // التحقق من نوع الغياب (Nature) لضبط الـ CheckBox والتواريخ
                if (row["NATURE"].ToString() == "S") // "S" تعني Sortie/ساعات
                {
                    chk_periodique.Checked = true;
                    txt_nbrABS.Text = row["HEURE"].ToString();
                    dt_au.Enabled = false;
                }
                else // "C" تعني Congé/أيام
                {
                    chk_periodique.Checked = false;
                    txt_nbrABS.Text = row["JOUR"].ToString();
                    dt_au.Value = row["DATE2"] != DBNull.Value ? Convert.ToDateTime(row["DATE2"]) : dt_du.Value;
                    dt_au.Enabled = true;
                }

                cb_type.SelectedValue = row["C_CONGE"].ToString();
                txt_observation.Text = row["OBS"].ToString();

                bt_ajouter.Enabled = true;
                // تفعيل زر التعديل وتعطيل الحفظ مؤقتاً
                bt_modifier.Enabled = true;
                bt_enregistrer.Enabled = false;
            }
        }
        private void ResetUI()
        {
            txt_matricule.Text = "";
            lb_nom_prenom.Text = "";
            txt_nbrABS.Text = "0";
            txt_observation.Text = "";
            dt_du.Value = DateTime.Now;
            dt_au.Value = DateTime.Now;
            chk_periodique.Checked = true;
            cb_horaire.SelectedIndex = -1;
            cb_type.SelectedIndex = -1;

            txt_matricule.Enabled = false;
            dt_du.Enabled = false;
            dt_au.Enabled = false;
            cb_horaire.Enabled = false;
            cb_type.Enabled = false;
            txt_nbrABS.Enabled = false;
            txt_observation.Enabled = false;

            bt_ajouter.Enabled = true;
            bt_modifier.Enabled = false;
            bt_enregistrer.Enabled = false;
            
            // تحديث الـ GridControl لعرض البيانات الجديدة
            // LoadDataGrid(); 
        }
    }
}