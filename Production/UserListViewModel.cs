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
        readonly string connectionString;

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
                        CreateEmployee createEmployee = new CreateEmployee();
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
                            SelectedUser = Users.LastOrDefault();
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
                            ChangeEmployee changeEmployee = new ChangeEmployee(ref selectedUser);
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
                        if (selectedUser != null)
                        {
                            AcceptWindow deleteUser = new AcceptWindow(warning: $"Вы действительно хотите уволить {selectedUser.Name}");
                            if (deleteUser.ShowDialog() == true)
                            {
                                try
                                {
                                    string sqlProc = "delete from Employee where IdUser = @id delete from [User] where UserID = @id";
                                    using (SqlConnection connection = new SqlConnection(connectionString))
                                    {
                                        connection.Open();

                                        SqlCommand command = new SqlCommand(sqlProc, connection);
                                        command.Parameters.Add(new SqlParameter
                                        {
                                            ParameterName = "@id",
                                            Value = SelectedUser.Id,
                                            SqlDbType = SqlDbType.UniqueIdentifier
                                        });

                                        command.ExecuteNonQuery();
                                    }
                                    Users.Remove(Users.FirstOrDefault(w => w.Id == selectedUser.Id));

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

        public User SelectedUser
        {
            get { return selectedUser; }
            set
            {
                selectedUser = value;
                OnPropertyChanged("SelectedUser");
            }
        }

        public UserListViewModel(bool isEmployee, bool isSeller, bool isCustomer, string sql = "", Guid shopId = default)
        {
            Users = new ObservableCollection<User>();
            connectionString = "Data Source=DESKTOP-N9D0K7G;Initial Catalog=Production;Integrated Security=true;TrustServerCertificate=True";
            
            if (isCustomer) CreateCustomerList();

            if (isEmployee) CreateEmployeeList("Select UserId, EmployeeId, RoleId, Role.Name, Employee.Name, LastName, PhoneNumber, " +
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
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }


        private void CreateCustomerList()
        {
            string sql = "Select UserId, CustomerId, RoleId, Role.Name, Customer.Name, LastName, PhoneNumber, Email, " +
                            "Login from [User] join Role on IdRole = RoleID join Customer on UserId = IdUser";
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
                        if ((string)reader.GetValue(3) == "Customer")
                        {
                            Users.LastOrDefault().Role = "Клиент";
                            Users.LastOrDefault().RoleData = "Customer";
                        }
                        Users.LastOrDefault().Name = (string)reader.GetValue(4);
                        Users.LastOrDefault().LastName = (string)reader.GetValue(5);
                        Users.LastOrDefault().PhoneNumber = (string)reader.GetValue(6);
                        Users.LastOrDefault().Email = (string)reader.GetValue(7);
                        Users.LastOrDefault().Login = (string)reader.GetValue(8);
                    }

                    reader.Close();
                }
            }
        }
        private void CreateEmployeeList(string sql, string login = "")
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
                            Users.Add(new User());
                            Users.LastOrDefault().Id = (Guid)reader.GetValue(0);
                            Users.LastOrDefault().EmployeeId = (Guid)reader.GetValue(1);
                            Users.LastOrDefault().RoleId = (Guid)reader.GetValue(2);
                            string role = (string)reader.GetValue(3);
                            switch (role) 
                            {
                                case "Administrator":
                                    Users.LastOrDefault().Role = "Администратор";
                                    Users.LastOrDefault().RoleData = "Administrator";
                                    break;
                                case "Meneger":
                                    Users.LastOrDefault().Role = "Менеджер";
                                    Users.LastOrDefault().RoleData = "Meneger";
                                    break;
                            }
                            Users.LastOrDefault().Name = (string)reader.GetValue(4);
                            Users.LastOrDefault().LastName = (string)reader.GetValue(5);
                            Users.LastOrDefault().PhoneNumber = (string)reader.GetValue(6);
                            Users.LastOrDefault().Email = (string)reader.GetValue(7);
                            Users.LastOrDefault().PassportNumberData = (string)reader.GetValue(8);
                            Users.LastOrDefault().PassportNumber = "Паспорт: " + Users.LastOrDefault().PassportNumberData;
                            Users.LastOrDefault().BirthDate = (DateTime)reader.GetValue(9);
                            Users.LastOrDefault().BirthDateStr = "Дата рождения: " + Users.LastOrDefault().BirthDate.ToShortDateString().ToString();
                            Users.LastOrDefault().AdressData = (string)reader.GetValue(10);
                            Users.LastOrDefault().Adress = "Адрес: " + Users.LastOrDefault().AdressData;
                            Users.LastOrDefault().PostCodeData = (string)reader.GetValue(11);
                            Users.LastOrDefault().PostCode = "Индекс: " + Users.LastOrDefault().PostCodeData;
                            Users.LastOrDefault().Login = (string)reader.GetValue(12);
                        }
                    }

                    reader.Close();
                }
            }
        }
        private void CreateSellerList(string sql, string login = "", Guid shopId = default)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                if (login != "")
                {
                    _ = command.Parameters.Add(new SqlParameter { ParameterName = "@login", Value = login, SqlDbType = SqlDbType.VarChar });
                } else if (shopId != default)
                {
                    _ = command.Parameters.Add(new SqlParameter { ParameterName = "@shop", Value = shopId, 
                                                                  SqlDbType = SqlDbType.UniqueIdentifier });
                }
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Users.Add(new User());
                        Users.LastOrDefault().Id = (Guid)reader.GetValue(0);
                        Users.LastOrDefault().EmployeeId = (Guid)reader.GetValue(1);
                        Users.LastOrDefault().RoleId = (Guid)reader.GetValue(2);
                        Users.LastOrDefault().Name = (string)reader.GetValue(4);
                        Users.LastOrDefault().LastName = (string)reader.GetValue(5);
                        Users.LastOrDefault().PhoneNumber = (string)reader.GetValue(6);
                        Users.LastOrDefault().Email = (string)reader.GetValue(7);
                        Users.LastOrDefault().PassportNumberData = (string)reader.GetValue(8);
                        Users.LastOrDefault().PassportNumber = "Паспорт: " + Users.LastOrDefault().PassportNumberData;
                        Users.LastOrDefault().BirthDate = (DateTime)reader.GetValue(9);
                        Users.LastOrDefault().BirthDateStr = "Дата рождения: " + Users.LastOrDefault().BirthDate.ToShortDateString().ToString();
                        Users.LastOrDefault().AdressData = (string)reader.GetValue(10);
                        Users.LastOrDefault().Adress = "Адрес: " + Users.LastOrDefault().AdressData;
                        Users.LastOrDefault().PostCodeData = (string)reader.GetValue(11);
                        Users.LastOrDefault().PostCode = "Индекс: " + Users.LastOrDefault().PostCodeData;
                        Users.LastOrDefault().Login = (string)reader.GetValue(16);
                        Users.LastOrDefault().ShopId = (Guid)reader.GetValue(12);
                        Users.LastOrDefault().ShopObj = new Shop
                        {
                            Id = Users.LastOrDefault().ShopId,
                            Name = (string)reader.GetValue(13),
                            Adress = (string)reader.GetValue(14),
                            PostCode = (string)reader.GetValue(15)
                        };
                        if ((string)reader.GetValue(3) == "Seller")
                        {
                            Users.LastOrDefault().Role = "Продавец в магазине \"" + 
                                Users.LastOrDefault().ShopObj.Name + "\"";
                            Users.LastOrDefault().RoleData = "Seller";
                        }
                    }

                    reader.Close();
                }
            }
        }
    }
}
