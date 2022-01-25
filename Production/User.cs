using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Production
{
    public class User : INotifyPropertyChanged
    {
        readonly string connectionString;
        Dictionary<Guid, string> roles;


        private Guid id; //не доступно в интерфейсе
        private Guid customerId; //не доступно в интерфейсе
        private Guid employeeId; //не доступно в интерфейсе
        private Guid shopId; //не доступно в интерфейсе
        private Guid roleId; //не доступно в интерфейсе
        private Shop shopObj;
        private string name;
        private string lastName;
        private string phoneNumber;
        private string email;
        private string passportNumber;
        private DateTime birthDate; //не доступно в интерфейсе
        private string birthDateStr;
        private string adress;
        private string postCode;
        private string role;
        private string roleData;
        private string login;

        public User()
        {
            roles = new Dictionary<Guid, string>();

            connectionString = "Data Source=DESKTOP-N9D0K7G;Initial Catalog=Production;Integrated Security=true;TrustServerCertificate=True";
            string sql = "select RoleID, Name from Role";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        roles.Add((Guid)reader.GetValue(0), (String)reader.GetValue(1));
                    }

                    reader.Close();
                }
                command.CommandText = "select ShopID, Name, Adress, PostCode from Shop";
            }

        }


        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public Guid Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        public Guid CustomerId
        {
            get => customerId;
            set
            {
                customerId = value;
                OnPropertyChanged("CustomerId");
            }
        }
        public Guid EmployeeId
        {
            get => employeeId;
            set
            {
                employeeId = value;
                OnPropertyChanged("EmployeeId");
            }
        }

        public Guid ShopId
        {
            get => shopId;
            set
            {
                shopId = value;
                OnPropertyChanged("ShopId");
            }
        }
        public Guid RoleId
        {
            get => roleId;
            set
            {
                roleId = value;
                OnPropertyChanged("RoleId");
            }
        }
        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                OnPropertyChanged("LastName");
            }
        }

        public string PhoneNumber
        {
            get => phoneNumber;
            set
            {
                phoneNumber = value;
                OnPropertyChanged("PhoneNumber");
            }
        }

        public string Email
        {
            get => email;
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }

        public string PassportNumber
        {
            get => passportNumber;
            set
            {
                passportNumber = value;
                OnPropertyChanged("PassportNumber");
            }
        }
        public string PassportNumberData { get; set; }

        public DateTime BirthDate
        {
            get => birthDate;
            set
            {
                birthDate = value;
                OnPropertyChanged("BirthDate");
            }
        }
        public string BirthDateStr
        {
            get => birthDateStr;
            set
            {
                birthDateStr = value;
                OnPropertyChanged("BirthDateStr");
            }
        }

        public string Adress
        {
            get => adress;
            set
            {
                adress = value;
                OnPropertyChanged("Adress");
            }
        }
        public string AdressData { get; set; }

        public string PostCode
        {
            get => postCode;
            set
            {
                postCode = value;
                OnPropertyChanged("PostCode");
            }
        }
        public string PostCodeData { get; set; }

        public string Role
        {
            get => role;
            set
            {
                role = value;
                OnPropertyChanged("Role");
            }
        }

        public string RoleData
        {
            get => roleData;
            set
            {
                roleData = value;
                OnPropertyChanged("RoleData");
            }
        }

        public string Login
        {
            get => login;
            set
            {
                login = value;
                OnPropertyChanged("Login");
            }
        }

        public Shop ShopObj
        {
            get => shopObj;
            set
            {
                shopObj = value;
                OnPropertyChanged("ShopObj");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
