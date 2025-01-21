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
    public partial class Form3AnaSayfa : Form
    {
        private int userId;
        private string username;
        public Form3AnaSayfa(int userId, string username)
        {
            InitializeComponent();
            this.userId = userId;
            this.username = username;

            label1.Text = $"Hoş Geldiniz {username} :)";

            // Gönderileri yükleme
            GonderiListele();
        }


        private void GonderiListele()
        {
            string query = "SELECT Posts.PostID, Posts.Content, Users.username FROM Posts " +
                           "INNER JOIN Users ON Posts.UsersID = Users.UsersID";

            using (SqlConnection connection = new SqlConnection("Server=DESKTOP-2SL889D; database=SosyalMedya2; Integrated security=true;"))
            {
                SqlCommand command = new SqlCommand(query, connection);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    DataTable dataTable = new DataTable();
                    dataTable.Load(reader);
                    dataGridView1.DataSource = dataTable;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Gönderiler yüklenirken hata: " + ex.Message);
                }
            }
        }


        private string GetYorumlar(int postID)
        {
            string query = "SELECT content FROM Comments WHERE post_id = @PostID;";
            StringBuilder yorumlar = new StringBuilder();

            using (SqlConnection connection = new SqlConnection("Server=DESKTOP-2SL889D; database=SosyalMedya2; Integrated security=true;"))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@PostID", postID);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                yorumlar.AppendLine($"- {reader["content"].ToString()}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Yorumlar yüklenirken hata: " + ex.Message);
                    }
                }
            }

            return yorumlar.ToString();
        }
        private void Form3AnaSayfa_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Kullanıcının kendi gönderilerini listelemek için formu parametreyle aç
            Form4Gönderilerim gonderilerim = new Form4Gönderilerim(this.userId);
            gonderilerim.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // Çıkış yap ve giriş ekranına dön
            Form1 KullanıcıGiris = new Form1();
            KullanıcıGiris.Show();
            this.Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            // Eğer geçerli bir satıra tıklanmışsa
            if (e.RowIndex >= 0)
            {
                // Seçili satırdan PostID'yi al
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                int postID = Convert.ToInt32(selectedRow.Cells["PostID"].Value);

                // Gönderinin içeriğini al
                string content = selectedRow.Cells["Content"].Value.ToString();

                // Yorumları getir
                string yorumlar = GetYorumlar(postID);

                // Gönderi içeriği ve yorumları Label'a ekle
                label2.Text = $"İçerik: {content}\nYorumlar:\n{yorumlar}";
            }
        }
    }
}
