using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization;

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

        // الحد الأقصى لساعات Bon Sortie في اليوم
        private const decimal MAX_HEURES_JOUR = 8m;

        // قراءة رقم يقبل الفاصلة والنقطة بغض النظر عن لغة النظام
        private static bool TryParseNbr(string s, out decimal v) =>
            decimal.TryParse((s ?? "").Trim().Replace(',', '.'),
                             NumberStyles.Number, CultureInfo.InvariantCulture, out v);

        private static SqlParameter DateParam(string name, DateTime value) =>
            new SqlParameter(name, SqlDbType.Date) { Value = value.Date };

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
            //LoadAllAbsences();

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
            chk_periodique.Enabled = false;                 // لا تحويل بين النوعين أثناء التعديل
            dt_au.Enabled = !chk_periodique.Checked;        // Congé: نعدّل تاريخ النهاية
            txt_nbrABS.Enabled = chk_periodique.Checked;    // Bon Sortie: نعدّل الساعات

            cb_horaire.Enabled = true;
            cb_type.Enabled = true;
            txt_observation.Enabled = true;

            // تفعيل أزرار التحكم
            bt_enregistrer.Enabled = true;
            bt_ajouter.Enabled = false;
            bt_modifier.Enabled = false;

            if (chk_periodique.Checked) txt_nbrABS.Focus();
            else dt_au.Focus();
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

        private bool load_personnel_details(bool showMessage = true)
        {
            string matricule = txt_matricule.Text.Trim();
            if (string.IsNullOrEmpty(matricule)) return false;
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
                return true;
            }
            else
            {
                lb_nom_prenom.Text = "";
                lb_nom_prenom.Visible = false;
                if (showMessage)
                    MessageBox.Show("المستخدم غير موجود", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return false;
            }
        }

        private void txt_matricule_KeyDown(object sender, KeyEventArgs e)
        {
            // 1. التحقق من ضغط زر Enter
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;                       // يمنع صوت "ding"
                SelectNextControl((Control)sender, true, true, true, true); // ننتقل للحقل التالي، و Leave يتحقق مرة واحدة
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

        private bool existe_absence(string matricule, DateTime date_debut, DateTime date_fin,
                            string nature, string num_bon_actuel = "")
        {
            try
            {
                if (string.IsNullOrEmpty(matricule)) return false;

                string query = @"SELECT COUNT(*) FROM [ABS] 
            WHERE MATRI = @matri 
            AND CAST(DATE1 AS date) <= @d_fin 
            AND CAST(CASE WHEN NATURE = 'S' THEN DATE1 
                          ELSE ISNULL(DATE2, DATE1) END AS date) >= @d_debut
            AND NOT (@nature = 'S' AND ISNULL(NATURE, 'C') = 'S')";

                var parameters = new List<SqlParameter>
        {
            new SqlParameter("@matri", matricule),
            DateParam("@d_debut", date_debut),
            DateParam("@d_fin", date_fin),
            new SqlParameter("@nature", nature)
        };

                // في التعديل نستثني السجل الحالي نفسه
                if (!string.IsNullOrEmpty(num_bon_actuel))
                {
                    query += " AND NOT (NUM_BON = @num_bon AND CAST(DATE1 AS date) = @d_debut)";
                    parameters.Add(new SqlParameter("@num_bon", num_bon_actuel));
                }

                DataTable dt = ConnectSqlServer.ExecuteSelect(query, parameters.ToArray());

                if (dt != null && dt.Rows.Count > 0)
                    return Convert.ToInt32(dt.Rows[0][0]) > 0;

                return false;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("خطأ في فحص التداخل: " + ex.Message);
                return false;
            }
        }

        private decimal heures_deja_prises(string matricule, DateTime jour, string numBonActuel)
        {
            string query = @"SELECT ISNULL(SUM(HEURE), 0) FROM [ABS] 
             WHERE MATRI = @matri AND NATURE = 'S' 
             AND CAST(DATE1 AS date) = @jour";

            var parameters = new List<SqlParameter>
    {
        new SqlParameter("@matri", matricule),
        DateParam("@jour", jour)
    };

            if (!string.IsNullOrEmpty(numBonActuel))
            {
                query += " AND NUM_BON <> @num_bon";   // عند التعديل لا نحسب البون نفسه
                parameters.Add(new SqlParameter("@num_bon", numBonActuel));
            }

            DataTable dt = ConnectSqlServer.ExecuteSelect(query, parameters.ToArray());

            if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0] != DBNull.Value)
                return Math.Round(Convert.ToDecimal(dt.Rows[0][0]), 2);

            return 0m;
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
            if (!chk_periodique.Checked) return;      // الحد الأقصى يخص Bon Sortie فقط

            if (TryParseNbr(txt_nbrABS.Text, out decimal val))
            {
                if (val < 0 || val > MAX_HEURES_JOUR) 
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

        // قبل (ما أرسلته في الخطوة 4، لا تطبقه)
        //return yearMonth + nextId.ToString().PadLeft(3, '0');

        // بعد: استبدل الدالة كاملة
        private string GetNextNumBon(string matri, DateTime date1)
        {
            string query = "SELECT NUM_BON FROM [ABS] WHERE MATRI = @matri AND DATE1 = @date1";

            SqlParameter[] parameters = {
        new SqlParameter("@matri", matri),
        DateParam("@date1", date1)
    };

            DataTable dt = ConnectSqlServer.ExecuteSelect(query, parameters);

            var utilises = new HashSet<string>();
            foreach (DataRow r in dt.Rows)
                utilises.Add(r["NUM_BON"].ToString().Trim());

            for (int i = 1; i <= 9; i++)
                if (!utilises.Contains(i.ToString()))
                    return i.ToString();

            return "";   // الأرقام 1..9 كلها مستعملة
        }

        private void bt_enregistrer_Click(object sender, EventArgs e)
        {
            string matricule = txt_matricule.Text.Trim();

            // 1) الحقول الإلزامية
            if (string.IsNullOrEmpty(matricule) || cb_type.SelectedValue == null)
            {
                XtraMessageBox.Show("Veuillez remplir les champs obligatoires", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 2) الموظف موجود فعلاً
            if (!load_personnel_details(false))
            {
                XtraMessageBox.Show("Matricule invalide. Veuillez vérifier.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_matricule.Focus();
                return;
            }

            bool bonSortie = chk_periodique.Checked;
            DateTime du = dt_du.Value.Date;
            DateTime au = bonSortie ? du : dt_au.Value.Date;
            string nature = bonSortie ? "S" : "C";

            // 3) منطقية التواريخ
            if (au < du)
            {
                XtraMessageBox.Show("تاريخ النهاية لا يمكن أن يسبق تاريخ البداية.", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 4) عدد الأيام / الساعات
            if (!TryParseNbr(txt_nbrABS.Text, out decimal nbr) || nbr <= 0)
            {
                XtraMessageBox.Show(bonSortie ? "Veuillez saisir le nombre d'heures." : "Nombre de jours invalide.",
                                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (txt_nbrABS.Enabled) txt_nbrABS.Focus();
                return;
            }

            // 5) في التعديل: رقم البون من الصف المحدد
            string numBonActuel = "";
            if (insertion_modification != 0)
            {
                if (gridView1.FocusedRowHandle < 0)
                {
                    XtraMessageBox.Show("Veuillez sélectionner un enregistrement à modifier.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                numBonActuel = gridView1.GetFocusedRowCellValue("NUM_BON")?.ToString() ?? "";
            }

            // 6) التداخل مع غياب آخر
            if (existe_absence(matricule, du, au, nature, numBonActuel))
            {
                XtraMessageBox.Show("عذراً، يوجد تداخل مع غياب آخر مسجل لهذا الموظف!", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 7) الحد الأقصى اليومي لساعات Bon Sortie
            if (bonSortie)
            {
                decimal deja = heures_deja_prises(matricule, du, numBonActuel);
                if (deja + nbr > MAX_HEURES_JOUR)
                {
                    decimal reste = Math.Max(0m, MAX_HEURES_JOUR - deja);
                    XtraMessageBox.Show(
                        $"Dépassement du maximum journalier ({MAX_HEURES_JOUR} h).\n\n" +
                        $"Déjà enregistré ce jour : {deja} h\n" +
                        $"Reste disponible : {reste} h",
                        "Attention", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txt_nbrABS.Focus();
                    return;
                }
            }

            // 8) الحفظ
            try
            {
                string numBon;
                string query;

                if (insertion_modification == 0) // إضافة
                {
                    numBon = GetNextNumBon(matricule, du);

                    if (numBon == "")
                    {
                        XtraMessageBox.Show("Maximum atteint : 9 bons par employé et par jour.", "Attention",
                                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    query = @"INSERT INTO [ABS] 
                      (MATRI, C_CONGE, DATE1, DATE2, JOUR, HEURE, NATURE, OBS, NUM_BON, CLOT) 
                      VALUES 
                      (@matri, @c_conge, @date1, @date2, @jour, @heure, @nature, @obs, @num_bon, 'N')";
                }
                else // تعديل
                {
                    numBon = numBonActuel;

                    query = @"UPDATE [ABS] SET 
                      C_CONGE=@c_conge, DATE2=@date2, JOUR=@jour, HEURE=@heure, 
                      NATURE=@nature, OBS=@obs 
                      WHERE MATRI=@matri AND CAST(DATE1 AS date)=@date1 AND NUM_BON=@num_bon
                      AND UPPER(ISNULL(CLOT,'N')) <> 'O'";
                }

                SqlParameter[] parameters = {
            new SqlParameter("@matri", matricule),
            new SqlParameter("@c_conge", cb_type.SelectedValue.ToString()),
            DateParam("@date1", du),
            DateParam("@date2", au),     // Bon Sortie: DATE2 = DATE1 وليس NULL
            new SqlParameter("@jour", bonSortie ? 0m : nbr),
            new SqlParameter("@heure", bonSortie ? nbr : 0m),
            new SqlParameter("@nature", nature),
            new SqlParameter("@obs", txt_observation.Text.Trim()),
            new SqlParameter("@num_bon", numBon)
        };

                if (!ConnectSqlServer.TryExecuteNonQuery(query, parameters, out int rows, out string error))
                {
                    XtraMessageBox.Show("Erreur lors de l'enregistrement : " + error, "Erreur",
                                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (rows > 0)
                {
                    XtraMessageBox.Show($"Enregistré avec succès ! Numéro de bon: {numBon}", "Succès",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadAllAbsences();
                    ResetUI();
                }
                else
                {
                    XtraMessageBox.Show("Aucune ligne n'a été enregistrée (enregistrement introuvable ou clôturé).", "Information",
                                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Erreur: " + ex.Message, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                load_personnel_details(false);

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
            chk_periodique.Enabled = false;
            txt_nbrABS.Enabled = false;
            txt_observation.Enabled = false;

            bt_ajouter.Enabled = true;
            bt_modifier.Enabled = false;
            bt_enregistrer.Enabled = false;
            
            // تحديث الـ GridControl لعرض البيانات الجديدة
            // LoadDataGrid(); 
        }

        // ✅ تصدير البيانات إلى Excel و PDF باستخدام DevExpress
        private void bt_exportexcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.Filter = "xlsx files (*.xlsx)|*.xlsx";
            sd.FilterIndex = 1;
            sd.RestoreDirectory = true;
            if (sd.ShowDialog() == DialogResult.OK)
            {
                gridControl1.ExportToXlsx(sd.FileName);
                Process.Start(sd.FileName);
            }
        }

        private void bt_exportpdf_Click(object sender, EventArgs e)
        {
            SaveFileDialog sd = new SaveFileDialog();
            sd.Filter = "Pdf files (*.pdf)|*.pdf";
            sd.FilterIndex = 1;
            sd.RestoreDirectory = true;
            if (sd.ShowDialog() == DialogResult.OK)
            {
                gridControl1.ExportToPdf(sd.FileName);
                Process.Start(sd.FileName);
            }
        }
    }
}