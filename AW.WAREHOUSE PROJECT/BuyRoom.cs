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
    
    public partial class BuyRoom : Form
    {
       

        DBconnection dBConnection = new DBconnection();
        private string currentUserName;
        private string currentUserId;


        public BuyRoom()
        {
            InitializeComponent();
            LoadDataPremises();
            RefreshDataGrid(dataGridView1, dataGridView2);
           
        }


        public BuyRoom(string username, string userid) : this()
        {
            currentUserName = username;
            currentUserId = userid;
            
        }


        private void LoadDataPremises()
        {
            dataGridView1.Columns.Add("id_premises", "Id помещения");
            dataGridView1.Columns.Add("_type", "Тип Цеха");
            dataGridView1.Columns.Add("_adress", "Адресс цеха");
            dataGridView1.Columns.Add("_price", "Цена цеха");
            dataGridView1.Columns.Add("id_seller", "Id продавца");
            dataGridView1.Columns.Add("seller_name", "Имя продавца");
            dataGridView1.Columns.Add("_time", "Срок в днях");

            dataGridView2.Columns.Add("id_purchase", "ID Покупки");
            dataGridView2.Columns.Add("id_user", "ID Покупателя");
            dataGridView2.Columns.Add("id_premises", "ID Помещения");
            dataGridView2.Columns.Add("purchase_date", "Дата Покупки");
        }
        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {
            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetString(3), record.GetString(4), record.GetString(5), record.GetString(6) /*RowState.ModifiedNew*/);


        }
        private void ReadSingleRowPurchase(DataGridView dgw1, IDataRecord record)
        {
            dgw1.Rows.Add(record.GetInt32(0), record.GetInt32(1), record.GetInt32(2), record.GetDateTime(3)/*RowState.ModifiedNew*/);
        }
       
        private void RefreshDataGrid(DataGridView dgw, DataGridView dgw1)
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

            dgw1.Rows.Clear();
            string queryStrings = "select * from purchases";
            SqlCommand commands = new SqlCommand(queryStrings, dBConnection.getConnection());
            dBConnection.openConnection();
            SqlDataReader readers = commands.ExecuteReader();

            while (readers.Read())
            {
                ReadSingleRowPurchase(dgw1, readers);
            }
            readers.Close();
        }
      
        
       
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
           
        }

        public void button1_Click(object sender, EventArgs e)
        {


            int saleId;

            string querystring = "INSERT INTO purchases (id_user, id_premises, purchase_date) VALUES (@id_user, @id_premises, @purchase_date)";
            using (SqlCommand command = new SqlCommand(querystring, dBConnection.getConnection()))
            {
                
                dBConnection.openConnection();
               
    
                int selectedRowIndex = dataGridView1.CurrentCell.RowIndex;

                string currentPrice = dataGridView1.Rows[selectedRowIndex].Cells["_price"].Value.ToString();
                string time = dataGridView1.Rows[selectedRowIndex].Cells["_time"].Value != null ? dataGridView1.Rows[selectedRowIndex].Cells["_time"].Value.ToString() : string.Empty;
                string sellerName = dataGridView1.Rows[selectedRowIndex].Cells["seller_name"].Value != null ? dataGridView1.Rows[selectedRowIndex].Cells["seller_name"].Value.ToString() : string.Empty;
                string idSellerName = dataGridView1.Rows[selectedRowIndex].Cells["id_seller"].Value != null ? dataGridView1.Rows[selectedRowIndex].Cells["id_seller"].Value.ToString() : string.Empty;   
                string typePremises = dataGridView1.Rows[selectedRowIndex].Cells["_type"].Value != null ? dataGridView1.Rows[selectedRowIndex].Cells["_type"].Value.ToString() : string.Empty;
                string address = dataGridView1.Rows[selectedRowIndex].Cells["_adress"].Value != null ? dataGridView1.Rows[selectedRowIndex].Cells["_adress"].Value.ToString() : string.Empty;

            
           

                // Отображаем форму PaymentByCard в диалоговом режиме
             
                command.Parameters.AddWithValue("@id_user", currentUserId);
                command.Parameters.AddWithValue("@id_premises", dataGridView1.Rows[selectedRowIndex].Cells["id_premises"].Value);
                command.Parameters.AddWithValue("@purchase_date", DateTime.Now);

                
                saleId = command.ExecuteNonQuery();
                //saleId = Convert.ToInt32(command.ExecuteScalar());
                PaymentByCard paymentByCard = new PaymentByCard(

                     time,
                     saleId,
                     idSellerName,
                     currentUserId,
                     currentPrice,
                     sellerName,
                     typePremises,
                     address
                 );
                paymentByCard.ShowDialog();
                
                dBConnection.closeConnection();
                //MessageBox.Show("Вы успешно приобрели помещение! Приятного пользования!");
            }
            }

        private void button3_Click(object sender, EventArgs e)
        {
            RefreshDataGrid(dataGridView1, dataGridView2);
        }
    }
    }

