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
    
    public partial class Form1 : Form
    {
        DBconnection dBConnection = new DBconnection();



        public Form1(string username1) : this()
        {
            string querystring = $"select id_user, role_user from registers where login_user = '{username1}'";
          

            using (SqlConnection connection = dBConnection.getConnection())
            {
                connection.Open();

                SqlCommand command = new SqlCommand(querystring, connection);

                username.Text = username1;
             
          
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        string userID = reader["id_user"].ToString();
                        string userRole = reader["role_user"].ToString();

                        userid.Text = userID;
                        userole.Text = userRole;
                       
                        if (userRole == "seller")
                        {
                            MessageBox.Show("Вы успешно вошли как Покупатель", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            button3.Visible = true;
                            button1.Visible = false;
                            button4.Visible = false;
                        }
                        else if (userRole == "suplier")
                        {
                            MessageBox.Show("Вы успешно вошли как Продавец", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            button3.Visible = false;
                            button1.Visible = true;
                            button4.Visible = false;
                        }
                        else if (userRole == "admin")
                        {
                            MessageBox.Show("Вы успешно вошли как Администратор", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        

                    }
                    dBConnection.closeConnection();
                }
                //LoadData();
                //RefreshDataGrid(dataGridView1);
            }
        }

        public Form1()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
            
        }

        private void Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Button1_Click(object sender, EventArgs e)
        {
            CreatingRoom creatingRoom = new CreatingRoom(username.Text, userid.Text);
            creatingRoom.Show();
                
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            BuyRoom buyRoom = new BuyRoom(username.Text, userid.Text);

            buyRoom.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //LoadData();
         
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AdminPanel adminPanel = new AdminPanel();
            adminPanel.Show();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            AuthorizationForm authorizationForm = new AuthorizationForm();
            
            authorizationForm.Show();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button7_Click(object sender, EventArgs e)
        {

        }

        private void Label12_Click(object sender, EventArgs e)
        {

        }
    }
}
