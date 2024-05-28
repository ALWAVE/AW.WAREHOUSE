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
    public partial class RegistrationForm : Form
    {
        public RegistrationForm()
        {

            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }

        DBconnection dBConnection = new DBconnection();

        private Boolean checkuser()
        {
            var loginUser = textBox1.Text;
            var passUser = textBox2.Text;
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            string querystring = $"select id_user from registers where login_user = @loginUser";
            SqlCommand command = new SqlCommand(querystring, dBConnection.getConnection());
            command.Parameters.AddWithValue("@loginUser", loginUser); 

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                MessageBox.Show("Пользователь с таким логином уже существует!");
                return true;
            }
            else
            {

                return false; 
            }
        }
        private Boolean checkLoginLength(string login)
        {
            if (login.Length < 5)
            {
                MessageBox.Show("Логин должен состоять как минимум из 5 символов!");
                return false;
            }
            return true;
        }
        private Boolean checkPasswordLength(string password)
        {
            if (password.Length < 5)
            {
                MessageBox.Show("Пароль должен состоять как минимум из 5 символов!");
                return false;
            }
            return true;
        }


        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            AuthorizationForm auth = new AuthorizationForm();
            this.Hide();
            auth.Show();
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            bool isBanned = false;
            var login = textBox1.Text;
            var password = textBox2.Text;
            string user_role = comboBox1.Items[comboBox1.SelectedIndex].ToString();
            string querystring = $"insert into registers(login_user,role_user, password_user, isBanned) values('{login}','{user_role}','{password}', {(isBanned ? 1 : 0)})";
            if (!checkLoginLength(login))
            {
                return; // Если длина логина недостаточна, прекращаем выполнение метода
            }
            if(!checkPasswordLength(password))
            {
                return;
            }
            SqlCommand command = new SqlCommand(querystring, dBConnection.getConnection());

            dBConnection.openConnection();
            if (checkuser())

            {
                return;
            }

            if (command.ExecuteNonQuery() == 1)
            {
                MessageBox.Show("Аккаунт успешно создан!", "Успешное создание!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                AuthorizationForm authorization = new AuthorizationForm();
                this.Hide();
                
                authorization.ShowDialog();

            }
            else
            {
                MessageBox.Show("Аккаунт не создан!");
            }

            dBConnection.closeConnection();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
                
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
           
        }
    }
}
