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
    public partial class Form17 : DevExpress.XtraEditors.XtraForm
    {
        public Form17()
        {
            InitializeComponent();
            ConnectSqlServer.Initialized();
        }
        public TimeSpan hr_entree, hr_sortie;
        private void Form17_Load(object sender, EventArgs e)
        {
            LoadHoraire_travail();
            load_affectation_data();
            cb_structure.SelectedIndex = -1;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void labelControl2_Click(object sender, EventArgs e)
        {

        }

        private void groupControl1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void ch_tous_CheckedChanged(object sender, EventArgs e)
        {
            if (ch_tous.Checked)
            {
                cb_structure.Enabled = false;
                txt_matricule.Enabled = false;
                lb_nom_prenom.Visible = false;

                cb_structure.SelectedIndex = -1;
                txt_matricule.Text = "";

                ch_structure.Enabled = false;
                ch_matricule.Enabled = false;

                ch_structure.Checked = false;
                ch_matricule.Checked = false;

            }
             else
            {
                cb_structure.Enabled = true;
                txt_matricule.Enabled = true;
                lb_nom_prenom.Visible = true;

                cb_structure.SelectedIndex = -1;
                txt_matricule.Text = "";

                ch_structure.Enabled = true;
                ch_matricule.Enabled = true;

                ch_structure.Checked = false;
                ch_matricule.Checked = false;
            }
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

        private void txt_matricule_Leave(object sender, EventArgs e)
        {
            load_personnel_details();
            //vider le datagridview 
            tableau.DataSource = null;
            tableau.Rows.Clear();
            tableau.Refresh();
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

        private void load_affectation_data()
        {
            
                string query = "SELECT c_affect, lib_affect FROM Affectation";
               


                        DataTable dt = ConnectSqlServer.ExecuteSelect(query);
            

                        cb_structure.DisplayMember = "lib_affect"; // ما يظهر للمستخدم
                        cb_structure.ValueMember = "c_affect";     // القيمة الداخلية
                        cb_structure.DataSource = dt;


            

        }

        private void cb_structure_SelectedIndexChanged(object sender, EventArgs e)
        {
           // lb_direction.Text = cb_structure.Text;
        }

        private void ch_structure_CheckedChanged(object sender, EventArgs e)
        {
            if (ch_structure.Checked)
            {
                cb_structure.Enabled = true;
                txt_matricule.Enabled = false;
                lb_nom_prenom.Visible = false;
                cb_structure.SelectedIndex = -1;
                txt_matricule.Text = "";

                ch_tous.Checked = false;
                ch_tous.Enabled = false;

                ch_matricule.Checked = false;
                ch_matricule.Enabled = false;

            }
            else
            {
                cb_structure.Enabled = false;
                txt_matricule.Enabled = true;
                lb_nom_prenom.Visible = true;
                cb_structure.SelectedIndex = -1;
                txt_matricule.Text = "";

                ch_tous.Checked = false;
                ch_tous.Enabled = true;

                ch_matricule.Checked = false;
                ch_matricule.Enabled = true;
            }
        }

        private void ch_matricule_CheckedChanged(object sender, EventArgs e)
        {
            if (ch_matricule.Checked)
            {
                cb_structure.Enabled = false;
                txt_matricule.Enabled = true;
                lb_nom_prenom.Visible = true;
                cb_structure.SelectedIndex = -1;
                txt_matricule.Text = "";

                ch_tous.Checked = false;
                ch_tous.Enabled = false;

                ch_structure.Checked = false;
                ch_structure.Enabled = false;

            }
            else
            {
                cb_structure.Enabled = true;
                txt_matricule.Enabled = false;
                lb_nom_prenom.Visible = false;
                cb_structure.SelectedIndex = -1;
                txt_matricule.Text = "";

                ch_tous.Checked = false;
                ch_tous.Enabled = true;

                ch_structure.Checked = false;
                ch_structure.Enabled = true;
            }
        }
       

        private void bt_apercu_Click(object sender, EventArgs e)
        {
            if (ch_structure.Checked)
            {
                tableau.DataSource = null;
                if (cb_structure.SelectedIndex == -1) return;
                string c_affect = cb_structure.SelectedValue.ToString();
                string query = "SELECT matricule,CONCAT(ISNULL(nom,''), ' ', ISNULL(prenom,'')) as nom FROM Personnel WHERE id_affectation = @c_affect";
                SqlParameter[] parameters = {
              new SqlParameter("@c_affect", c_affect)
                                        };
                DataTable dt = ConnectSqlServer.ExecuteSelect(query, parameters);

                tableau.DataSource = dt;
                if (tableau.Columns["nom"] != null)
                {
                    tableau.Columns["nom"].Width = 250; // جرب رفع القيمة حتى يظهر الاسم كاملاً
                    tableau.Columns["nom"].HeaderText = "الاسم واللقب"; // اختياري لتغيير العنوان
                }
            }
            else if (ch_matricule.Checked)
            {
                tableau.DataSource = null;
                if (string.IsNullOrEmpty(txt_matricule.Text.Trim()))
                {
                    MessageBox.Show("الرجاء إدخال رقم المستخدم", "تنبيه", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                string query = "SELECT matricule,CONCAT(ISNULL(nom,''), ' ', ISNULL(prenom,'')) as nom FROM Personnel where matricule=@matricule";
                SqlParameter[] parameters = {
              new SqlParameter("@matricule", txt_matricule.Text) };
                DataTable dt = ConnectSqlServer.ExecuteSelect(query,parameters);
                tableau.DataSource = dt;
                if (tableau.Columns["nom"] != null)
                {
                    tableau.Columns["nom"].Width = 250; // جرب رفع القيمة حتى يظهر الاسم كاملاً
                    tableau.Columns["nom"].HeaderText = "الاسم واللقب"; // اختياري لتغيير العنوان
                }
            }
            else if (ch_tous.Checked)
            {
                tableau.DataSource = null;
                string query = "SELECT matricule,CONCAT(ISNULL(nom,''), ' ', ISNULL(prenom,'')) as nom FROM Personnel";
                DataTable dt = ConnectSqlServer.ExecuteSelect(query);
                tableau.DataSource = dt;
                if (tableau.Columns["nom"] != null)
                {
                    tableau.Columns["nom"].Width = 250; // جرب رفع القيمة حتى يظهر الاسم كاملاً
                    tableau.Columns["nom"].HeaderText = "الاسم واللقب"; // اختياري لتغيير العنوان
                }
            }

            int position = 0, nbr_transport, nbr_pannier;
            TimeSpan hr_pointage;
            entet2.RowCount = 0;

            DataRow ligne, ligne_conge, ligne_sanction;

            Boolean existe = false;



            //insertion des données!


            for (int i = 0; i < tableau.RowCount; i++)
            {
                hr_pointage = TimeSpan.Zero;

                entet2.RowCount++;
                existe = false;
                

                //hb////
                // 1. تعريف الجدول خارجاً
                DataTable dtJ = new DataTable();
                int nbr_poitage = 0;

                // 2. تمرير المتغير للدالة
                chargement_pointage_journaliere(tableau.Rows[i].Cells[0].Value.ToString(), date_jour.Value.Date, nbr_poitage, dtJ);

                // 3. الآن يمكنك الوصول للبيانات الموجودة في dtJ هنا بكل سهولة
                if (dtJ.Rows.Count > 0)
                {
                    
                    nbr_poitage = Convert.ToInt32(dtJ.Rows[0][0].ToString());
                }
                
                position = 3;

                for (int j = 0; j < nbr_poitage; j++)
                {

                    hr_pointage = TimeSpan.Parse(dtJ.Rows[j]["heurs_pointage"].ToString());

                    //1er pointgae entree
                    if (hr_pointage <= hr_entree)
                        entet2.Rows[entet2.RowCount - 1].Cells[0].Value = hr_pointage;
                    else
                        //2eme pointage sortie
                        if (hr_pointage >= hr_sortie)
                    {
                        entet2.Rows[entet2.RowCount - 1].Cells[6].Value = hr_pointage;
                        if ((hr_pointage - hr_sortie).Minutes >= Form2.plage_hrs)
                        {
                            //heurs sup
                            entet2.Rows[entet2.RowCount - 1].Cells[7].Value = TimeSpan.FromMinutes((hr_pointage.TotalMinutes - hr_sortie.TotalMinutes)).ToString();
                        }
                    }

                    else

                            if ((hr_pointage >= hr_entree) & (hr_pointage <= hr_sortie))
                    {

                        if ((hr_pointage - hr_entree).TotalMinutes <= Form2.plage_retards)
                        {
                            //3eme pointage entree retard
                            entet2.Rows[entet2.RowCount - 1].Cells[0].Value = hr_pointage;
                            entet2.Rows[entet2.RowCount - 1].Cells[2].Value = (hr_pointage - hr_entree).TotalMinutes.ToString();
                        }
                        else
                        {
                            //recherche bon_sortie                                            
                            entet2.Rows[entet2.RowCount - 1].Cells[position].Value = hr_pointage;
                            position++;
                        }
                    }

                }

                //vérifier les congés                        
                //////////////************************HB*************************//////////////////////
                ///
                /// 
                DataTable dtABS = new DataTable();
                string c_conge, lib_conge;
                LoadAbsences(tableau.Rows[i].Cells[0].Value.ToString(), date_jour.Value.Date, dtABS);
                //LoadAbsences(matricu, d_jour, dtABS);

                if (dtABS.Rows.Count > 0)
                {
                    c_conge = dtABS.Rows[0]["c_conge"].ToString();
                    lib_conge = dtABS.Rows[0]["lib"].ToString();
                    entet2.Rows[i - 1].Cells[8].Value = lib_conge;
                }

                ///*********************************************************************************************************/

            }


        }
        private string LoadAbsences(string matricule, DateTime date_jr, DataTable dtABS)
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

        private void chargement_donnee_pointage(string matri)
        {
           

            ///////////////////////
            //insertion des données!

            string horaire_trv;
            int position = 0, nbr_transport, nbr_pannier;
            TimeSpan hr_pointage;
            entet2.RowCount = 0;

            DataRow ligne, ligne_conge, ligne_sanction;

            Boolean existe = false;

            //int nbr_jour = DateTime.DaysInMonth(Convert.ToInt32(txt_annee.Text.Trim()), liste_mois.SelectedIndex + 1);
            int nbr_personnel=tableau.Rows.Count;
            //charger les jours de mois
            for (int i = 1; i <= nbr_personnel; i++)
            {
                entet2.RowCount++;
               string matricule = tableau.Rows[i - 1].Cells[0].Value.ToString();

                DateTime d_jour = date_jour.Value.Date.AddDays(i - 1); // الحصول على تاريخ اليوم i من الشهر المحدد


                









                //pointage journalier
                /*************************************************************************************************************/
                /*************************************************************************************************************/
                /*************************************************************************************************************/

                //hr_pointage = TimeSpan.Zero;
                //existe = false;
                ////chargemenT_POITEUSETableAdapter1.par_matricule(cRM.CHARGEMENT_POITEUSE, matri, d_jour);
                ////hb////
                //// 1. تعريف الجدول خارجاً
                //DataTable dtJ = new DataTable();
                //int nbr_poitage = 0;

                //// 2. تمرير المتغير للدالة
                //chargement_pointage_journaliere(matri, d_jour, nbr_poitage, dtJ);

                //// 3. الآن يمكنك الوصول للبيانات الموجودة في dtJ هنا بكل سهولة
                //if (dtJ.Rows.Count > 0)
                //{
                //    //entet2.Rows[i - 1].Cells[2].Value = dtJ.Rows[0][3].ToString();
                //    nbr_poitage = Convert.ToInt32(dtJ.Rows[0][0].ToString());
                //}
                ////int nbr_poitage = chargemenT_POITEUSETableAdapter1.GetDataBy2(matri, d_jour).Count;
                //position = 5;

                //for (int j = 0; j < nbr_poitage; j++)
                //{

                //    hr_pointage = TimeSpan.Parse(dtJ.Rows[j]["heurs_pointage"].ToString());

                //    //1er pointgae entree
                //    if (hr_pointage <= hr_entree)
                //        entet2.Rows[i - 1].Cells[2].Value = hr_pointage;
                //    else
                //        //2eme pointage sortie
                //        if (hr_pointage >= hr_sortie)
                //    {
                //        entet2.Rows[i - 1].Cells[8].Value = hr_pointage;
                //        if ((hr_pointage - hr_sortie).Minutes >= Form2.plage_hrs)
                //        {
                //            //heurs sup
                //            entet2.Rows[i - 1].Cells[9].Value = TimeSpan.FromMinutes((hr_pointage.TotalMinutes - hr_sortie.TotalMinutes)).ToString();
                //        }
                //    }

                //    else

                //                if ((hr_pointage >= hr_entree) & (hr_pointage <= hr_sortie))
                //    {

                //        if ((hr_pointage - hr_entree).TotalMinutes <= Form2.plage_retards)
                //        {
                //            //3eme pointage entree retard
                //            entet2.Rows[i - 1].Cells[2].Value = hr_pointage;
                //            entet2.Rows[i - 1].Cells[4].Value = (hr_pointage - hr_entree).TotalMinutes.ToString();
                //        }
                //        else
                //        {
                //            //recherche bon_sortie                                            
                //            entet2.Rows[i - 1].Cells[position].Value = hr_pointage;
                //            position++;
                //        }
                //    }

                //}
                ////////////////************************HB*************************//////////////////////
                /////
                ///// 
                //DataTable dtABS = new DataTable();
                //string c_conge, lib_conge;

                //LoadAbsences(matri, d_jour, dtABS);

                //if (dtABS.Rows.Count > 0)
                //{
                //    c_conge = dtABS.Rows[0]["c_conge"].ToString();
                //    lib_conge = dtABS.Rows[0]["lib"].ToString();
                //    entet2.Rows[i - 1].Cells[10].Value = lib_conge;
                //}

                
            }
        }
        private void LoadHoraire_travail()
        {
            string query = "SELECT code,designation FROM HORAIRE_TRAVAIL ";
            
            DataTable dt = ConnectSqlServer.ExecuteSelect(query);
            cb_horaire_travail.DisplayMember = "designation"; // ما يظهر للمستخدم
            cb_horaire_travail.ValueMember = "code";     // القيمة الداخلية
            cb_horaire_travail.DataSource = dt;
        }

        private void cb_horaire_travail_SelectedIndexChanged(object sender, EventArgs e)
        {
            chargement_horaire_travail();
        }

        private void entet2_Scroll(object sender, ScrollEventArgs e)
        {
            
            tableau.FirstDisplayedScrollingRowIndex = e.NewValue;
        }

        private void entet2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            tableau.Rows[entet2.CurrentRow.Index].Selected = true;
        }

        private void tableau_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            entet2.CurrentCell = entet2.Rows[tableau.CurrentRow.Index].Cells[0];
            entet2.Rows[tableau.CurrentRow.Index].Selected = true;
        }

        private void tableau_Scroll(object sender, ScrollEventArgs e)
        {
            entet2.FirstDisplayedScrollingRowIndex = e.NewValue;
        }

        public DataSet ds;


        public Reports.XtraReport_PointageJR XtraReport_PointageJR1 = new Reports.XtraReport_PointageJR();
        private void bt_imprimer_Click(object sender, EventArgs e)
        {
            Form2.fn_imprission = new Form_imprission();
            ds = new DataSet1();
            ds.Tables["dts_pointage"].Clear();

            // يفضل التأكد من وجود أسطر قبل البدء
            for (int i = 0; i < entet2.RowCount; i++)
            {
                // دالة مساعدة لتجنب التكرار والتعامل مع القيم الفارغة
                ds.Tables["dts_pointage"].Rows.Add(
                    GetCellValue(entet2, i, 0),
                    GetCellValue(entet2, i, 1),
                    GetCellValue(entet2, i, 2),
                    GetCellValue(entet2, i, 3),
                    GetCellValue(entet2, i, 4),
                    GetCellValue(entet2, i, 5),
                    GetCellValue(entet2, i, 6),
                    GetCellValue(entet2, i, 7),
                    GetCellValue(entet2, i, 8),
                    cb_structure.Text,
                    GetCellValue(tableau, i, 0),
                    GetCellValue(tableau, i, 1),
                    date_jour.Value.ToString("d"),
                    date_jour.Value.ToString("MMMM", new System.Globalization.CultureInfo("ar-AR"))
                );
            }

            XtraReport_PointageJR1.DataSource = ds;
            XtraReport_PointageJR1.RequestParameters = false;

            // تأكد من أن Form2 والتابع fn_imprission مهيآن بشكل صحيح
            Form2.fn_imprission.documentViewer1.DocumentSource = XtraReport_PointageJR1;
            Form2.fn_imprission.ShowDialog();
        }

        // دالة مساعدة لجعل الكود أنظف
        private string GetCellValue(DataGridView dgv, int rowIndex, int colIndex)
        {
            var value = dgv.Rows[rowIndex].Cells[colIndex].Value;
            // إذا كانت القيمة فارغة، نرجع نقطة أو نص فارغ حسب رغبتك
            return value != null ? value.ToString() : ".";
        }

        private void chargement_horaire_travail()
        {
            string horaire_trv = cb_horaire_travail.SelectedValue != null ? cb_horaire_travail.SelectedValue.ToString() : string.Empty;


            string s_horaire = "";
            string query = "SELECT Code, Designation, SAM, DIM, LUN, MAR, MER, JEU, VEN FROM HORAIRE_TRAVAIL WHERE code = @code";
            SqlParameter[] parameters = {
                    new SqlParameter("@code", SqlDbType.Char, 5) { Value = horaire_trv }
                };
            DataTable dt_horaire = ConnectSqlServer.ExecuteSelect(query, parameters);



            // نتحقق أيضاً أن الجدول يحتوي على بيانات قبل محاولة القراءة منه
            if (dt_horaire != null && dt_horaire.Rows.Count > 0)
            {
                DataRow rowH = dt_horaire.Rows[0];
                string dayName = date_jour.Value.DayOfWeek.ToString();

                // استخدام Switch بدلاً من If المتكررة لجعل الكود أنظف وأسرع
                switch (dayName)
                {
                    case "Friday":
                        s_horaire = rowH["VEN"].ToString();
                        // entet2.Rows[i - 1].DefaultCellStyle.BackColor = SystemColors.ActiveBorder;
                        break;
                    case "Monday":
                        s_horaire = rowH["LUN"].ToString();
                        break;
                    case "Saturday":
                        s_horaire = rowH["SAM"].ToString();
                        // entet2.Rows[i - 1].DefaultCellStyle.BackColor = SystemColors.ActiveBorder;
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
                // تعيين قيم التوقيت في DataGridView : entet2

                entet2.Columns[1].HeaderText = partie[0];
                entet2.Columns[5].HeaderText = partie[1];
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
            }
        }
    }