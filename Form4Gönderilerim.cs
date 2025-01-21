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
    public partial class Form4Gönderilerim : Form
    {
        private int userId;
        public Form4Gönderilerim(int userId)
        {
            InitializeComponent();
            this.userId = userId;
            gonderilerimiListele(userId);
        }

        private void gonderilerimiListele(int userId)
        {
            // Veritabanı bağlantısı
            string connectionString = "Server=DESKTOP-2SL889D; database=SosyalMedya2; Integrated security=true;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT PostID, Content, CreatedAt FROM Posts WHERE UsersID = @UserID";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@UserID", userId);

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable postsTable = new DataTable();
                    adapter.Fill(postsTable);

                    // DataGridView'e bağlama
                    dataGridView1.DataSource = postsTable;

                    // Eğer ListBox kullanıyorsanız:
                    // foreach (DataRow row in postsTable.Rows)
                    // {
                    //     listBoxPosts.Items.Add(row["Title"]);
                    // }
                }
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Seçili gönderinin bilgilerini alın
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];

                string content = selectedRow.Cells["Content"].Value.ToString();

                // Detayları bir metin kutusunda veya label'da gösterin
                label1.Text = $"\nİçerik: {content}";
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Gönderi ekleme formunu aç
            Form5GönderiEkle addPostForm = new Form5GönderiEkle(this.userId);
            addPostForm.userId = this.userId; // Giriş yapan kullanıcının ID'sini gönderi ekleme formuna aktar
            addPostForm.ShowDialog();

            // Yeni gönderi eklendikten sonra gönderi listesini güncelle
            gonderilerimiListele(this.userId);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0) // Eğer satır seçilmişse
            {
                // Seçili satırı al
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Gönderi içeriği ve ID'sini al
                string content = selectedRow.Cells["Content"].Value.ToString();
                int post_id = Convert.ToInt32(selectedRow.Cells["PostID"].Value);

                // Gönderi detaylarını ve yorumlarını çekmek için SQL sorgusu
                string query = "SELECT content FROM Comments WHERE post_id = @PostID;";

                using (SqlConnection connection = new SqlConnection("Server=DESKTOP-2SL889D; database=SosyalMedya2; Integrated security=true;"))
                {
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Parametreyi ekleyin
                        command.Parameters.AddWithValue("@PostID", post_id);

                        // Yorumları birleştirmek için
                        StringBuilder yorumlar = new StringBuilder();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                yorumlar.AppendLine($"- {reader["content"].ToString()}");
                            }
                        }

                        // Label'a detayları ekleyin
                        label1.Text = $"İçerik: {content}\nYorumlar:\n{yorumlar}";
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen bir gönderi seçiniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}
    

