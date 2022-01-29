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
    class ShopListViewModel : INotifyPropertyChanged
    {
        private Shop selectedShop;
        readonly string connectionString;

        public ObservableCollection<Shop> Shops { get; set; }

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
                        CreateShop createShop = new CreateShop();
                        if (createShop.IsSaving)
                        {
                            CreateShopList();
                            SelectedShop = Shops.LastOrDefault();
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
                        if (selectedShop.EmployeeList != null && selectedShop.EmployeeList.Count != 0)
                        {
                            UserList userList = new UserList(sql: "Select UserId, EmployeeId, RoleId, Role.Name, " +
                                            "Employee.Name, LastName, PhoneNumber, Email, PassportNumber, BirthDate, " +
                                            "Employee.Adress, Employee.PostCode, ShopId, Shop.Name, Shop.Adress, " +
                                            "Shop.PostCode, Login from [User] join Role on IdRole = RoleID join Employee " +
                                            "on UserId = IdUser join Shop on ShopID = IdShop where ShopId = @shop", 
                                            id: selectedShop.Id);
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
                        if (selectedShop != null)
                        {
                            AcceptWindow deleteShop = new AcceptWindow(warning: $"Вы действительно хотите уволить {selectedShop.Name}");
                            if (deleteShop.ShowDialog() == true)
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
                                            Value = SelectedShop.Id,
                                            SqlDbType = SqlDbType.UniqueIdentifier
                                        });

                                        command.ExecuteNonQuery();
                                    }
                                    Shops.Remove(Shops.FirstOrDefault(w => w.Id == selectedShop.Id));

                                }
                                catch (Exception ex)
                                {
                                    _ = MessageBox.Show(ex.Message);
                                }
                            }
                        }
                    }));
            }
        }

        public Shop SelectedShop
        {
            get => selectedShop;
            set
            {
                selectedShop = value;
                selectedShop.EmployeeList = new ObservableCollection<string>();
                string sql = "select EmployeeId, Name, LastName from Employee where IdShop = @shop";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);
                    _ = command.Parameters.Add(new SqlParameter
                    {
                        ParameterName = "@shop",
                        Value = selectedShop.Id,
                        SqlDbType = SqlDbType.UniqueIdentifier
                    });
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            selectedShop.EmployeeList.Add((string)reader.GetValue(2) + " " + (string)reader.GetValue(1));
                        }
                        reader.Close();
                    }
                }
                switch (selectedShop.EmployeeList.Count)
                {
                    case 0:
                        break;
                    case 1:
                        selectedShop.First = selectedShop.EmployeeList[0];
                        goto case 0;
                    case 2:
                        selectedShop.Second = selectedShop.EmployeeList[1];
                        goto case 1;
                    case 3:
                        selectedShop.Third = selectedShop.EmployeeList[2];
                        goto case 2;
                    default:
                        selectedShop.Ellipsis = "...";
                        goto case 3;
                }
                OnPropertyChanged("SelectedShop");
            }
        }

        public ShopListViewModel()
        {
            Shops = new ObservableCollection<Shop>();
            connectionString = "Data Source=DESKTOP-N9D0K7G;Initial Catalog=Production;Integrated Security=true;TrustServerCertificate=True";

            CreateShopList();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }


        private void CreateShopList()
        {
            string sql = "Select ShopId, Name, Adress, Postcode from [Shop]";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        Shops.Add(new Shop());
                        Shops.LastOrDefault().Id = (Guid)reader.GetValue(0);
                        Shops.LastOrDefault().Name = (string)reader.GetValue(1);
                        Shops.LastOrDefault().Adress = (string)reader.GetValue(2);
                        Shops.LastOrDefault().PostCode = (string)reader.GetValue(3);
                    }

                    reader.Close();
                }
            }
        }
    }
}
