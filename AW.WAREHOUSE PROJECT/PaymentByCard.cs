using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;
using Application = Microsoft.Office.Interop.Word.Application;
using Document = Microsoft.Office.Interop.Word.Document;
namespace AW.WAREHOUSE_PROJECT
{
    public partial class PaymentByCard : Form
    {
        private string currentTime;
        private int currentIdPurchase;
        private string currentIdSeller;
        private string currentIdUser;
        private string currentPrice;
        private string currentSellerName;
        private string currentTypePremises;
        private string currentAdress;

        public PaymentByCard()
        {
            InitializeComponent();
            InitializeTextBoxPlaceholders();
        }
        public PaymentByCard(string time, int id_purchase, string id_seller, string userid, string price, string seller_name, string type_premises, string _adress) : this()
        {
            currentTime = time;
            currentIdPurchase = id_purchase;
            currentIdSeller = id_seller;
            currentIdUser = userid;
            currentPrice = price;

            currentSellerName = seller_name;
            currentTypePremises = type_premises;
            currentAdress = _adress;
        }
        private void InitializeTextBoxPlaceholders()
        {
            AddPlaceholderText(textNumberCard, "XXXXX XXXX XXXX XXXX");
            AddPlaceholderText(textTern1, "XX ");
            AddPlaceholderText(textTern2, "XX ");
            AddPlaceholderText(textName, "XXXXXXXXXXX XXXXXXXXXXXX");
            AddPlaceholderText(textCvv, "123 ");


            textNumberCard.GotFocus += new EventHandler(RemoveText);
            textNumberCard.LostFocus += new EventHandler(AddText);
            textTern1.GotFocus += new EventHandler(RemoveText);
            textTern1.LostFocus += new EventHandler(AddText);
            textTern2.GotFocus += new EventHandler(RemoveText);
            textTern2.LostFocus += new EventHandler(AddText);
            textName.GotFocus += new EventHandler(RemoveText);
            textName.LostFocus += new EventHandler(AddText);
            textCvv.GotFocus += new EventHandler(RemoveText);
            textCvv.LostFocus += new EventHandler(AddText);


        }
        private void AddPlaceholderText(TextBox textBox, string placeholder)
        {
            textBox.Text = placeholder;
            textBox.Tag = placeholder;

            textBox.ForeColor = Color.Gray;

            textBox.TextChanged += (sender, e) =>
            {
                if (textBox.Text.Length == placeholder.Length)
                {
                    if (!IsFormatValid(textBox.Text, placeholder))
                    {
                        textBox.ForeColor = Color.Red;
                    }
                    else
                    {
                        textBox.ForeColor = Color.Black;
                    }
                }
                else if (textBox.Text.Length < placeholder.Length)
                {
                    textBox.ForeColor = Color.Gray;
                }
            };

            textBox.KeyPress += (sender, e) =>
            {
                if (char.IsDigit(e.KeyChar) || e.KeyChar == '\b')
                {
                    if (textBox == textNumberCard)
                    {
                        // Если введено четыре цифры, добавляем пробел
                        if (textBox.Text.Length == 4 || textBox.Text.Length == 9 || textBox.Text.Length == 14)
                        {
                            textBox.Text += " ";
                            textBox.SelectionStart = textBox.Text.Length;
                        }
                        e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == '\b');
                    }
                    else if (textBox == textTern1 || textBox == textTern2)
                    {
                        e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == '\b');
                    }
                    else if (textBox == textCvv)
                    {
                        e.Handled = !(char.IsDigit(e.KeyChar) || e.KeyChar == '\b');
                    }
                }
                else if (textBox == textName)
                {
                    // Разрешаем ввод только букв и пробелов
                    e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == ' ' || e.KeyChar == '\b');
                }
                else
                {
                    e.Handled = true;
                }
            };
        }

        private bool IsFormatValid(string text, string placeholder)
        {
            text = text.Replace(" ", "");

            if (placeholder == "XXXX XXXX XXXX XXXX")
            {
                string regexPattern = @"^\d{16}$"; // Ожидаем 16 цифр
                return Regex.IsMatch(text, regexPattern);
            }
            else if (placeholder == "XX")
            {
                string regexPattern = @"^\d{1,2}$"; // Ожидаем не больше двух цифр
                return Regex.IsMatch(text, regexPattern);
            }
            else if (placeholder == "ИВАН ИВАНОВИЧ")
            {
                string regexPattern = @"^[А-Яа-я\s]+$"; // Ожидаем только буквы и пробелы
                return Regex.IsMatch(text, regexPattern);
            }
            else if (placeholder == "123")
            {
                string regexPattern = @"^\d{1,3}$";
                return Regex.IsMatch(text, regexPattern);
            }
            else
            {
                return false;
            }
        }

    private void RemoveText(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == textBox.Tag.ToString())
            {
                textBox.Text = "";
            }
        }

        private void AddText(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (string.IsNullOrWhiteSpace(textBox.Text))
                textBox.Text = textBox.Tag.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        private bool IsAllDataValid()
        {

            if (textNumberCard.ForeColor == Color.Red)
            {
                MessageBox.Show("Вы ввели поле с номером карты не правильно!");
                return false;
            }
            else if (textTern1.ForeColor == Color.Red || textTern2.ForeColor == Color.Red)
            {
                MessageBox.Show("Вы ввели поле с датой не правильно!");
                return false;
            }
            else if (textName.ForeColor == Color.Red)
            {
                MessageBox.Show("Вы ввели поле с вашем именем не правильно!");
                return false;
            }
            else if (textCvv.ForeColor == Color.Red)
            {
                MessageBox.Show("Вы ввели поле с 3-ех значным кодом не правильно!");
                return false;
            }
            else
            {
                return true; 
            }

        }
        private void btn_Buy_Click(object sender, EventArgs e)
        {

            if (!IsAllDataValid())
            {
                MessageBox.Show("Пожалуйста, заполните все поля корректно перед покупкой.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Вы успешно приобрели помещение! Приятного пользования!");
            this.Hide();

            string inputFilePath = $@"{System.Windows.Forms.Application.StartupPath}\Receipt\Example.docx";
            string outputFilePath = $@"{System.Windows.Forms.Application.StartupPath}\Receipt\receipt {DateTime.Now.ToString("dd-mm-yyyy hh-mm-ss")}.pdf";

            var replacements = new System.Collections.Generic.Dictionary<string, string>
            {
                {"<textName>", $"{textName.Text}"},
                {"<textNumberCard>", $"{textNumberCard.Text}"},
                {"<dataGridView>", $"{DateTime.Now}"},
                {"<id_purchase>", $"{currentIdPurchase}"},
                {"<_type>", $"{currentTypePremises}"},
                {"<_price>", $"{currentPrice}"},
                {"<seller_name>", $"{currentSellerName}"},
                {"<_adress>", $"{currentAdress}"},
                {"<_time>", $"{currentTime}"},
                {"<id_seller>", $"{currentIdSeller}"},
                {"<id_user>", $"{currentIdUser}"},


            };
            if(ReplaceTags(inputFilePath,outputFilePath,replacements) == true)
            {
                MessageBox.Show("Чек успешно сформирован", "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public static bool ReplaceTags(string inputFilePath, string outputFilePath, Dictionary<string, string> replacements)
        {
            Application wordApp = new Application();

            try
            {
                Document doc = wordApp.Documents.Open(inputFilePath, ReadOnly: true);
                Range range = doc.Content;
                Document newDoc = wordApp.Documents.Add();
                range.Copy();
                newDoc.Range().Paste();

                foreach (var replacement in replacements)
                {
                    newDoc.Content.Find.Execute(FindText: replacement.Key, ReplaceWith: replacement.Value, Replace: WdReplace.wdReplaceAll);
                }
                newDoc.SaveAs2(outputFilePath, WdSaveFormat.wdFormatPDF);
                newDoc.Close(SaveChanges: false);
                Process.Start(outputFilePath);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при формировании чека: " + ex.Message, "Уведомление", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return false;
                
            }
            finally
            {
                wordApp.Quit();
            }

        }
        private void Info_NumberCard_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Введите информацию карты она состоит из 16 цифр \n" +
                "При приоберетения неверного помещения мы не ненсем ни какой отвественности!");
        }

        private void Info_Tern_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Введите действительность карты! \n" +
     "При приоберетения неверного помещения мы не ненсем ни какой отвественности!");
        }

        private void Info_FName_SName_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Введите свои инициалы \n" +
    "При приоберетения неверного помещения мы не ненсем ни какой отвественности!");
        }

        private void Info_CVVKey_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Введите информацию карты она состоит из 3 цифр \n" +
    "Не кому не разглошайте этот код!");
        }


    }
}