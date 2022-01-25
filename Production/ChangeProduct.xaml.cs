using Microsoft.Data.SqlClient;
using System;
using System.Data;
using System.Windows;

namespace Production
{

    public partial class ChangeProduct : Window
    {
        private bool isSaving = false;
        public bool IsSaving { get { return isSaving; } set { isSaving = value; } }
        string connectionString;
        Product thisProd;
        public ChangeProduct(ref Product product)
        {
            InitializeComponent();
            thisProd = product;
            connectionString = "Data Source=DESKTOP-N9D0K7G;Initial Catalog=Production;Integrated Security=true;TrustServerCertificate=True";
            TextBoxName.Text = thisProd.Name;
            TextBoxSerialNumber.Text = thisProd.SerialNumberData;
            TextBoxPrice.Text = thisProd.PriceData.ToString();
            TextBoxQuantity.Text = thisProd.StockQuantityData.ToString();
            TextBoxIsProd.Text = thisProd.IsProducedData ? "Производится" : "Снят с производства";
            TextBoxType.Text = thisProd.Type;
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
                String name = TextBoxName.Text;
                thisProd.Name = name;
                String sNum = TextBoxSerialNumber.Text;
                foreach (var symbol in sNum)
                    if (!char.IsDigit(symbol))
                    {
                        throw new Exception("Серийный номер может содержать только цифры");
                    }
                thisProd.SerialNumberData = sNum;
                thisProd.SerialNumber = "№ " + thisProd.SerialNumberData;
                decimal price = decimal.Parse(TextBoxPrice.Text);
                thisProd.PriceData = price;
                thisProd.Price = String.Format("{0:0.00}", thisProd.PriceData) + " руб";
                String type = TextBoxType.Text;
                thisProd.Type = type;
                int quan = Int32.Parse(TextBoxQuantity.Text.Trim());
                thisProd.StockQuantityData = quan;
                thisProd.StockQuantity = "В наличии на складе: " + quan.ToString();
                bool isProd = TextBoxIsProd.Text == "Производится";
                thisProd.IsProducedData = isProd;
                thisProd.IsProduced = isProd ? (quan == 0 ?
                            "Покупка под заказ" : "") : "Снят с производства";
                Guid typeId = thisProd.Id;
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
                    command.Parameters.Add(new SqlParameter { ParameterName = "@productId", Value = thisProd.Id, SqlDbType = SqlDbType.UniqueIdentifier });

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
