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
    public partial class Form5GönderiEkle : Form
    {
        public int userId { get; set; } // Giriş yapan kullanıcının ID'si
        public Form5GönderiEkle(int userId)
        {
            InitializeComponent();
            this.userId = userId;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string content = richTextBox1.Text;

            // Veritabanı bağlantısını aç
            using (SqlConnection connection = new SqlConnection("Server=DESKTOP-2SL889D; database=SosyalMedya2; Integrated security=true;"))
            {
                connection.Open();

                // Gönderiyi veritabanına ekle
                string query = "INSERT INTO Posts ( Content, CreatedAt, UsersID) VALUES ( @Content, @CreatedAt, @UserID)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@Content", content);
                    command.Parameters.AddWithValue("@CreatedAt", DateTime.Now); // Gönderi oluşturma zamanı
                    command.Parameters.AddWithValue("@UserID", this.userId); // Giriş yapan kullanıcının ID'si
                    command.ExecuteNonQuery();

                    MessageBox.Show("Gönderi başarıyla eklendi!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Close(); // Formu kapat
                }
            }
        }
    }
}
