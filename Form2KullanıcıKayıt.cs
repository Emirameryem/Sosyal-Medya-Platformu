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
    public partial class Form2KullanıcıKayıt : Form
    {
        public Form2KullanıcıKayıt()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text;
            string email = textBox2.Text;
            string password = textBox3.Text;

            // Şifreyi hashliyoruz
            string hashedPassword = Helper.HashPassword(password);

            // Kullanıcıyı veritabanına ekliyoruz
            string query = "INSERT INTO Users (username, email, Password_) VALUES (@username, @email, @PasswordHash)";

            using (SqlConnection connection = new SqlConnection("Server=DESKTOP-2SL889D; database=SosyalMedya2; Integrated security=true;"))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@PasswordHash", hashedPassword);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    MessageBox.Show("Kayıt başarılı! Giriş ekranına yönlendiriliyorsunuz.");

                    // Giriş ekranına yönlendirme
                    Form1 KullanıcıGiris = new Form1();
                    KullanıcıGiris.Show();
                    this.Hide();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                }
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
