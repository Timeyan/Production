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
    class UserListViewModel : INotifyPropertyChanged
    {
        private User selectedUser;
        string connectionString;

        public ObservableCollection<User> Users { get; set; }

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
                            Users.Clear();
                            //CreateUserList();
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
                        if (selectedUser != null)
                        {
                            //ChangeProduct changeProduct = new ChangeProduct(ref selectedUser);
                            //if (changeProduct.IsSaving)
                            //{
                            //    OnPropertyChanged("SelectedUser");
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
                        /*if (selectedProduct != null)
                        {
                            if (selectedProduct.IsProducedData)
                            {
                                try
                                {
                                    selectedProduct.IsProducedData = false;
                                    selectedProduct.IsProduced = "Снят с производства";
                                    OnPropertyChanged("SelectedProduct");
                                    string sqlProc = "if exists (select * from Product where ProductID = @productId) " +
                                                            "update [Product] set isProduced = 0 where ProductID = @productId";
                                    using (SqlConnection connection = new SqlConnection(connectionString))
                                    {
                                        connection.Open();

                                        SqlCommand command = new SqlCommand(sqlProc, connection);
                                        command.Parameters.Add(new SqlParameter
                                        {
                                            ParameterName = "@productId",
                                            Value = SelectedProduct.Id,
                                            SqlDbType = SqlDbType.UniqueIdentifier
                                        });

                                        command.ExecuteNonQuery();
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                        }*/
                    }));
            }
        }

        public User SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }

        public UserListViewModel(bool isEmployee, bool isSeller, bool isCustomer)
        {
            Users = new ObservableCollection<User>();
            connectionString = "Data Source=DESKTOP-N9D0K7G;Initial Catalog=Production;Integrated Security=true;TrustServerCertificate=True";
            if (isCustomer) CreateCustomerList();
            if (isEmployee) CreateEmployeeList();
            if (isSeller) CreateSellerList();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }


        private void CreateCustomerList()
        {
            string sql = "Select UserId, CustomerId, RoleId, Role.Name, Customer.Name, LastName, PhoneNumber, Email " +
                            "from [User] join Role on IdRole = RoleID join Customer on UserId = IdUser";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Users.Add(new User());
                        Users.LastOrDefault().Id = (Guid)reader.GetValue(0);
                        Users.LastOrDefault().CustomerId = (Guid)reader.GetValue(1);
                        Users.LastOrDefault().RoleId = (Guid)reader.GetValue(2);
                        Users.LastOrDefault().Role = (String)reader.GetValue(3);
                        Users.LastOrDefault().Name = (String)reader.GetValue(4);
                        Users.LastOrDefault().LastName = (String)reader.GetValue(5);
                        Users.LastOrDefault().PhoneNumber = (String)reader.GetValue(6);
                        Users.LastOrDefault().Email = (String)reader.GetValue(7);
                    }

                    reader.Close();
                }
            }
        }
        private void CreateEmployeeList()
        {
            string sql = "Select UserId, EmployeeId, RoleId, Role.Name, Employee.Name, LastName, PhoneNumber, " +
                    "Email, PassportNumber, BirthDate, Adress, PostCode from [User] join Role " +
                    "on IdRole = RoleID join Employee on UserId = IdUser";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        if ((String)reader.GetValue(3) != "Seller")
                        {
                            Users.Add(new User());
                            Users.LastOrDefault().Id = (Guid)reader.GetValue(0);
                            Users.LastOrDefault().EmployeeId = (Guid)reader.GetValue(1);
                            Users.LastOrDefault().RoleId = (Guid)reader.GetValue(2);
                            Users.LastOrDefault().Role = (String)reader.GetValue(3);
                            Users.LastOrDefault().Name = (String)reader.GetValue(4);
                            Users.LastOrDefault().LastName = (String)reader.GetValue(5);
                            Users.LastOrDefault().PhoneNumber = (String)reader.GetValue(6);
                            Users.LastOrDefault().Email = (String)reader.GetValue(7);
                            Users.LastOrDefault().PassportNumberData = (String)reader.GetValue(8);
                            Users.LastOrDefault().PassportNumber = "Паспорт: " + Users.LastOrDefault().PassportNumberData;
                            Users.LastOrDefault().BirthDate = (DateTime)reader.GetValue(9);
                            Users.LastOrDefault().BirthDateStr = "Дата рождения: " + Users.LastOrDefault().BirthDate.ToShortDateString().ToString();
                            Users.LastOrDefault().AdressData = (String)reader.GetValue(10);
                            Users.LastOrDefault().Adress = "Адрес: " + Users.LastOrDefault().AdressData;
                            Users.LastOrDefault().PostCodeData = (String)reader.GetValue(11);
                            Users.LastOrDefault().PostCode = "Индекс: " + Users.LastOrDefault().PostCodeData;
                        }
                    }

                    reader.Close();
                }
            }
        }
        private void CreateSellerList()
        {
            string sql = "Select UserId, EmployeeId, RoleId, Role.Name, Employee.Name, LastName, " +
                    "PhoneNumber, Email, PassportNumber, BirthDate, Employee.Adress, Employee.PostCode, " +
                    "ShopId, Shop.Name, Shop.Adress, Shop.PostCode from [User] join Role on IdRole = RoleID join Employee " +
                    "on UserId = IdUser join Shop on ShopID = IdShop";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Users.Add(new User());
                        Users.LastOrDefault().Id = (Guid)reader.GetValue(0);
                        Users.LastOrDefault().EmployeeId = (Guid)reader.GetValue(1);
                        Users.LastOrDefault().RoleId = (Guid)reader.GetValue(2);
                        Users.LastOrDefault().Role = (String)reader.GetValue(3);
                        Users.LastOrDefault().Name = (String)reader.GetValue(4);
                        Users.LastOrDefault().LastName = (String)reader.GetValue(5);
                        Users.LastOrDefault().PhoneNumber = (String)reader.GetValue(6);
                        Users.LastOrDefault().Email = (String)reader.GetValue(7);
                        Users.LastOrDefault().PassportNumberData = (String)reader.GetValue(8);
                        Users.LastOrDefault().PassportNumber = "Паспорт: " + Users.LastOrDefault().PassportNumberData;
                        Users.LastOrDefault().BirthDate = (DateTime)reader.GetValue(9);
                        Users.LastOrDefault().BirthDateStr = "Дата рождения: " + Users.LastOrDefault().BirthDate.ToShortDateString().ToString();
                        Users.LastOrDefault().AdressData = (String)reader.GetValue(10);
                        Users.LastOrDefault().Adress = "Адрес: " + Users.LastOrDefault().AdressData;
                        Users.LastOrDefault().PostCodeData = (String)reader.GetValue(11);
                        Users.LastOrDefault().PostCode = "Индекс: " + Users.LastOrDefault().PostCodeData;
                        Users.LastOrDefault().ShopObj = new Shop
                        {
                            Id = (Guid)reader.GetValue(12),
                            Name = (String)reader.GetValue(13),
                            Adress = (String)reader.GetValue(13),
                            PostCode = (String)reader.GetValue(13)
                        };
                    }

                    reader.Close();
                }
            }
        }
    }
}
