using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Runtime.CompilerServices;

namespace Production
{
    class User : INotifyPropertyChanged
    {
        String connectionString;
        Dictionary<Guid, String> roles;


        private Guid id;
        private Guid customerId;
        private Guid employeeId;
        private Guid shopId;
        private Guid roleId;
        private Shop shopObj;
        private string name;
        private string lastName;
        private string phoneNumber;
        private string email;
        private string passportNumber;
        private string passportNumberData;
        private DateTime birthDate;
        private string birthDateStr;
        private string adress;
        private string adressData;
        private string postCode;
        private string role;
        private string postCodeData;

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
            get { return name; }
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public Guid Id
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        public Guid CustomerId
        {
            get { return customerId; }
            set
            {
                customerId = value;
                OnPropertyChanged("CustomerId");
            }
        }
        public Guid EmployeeId
        {
            get { return employeeId; }
            set
            {
                employeeId = value;
                OnPropertyChanged("EmployeeId");
            }
        }

        public Guid ShopId
        {
            get { return shopId; }
            set
            {
                shopId = value;
                OnPropertyChanged("ShopId");
            }
        }
        public Guid RoleId
        {
            get { return roleId; }
            set
            {
                roleId = value;
                OnPropertyChanged("RoleId");
            }
        }
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                OnPropertyChanged("LastName");
            }
        }

        public string PhoneNumber
        {
            get { return phoneNumber; }
            set
            {
                phoneNumber = value;
                OnPropertyChanged("PhoneNumber");
            }
        }

        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }

        public string PassportNumber
        {
            get { return passportNumber; }
            set
            {
                passportNumber = value;
                OnPropertyChanged("PassportNumber");
            }
        }
        public string PassportNumberData
        {
            get { return passportNumberData; }
            set
            {
                passportNumberData = value;
            }
        }

        public DateTime BirthDate
        {
            get { return birthDate; }
            set
            {
                birthDate = value;
                OnPropertyChanged("BirthDate");
            }
        }
        public string BirthDateStr
        {
            get { return birthDateStr; }
            set
            {
                birthDateStr = value;
                OnPropertyChanged("BirthDateStr");
            }
        }

        public string Adress
        {
            get { return adress; }
            set
            {
                adress = value;
                OnPropertyChanged("Adress");
            }
        }
        public string AdressData
        {
            get { return adressData; }
            set
            {
                adressData = value;
            }
        }

        public string PostCode
        {
            get { return postCode; }
            set
            {
                postCode = value;
                OnPropertyChanged("PostCode");
            }
        }
        public string PostCodeData
        {
            get { return postCodeData; }
            set
            {
                postCodeData = value;
            }
        }

        public string Role
        {
            get { return role; }
            set
            {
                role = value;
                OnPropertyChanged("Role");
            }
        }

        public Shop ShopObj
        {
            get { return shopObj; }
            set
            {
                shopObj = value;
                OnPropertyChanged("ShopObj");
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }

    }

}
