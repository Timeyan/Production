using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Configuration;
using System.Data;
using System.Windows.Forms;

namespace Production
{
    class ApplicationViewModel: INotifyPropertyChanged
    {
        private Product selectedProduct;
        string connectionString;

        public ObservableCollection<Product> Products { get; set; }

        private RelayCommand addCommand;
        private RelayCommand changeCommand;
        private RelayCommand delCommand;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                    (addCommand = new RelayCommand(obj =>
                    {
                        CreateProduct createProduct = new CreateProduct();
                        if (createProduct.IsSaving)
                        {
                            Products.Clear();
                            CreateProductList();
                        }
                    }));
            }
        }

        public RelayCommand ChangeCommand
        {
            get
            {
                return changeCommand ??
                    (changeCommand = new RelayCommand(obj =>
                    {
                        if (selectedProduct != null)
                        {
                            ChangeProduct changeProduct = new ChangeProduct(ref selectedProduct);
                            //if (changeProduct.IsSaving)
                            //{
                            //    OnPropertyChanged("SelectedProduct");
                            //}
                        }
                    }));
            }
        }

        public RelayCommand DelCommand
        {
            get
            {
                return delCommand ??
                    (delCommand = new RelayCommand(obj =>
                    {
                        if (selectedProduct != null)
                        {
                            if (selectedProduct.IsProducedData)
                            {
                                try {
                                    selectedProduct.IsProducedData = false;
                                    selectedProduct.IsProduced = "Снят с производства";
                                    OnPropertyChanged("SelectedProduct");
                                    string sqlProc = "if exists (select * from Product where ProductID = @productId) " +
                                                            "update [Product] set isProduced = 0 where ProductID = @productId";
                                    using (SqlConnection connection = new SqlConnection(connectionString))
                                    {
                                        connection.Open();

                                        SqlCommand command = new SqlCommand(sqlProc, connection);
                                        command.Parameters.Add(new SqlParameter { ParameterName = "@productId", 
                                                                                  Value = SelectedProduct.Id, 
                                                                                  SqlDbType = SqlDbType.UniqueIdentifier });

                                        command.ExecuteNonQuery();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                        }
                    }));
            }
        }

        public Product SelectedProduct
        {
            get { return selectedProduct; }
            set
            {
                selectedProduct = value;
                OnPropertyChanged("SelectedProduct");
            }
        }

        public ApplicationViewModel()
        {
            Products = new ObservableCollection<Product>();
            CreateProductList();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


        private void CreateProductList()
        {
            connectionString = ConfigurationManager.ConnectionStrings["ProductionConnectionString"].ConnectionString;
            string sql = "select productId, Product.name, serialNumber, isProduced, Price, StockQuantity, ProductType.Name as Type from Product join ProductType on IdProductType = ProductTypeID";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Products.Add(new Product());
                        Products.LastOrDefault().StockQuantityData = (int)reader.GetValue(5);
                        int quan = Products.LastOrDefault().StockQuantityData;
                        Products.LastOrDefault().StockQuantity = "В наличии на складе: " + quan.ToString();
                        Products.LastOrDefault().Id = (Guid)reader.GetValue(0);
                        Products.LastOrDefault().Name = reader.GetValue(1) + "";
                        Products.LastOrDefault().Type = reader.GetValue(6) + "";
                        Products.LastOrDefault().SerialNumberData = reader.GetValue(2) + "";
                        Products.LastOrDefault().SerialNumber = "№ " + Products.LastOrDefault().SerialNumberData;
                        Products.LastOrDefault().IsProducedData = (bool)reader.GetValue(3);
                        Products.LastOrDefault().IsProduced = Products.LastOrDefault().IsProducedData ? (quan == 0 ? 
                            "Покупка под заказ" : "") : "Снят с производства";
                        Products.LastOrDefault().PriceData = (decimal)reader.GetValue(4);
                        Products.LastOrDefault().Price = String.Format("{0:0.00}", Products.LastOrDefault().PriceData) + " руб";
                    }

                    reader.Close();
                }
            }
        }
    }
}
