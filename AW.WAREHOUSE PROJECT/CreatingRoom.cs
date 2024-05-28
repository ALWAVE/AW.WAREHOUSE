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
using System.Text.RegularExpressions;

namespace AW.WAREHOUSE_PROJECT
{
   
    public partial class CreatingRoom : Form
    {
        private string currentUserName;
        private string currentUserId;
        public CreatingRoom(string username, string userid) : this()
        {
            currentUserName = username;
            currentUserId = userid;
            if(currentUserName == "admin")
            {
                button4.Visible = true;
            }
        }

        public CreatingRoom() 
        {
            InitializeComponent();
            LoadData();
            RefreshDataGrid(dataGridView1);
        }

        DBconnection dBConnection = new DBconnection();

        private void button1_Click(object sender, EventArgs e)
        {
            string typePremises = comboBox1.Items[comboBox1.SelectedIndex].ToString();
            var adress = textBox1.Text;
            var price = textBox2.Text;
            int time;
            if (!int.TryParse(textBox3.Text, out time))
            {
                MessageBox.Show("Некорректное значение времени.");
                return;
            }
            if (time > 365)
            {
                MessageBox.Show("Срок не может превышать 365 дней.");
                return;
            }

            if (!IsDigitsOnly(textBox2.Text))
            {
                MessageBox.Show("В поле \"Цена\" можно вводить только цифры и десятичную точку.");
                return;
            }
            else if (!IsDigitsOnlyForTime(textBox3.Text))
            {
                MessageBox.Show("В поле \"Срок\" можно вводить только цифры.");
                return;
            }
            
            string querystring = "INSERT INTO premises(_type, _adress, _price, id_seller, seller_name, _time) VALUES (@type, @adress, @price, @id_seller, @seller_name, @time)";


            using (SqlCommand command = new SqlCommand(querystring, dBConnection.getConnection()))
            {
                
                command.Parameters.AddWithValue("@type", typePremises);
                command.Parameters.AddWithValue("@adress", adress);
                command.Parameters.AddWithValue("@price", price + " руб");
                command.Parameters.AddWithValue("@id_seller", currentUserId);
                command.Parameters.AddWithValue("@seller_name", currentUserName);
                command.Parameters.AddWithValue("@time", time + " дней(ня)");

                dBConnection.openConnection();

                // Выполнение запроса
                command.ExecuteNonQuery();

                dBConnection.closeConnection();
            }

           
            MessageBox.Show("Данные успешно добавлены в базу данных!");
        }
        private bool IsDigitsOnly(string input)
        {
            Regex regex = new Regex(@"^\d+(\.\d+)?$");
            return regex.IsMatch(input);
        }
        private bool IsDigitsOnlyForTime(string input)
        {
            Regex regex = new Regex(@"^[0-9]{1,3}$");
            return regex.IsMatch(input);
        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        private void LoadData()
        {
            dataGridView1.Columns.Add("id_premises", "Id помещения");
            dataGridView1.Columns.Add("_type", "Тип Цеха");
            dataGridView1.Columns.Add("_adress", "Адресс цеха");
            dataGridView1.Columns.Add("_price", "Цена цеха");
            dataGridView1.Columns.Add("id_seller", "Id продавца");
            dataGridView1.Columns.Add("seller_name", "Имя продавца");
            dataGridView1.Columns.Add("_time", "Срок в днях");
        }
        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetString(3), record.GetString(4), record.GetString(5), record.GetString(6) /*RowState.ModifiedNew*/);

        }
        private void RefreshDataGrid(DataGridView dgw)
        {
            dgw.Rows.Clear();

            string queryString = "select * from premises";

            SqlCommand command = new SqlCommand(queryString, dBConnection.getConnection());

            dBConnection.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dgw, reader);
            }
            reader.Close();

            //dBConnection.closeConnection();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1);
        }

        private void textBox3_MouseHover(object sender, EventArgs e)
        {
            label6.Visible = true;
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_MouseLeave(object sender, EventArgs e)
        {
            label6.Visible = false;
        }
        private void DeleteRowFromPremises(int premisesId)
        {
            string querystring = "DELETE FROM premises WHERE id_premises = @premisesId";

            using (SqlCommand command = new SqlCommand(querystring, dBConnection.getConnection()))
            {
                command.Parameters.AddWithValue("@premisesId", premisesId);

                dBConnection.openConnection();

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    MessageBox.Show("Строка успешно удалена из базы данных!");
                }
                else
                {
                    MessageBox.Show("Ошибка при удалении строки из базы данных!");
                }

                dBConnection.closeConnection();
            }
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int premisesId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["id_premises"].Value);
                DeleteRowFromPremises(premisesId);
                RefreshDataGrid(dataGridView1);
            }
            else
            {
                MessageBox.Show("Выберите строку для удаления!");
            }
        }
    }
}
