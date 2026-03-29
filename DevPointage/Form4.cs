using DevExpress.XtraEditors;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using zkemkeeper;

namespace DevPointage
{
    public partial class Form4 : XtraForm
    {
        public Form4()
        {
            InitializeComponent();
            ConnectSqlServer.Initialized();
        }
        CZKEM zkDevice = new CZKEM(); // كائن الاتصال بالجهاز
        private void Form4_Load(object sender, EventArgs e)
        {
            try
            {
                // تهيئة إعدادات الاتصال من الريجستري
                ConnectSqlServer.Initialized();

                //XtraMessageBox.Show($"Base de données active : {ConnectSqlServer.DATABASE_NAME}");

                // الاستعلام عن جدول pointeuse
                string query = "SELECT * FROM POINTEUSE where etat='O' ORDER BY ADRESSE_IP";

                // جلب البيانات في DataTable
                DataTable dt = ConnectSqlServer.ExecuteSelect(query);

                // ربطها بالـ DataGridView أو GridControl
                tableau.DataSource = dt;

                // تنسيقات اختيارية
               // tableau.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                tableau.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

                //tableau.ReadOnly = true;                    // ✅ منع التعديل المباشر
                tableau.AllowUserToAddRows = false;         // ✅ إخفاء الصف الجديد الفارغ
                tableau.AllowUserToDeleteRows = false;      // ✅ منع الحذف المباشر
                tableau.MultiSelect = false;                // ✅ تحديد صف واحد فقط
                tableau.BackgroundColor = Color.White;      // ✅ لون الخلفية

                // ✅ تنسيق رؤوس الأعمدة
                tableau.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(0, 114, 198); // DevExpress Blue
                tableau.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
                tableau.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                tableau.EnableHeadersVisualStyles = false;  // ✅ لتطبيق الألوان المخصصة

                // ✅ تنسيق الصفوف (Alternating Rows)
                tableau.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);

                tableau.Columns[0].Width = 50;   // ✅ ضبط عرض عمود ID (اختياري)
                tableau.Columns[0].ReadOnly = false; // ✅ السماح بالقراءة فقط لعمود ID (اختياري)
                tableau.Columns[1].Width = 150;
                tableau.Columns[2].Width = 300;
                tableau.Columns[3].Width = 100;
                tableau.Columns[1].ReadOnly = true;
                tableau.Columns[2].ReadOnly = true;
                tableau.Columns[3].ReadOnly = true;
                tableau.Columns[4].Visible = false;
                tableau.Columns[5].Visible = false;




                // ✅ تعديل عناوين الأعمدة للعرض (اختياري)
                if (tableau.Columns.Contains("LIB_POINTEUSE"))
                    tableau.Columns["LIB_POINTEUSE"].HeaderText = "Appareil";
                if (tableau.Columns.Contains("ADRESSE_IP"))
                    tableau.Columns["ADRESSE_IP"].HeaderText = "Adresse IP";
                if (tableau.Columns.Contains("N_PORT"))
                    tableau.Columns["N_PORT"].HeaderText = "Port";
                if (tableau.Columns.Contains("N_MACHINE"))
                    tableau.Columns["N_MACHINE"].HeaderText = "N_MACHINE";
                if (tableau.Columns.Contains("etat"))
                    tableau.Columns["etat"].HeaderText = "État";
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Erreur lors du chargement des données : " + ex.Message,
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (tableau.Rows.Count >= Form2.nbr_machine)
            {
                bt_ajouter.Enabled = false;
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void bt_telechargerD_Click(object sender, EventArgs e)
        {
            ////------------------------1 er methode ------------------------------
            //gridControl1.DataSource = null;
            //gridView1.RefreshData();

            //// جدول موحد لتجميع البيانات من جميع الأجهزة
            //DataTable allData = new DataTable();
            //allData.Columns.Add("Matricule");
            //allData.Columns.Add("Date_Pointage");
            //allData.Columns.Add("Heure_Pointage");

            //// التكرار على الأجهزة المعلمة (checkbox = true)
            //foreach (DataGridViewRow row in tableau.Rows)
            //{
            //    bool isChecked = Convert.ToBoolean(row.Cells[0].Value);
            //    if (!isChecked) continue;

            //    string ip = row.Cells[1].Value.ToString(); // اسم العمود الذي فيه IP
            //    int port = 4370; // أو خذه من عمود آخر إن وجد

            //    ZKHelper zk = new ZKHelper();
            //    DataTable logs = zk.ReadAttendanceLogs(ip, port);

            //    foreach (DataRow dr in logs.Rows)
            //        allData.Rows.Add(dr.ItemArray);
            //}

            //// عرض جميع البيانات بعد الانتهاء من قراءة كل الأجهزة
            //gridControl1.DataSource = allData;



            //////////////////
            ///
            //------------------------ 1ère méthode ------------------------------

            //------------------------1 er methode ------------------------------
            gridControl1.DataSource = null;
            gridView1.RefreshData();

            // جدول موحد لتجميع البيانات من جميع الأجهزة
            DataTable allData = new DataTable();
            allData.Columns.Add("Matricule");
            allData.Columns.Add("Date_Pointage");
            allData.Columns.Add("Heure_Pointage");

            // --- إعداد شريط التقدم لعملية القراءة من الأجهزة ---
            progressBar1.Minimum = 0;
            progressBar1.Maximum = tableau.Rows.Count;
            progressBar1.Value = 0;
            progressBar1.Visible = true;


            foreach (DataGridViewRow row in tableau.Rows)
            {
                progressBar1.Value++; // تحديث الشريط لكل جهاز
                Application.DoEvents();

                if (row.Cells[0].Value?.ToString().Trim().ToUpper() == "O")
                {
                    string ip = row.Cells[1].Value?.ToString().Trim();
                    string portStr = row.Cells[3].Value?.ToString().Trim();
                    if (!int.TryParse(portStr, out int port))
                    {
                        MessageBox.Show("⚠️ رقم المنفذ غير صحيح في أحد الصفوف.");
                        continue;
                    }

                    if (!zkDevice.Connect_Net(ip, port))
                    {
                        int err = 0;
                        zkDevice.GetLastError(ref err);
                        MessageBox.Show($"❌ فشل الاتصال بالجهاز {ip}:{port} | كود الخطأ: {err}");
                        MessageBox.Show($"❌ فشل الاتصال بالجهاز: {ip}:{port}");
                        continue;
                    }

                    zkDevice.EnableDevice(1, false); // تعطيل الجهاز مؤقتًا
                    zkDevice.ReadGeneralLogData(1); // تحميل البيانات

                    string sEnrollNumber;
                    int idwVerifyMode, idwInOutMode, idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond;
                    int idwWorkcode = 0;

                    while (zkDevice.SSR_GetGeneralLogData(1, out sEnrollNumber, out idwVerifyMode, out idwInOutMode,
                                                          out idwYear, out idwMonth, out idwDay,
                                                          out idwHour, out idwMinute, out idwSecond,
                                                          ref idwWorkcode))
                    {
                        DateTime dateTime = new DateTime(idwYear, idwMonth, idwDay, idwHour, idwMinute, idwSecond);
                        allData.Rows.Add(sEnrollNumber, dateTime.ToShortDateString(), dateTime.ToShortTimeString());

                    }

                    row.Cells[0].ReadOnly = true;
                    row.DefaultCellStyle.BackColor = Color.Lime;

                    zkDevice.EnableDevice(1, true);
                    zkDevice.Disconnect(); // فصل الاتصال بالجهاز الحالي للانتقال للتالي
                }
            }

            // عرض جميع البيانات بعد الانتهاء من قراءة كل الأجهزة
            gridControl1.DataSource = allData;

            // حفظ البيانات في قاعدة البيانات داخل جدول chargement_pointage 
            // ... بعد انتهاء حلقة جلب البيانات من الأجهزة وملء allData ...

            // 1. التأكد من تهيئة إعدادات الاتصال (يجلب البيانات من Registry)
            //DevPointage.ConnectSqlServer.Initialized();
            // --- إعداد شريط التقدم لعملية الحفظ في قاعدة البيانات ---
            if (allData.Rows.Count > 0)
            {
                int savedCount = 0;
                progressBar1.Value = 0;
                progressBar1.Maximum = allData.Rows.Count;
                // 2. البدء في عملية الحفظ لكل سجل في allData
                foreach (DataRow row in allData.Rows)
            {
                    progressBar1.Value++;
                    Application.DoEvents();
                    // استعلام الإدخال مع التحقق من عدم التكرار (UPSERT pattern)
                    string query = @"
        IF NOT EXISTS (SELECT 1 FROM CHARGEMENT_POITEUSE 
                       WHERE MATRI = @matri 
                       AND DATE_POINTAGE = @dateP 
                       AND HEURS_POINTAGE = @hourP)
        BEGIN
            INSERT INTO CHARGEMENT_POITEUSE (MATRI, DATE_POINTAGE, HEURS_POINTAGE, DATE_TELECHARGEMENT)
            VALUES (@matri, @dateP, @hourP, GETDATE())
        END";

                // تجهيز البارامترات لتجنب SQL Injection
                SqlParameter[] parameters = {
        new SqlParameter("@matri", row["Matricule"].ToString().Trim()),
        new SqlParameter("@dateP", SqlDbType.Date) { Value = DateTime.Parse(row["Date_Pointage"].ToString()) },
        new SqlParameter("@hourP", row["Heure_Pointage"].ToString().Trim())
    };

                // تنفيذ العملية باستخدام الكلاس الخاص بك
                int result = DevPointage.ConnectSqlServer.ExecuteNonQuery(query, parameters);

                if (result > 0) savedCount++;
            }
                progressBar1.Visible = false;
                MessageBox.Show($"✅ تم الحفظ بنجاح!\nعدد السجلات الجديدة المضافة: {savedCount}",
                            "إتمام العملية", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                progressBar1.Visible = false;
                MessageBox.Show("⚠️ لا توجد بيانات جديدة لتحميلها.");
            }
        }

        private void bt_telechargerAttlog_Click(object sender, EventArgs e)
        {
            // 1. إعداد نافذة اختيار الملف
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "DAT Files (*.dat)|*.dat|Text Files (*.txt)|*.txt|All Files (*.*)|*.*";
            openFileDialog.Title = "Sélectionner le fichier des pointages (attlog)";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = openFileDialog.FileName; // المسار الذي اختاره المستخدم
                DataTable dt = new DataTable();

                dt.Columns.Add("Matricule");
                dt.Columns.Add("Date_Pointage");
                dt.Columns.Add("Heure_Pointage");

                try
                {
                    // 2. قراءة الأسطر وتجهيز الـ ProgressBar
                    var lignes = File.ReadAllLines(filePath);
                    int totalLines = lignes.Length;

                    progressBar1.Minimum = 0;
                    progressBar1.Maximum = totalLines;
                    progressBar1.Value = 0;
                    progressBar1.Visible = true;

                    // المرحلة الأولى: قراءة الملف وملء DataTable
                    foreach (string ligne in lignes)
                    {
                        progressBar1.Value++;
                        if (string.IsNullOrWhiteSpace(ligne)) continue;

                        string[] parties = ligne.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parties.Length >= 2)
                        {
                            string matricule = parties[0].Trim();
                            string fullDateTimeStr = parties[1].Trim();
                            if (parties.Length >= 3) fullDateTimeStr += " " + parties[2].Trim();

                            if (DateTime.TryParse(fullDateTimeStr, out DateTime dateHeure))
                            {
                                dt.Rows.Add(matricule, dateHeure.ToString("yyyy-MM-dd"), dateHeure.ToString("HH:mm:ss"));
                            }
                        }
                        Application.DoEvents();
                    }

                    gridControl1.DataSource = dt;

                    // المرحلة الثانية: الحفظ في قاعدة البيانات
                    int savedCount = 0;
                    progressBar1.Value = 0;
                    progressBar1.Maximum = dt.Rows.Count;

                    foreach (DataRow row in dt.Rows)
                    {
                        progressBar1.Value++;

                        string query = @"
                    IF NOT EXISTS (SELECT 1 FROM CHARGEMENT_POITEUSE 
                                   WHERE MATRI = @matri 
                                   AND DATE_POINTAGE = @dateP 
                                   AND HEURS_POINTAGE = @hourP)
                    BEGIN
                        INSERT INTO CHARGEMENT_POITEUSE (MATRI, DATE_POINTAGE, HEURS_POINTAGE, DATE_TELECHARGEMENT)
                        VALUES (@matri, @dateP, @hourP, GETDATE())
                    END";

                        SqlParameter[] parameters = {
                    new SqlParameter("@matri", row["Matricule"].ToString().Trim()),
                    new SqlParameter("@dateP", SqlDbType.Date) { Value = DateTime.Parse(row["Date_Pointage"].ToString()) },
                    new SqlParameter("@hourP", row["Heure_Pointage"].ToString().Trim())
                };

                        int result = DevPointage.ConnectSqlServer.ExecuteNonQuery(query, parameters);
                        if (result > 0) savedCount++;

                        Application.DoEvents();
                    }

                    progressBar1.Visible = false;
                    MessageBox.Show($"✅ تم تحميل الملف: {Path.GetFileName(filePath)}\nعدد السجلات الجديدة: {savedCount}",
                                    "نجاح العملية", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    progressBar1.Visible = false;
                    MessageBox.Show($"خطأ أثناء المعالجة: {ex.Message}", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void bt_ajouter_Click(object sender, EventArgs e)
        {
            Form18 fn_ajouterpointeuse = new Form18();
            Form18.insertion_modification = 0; // وضع الإدراج
            fn_ajouterpointeuse.ShowDialog();
            fn_ajouterpointeuse.Dispose();
            Form4_Load(sender,e);
        }

        private void bt_supprimer_Click(object sender, EventArgs e)
        {
            supprimerPointeuse(tableau.CurrentRow.Cells["ADRESSE_IP"].Value.ToString());
            Form4_Load(null, null); // إعادة تحميل البيانات بعد الحذف
        }

        private void btnSupprimerLogs_Click(object sender, EventArgs e)
        {
            // ✅ تأكيد الحذف
            var result = XtraMessageBox.Show("هل أنت متأكد أنك تريد حذف سجلات الحضور من الجهاز؟",
                                              "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;

            foreach (DataGridViewRow row in tableau.Rows)
            {
                if (row.Cells[0].Value?.ToString().Trim().ToUpper() == "O")
                {
                    if (zkDevice.ClearGLog(1)) // رقم الجهاز غالبًا 1
                    {
                        zkDevice.RefreshData(1); // لتحديث البيانات داخل الجهاز بعد الحذف
                        row.Cells[0].ReadOnly = true;
                        row.DefaultCellStyle.BackColor = Color.Lime;

                        MessageBox.Show("✅ تم حذف سجلات الحضور من الجهاز بنجاح");
                    }
                    else
                    {
                        row.Cells[0].ReadOnly = true;
                        row.DefaultCellStyle.BackColor = Color.Lime;

                        MessageBox.Show("❌ فشل في حذف سجلات الحضور");
                    }
                }
            }
        }

        private void tableau_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Form18 fn_ajouterpointeuse = new Form18();
            Form18.insertion_modification = 1; // وضع التعديل
            fn_ajouterpointeuse.txt_ip.Text = tableau.Rows[e.RowIndex].Cells["ADRESSE_IP"].Value.ToString();
            fn_ajouterpointeuse.txt_lib.Text = tableau.Rows[e.RowIndex].Cells["LIB_POINTEUSE"].Value.ToString();
            fn_ajouterpointeuse.txt_ip.Enabled = false; // منع تعديل IP أثناء التعديل
            fn_ajouterpointeuse.txt_lib.Enabled = true; // السماح بتعديل الاسم
            fn_ajouterpointeuse.ShowDialog();
            fn_ajouterpointeuse.Dispose();
            Form4_Load(null, null); // إعادة تحميل البيانات بعد الإضافة
        }

        private void supprimerPointeuse(string ip)
        {
            // ✅ تأكيد الحذف
            var result = XtraMessageBox.Show($"هل أنت متأكد أنك تريد حذف الجهاز {ip}؟",
                                              "تأكيد الحذف", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;
            string query = "DELETE FROM POINTEUSE WHERE ADRESSE_IP = @ip";
            var param = new SqlParameter("@ip", ip);
            int rowsAffected = ConnectSqlServer.ExecuteNonQuery(query, param);
            if (rowsAffected > 0)
            {
                XtraMessageBox.Show("✅ تم حذف الجهاز بنجاح.", "Succès", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Form4_Load(null, null); // إعادة تحميل البيانات بعد الحذف
            }
            else
            {
                XtraMessageBox.Show("❌ فشل في حذف الجهاز. قد يكون غير موجود.", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bt_telechargerD_MouseHover(object sender, EventArgs e)
        {
            // عرض تلميح عند المرور على زر تحميل البيانات
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(bt_telechargerD, "تحميل بيانات الحضور من الأجهزة المحددة في القائمة.");
        }

        private void bt_ajouter_MouseHover(object sender, EventArgs e)
        {
            // عرض تلميح عند المرور على زر إضافة جهاز
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(bt_ajouter, "إضافة جهاز جديد إلى القائمة.");
        }

        private void bt_supprimer_MouseHover(object sender, EventArgs e)
        {
            // عرض تلميح عند المرور على زر حذف جهاز
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(bt_supprimer, "حذف الجهاز المحدد من القائمة.");
        }

        private void btnSupprimerLogs_MouseHover(object sender, EventArgs e)
        {
            // عرض تلميح عند المرور على زر حذف سجلات الحضور
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(btnSupprimerLogs, "احذر! هذا سيؤدي إلى حذف جميع سجلات الحضور من الجهاز المحدد.");
        }

        private void bt_telechargerAttlog_MouseHover(object sender, EventArgs e)
        {
            // عرض تلميح عند المرور على زر تحميل ملف attlog
            ToolTip toolTip = new ToolTip();
            toolTip.SetToolTip(bt_telechargerAttlog, "تحميل بيانات الحضور من ملف attlog الذي يحتوي على سجلات الحضور.");
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try { zkDevice?.Disconnect(); } catch { }
            base.OnFormClosing(e);
        }
    }
}
