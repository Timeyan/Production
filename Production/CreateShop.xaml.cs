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
            connectionString = "Data Source=DESKTOP-N9D0K7G;Initial Catalog=Production;" +
                               "Integrated Security=true;TrustServerCertificate=True";
            string sql = @"Select Employee.Name, LastName from [Employee] join [Shop] on ShopId = IdShop where ShopId is not null";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        _ = TextBoxSellers.Items.Add((string)reader.GetValue(0) + " " + (string)reader.GetValue(1));
                    }

                    reader.Close();
                }
            }
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
                List<string> sellers = new List<string>
                {
                    TextBoxSellers.Text
                };
                if (name == "" || adress == "" || postCode == "") throw new Exception("Не все поля заполнены");

                string sqlProc = "insert into [Shop] values(@name, @adress, @postCode)";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    SqlCommand command = new SqlCommand(sqlProc, connection);
                    _ = command.Parameters.Add(new SqlParameter { ParameterName = "@name", Value = name, SqlDbType = SqlDbType.NVarChar });
                    _ = command.Parameters.Add(new SqlParameter { ParameterName = "@adress", Value = adress, SqlDbType = SqlDbType.NVarChar });
                    _ = command.Parameters.Add(new SqlParameter { ParameterName = "@postCode", Value = postCode, SqlDbType = SqlDbType.VarChar });
                    //command.Parameters.Add(new SqlParameter { ParameterName = "@type", Value = type, SqlDbType = SqlDbType.NVarChar });

                    _ = await command.ExecuteNonQueryAsync();
                }
                _ = MessageBox.Show("Данные успешно сохранены");
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
