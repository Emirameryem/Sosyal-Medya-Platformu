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

namespace SosyalMedyaPlatformu1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string email = textBox2.Text;
            string password = textBox3.Text;

            // Şifreyi hashleyip kontrol ediyoruz
            string hashedPassword = Helper.HashPassword(password);

            //Bu sorgu, kullanıcı giriş işlemi sırasında e-posta ve şifre doğrulaması yapmak için kullanılır
            string query = "SELECT UsersID, username FROM Users WHERE email = @Email AND Password_ = @PasswordHash";

            using (SqlConnection connection = new SqlConnection("Server=DESKTOP-2SL889D; database=SosyalMedya2; Integrated security=true;")) //SqlConnection: Veritabanına bağlanmak için kullanılan bir sınıf.
            {
                SqlCommand command = new SqlCommand(query, connection);//SqlCommand: SQL sorgularını çalıştırmak için kullanılan sınıf
                //query: Daha önce tanımladığınız SQL sorgusu, connection: Daha önce tanımlanan veritabanı bağlantısı.

                //connection: Daha önce tanımlanan veritabanı bağlantısı.
                command.Parameters.AddWithValue("@Email", email);
                command.Parameters.AddWithValue("@PasswordHash", hashedPassword);//AddWithValue: Parametrelerin isimlerini ve değerlerini sorguya güvenli bir şekilde bağlar.

                try
                {
                    connection.Open(); //Veritabanına fiziksel bir bağlantı açar. Bu işlem sırasında bağlantı hatası olursa, catch bloğuna düşer.
                    SqlDataReader reader = command.ExecuteReader();//ExecuteReader(): Sorguyu çalıştırır ve sonuçları okumanıza olanak tanır
                    //reader: Sorgudan dönen verileri temsil eder.


                    if (reader.Read()) //reader.Read(): Sorgudan bir satır okur. Veri varsa true, yoksa false döner.
                    {
                        // Kullanıcı bilgilerini al
                        int userId = reader.GetInt32(0);//Dönen satırın ilk sütunundaki değeri int olarak alır (burada UserID).
                        string username = reader.GetString(1);//Dönen satırın ikinci sütunundaki değeri string olarak alır (burada username).

                        MessageBox.Show("Giriş başarılı!");


                        // Ana ekrana kullanıcı bilgisiyle yönlendir
                        Form3AnaSayfa anaSayfa = new Form3AnaSayfa(userId, username);
                        anaSayfa.Show();
                        this.Hide(); //Şu anki form (giriş ekranı) kullanıcıdan gizlenir.
                    }
                    else
                    {
                        MessageBox.Show("E-posta veya şifre yanlış!");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Kayıt formuna yönlendiriyoruz
            Form2KullanıcıKayıt kayıtol = new Form2KullanıcıKayıt();
            kayıtol.Show();
            this.Hide();
        }
    }
}
