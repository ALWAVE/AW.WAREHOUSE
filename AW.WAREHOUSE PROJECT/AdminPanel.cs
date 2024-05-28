using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace AW.WAREHOUSE_PROJECT
{
  

    public partial class AdminPanel : Form
    {

        DBconnection dBConnection = new DBconnection();

        private void LoadData()
        {
            dataGridView1.Columns.Add("id_user", "Id пользователя");
            dataGridView1.Columns.Add("login_user", "Имя пользователя");
            dataGridView1.Columns.Add("role_user", "Роль пользователя");
            dataGridView1.Columns.Add("isBanned", "Блокировка");
        }
        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), Convert.ToBoolean(record["isBanned"]));

        }
        private void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string queryString = "select * from registers";

            SqlCommand command = new SqlCommand(queryString, dBConnection.getConnection());

            dBConnection.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dgw, reader);
            }
            reader.Close();

        }
        public AdminPanel()
        {
            InitializeComponent();
            LoadData();
            RefreshDataGrid(dataGridView1);
        }

        private void Banned()
        {
            int selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
            string role = dataGridView1.Rows[selectedRowIndex].Cells["role_user"].Value.ToString();
            if (role == "admin")
            {
                MessageBox.Show("Администратора нельзя забанить!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            bool isBanned = true;
            string querystring = $"update registers set isBanned = {(isBanned ? 1 : 0)} where id_user = {dataGridView1.Rows[selectedRowIndex].Cells["id_user"].Value}";
            SqlCommand command = new SqlCommand(querystring, dBConnection.getConnection());
            command.ExecuteNonQuery();
            MessageBox.Show("Вы успешно заблокировали пользователя!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);

            RefreshDataGrid(dataGridView1);
        }
        private void unBanned()
        {
            int selectedRowIndex = dataGridView1.CurrentCell.RowIndex;
            bool isBanned = false;
         
            string querystring = $"update registers set isBanned = {(isBanned ? 1 : 0)} where id_user = {dataGridView1.Rows[selectedRowIndex].Cells["id_user"].Value}";
            SqlCommand command = new SqlCommand(querystring, dBConnection.getConnection());
            command.ExecuteNonQuery();
            MessageBox.Show("Вы успешно разблокировали пользователя!", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Information);
            RefreshDataGrid(dataGridView1);
        }
        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Banned();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            unBanned();
        }

        private void button3_Click(object sender, EventArgs e)
        {
          
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            this.Hide();
           
        }
    }
}
