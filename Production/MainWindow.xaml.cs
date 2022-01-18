using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Production
{
    public partial class MainWindow : Window
    {
        string userRole = "";
        Guid userId;
        string userLogin = "";

        string connectionString;
        SqlDataAdapter adapter;
        public MainWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["ProductionConnectionString"].ConnectionString;
        }


        private void EnterClick(object sender, RoutedEventArgs e)
        {
            userLogin = TextBoxLogin.Text;
            string otherPass = TextBoxPassword.Password;
            string password = BCrypt.Net.BCrypt.HashPassword(TextBoxPassword.Password/*, BCrypt.Net.BCrypt.GenerateSalt()*/);
            try
            {
                string sql = "SELECT UserID, Name as Role FROM [User] join Role on RoleID = IdRole where login = @login and password = @pass";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(sql, connection);
                    SqlParameter loginParam = new SqlParameter { ParameterName = "@login", SqlDbType = SqlDbType.VarChar, Value = userLogin };
                    command.Parameters.Add(loginParam);
                    SqlParameter passParam = new SqlParameter { ParameterName = "@pass", SqlDbType = SqlDbType.VarChar, Value = otherPass };
                    command.Parameters.Add(passParam);
                    SqlDataReader reader = command.ExecuteReader();
                    bool b = reader.HasRows;
                    reader.Read();
                    userId = (Guid)reader.GetValue(0);
                    userRole = reader.GetValue(1) + "";
                    UserInfo.UserId = userId;
                    UserInfo.UserRole = userRole;

                    reader.Close();
                }
                switch (userRole)
                {
                    case "Seller":
                        sql = "SELECT Employee.Name, LastName, Email, BirthDate, Employee.Adress, PhoneNumber, PassportNumber, Employee.PostCode, Shop.Name as Shop " +
                            "FROM Employee join Shop on ShopId = IdShop where Iduser = @userid";
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            SqlCommand command = new SqlCommand(sql, connection);
                            SqlParameter idParam = new SqlParameter { ParameterName = "@userid", Value = userId };
                            command.Parameters.Add(idParam);
                            SqlDataReader reader = command.ExecuteReader();
                            reader.Read();
                            UserInfo.UserName = reader["Name"] + "";
                            UserInfo.UserLastName = reader["LastName"] + "";
                            UserInfo.UserEmail = reader["Email"] + "";
                            UserInfo.UserAddress = reader["Adress"] + "";
                            UserInfo.UserPhone = reader["PhoneNumber"] + "";
                            UserInfo.UserPassport = reader["PassportNumber"] + "";
                            UserInfo.UserPostCode = reader["PostCode"] + "";
                            UserInfo.UserShop = reader["Shop"] + "";
                            UserInfo.UserBirthDate = reader["BirthDate"] + "";

                            reader.Close();
                        }
                        SellerMenuWindow selWindow = new SellerMenuWindow();
                        selWindow.Show();
                        Hide();
                        break;
                    case "Administrator":
                        sql = "SELECT Employee.Name, LastName, Email, BirthDate, Adress, PhoneNumber, PassportNumber, PostCode " +
                                "FROM Employee where Iduser = @userid";
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            SqlCommand command = new SqlCommand(sql, connection);
                            SqlParameter idParam = new SqlParameter { ParameterName = "@userid", Value = userId };
                            command.Parameters.Add(idParam);
                            SqlDataReader reader = command.ExecuteReader();
                            reader.Read();
                            UserInfo.UserName = reader["Name"] + "";
                            UserInfo.UserLastName = reader["LastName"] + "";
                            UserInfo.UserEmail = reader["Email"] + "";
                            UserInfo.UserAddress = reader["Adress"] + "";
                            UserInfo.UserPhone = reader["PhoneNumber"] + "";
                            UserInfo.UserPassport = reader["PassportNumber"] + "";
                            UserInfo.UserPostCode = reader["PostCode"] + "";
                            UserInfo.UserBirthDate = reader["BirthDate"] + "";

                            reader.Close();
                        }
                        AdminMenuWindow admWindow = new AdminMenuWindow();
                        admWindow.Show();
                        Hide();
                        break;
                    case "Customer":
                        sql = "SELECT Name, LastName, Email, PhoneNumber " +
                                "FROM Customer where Iduser = @userid";
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            SqlCommand command = new SqlCommand(sql, connection);
                            SqlParameter idParam = new SqlParameter { ParameterName = "@userid", SqlDbType = SqlDbType.UniqueIdentifier, Value = userId };
                            command.Parameters.Add(idParam);
                            SqlDataReader reader = command.ExecuteReader();
                            reader.Read();
                            UserInfo.UserName = reader["Name"] + "";
                            UserInfo.UserLastName = reader["LastName"] + "";
                            UserInfo.UserEmail = reader["Email"] + "";
                            UserInfo.UserPhone = reader["PhoneNumber"] + "";

                            reader.Close();
                        }
                        CustomerMenuWindow cusWindow = new CustomerMenuWindow();
                        cusWindow.Show();
                        Hide();
                        break;
                    case "Meneger":
                        sql = "SELECT Employee.Name, LastName, Email, BirthDate, Adress, PhoneNumber, PassportNumber, PostCode " +
                                "FROM Employee where Iduser = @userid";
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            SqlCommand command = new SqlCommand(sql, connection);
                            SqlParameter idParam = new SqlParameter { ParameterName = "@userid", SqlDbType = SqlDbType.UniqueIdentifier, Value = userId };
                            command.Parameters.Add(idParam);
                            SqlDataReader reader = command.ExecuteReader();
                            reader.Read();
                            UserInfo.UserName = reader["Name"] + "";
                            UserInfo.UserLastName = reader["LastName"] + "";
                            UserInfo.UserEmail = reader["Email"] + "";
                            UserInfo.UserAddress = reader["Adress"] + "";
                            UserInfo.UserPhone = reader["PhoneNumber"] + "";
                            UserInfo.UserPassport = reader["PassportNumber"] + "";
                            UserInfo.UserPostCode = reader["PostCode"] + "";
                            UserInfo.UserBirthDate = reader["BirthDate"] + "";

                            reader.Close();
                        }
                        MenegerMenuWindow menWindow = new MenegerMenuWindow();
                        menWindow.Show();
                        Hide();
                        break;
                    default:
                        MessageBox.Show("Ошибка");
                        break;
                }
            }
            catch
            {
                MessageBox.Show("Ошибка");
            }

            //как сохранить соль?
        }

        private void EnterClickReg(object sender, RoutedEventArgs e)
        {
            RegWindow regWindow = new RegWindow();
            regWindow.Show();
            Hide();
        }
    }
}
