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
using AW.WAREHOUSE_PROJECT;


namespace AW.WAREHOUSE_PROJECT
{
    public partial class AuthorizationForm : Form
    {

        DBconnection dbConnection = new DBconnection();
        public delegate void CloseDelegate();
        public CloseDelegate CloseEvent;


        public AuthorizationForm()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            CloseEvent = null;
            label3.Visible = false;
            textBox2.UseSystemPasswordChar = true;
        }
        public void AddTestUser(string login, string password, bool isBanned = false)
        {
            try
            {
                dbConnection.openConnection();
                using (SqlCommand command = new SqlCommand("INSERT INTO registers (login_user, password_user, is_banned) VALUES (@login, @password, @isBanned)", dbConnection.getConnection()))
                {
                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@password", password);
                    command.Parameters.AddWithValue("@isBanned", isBanned);
                    command.ExecuteNonQuery();
                }
            }
            finally
            {
                dbConnection.closeConnection();
            }
        }
        private bool checkUserIsBanned()
        {
            var loginUser = textBox1.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            string querystring = $"select isBanned from registers where login_user = @loginUser";
            SqlCommand command = new SqlCommand(querystring, dbConnection.getConnection());
            command.Parameters.AddWithValue("@loginUser", loginUser);

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                // Получаем значение isBanned
                bool isBanned = (bool)table.Rows[0]["isBanned"];

                // Возвращаем true, если пользователь забанен
                if (isBanned)
                {
                    MessageBox.Show("Проблема, ваш аккаунт был заблокирован, если ошибка свяжитесь с админом +7999111111");
                    linkLabel1.Visible = false;
                    //button1.Visible = false;
                    label3.Visible = true;
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                // Пользователь не найден в базе данных
                MessageBox.Show("Пользователь не найден");
                return false;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        public void Button1_Click(object sender, EventArgs e)
        {
            var loginUser = textBox1.Text;
            var passUser = textBox2.Text;
            
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            string querystring = $"select id_user, login_user, password_user from registers where login_user = '{loginUser}' and password_user = '{passUser}'";
            
            SqlCommand command = new SqlCommand(querystring, dbConnection.getConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);


            bool isBanned = checkUserIsBanned();

            if (!isBanned)
            {
                if (table.Rows.Count == 1)
                {
                    //MessageBox.Show("Вы успешно вошли!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Form1 menu = new Form1(textBox1.Text);
                    
                    this.Hide();

                    menu.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Увы.. Братец или Сестренка такого аккаунта не существует!", "Акаунта не существует!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                MessageBox.Show("Вы были забанены!", "Аккаунт забанен!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RegistrationForm reg = new RegistrationForm();
            this.Hide();
            reg.ShowDialog();
            this.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CloseEye_Click(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = true;
            OpenEye.Visible = true;
            CloseEye.Visible = false;
        }

        private void OpenEye_Click(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = false;
            CloseEye.Visible = true;
            OpenEye.Visible = false;
        }

    }
}
