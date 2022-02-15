using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Production
{
    class OrderListViewModel : INotifyPropertyChanged
    {
        private Order selectedOrder;
        readonly string connectionString;

        public ObservableCollection<Order> Orders { get; set; }

        private RelayCommand addCommand;
        private RelayCommand changeCommand;
        private RelayCommand delCommand;
        private RelayCommand clickList;
        public RelayCommand AddCommand
        {
            get
            {
                return addCommand ??
                    (addCommand = new RelayCommand(obj =>
                    {
                        /*CreateEmployee createEmployee = new CreateEmployee();
                        if (createEmployee.IsSaving)
                        {
                            if (createEmployee.Role == "Seller")
                            {
                                CreateSellerList("Select UserId, EmployeeId, RoleId, Role.Name, Employee.Name, LastName, " +
                                                 "PhoneNumber, Email, PassportNumber, BirthDate, Employee.Adress, Employee.PostCode, " +
                                                 "ShopId, Shop.Name, Shop.Adress, Shop.PostCode, Login from [User] join Role " +
                                                 "on IdRole = RoleID join Employee on UserId = IdUser join Shop " +
                                                 "on ShopID = IdShop where Login = @login", createEmployee.Login);
                            }
                            else
                            {
                                CreateEmployeeList("Select UserId, EmployeeId, RoleId, Role.Name, Employee.Name, " +
                                                   "LastName, PhoneNumber, Email, PassportNumber, BirthDate, Adress, PostCode " +
                                                   "Login from [User] join Role on IdRole = RoleID join Employee " +
                                                   "on UserId = IdUser where Login = @login", createEmployee.Login);
                            }
                            SelectedOrder = Orders.LastOrDefault();
                        }*/
                    }));
            }
        }

        /*public RelayCommand ClickList
        {
            get
            {
                return clickList ??
                    (clickList = new RelayCommand(obj =>
                    {
                        _ = new OrderView(SelectedOrder);
                    }));
            }
        }*/

        public RelayCommand ChangeCommand
        {
            get
            {
                return changeCommand ??
                    (changeCommand = new RelayCommand(obj =>
                    {
                        if (selectedOrder != null)
                        {
                            //ChangeEmployee changeEmployee = new ChangeEmployee(ref selectedOrder);
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
                        if (selectedOrder != null)
                        {
                            AcceptWindow deleteOrder = new AcceptWindow(warning: $"Вы действительно хотите удалить заказ {selectedOrder.Number}");
                            if (deleteOrder.ShowDialog() == true)
                            {
                                try
                                {
                                    string sqlProc = "delete from [Order] where OrderId = @id";
                                    using (SqlConnection connection = new SqlConnection(connectionString))
                                    {
                                        connection.Open();

                                        SqlCommand command = new SqlCommand(sqlProc, connection);
                                        command.Parameters.Add(new SqlParameter
                                        {
                                            ParameterName = "@id",
                                            Value = SelectedOrder.Id,
                                            SqlDbType = SqlDbType.UniqueIdentifier
                                        });

                                        command.ExecuteNonQuery();
                                    }
                                    Orders.Remove(Orders.FirstOrDefault(w => w.Id == selectedOrder.Id));

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

        public Order SelectedOrder
        {
            get => selectedOrder;
            set
            {
                selectedOrder = value;
                selectedOrder.ProductList = new ObservableCollection<Prod>();
                string sql = "select ProductID, Quantity, Name, SerialNumber, Price from Product " +
                             "join ProductOrder on ProductID = idProduct " +
                             "join [Order] on idOrder = OrderID where OrderID = @id";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);
                    _ = command.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@id",
                        Value = selectedOrder.Id,
                        SqlDbType = SqlDbType.UniqueIdentifier
                    });
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            selectedOrder.ProductList.Add(new Prod());
                            selectedOrder.ProductList.LastOrDefault().Id = (Guid)reader.GetValue(0);
                            selectedOrder.ProductList.LastOrDefault().Quantity = (int)reader.GetValue(1);
                            selectedOrder.ProductList.LastOrDefault().Name = (string)reader.GetValue(2);
                            selectedOrder.ProductList.LastOrDefault().Number = (string)reader.GetValue(3);
                            selectedOrder.ProductList.LastOrDefault().Price = (decimal)reader.GetValue(4);
                        }

                        reader.Close();
                    }
                }
                OnPropertyChanged("SelectedOrder");
            }
        }

        public OrderListViewModel()
        {
            Orders = new ObservableCollection<Order>();
            connectionString = "Data Source=DESKTOP-N9D0K7G;Initial Catalog=Production;Integrated Security=true;TrustServerCertificate=True";

            CreateOrderList();

            /*if (isEmployee) CreateEmployeeList("Select UserId, EmployeeId, RoleId, Role.Name, Employee.Name, LastName, PhoneNumber, " +
                    "Email, PassportNumber, BirthDate, Adress, PostCode, Login from [User] join Role " +
                    "on IdRole = RoleID join Employee on UserId = IdUser");

            if (isSeller && string.IsNullOrEmpty(sql))
            {
                CreateSellerList("Select UserId, EmployeeId, RoleId, Role.Name, Employee.Name, LastName, " +
                                 "PhoneNumber, Email, PassportNumber, BirthDate, Employee.Adress, Employee.PostCode, " +
                                 "ShopId, Shop.Name, Shop.Adress, Shop.PostCode, Login from [User] join Role " +
                                 "on IdRole = RoleID join Employee on UserId = IdUser join Shop on ShopID = IdShop");
            }
            else
            {
                CreateSellerList(sql, shopId: shopId);
            }*/
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }


        private void CreateOrderList()
        {
            string sql = "Select OrderId, idCustomer, Customer.Name, Customer.LastName, Customer.PhoneNumber, " +
                            "OrderDate, OrderState.Name, totalPrice, idShop, Shop.Name " +
                            "from [Order] join Customer on IdCustomer = CustomerID join OrderState on StateId = IdState " +
                            "join Shop on ShopId = IdShop";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Orders.Add(new Order());
                        Orders.LastOrDefault().Id = (Guid)reader.GetValue(0);
                        Orders.LastOrDefault().Number = Math.Abs(Orders.LastOrDefault().Id.GetHashCode());
                        Orders.LastOrDefault().CustomerId = (Guid)reader.GetValue(1);
                        Orders.LastOrDefault().CustomerName = (string)reader.GetValue(2) + " " + (string)reader.GetValue(3);
                        Orders.LastOrDefault().Tel = (string)reader.GetValue(4);
                        Orders.LastOrDefault().OrderDate = (DateTime)reader.GetValue(5);
                        Orders.LastOrDefault().State = (string)reader.GetValue(6);
                        Orders.LastOrDefault().TotalPrice = (Decimal)reader.GetValue(7);
                        Orders.LastOrDefault().ShopId = (Guid)reader.GetValue(8);
                        Orders.LastOrDefault().ShopName = (string)reader.GetValue(9);
                    }

                    reader.Close();
                }
            }
        }
        /*private void CreateEmployeeList(string sql, string login = "")
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                if (login != "")
                {
                    _ = command.Parameters.Add(new SqlParameter { ParameterName = "@login", Value = login, SqlDbType = SqlDbType.VarChar });
                }
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if ((string)reader.GetValue(3) != "Seller")
                        {
                            Orders.Add(new Order());
                            Orders.LastOrDefault().Id = (Guid)reader.GetValue(0);
                            Orders.LastOrDefault().EmployeeId = (Guid)reader.GetValue(1);
                            Orders.LastOrDefault().RoleId = (Guid)reader.GetValue(2);
                            string role = (string)reader.GetValue(3);
                            switch (role)
                            {
                                case "Administrator":
                                    Orders.LastOrDefault().Role = "Администратор";
                                    Orders.LastOrDefault().RoleData = "Administrator";
                                    break;
                                case "Meneger":
                                    Orders.LastOrDefault().Role = "Менеджер";
                                    Orders.LastOrDefault().RoleData = "Meneger";
                                    break;
                            }
                            Orders.LastOrDefault().Name = (string)reader.GetValue(4);
                            Orders.LastOrDefault().LastName = (string)reader.GetValue(5);
                            Orders.LastOrDefault().PhoneNumber = (string)reader.GetValue(6);
                            Orders.LastOrDefault().Email = (string)reader.GetValue(7);
                            Orders.LastOrDefault().PassportNumberData = (string)reader.GetValue(8);
                            Orders.LastOrDefault().PassportNumber = "Паспорт: " + Orders.LastOrDefault().PassportNumberData;
                            Orders.LastOrDefault().BirthDate = (DateTime)reader.GetValue(9);
                            Orders.LastOrDefault().BirthDateStr = "Дата рождения: " + Orders.LastOrDefault().BirthDate.ToShortDateString().ToString();
                            Orders.LastOrDefault().AdressData = (string)reader.GetValue(10);
                            Orders.LastOrDefault().Adress = "Адрес: " + Orders.LastOrDefault().AdressData;
                            Orders.LastOrDefault().PostCodeData = (string)reader.GetValue(11);
                            Orders.LastOrDefault().PostCode = "Индекс: " + Orders.LastOrDefault().PostCodeData;
                            Orders.LastOrDefault().Login = (string)reader.GetValue(12);
                        }
                    }

                    reader.Close();
                }
            }
        }*/
        /*private void CreateSellerList(string sql, string login = "", Guid shopId = default)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                if (login != "")
                {
                    _ = command.Parameters.Add(new SqlParameter { ParameterName = "@login", Value = login, SqlDbType = SqlDbType.VarChar });
                }
                else if (shopId != default)
                {
                    _ = command.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@shop",
                        Value = shopId,
                        SqlDbType = SqlDbType.UniqueIdentifier
                    });
                }
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Orders.Add(new Order());
                        Orders.LastOrDefault().Id = (Guid)reader.GetValue(0);
                        Orders.LastOrDefault().EmployeeId = (Guid)reader.GetValue(1);
                        Orders.LastOrDefault().RoleId = (Guid)reader.GetValue(2);
                        Orders.LastOrDefault().Name = (string)reader.GetValue(4);
                        Orders.LastOrDefault().LastName = (string)reader.GetValue(5);
                        Orders.LastOrDefault().PhoneNumber = (string)reader.GetValue(6);
                        Orders.LastOrDefault().Email = (string)reader.GetValue(7);
                        Orders.LastOrDefault().PassportNumberData = (string)reader.GetValue(8);
                        Orders.LastOrDefault().PassportNumber = "Паспорт: " + Orders.LastOrDefault().PassportNumberData;
                        Orders.LastOrDefault().BirthDate = (DateTime)reader.GetValue(9);
                        Orders.LastOrDefault().BirthDateStr = "Дата рождения: " + Orders.LastOrDefault().BirthDate.ToShortDateString().ToString();
                        Orders.LastOrDefault().AdressData = (string)reader.GetValue(10);
                        Orders.LastOrDefault().Adress = "Адрес: " + Orders.LastOrDefault().AdressData;
                        Orders.LastOrDefault().PostCodeData = (string)reader.GetValue(11);
                        Orders.LastOrDefault().PostCode = "Индекс: " + Orders.LastOrDefault().PostCodeData;
                        Orders.LastOrDefault().Login = (string)reader.GetValue(16);
                        Orders.LastOrDefault().ShopId = (Guid)reader.GetValue(12);
                        Orders.LastOrDefault().ShopObj = new Shop
                        {
                            Id = Orders.LastOrDefault().ShopId,
                            Name = (string)reader.GetValue(13),
                            Adress = (string)reader.GetValue(14),
                            PostCode = (string)reader.GetValue(15)
                        };
                        if ((string)reader.GetValue(3) == "Seller")
                        {
                            Orders.LastOrDefault().Role = "Продавец в магазине \"" +
                                Orders.LastOrDefault().ShopObj.Name + "\"";
                            Orders.LastOrDefault().RoleData = "Seller";
                        }
                    }

                    reader.Close();
                }
            }
        }*/
    }
}
