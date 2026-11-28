using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Filtering.Templates;
using DevExpress.XtraGrid;
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
    public partial class Form14 : DevExpress.XtraEditors.XtraForm
    {
        public TimeSpan hr_entree, hr_sortie;
        public Form14()
        {
            InitializeComponent();
            ConnectSqlServer.Initialized();
        }

        private void Form14_Load(object sender, EventArgs e)
        {
            txt_annee.Text = Form2.annee_en_cours;
            ActiveControl = txt_matricule;

            liste_mois.SelectedIndex = DateTime.Now.Month - 1;

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
                lb_nom_prenom.Text = row["nom"].ToString()+" "+ row["prenom"].ToString();
               lb_nom_prenom.Visible = true;

            }
            else
            {
                lb_nom_prenom.Text = "";
                lb_nom_prenom.Visible = false;
                MessageBox.Show("المستخدم غير موجود", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void txt_matricule_Leave(object sender, EventArgs e)
        {
            load_personnel_details();
            //vider le datagridview 
            dataGridView1.DataSource = null;
             dataGridView1.Rows.Clear();
             dataGridView1.Refresh();
            //vider le datagridview 
             entet2.RowCount = 0;

        }

        private void txt_matricule_KeyDown(object sender, KeyEventArgs e)
        {
            // 1. التحقق من ضغط زر Enter
            if (e.KeyCode == Keys.Enter)
            {

                e.SuppressKeyPress = true;   // بدون صوت التنبيه
                bt_apercu.Focus();

            }
        }
        private void Vue_Pointage_mensuelle(string matri)
            {
            // 1. التحقق من اختيار الشهر والسنة
            if (liste_mois.SelectedIndex == -1)
            {
                MessageBox.Show("الرجاء اختيار الشهر.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(txt_annee.Text))
            {
                MessageBox.Show("الرجاء إدخال السنة.", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 2. استخراج الشهر والسنة
            // بما أن قائمة الشهور تبدأ من يناير (فهرس 0)، فإن رقم الشهر هو الفهرس + 1
            int mois = liste_mois.SelectedIndex + 1;
            string annee = txt_annee.Text.Trim();

            // 3. تعريف الاستعلام
            // الاستعلام يقوم بالتصفية على الـ View الجديدة (Vue_Pointage_mensuelle)
            string query = @"
                SELECT 
                    LeJour, 
                    JourSemaine, 
                    HeureEntree, 
                    HeureSortie
                    
                FROM 
                    Vue_Pointage_mensuelle 
                WHERE 
                    MATRI = @matri AND 
                    MONTH(LeJour) = @mois AND 
                    YEAR(LeJour) = @annee
                ORDER BY 
                    LeJour";

            //string query = @"
            //    SELECT 
            //        LeJour, 
            //        JourSemaine, 
            //        HeureEntree, 
            //        HeureSortie, 
            //        DureeTravailMinutes,
            //        Col_6_StatutRetard,
            //        Col_7_HeureTheoriqueEntree,
            //        Col_8_HeureTheoriqueSortie,
            //        Col_9_HeuresSupplementaires,
            //        Col_10_HeuresAbsence,
            //        Col_11_MotifAbsence,
            //        Col_12_ValidationChef,
            //        Col_13_Commentaire,
            //        Col_14_ChampLibre1,
            //        Col_15_ChampLibre2
            //    FROM 
            //        Vue_Pointage_mensuelle 
            //    WHERE 
            //        MATRI = @matri AND 
            //        MONTH(LeJour) = @mois AND 
            //        YEAR(LeJour) = @annee
            //    ORDER BY 
            //        LeJour";

            // 4. تجهيز البارامترات
            SqlParameter[] parameters = {
              new SqlParameter("@matri", matri),
              new SqlParameter("@mois", mois),
              new SqlParameter("@annee", annee)
            };

            // 5. جلب البيانات
            DataTable dt = ConnectSqlServer.ExecuteSelect(query, parameters);

            // 6. عرض البيانات في DataGridView
            dataGridView1.DataSource = dt;

            // 7. تنسيق الأعمدة (اختياري - يفضل القيام به في التصميم أو بعد تحميل البيانات)
            // بما أن الـ DataGridView يحتوي على 15 عمودًا، يجب التأكد من أن الـ View ترجع 15 عمودًا أو أقل.
            // هنا نقوم بتعيين رؤوس الأعمدة لتتوافق مع أسماء الأعمدة في الـ View
            if (dataGridView1.Columns.Count > 0)
            {
                dataGridView1.Columns["LeJour"].HeaderText = "التاريخ";
                dataGridView1.Columns["JourSemaine"].HeaderText = "اليوم";
                dataGridView1.Columns["HeureEntree"].HeaderText = "وقت الدخول";
                dataGridView1.Columns["HeureSortie"].HeaderText = "وقت الخروج";
               // dataGridView1.Columns["DureeTravailMinutes"].HeaderText = "مدة العمل (دقيقة)";
                // يمكنك إضافة المزيد من التنسيقات هنا
            }
        }
        private void chargement_donnee_pointage_mensuelle(string matri)
        {
            if (liste_mois.SelectedIndex == -1 ||
        !int.TryParse(txt_annee.Text.Trim(), out int annee) || annee < 1900 || annee > 2100)
            {
                XtraMessageBox.Show("Mois ou année invalide.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            int mois = liste_mois.SelectedIndex + 1;

            Vue_Pointage_mensuelle(matri);

            ///////////////////////
            //insertion des données!

            string horaire_trv;
            int position = 0, nbr_transport, nbr_pannier;
            TimeSpan hr_pointage;
            entet2.RowCount = 0;

            DataRow ligne, ligne_conge, ligne_sanction;

            Boolean existe = false;

            int nbr_jour = DateTime.DaysInMonth(Convert.ToInt32(txt_annee.Text.Trim()), liste_mois.SelectedIndex + 1);

            //charger les jours de mois
            for (int i = 1; i <= nbr_jour; i++)
            {
                entet2.RowCount++;
                entet2.Rows[i - 1].Cells[1].Value = i.ToString();
                

                ///////
                ///2 em methode/////
                ///
                //DateTime d_jour = Convert.ToDateTime(i.ToString("00") + "/" + (liste_mois.SelectedIndex + 1).ToString("00") + "/" + txt_annee.Text);
                DateTime d_jour = new DateTime(annee, mois, i);

                // جلب كود جدول المواعيد
                var result = get_personnel_emploi_code(txt_matricule.Text, d_jour);

                // التحقق: إذا كان الكود فارغاً أو نل، ننتقل لليوم التالي أو نضع قيمة افتراضية
                if (result == null || string.IsNullOrWhiteSpace(result.ToString()))
                {
                    entet2.Rows[i - 1].Cells[0].Value = "غير محدد";
                    entet2.Rows[i - 1].DefaultCellStyle.BackColor = Color.LightGray;
                    continue; // يتجاوز هذا اليوم وينتقل لليوم i+1 لتجنب الخطأ
                }

                horaire_trv = result.ToString().Trim();
                entet2.Rows[i - 1].Cells[0].Value = horaire_trv;

                string s_horaire = "";
                DataTable dt_horaire = get_horaire_travail(horaire_trv);

                // نتحقق أيضاً أن الجدول يحتوي على بيانات قبل محاولة القراءة منه
                if (dt_horaire != null && dt_horaire.Rows.Count > 0)
                {
                    DataRow rowH = dt_horaire.Rows[0];
                    string dayName = d_jour.DayOfWeek.ToString();

                    // استخدام Switch بدلاً من If المتكررة لجعل الكود أنظف وأسرع
                    switch (dayName)
                    {
                        case "Friday":
                            s_horaire = rowH["VEN"].ToString();
                            entet2.Rows[i - 1].DefaultCellStyle.BackColor = SystemColors.ActiveBorder;
                            break;
                        case "Monday":
                            s_horaire = rowH["LUN"].ToString();
                            break;
                        case "Saturday":
                            s_horaire = rowH["SAM"].ToString();
                            entet2.Rows[i - 1].DefaultCellStyle.BackColor = SystemColors.ActiveBorder;
                            break;
                        case "Tuesday":
                            s_horaire = rowH["MAR"].ToString();
                            break;
                        case "Wednesday":
                            s_horaire = rowH["MER"].ToString();
                            break;
                        case "Thursday":
                            s_horaire = rowH["JEU"].ToString();
                            break;
                        case "Sunday":
                            s_horaire = rowH["DIM"].ToString();
                            break;
                    }
                }

                // التحقق من أن s_horaire يحتوي على الفاصلة '-' قبل عمل Split
                hr_entree = TimeSpan.Zero;
                hr_sortie = TimeSpan.Zero;

                if (!string.IsNullOrEmpty(s_horaire) && s_horaire.Contains("-"))
                {
                    string[] partie = s_horaire.Split('-');
                    if (partie.Length >= 2)
                    {
                        if (TimeSpan.TryParse(partie[0].Trim(), out TimeSpan parsedEntree))
                        {
                            hr_entree = parsedEntree;
                            entet2.Rows[i - 1].Cells[3].Value = partie[0].Trim();
                        }
                        if (TimeSpan.TryParse(partie[1].Trim(), out TimeSpan parsedSortie))
                        {
                            hr_sortie = parsedSortie;
                            entet2.Rows[i - 1].Cells[7].Value = partie[1].Trim();
                        }
                    }
                }
                //////////Fin 2em methode




                //pointage journalier
                /*************************************************************************************************************/
                /*************************************************************************************************************/
                /*************************************************************************************************************/

                hr_pointage = TimeSpan.Zero;
                existe = false;
                //chargemenT_POITEUSETableAdapter1.par_matricule(cRM.CHARGEMENT_POITEUSE, matri, d_jour);
                //hb////
                // 1. تعريف الجدول خارجاً
                int nbr_poitage = 0;
                DataTable dtJ = chargement_pointage_journaliere(matri, d_jour, ref nbr_poitage);

                position = 5;                // الخانتان 5 و 6 فقط للتمريرات الوسطية
                bool entreeFaite = false;

                foreach (DataRow pr in dtJ.Rows)
                {
                    if (!TimeSpan.TryParse(pr["heurs_pointage"]?.ToString()?.Trim(), out hr_pointage))
                        continue;

                    if (hr_pointage <= hr_entree)
                    {
                        // دخول في الوقت أو قبله: نحتفظ بأول تمريرة فقط
                        if (!entreeFaite)
                        {
                            entet2.Rows[i - 1].Cells[2].Value = hr_pointage.ToString(@"hh\:mm");
                            entreeFaite = true;
                        }
                    }
                    else if (hr_sortie > TimeSpan.Zero && hr_pointage >= hr_sortie)
                    {
                        // خروج: آخر تمريرة هي التي تبقى
                        entet2.Rows[i - 1].Cells[8].Value = hr_pointage.ToString(@"hh\:mm");

                        double extra = (hr_pointage - hr_sortie).TotalMinutes;
                        entet2.Rows[i - 1].Cells[9].Value = extra >= Form2.plage_hrs
                            ? TimeSpan.FromMinutes(extra).ToString(@"hh\:mm")
                            : null;
                    }
                    else if (hr_entree > TimeSpan.Zero)
                    {
                        // بين الدخول والخروج
                        double retard = (hr_pointage - hr_entree).TotalMinutes;

                        if (!entreeFaite && retard <= Form2.plage_retards)
                        {
                            entet2.Rows[i - 1].Cells[2].Value = hr_pointage.ToString(@"hh\:mm");
                            entet2.Rows[i - 1].Cells[4].Value = retard.ToString("0");
                            entreeFaite = true;
                        }
                        else if (position <= 6)
                        {
                            entet2.Rows[i - 1].Cells[position].Value = hr_pointage.ToString(@"hh\:mm");
                            position++;
                        }
                    }
                }
                //////////////************************HB*************************//////////////////////
                ///
                /// 
                DataTable dtABS = new DataTable();
                string c_conge,lib_conge;

                LoadAbsences(matri, d_jour, dtABS);

                    if (dtABS.Rows.Count > 0)
                    {
                        c_conge = dtABS.Rows[0]["c_conge"].ToString();
                        lib_conge = dtABS.Rows[0]["lib"].ToString();
                        entet2.Rows[i - 1].Cells[10].Value = lib_conge;
                    }

                ///*********************************************************************************************************/
               
            }
        }

       
        private DataTable get_horaire_travail(string code)
        {
            string query = @"SELECT Code, Designation, SAM, DIM, LUN, MAR, MER, JEU, VEN 
                     FROM HORAIRE_TRAVAIL 
                     WHERE code = @code";
            SqlParameter[] parameters = {
        new SqlParameter("@code", SqlDbType.Char, 2) { Value = code }
    };
            return ConnectSqlServer.ExecuteSelect(query, parameters);
        }

        private string get_personnel_emploi_code(string matricule,DateTime date_jr)
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

        private void bt_apercu_Click(object sender, EventArgs e)
        {
            // 1. التحقق من وجود ماتريكول
            string matricule = txt_matricule.Text.Trim();
            if (string.IsNullOrEmpty(matricule))
            {
                MessageBox.Show("الرجاء إدخال الرقم الوظيفي (الماتريكول).", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txt_matricule.Focus();
                return;
            }

            // 2. جلب وعرض بيانات الحضور الشهرية
            chargement_donnee_pointage_mensuelle(matricule);
        }

        private DataTable chargement_pointage_journaliere(string matricule, DateTime date_jr, ref int nb_pointage)
        {
            string query = @"SELECT MATRI, DATE_POINTAGE, HEURS_POINTAGE 
                     FROM CHARGEMENT_POITEUSE 
                     WHERE MATRI = @matricule 
                       AND DATE_POINTAGE = @jour
                     ORDER BY HEURS_POINTAGE";

            SqlParameter[] parameters = {
        new SqlParameter("@matricule", SqlDbType.Char, 5) { Value = matricule },
        new SqlParameter("@jour", SqlDbType.Date) { Value = date_jr.Date }
    };

            DataTable dt = ConnectSqlServer.ExecuteSelect(query, parameters);
            nb_pointage = dt != null ? dt.Rows.Count : 0;
            return dt ?? new DataTable();
        }

        public DataSet ds;
        
        
        public Reports.XtraReport_PointageM XtraReport_PointageM1 = new Reports.XtraReport_PointageM();
        private void bt_imprimer_Click(object sender, EventArgs e)
        {
            Form2.fn_imprission = new Form_imprission();
            ds = new DataSet1();
            ds.Tables["dts_pointage"].Clear();

            // يفضل التأكد من وجود أسطر قبل البدء
            for (int i = 0; i < entet2.Rows.Count; i++)
            {
                // دالة مساعدة لتجنب التكرار والتعامل مع القيم الفارغة
                ds.Tables["dts_pointage"].Rows.Add(
                    GetCellValue(entet2, i, 1),
                    GetCellValue(entet2, i, 2),
                    GetCellValue(entet2, i, 3),
                    GetCellValue(entet2, i, 4),
                    GetCellValue(entet2, i, 5),
                    GetCellValue(entet2, i, 6),
                    GetCellValue(entet2, i, 7),
                    GetCellValue(entet2, i, 8),
                    GetCellValue(entet2, i, 9),
                    GetCellValue(entet2, i, 10),
                    txt_matricule.Text,
                    lb_nom_prenom.Text,
                    liste_mois.SelectedItem.ToString(),
                    txt_annee.Text
                );
            }

            XtraReport_PointageM1.DataSource = ds;
            XtraReport_PointageM1.RequestParameters = false;

            // تأكد من أن Form2 والتابع fn_imprission مهيآن بشكل صحيح
            Form2.fn_imprission.documentViewer1.DocumentSource = XtraReport_PointageM1;
            Form2.fn_imprission.ShowDialog();
        }

        // دالة مساعدة لجعل الكود أنظف
        private string GetCellValue(DataGridView dgv, int rowIndex, int colIndex)
        {
            var value = dgv.Rows[rowIndex].Cells[colIndex].Value;
            // إذا كانت القيمة فارغة، نرجع نقطة أو نص فارغ حسب رغبتك
            return value != null ? value.ToString() : ".";
        }

        private string LoadAbsences(string matricule, DateTime date_jr,DataTable dtABS)
        {
            try
            {
                if (string.IsNullOrEmpty(matricule)) return "";

                // الاستعلام لجلب كود الإجازة بناءً على الموظف والتاريخ
                string query = @"SELECT  COUNT(*) AS count, A.C_CONGE, T.LIB
                                    FROM [ABS] A
                                    INNER JOIN [type_conge] T ON A.C_CONGE = T.CODE
                                    WHERE A.DATE1 = @date_jr 
                                      AND A.MATRI = @matricule
                                    GROUP BY A.C_CONGE, T.LIB";

                SqlParameter[] parameters = {
            new SqlParameter("@date_jr", SqlDbType.Date) { Value = date_jr.Date }, // نستخدم .Date لضمان مقارنة التاريخ فقط بدون الوقت
            new SqlParameter("@matricule", SqlDbType.VarChar) { Value = matricule }
        };

                DataTable dt = ConnectSqlServer.ExecuteSelect(query, parameters);
                dtABS.Merge(dt); // دمج النتائج في DataTable الممررة كوسيط

                // التحقق مما إذا كان هناك نتائج وإرجاع القيمة
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["C_CONGE"].ToString();
                }

                return ""; // إرجاع نص فارغ في حال عدم وجود بيانات
            }
            catch (Exception ex)
            {
                // في حال حدوث خطأ، نعرض الرسالة ونرجع نص فارغ لضمان عمل البرنامج
                MessageBox.Show("Erreur lors du chargement : " + ex.Message);
                return "";
            }
        }
    }
}