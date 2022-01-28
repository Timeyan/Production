using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Production
{
    /// <summary>
    /// Interaction logic for CreateShop.xaml
    /// </summary>
    public partial class CreateShop : Window
    {
        private bool isSaving = false;
        public bool IsSaving { get { return isSaving; } set { isSaving = value; } }
        string connectionString;
        public CreateShop()
        {
            InitializeComponent();
            connectionString = "Data Source=DESKTOP-N9D0K7G;Initial Catalog=Production;Integrated Security=true;TrustServerCertificate=True";
            _ = ShowDialog();
        }

        private void EnterClickBack(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private async void EnterClickAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = TextBoxName.Text;
                string adress = TextBoxAdress.Text;
                string postCode = TextBoxPostCode.Text;
                List<string> sellers = new List<string>();
                sellers.Add(TextBoxSellers.Text);
                if (name == "" || adress == "" || postCode == "") throw new Exception("Не все поля заполнены");

                string sqlProc = "insert into [Shop] values(@name, @adress, @postCode)";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    SqlCommand command = new SqlCommand(sqlProc, connection);
                    command.Parameters.Add(new SqlParameter { ParameterName = "@name", Value = name, SqlDbType = SqlDbType.NVarChar });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@adress", Value = adress, SqlDbType = SqlDbType.NVarChar });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@postCode", Value = postCode, SqlDbType = SqlDbType.VarChar });
                    //command.Parameters.Add(new SqlParameter { ParameterName = "@type", Value = type, SqlDbType = SqlDbType.NVarChar });

                    _ = await command.ExecuteNonQueryAsync();
                }
                MessageBox.Show("Данные успешно сохранены");
                IsSaving = true;
                Close();
            }
            catch (Exception ex)
            {
                _ = MessageBox.Show(ex.Message);
            }
        }
    }
}
