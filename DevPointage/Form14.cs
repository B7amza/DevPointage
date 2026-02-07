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

                // 2. منادات الداله txt_matricule_Leave
                txt_matricule_Leave(sender, e);

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
                    HeureSortie, 
                    DureeTravailMinutes,
                    Col_6_StatutRetard,
                    Col_7_HeureTheoriqueEntree,
                    Col_8_HeureTheoriqueSortie,
                    Col_9_HeuresSupplementaires,
                    Col_10_HeuresAbsence,
                    Col_11_MotifAbsence,
                    Col_12_ValidationChef,
                    Col_13_Commentaire,
                    Col_14_ChampLibre1,
                    Col_15_ChampLibre2
                FROM 
                    Vue_Pointage_mensuelle 
                WHERE 
                    MATRI = @matri AND 
                    MONTH(LeJour) = @mois AND 
                    YEAR(LeJour) = @annee
                ORDER BY 
                    LeJour";

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
                dataGridView1.Columns["DureeTravailMinutes"].HeaderText = "مدة العمل (دقيقة)";
                // يمكنك إضافة المزيد من التنسيقات هنا
            }
        }
        private void chargement_donnee_pointage_mensuelle(string matri)
        {
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
                //DateTime d_jour = Convert.ToDateTime(i.ToString("00") + "/" + (liste_mois.SelectedIndex + 1).ToString("00") + "/" + txt_annee.Text);
                ////charger l'emploi des temps
                ////horaire_trv.SelectedValue = pERSONNEL_EMPLOI_TEMPSTableAdapter.par_matri_date(d_jour, matri).ToString();
                //horaire_trv = get_personnel_emploi_code(txt_matricule.Text,d_jour).ToString();
                //entet2.Rows[i - 1].Cells[0].Value = get_personnel_emploi_code(txt_matricule.Text, d_jour).ToString().Trim();
                //string s_horaire = "";
                ////DataTable dt_horaire = get_horaire_travail(horaire_trv);
                ////DataRow row_h = dt_horaire.Rows[0];

                //if (d_jour.DayOfWeek.ToString() == Day.Friday.ToString())
                //{
                //    s_horaire = get_horaire_travail(horaire_trv).Rows[0]["VEN"].ToString();
                //    entet2.Rows[i - 1].DefaultCellStyle.BackColor = SystemColors.ActiveBorder;

                //}
                //if (d_jour.DayOfWeek.ToString() == Day.Monday.ToString())
                //{
                //    s_horaire = get_horaire_travail(horaire_trv).Rows[0]["LUN"].ToString();
                //}

                //if (d_jour.DayOfWeek.ToString() == Day.Saturday.ToString())
                //{
                //    s_horaire = get_horaire_travail(horaire_trv).Rows[0]["SAM"].ToString();
                //    entet2.Rows[i - 1].DefaultCellStyle.BackColor = SystemColors.ActiveBorder;
                //}
                //if (d_jour.DayOfWeek.ToString() == Day.Tuesday.ToString())
                //{
                //    s_horaire = get_horaire_travail(horaire_trv).Rows[0]["MAR"].ToString();
                //}

                //if (d_jour.DayOfWeek.ToString() == Day.Wednesday.ToString())
                //{
                //    s_horaire = get_horaire_travail(horaire_trv).Rows[0]["MER"].ToString();
                //}

                //if (d_jour.DayOfWeek.ToString() == Day.Thursday.ToString())
                //{
                //    s_horaire = get_horaire_travail(horaire_trv).Rows[0]["JEU"].ToString();
                //}

                //if (d_jour.DayOfWeek.ToString() == Day.Sunday.ToString())
                //{

                //    s_horaire = get_horaire_travail(horaire_trv).Rows[0]["DIM"].ToString();
                //}
                //string[] partie = s_horaire.Split('-');
                //entet2.Rows[i - 1].Cells[3].Value = partie[0];
                //entet2.Rows[i - 1].Cells[7].Value = partie[1];
                //hr_entree = TimeSpan.Parse(partie[0]);
                //hr_sortie = TimeSpan.Parse(partie[1]);

                ///////
                ///2 em methode/////
                ///
                DateTime d_jour = Convert.ToDateTime(i.ToString("00") + "/" + (liste_mois.SelectedIndex + 1).ToString("00") + "/" + txt_annee.Text);

                // جلب كود جدول المواعيد
                var result = get_personnel_emploi_code(txt_matricule.Text, d_jour);

                // التحقق: إذا كان الكود فارغاً أو نل، ننتقل لليوم التالي أو نضع قيمة افتراضية
                if (result == null || string.IsNullOrWhiteSpace(result.ToString()))
                {
                    entet2.Rows[i - 1].Cells[0].Value = "غير محدد";
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
                if (!string.IsNullOrEmpty(s_horaire) && s_horaire.Contains("-"))
                {
                    string[] partie = s_horaire.Split('-');
                    entet2.Rows[i - 1].Cells[3].Value = partie[0];
                    entet2.Rows[i - 1].Cells[7].Value = partie[1];
                    hr_entree = TimeSpan.Parse(partie[0]);
                    hr_sortie = TimeSpan.Parse(partie[1]);
                }
                else
                {
                    // تعيين قيم افتراضية في حال عدم وجود توقيت لهذا اليوم
                    hr_entree = TimeSpan.Zero;
                    hr_sortie = TimeSpan.Zero;
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
                DataTable dtJ = new DataTable();
                int nbr_poitage = 0;

                // 2. تمرير المتغير للدالة
                chargement_pointage_journaliere(matri, d_jour, nbr_poitage, dtJ);

                // 3. الآن يمكنك الوصول للبيانات الموجودة في dtJ هنا بكل سهولة
                if (dtJ.Rows.Count > 0)
                {
                    //entet2.Rows[i - 1].Cells[2].Value = dtJ.Rows[0][3].ToString();
                    nbr_poitage =Convert.ToInt32(dtJ.Rows[0][0].ToString());
                }
                //int nbr_poitage = chargemenT_POITEUSETableAdapter1.GetDataBy2(matri, d_jour).Count;
                position = 5;

                for (int j = 0; j < nbr_poitage; j++)
                {

                    hr_pointage = TimeSpan.Parse(dtJ.Rows[j]["heurs_pointage"].ToString());

                    //1er pointgae entree
                    if (hr_pointage <= hr_entree)
                        entet2.Rows[i - 1].Cells[2].Value = hr_pointage;
                    else
                        //2eme pointage sortie
                        if (hr_pointage >= hr_sortie)
                    {
                        entet2.Rows[i - 1].Cells[8].Value = hr_pointage;
                        if ((hr_pointage - hr_sortie).Minutes >= Form2.plage_hrs)
                        {
                            //heurs sup
                            entet2.Rows[i - 1].Cells[9].Value = TimeSpan.FromMinutes((hr_pointage.TotalMinutes - hr_sortie.TotalMinutes)).ToString();
                        }
                    }

                    else

                                if ((hr_pointage >= hr_entree) & (hr_pointage <= hr_sortie))
                    {

                        if ((hr_pointage - hr_entree).TotalMinutes <= Form2.plage_retards)
                        {
                            //3eme pointage entree retard
                            entet2.Rows[i - 1].Cells[2].Value = hr_pointage;
                            entet2.Rows[i - 1].Cells[4].Value = (hr_pointage - hr_entree).TotalMinutes.ToString();
                        }
                        else
                        {
                            //recherche bon_sortie                                            
                            entet2.Rows[i - 1].Cells[position].Value = hr_pointage;
                            position++;
                        }
                    }

                }
                ///*********************************************************************************************************/
                //if (etat_pointage_mensuelleTableAdapter.GetData(matri, d_jour).Count > 0)
                //{
                //    entet2.Rows[i - 1].Cells[10].Value = etat_pointage_mensuelleTableAdapter.GetData(matri, d_jour).Rows[0]["LIB"].ToString();
                //}
                //if (entet2.Rows[i - 1].Cells[8].Value != null)
                //{
                //    //entet2.Rows[i - 1].Cells[12].Value = "0";
                //    //entet2.Rows[i - 1].Cells[13].Value="0";
                //    //entet2.Rows[i - 1].Cells[12].Value = "0";
                //    TimeSpan s;
                //    s = TimeSpan.Parse(entet2.Rows[i - 1].Cells[8].Value.ToString());

                //    TimeSpan e;
                //    if (entet2.Rows[i - 1].Cells[2].Value != null)
                //    {
                //        e = TimeSpan.Parse(entet2.Rows[i - 1].Cells[2].Value.ToString());
                //        entet2.Rows[i - 1].Cells[12].Value = Math.Round((s - e).TotalHours, 2).ToString();
                //        entet2.Rows[i - 1].Cells[13].Value = personnelTableAdapter1.GetDataBy19(matricule.Text).Rows[0]["NUM_CPT"].ToString();
                //        entet2.Rows[i - 1].Cells[14].Value = Convert.ToDecimal(entet2.Rows[i - 1].Cells[12].Value) * Convert.ToDecimal(entet2.Rows[i - 1].Cells[13].Value);
                //    }
                //    else
                //    {
                //        if (entet2.Rows[i - 1].Cells[5].Value != null)
                //        {
                //            e = TimeSpan.Parse(entet2.Rows[i - 1].Cells[5].Value.ToString());

                //            entet2.Rows[i - 1].Cells[12].Value = Math.Round((s - e).TotalHours, 2).ToString();
                //            entet2.Rows[i - 1].Cells[13].Value = personnelTableAdapter1.GetDataBy19(matricule.Text).Rows[0]["NUM_CPT"].ToString();
                //            entet2.Rows[i - 1].Cells[14].Value = Convert.ToDecimal(entet2.Rows[i - 1].Cells[12].Value) * Convert.ToDecimal(entet2.Rows[i - 1].Cells[13].Value);
                //        }
                //        else
                //        {
                //            entet2.Rows[i - 1].Cells[12].Value = "0";
                //            entet2.Rows[i - 1].Cells[13].Value = personnelTableAdapter1.GetDataBy19(matricule.Text).Rows[0]["NUM_CPT"].ToString();
                //            entet2.Rows[i - 1].Cells[14].Value = Convert.ToDecimal(entet2.Rows[i - 1].Cells[12].Value) * Convert.ToDecimal(entet2.Rows[i - 1].Cells[13].Value);
                //        }

                //    }


                //}
                //else  // modificaion hamza du 02/12/2023
                //{
                //    if (entet2.Rows[i - 1].Cells[6].Value != null)
                //    {
                //        TimeSpan s;
                //        s = TimeSpan.Parse(entet2.Rows[i - 1].Cells[6].Value.ToString());

                //        TimeSpan e;
                //        if (entet2.Rows[i - 1].Cells[2].Value != null)
                //        {
                //            e = TimeSpan.Parse(entet2.Rows[i - 1].Cells[2].Value.ToString());
                //            entet2.Rows[i - 1].Cells[12].Value = Math.Round((s - e).TotalHours, 2).ToString();
                //            entet2.Rows[i - 1].Cells[13].Value = personnelTableAdapter1.GetDataBy19(matricule.Text).Rows[0]["NUM_CPT"].ToString();
                //            entet2.Rows[i - 1].Cells[14].Value = Convert.ToDecimal(entet2.Rows[i - 1].Cells[12].Value) * Convert.ToDecimal(entet2.Rows[i - 1].Cells[13].Value);
                //        }
                //        else
                //        {
                //            if (entet2.Rows[i - 1].Cells[5].Value != null)
                //            {
                //                e = TimeSpan.Parse(entet2.Rows[i - 1].Cells[5].Value.ToString());

                //                entet2.Rows[i - 1].Cells[12].Value = Math.Round((s - e).TotalHours, 2).ToString();
                //                entet2.Rows[i - 1].Cells[13].Value = personnelTableAdapter1.GetDataBy19(matricule.Text).Rows[0]["NUM_CPT"].ToString();
                //                entet2.Rows[i - 1].Cells[14].Value = Convert.ToDecimal(entet2.Rows[i - 1].Cells[12].Value) * Convert.ToDecimal(entet2.Rows[i - 1].Cells[13].Value);
                //            }
                //            else
                //            {
                //                entet2.Rows[i - 1].Cells[12].Value = "0";
                //                entet2.Rows[i - 1].Cells[13].Value = personnelTableAdapter1.GetDataBy19(matricule.Text).Rows[0]["NUM_CPT"].ToString();
                //                entet2.Rows[i - 1].Cells[14].Value = Convert.ToDecimal(entet2.Rows[i - 1].Cells[12].Value) * Convert.ToDecimal(entet2.Rows[i - 1].Cells[13].Value);
                //            }

                //        }
                //    }
                //    else
                //    {
                //        if (entet2.Rows[i - 1].Cells[5].Value != null)
                //        {
                //            TimeSpan s;
                //            s = TimeSpan.Parse(entet2.Rows[i - 1].Cells[5].Value.ToString());

                //            TimeSpan e;
                //            if (entet2.Rows[i - 1].Cells[2].Value != null)
                //            {
                //                e = TimeSpan.Parse(entet2.Rows[i - 1].Cells[2].Value.ToString());
                //                entet2.Rows[i - 1].Cells[12].Value = Math.Round((s - e).TotalHours, 2).ToString();
                //                entet2.Rows[i - 1].Cells[13].Value = personnelTableAdapter1.GetDataBy19(matricule.Text).Rows[0]["NUM_CPT"].ToString();
                //                entet2.Rows[i - 1].Cells[14].Value = Convert.ToDecimal(entet2.Rows[i - 1].Cells[12].Value) * Convert.ToDecimal(entet2.Rows[i - 1].Cells[13].Value);
                //            }
                //            else
                //            {
                //                entet2.Rows[i - 1].Cells[12].Value = "0";
                //                entet2.Rows[i - 1].Cells[13].Value = personnelTableAdapter1.GetDataBy19(matricule.Text).Rows[0]["NUM_CPT"].ToString();
                //                entet2.Rows[i - 1].Cells[14].Value = Convert.ToDecimal(entet2.Rows[i - 1].Cells[12].Value) * Convert.ToDecimal(entet2.Rows[i - 1].Cells[13].Value);
                //            }
                //        }
                //    }
                //}
            }
        }

        //    private void load_personnel_emploi_temps_data(DateTime date_jr)
        //    {
        //        // 1. التأكد من وجود نص في خانة الماتريكول قبل البحث
        //        string matricule = txt_matricule.Text.Trim();

        //        if (string.IsNullOrEmpty(matricule)) return;

        //        // 2. تعريف الاستعلام
        //        // استخدام BETWEEN أو المقارنة المباشرة صحيح، ولكن يفضل التأكد من تنسيق التاريخ
        //        string query = @"SELECT matri, code, date_du, date_au 
        //                 FROM Personnel_emploi_temps 
        //                 WHERE matri = @matricule 
        //                 AND @date_jr BETWEEN date_du AND date_au";

        //        // 3. تجهيز البارامترات
        //        SqlParameter[] parameters = {
        //    new SqlParameter("@matricule", SqlDbType.Char, 5) { Value = matricule }, // تحديد النوع والطول بدقة
        //    new SqlParameter("@date_jr", SqlDbType.Date) { Value = date_jr.Date } // إرسال التاريخ فقط بدون الوقت
        //};

        //        // 4. استخدام ExecuteSelect من الكلاس الخاص بك (يغنيك عن SqlConnection و DataAdapter)
        //        DataTable dt = ConnectSqlServer.ExecuteSelect(query, parameters);

        //        // 5. ربط النتيجة 
        //        return dt.Rows.Count > 0 ? dt.Rows[0]["code"].ToString() : "";
        //    }
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

        private string chargement_pointage_journaliere(string matricule, DateTime date_jr, int nb_pointage,DataTable dtJ)
        {
            // مقارنة التاريخ بطريقة فعّالة (بدون دوال على العمود لتمكين استخدام الفهارس)
            string query = @"SELECT 
                        COUNT(*) OVER() AS total_count,
                        p.MATRI, 
                        p.DATE_POINTAGE, 
                        p.HEURS_POINTAGE 
                     FROM CHARGEMENT_POITEUSE AS p  
                     WHERE p.MATRI = @matricule 
                       AND p.DATE_POINTAGE >= @date_debut 
                       AND p.DATE_POINTAGE < @date_fin";

            SqlParameter[] parameters = {
        new SqlParameter("@matricule", SqlDbType.Char, 5) { Value = matricule },
        new SqlParameter("@date_debut", SqlDbType.DateTime2) { Value = date_jr.Date },
        new SqlParameter("@date_fin", SqlDbType.DateTime2) { Value = date_jr.Date.AddDays(1) }
    };

            DataTable dt = ConnectSqlServer.ExecuteSelect(query, parameters);
            dtJ.Merge(dt); // دمج النتائج في DataTable الممررة كوسيط
            // إرجاع "0" عند عدم وجود سجلات (أكثر منطقية من سلسلة فارغة للعد)
            return dt.Rows.Count > 0
                ? dt.Rows[0]["total_count"].ToString()
                : "0";
        }
    }
}