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
    /// Interaction logic for CreateProduct.xaml
    /// </summary>
    public partial class CreateProduct : Window
    {
        private bool isSaving = false;
        public bool IsSaving { get { return isSaving; } set { isSaving = value; } }
        string connectionString;
        public CreateProduct()
        {
            InitializeComponent();
            connectionString = "Data Source=DESKTOP-N9D0K7G;Initial Catalog=Production;Integrated Security=true;TrustServerCertificate=True";
            this.ShowDialog();
        }

        private void EnterClickBack(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private async void EnterClickAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                String name = TextBoxName.Text;
                String sNum = TextBoxSerialNumber.Text;
                String price = TextBoxPrice.Text;
                String type = TextBoxType.Text;
                if (name == "" || sNum == "" || price == "" || type == "") throw new Exception("Должны быть заполнены все поля");

                string sqlProc = "insert into [Product] values(default, @name, @number, default, @price, default, (select ProductTypeID from ProductType where name = @type))";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    SqlCommand command = new SqlCommand(sqlProc, connection);
                    command.Parameters.Add(new SqlParameter { ParameterName = "@name", Value = name, SqlDbType = SqlDbType.NVarChar });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@number", Value = sNum, SqlDbType = SqlDbType.VarChar });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@price", Value = price, SqlDbType = SqlDbType.Money });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@type", Value = type, SqlDbType = SqlDbType.NVarChar });

                    await command.ExecuteNonQueryAsync();
                    //command.CommandText
                }
                MessageBox.Show("Данные успешно сохранены");
                IsSaving = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
