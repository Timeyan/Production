using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Windows;

namespace Production
{
    /// <summary>
    /// Interaction logic for ChangeProduct.xaml
    /// </summary>
    public partial class ChangeProduct : Window
    {
        private bool isSaving = false;
        public bool IsSaving { get { return isSaving; } set { isSaving = value; } }
        string connectionString;
        Guid productId;
        public ChangeProduct(ref Product product)
        {
            InitializeComponent();
            connectionString = "Data Source=DESKTOP-N9D0K7G;Initial Catalog=Production;Integrated Security=true;TrustServerCertificate=True";
            TextBoxName.Text = product.Name;
            TextBoxSerialNumber.Text = product.SerialNumberData;
            TextBoxPrice.Text = product.PriceData.ToString();
            TextBoxQuantity.Text = product.StockQuantityData.ToString();
            TextBoxIsProd.Text = product.IsProducedData ? "Производится" : "Снят с производства";
            TextBoxType.Text = product.Type;
            productId = product.Id;
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
                bool flag = true;
                foreach (var symbol in sNum)
                    if (!char.IsDigit(symbol))
                    {
                        flag = false;
                        throw new Exception("Серийный номер может содержать только цифры");
                    }
                decimal price = decimal.Parse(TextBoxPrice.Text);
                String type = TextBoxType.Text;
                int quan = Int32.Parse(TextBoxQuantity.Text.Trim());
                bool isProd = TextBoxIsProd.Text == "Производится";
                Guid typeId = productId;
                if (name == "" || sNum == "" || type == "") 
                    throw new Exception("Должны быть заполнены все поля");

                string sqlProc = "select ProductTypeId from [ProductType] where Name = @type";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    SqlCommand command = new SqlCommand(sqlProc, connection);
                    command.Parameters.Add(new SqlParameter { ParameterName = "@type", Value = type, SqlDbType = SqlDbType.NVarChar });
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        reader.Read();
                        typeId = (Guid)reader.GetValue(0);
                    }
                    reader.Close();

                    command.Parameters.Clear();
                    command.CommandText = "update Product set Name = @name, SerialNumber = @sNum, Price = @price, StockQuantity = @quan, " +
                    "IdProductType = @typeId, isProduced = @isProd where ProductID = @productId";
                    command.Parameters.Add(new SqlParameter { ParameterName = "@name", Value = name, SqlDbType = SqlDbType.NVarChar });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@sNum", Value = sNum, SqlDbType = SqlDbType.VarChar });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@price", Value = price, SqlDbType = SqlDbType.Money });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@typeId", Value = typeId, SqlDbType = SqlDbType.UniqueIdentifier });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@quan", Value = quan, SqlDbType = SqlDbType.Int });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@isProd", Value = isProd, SqlDbType = SqlDbType.Bit });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@productId", Value = productId, SqlDbType = SqlDbType.UniqueIdentifier });

                    await command.ExecuteNonQueryAsync();
                }
                MessageBox.Show("Данные успешно сохранены");
                IsSaving = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
