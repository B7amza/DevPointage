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
                string query = "SELECT * FROM POINTEUSE where etat='O'";

                // جلب البيانات في DataTable
                DataTable dt = ConnectSqlServer.ExecuteSelect(query);

                // ربطها بالـ DataGridView أو GridControl
                tableau.DataSource = dt;

                // تنسيقات اختيارية
                tableau.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                tableau.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show("Erreur lors du chargement des données : " + ex.Message,
                    "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
}
