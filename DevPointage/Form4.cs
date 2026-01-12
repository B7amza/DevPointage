using DevExpress.XtraEditors;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using zkemkeeper;

namespace DevPointage
{
    public partial class Form4 : XtraForm
    {
        public Form4()
        {
            InitializeComponent();
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

            foreach (DataGridViewRow row in tableau.Rows)
            {
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
                }
            }

            // عرض جميع البيانات بعد الانتهاء من قراءة كل الأجهزة
            gridControl1.DataSource = allData;


        }


    }
}
