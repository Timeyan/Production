using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using Microsoft.Data.SqlClient;
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
using System.Windows.Shapes;

namespace Production
{
    /// <summary>
    /// Interaction logic for RegWindow.xaml
    /// </summary>
    public partial class RegWindow : Window
    {
        string connectionString;
        public RegWindow()
        {
            InitializeComponent();
            connectionString = "Data Source=DESKTOP-N9D0K7G;Initial Catalog=Production;Integrated Security=true;TrustServerCertificate=True";
        }

        private async void EnterClickAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                String name = TextBoxName.Text;
                String lastName = TextBoxLastName.Text;
                String phone = TextBoxPhone.Text;
                String email = TextBoxEmail.Text;
                String login = TextBoxLogin.Text;
                String password = BCrypt.Net.BCrypt.HashPassword(TextBoxPassword.Password);
                if (password == "" || name == "" || lastName == "" || 
                        phone == "" || email == "" || login == "") throw new Exception("Должны быть заполнены все поля");

                string sqlProc = "customerReg";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    SqlCommand command = new SqlCommand(sqlProc, connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("@name", name));
                    command.Parameters.Add(new SqlParameter("@lastname", lastName));
                    command.Parameters.Add(new SqlParameter { ParameterName = "@phonenumber", Value = phone, SqlDbType = SqlDbType.VarChar });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@email", Value = email, SqlDbType = SqlDbType.VarChar });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@login", Value = login, SqlDbType = SqlDbType.VarChar });
                    command.Parameters.Add(new SqlParameter { ParameterName = "@password", Value = password, SqlDbType = SqlDbType.VarChar });

                    _ = await command.ExecuteNonQueryAsync();
                }
                //SqlCommand command = new SqlCommand(sql, connection);
                //SqlDataReader result = command.ExecuteReader();

                //adapter = new SqlDataAdapter(command);

            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            MessageBox.Show("Данные успешно сохранены");
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Hide();
        }

        private void EnterClickBack(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Hide();
        }
    }
}
