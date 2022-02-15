using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Надо добавить отображение логина и пароля
    /// Вывод ошибок уникальности
    /// </summary>
    public partial class CreateEmployee : Window
    {
        private bool isCorrect = true;
        public bool IsSaving { get; set; } = false;

        private readonly string connectionString;
        Brush defaultBackground;

        public string Role { get; private set; }
        public string Login { get; private set; }
        public CreateEmployee()
        {
            InitializeComponent();
            defaultBackground = TextBoxPhone.Background;
            BirthDatePicker.DisplayDateEnd = DateTime.Today;
            connectionString = "Data Source=DESKTOP-N9D0K7G;Initial Catalog=Production;Integrated Security=true;TrustServerCertificate=True";
            string sql = @"Select Name from [Role] where Name != 'Customer'";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        _ = TextBoxRole.Items.Add((string)reader.GetValue(0));
                    }

                    reader.Close();
                }
                command.CommandText = "Select Name from [Shop]";
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        _ = TextBoxShop.Items.Add((string)reader.GetValue(0));
                    }

                    reader.Close();
                }
            }
            _ = ShowDialog();
        }

        private async void EnterClickAsync(object sender, RoutedEventArgs e)
        {
            try
            {
                string name = TextBoxName.Text;
                string lastName = TextBoxLastName.Text;
                string phone = TextBoxPhone.Text;
                string email = TextBoxEmail.Text;
                string passport = TextBoxPassport.Text;
                string adress = TextBoxAdress.Text;
                string postCode = TextBoxPostCode.Text;
                Login = TextBoxLogin.Text;
                string password = TextBoxPassword.Text;
                Role = (string)TextBoxRole.SelectedValue;
                string shop = (string)TextBoxShop.SelectedValue;
                if (name == "" || lastName == "" || phone == "" || email == ""
                    || passport == "" || adress == "" || postCode == "" || Login == "" 
                    || password == "" || Role == "" || 
                    BirthDatePicker.SelectedDate == null) throw new Exception("Должны быть заполнены все поля");
                DateTime birthDate = (DateTime)BirthDatePicker.SelectedDate;
                if (!isCorrect) throw new Exception("Не все поля заполнены корректно");

                //Поменять это в изменении аккаунта
                string sqlProc = "employeeReg";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    SqlCommand command = new SqlCommand(sqlProc, connection)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    _ = command.Parameters.Add(new SqlParameter { ParameterName = "@name", Value = name, SqlDbType = SqlDbType.NVarChar });
                    _ = command.Parameters.Add(new SqlParameter { ParameterName = "@lastname", Value = lastName, SqlDbType = SqlDbType.NVarChar });
                    _ = command.Parameters.Add(new SqlParameter { ParameterName = "@phonenumber", Value = phone, SqlDbType = SqlDbType.VarChar });
                    _ = command.Parameters.Add(new SqlParameter { ParameterName = "@email", Value = email, SqlDbType = SqlDbType.VarChar });
                    _ = command.Parameters.Add(new SqlParameter { ParameterName = "@passportnumber", Value = passport, SqlDbType = SqlDbType.VarChar });
                    _ = command.Parameters.Add(new SqlParameter { ParameterName = "@birthDate", Value = birthDate, SqlDbType = SqlDbType.Date });
                    _ = command.Parameters.Add(new SqlParameter { ParameterName = "@address", Value = adress, SqlDbType = SqlDbType.NVarChar });
                    _ = command.Parameters.Add(new SqlParameter { ParameterName = "@postcode", Value = postCode, SqlDbType = SqlDbType.VarChar });
                    _ = command.Parameters.Add(new SqlParameter { ParameterName = "@login", Value = Login, SqlDbType = SqlDbType.VarChar });
                    _ = command.Parameters.Add(new SqlParameter { ParameterName = "@password", Value = password, SqlDbType = SqlDbType.VarChar });
                    _ = command.Parameters.Add(new SqlParameter { ParameterName = "@role", Value = Role, SqlDbType = SqlDbType.NVarChar });
                    _ = command.Parameters.Add(new SqlParameter { ParameterName = "@shop", Value = shop, SqlDbType = SqlDbType.NVarChar });

                    _ = await command.ExecuteNonQueryAsync();

                }
                MessageBox.Show("Данные успешно сохранены");
                IsSaving = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void EnterClickBack(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void RoleChange(object sender, SelectionChangedEventArgs e)
        {
            if ((string)TextBoxRole.SelectedValue == "Seller")
            {
                TextBoxShop.IsEnabled = true;
            }
            else
            {
                TextBoxShop.SelectedIndex = -1;
                TextBoxShop.IsEnabled = false;
            }
        }


        private void TextBoxPhone_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TextBoxPhone.Text == "")
            {
                TextBoxPhone.Text = "+7";
            }
        }

        private void TextBoxPhone_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TextBoxPhone.Text != "")
            {
                if (TextBoxPhone.Text == "+7" || TextBoxPhone.Text == "+" || TextBoxPhone.Text[0] != '+')
                {
                    TextBoxPhone.Text = "";
                }
                else if (TextBoxPhone.Text.Length == 12)
                {
                    isCorrect = true;
                    TextBoxPhone.Background = defaultBackground;
                }
                else
                {
                    isCorrect = false;
                    TextBoxPhone.Background = new SolidColorBrush(Colors.IndianRed);
                }
            }
        }

        private void TextBoxPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            int fin = TextBoxPhone.Text.Length - 1;
            if (TextBoxPhone.Text == "+")
            {
                TextBoxPhone.Text = "+7";
            }
            else if (TextBoxPhone.Text != "" && !int.TryParse(TextBoxPhone.Text[fin].ToString(), out _))
            {
                TextBoxPhone.Text = TextBoxPhone.Text.Remove(fin);
            }
            TextBoxPhone.Select(TextBoxPhone.Text.Length, 0);
        }

        private void TextBoxName_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox a = (TextBox)sender;
            string value = a.Text;
        }

        private void TextBoxPassport_TextChanged(object sender, TextChangedEventArgs e)
        {
            int fin = TextBoxPassport.Text.Length - 1;
            if (TextBoxPassport.Text != "" && !int.TryParse(TextBoxPassport.Text[fin].ToString(), out _))
            {
                TextBoxPassport.Text = TextBoxPassport.Text.Remove(fin);
            }
            TextBoxPassport.Select(TextBoxPassport.Text.Length, 0);
        }

        private void TextBoxPassport_GotFocus(object sender, RoutedEventArgs e)
        {
            if (isCorrect && TextBoxPassport.Text != "")
            {
                TextBoxPassport.Text = TextBoxPassport.Text.Substring(0, 4) + TextBoxPassport.Text.Substring(5, 6);
            }
            TextBoxPassport.MaxLength = 10;
        }

        private void TextBoxPassport_LostFocus(object sender, RoutedEventArgs e)
        {
            TextBoxPassport.MaxLength = 11;
            if (TextBoxPassport.Text.Length == 10)
            {
                isCorrect = true;
                TextBoxPassport.Background = defaultBackground;
                TextBoxPassport.Text = TextBoxPassport.Text.Substring(0, 4) + " " + TextBoxPassport.Text.Substring(4, 6);
            }
            else
            {
                isCorrect = false;
                TextBoxPassport.Background = new SolidColorBrush(Colors.IndianRed);
            }
        }

        private void TextBoxPostCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            int fin = TextBoxPostCode.Text.Length - 1;
            if (TextBoxPostCode.Text != "" && !int.TryParse(TextBoxPostCode.Text[fin].ToString(), out _))
            {
                TextBoxPostCode.Text = TextBoxPostCode.Text.Remove(fin);
            }
            TextBoxPostCode.Select(TextBoxPostCode.Text.Length, 0);
        }

        private void TextBoxPostCode_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private void TextBoxPostCode_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TextBoxPostCode.Text.Length == 6)
            {
                isCorrect = true;
                TextBoxPostCode.Background = defaultBackground;
            }
            else
            {
                isCorrect = false;
                TextBoxPostCode.Background = new SolidColorBrush(Colors.IndianRed);
            }
        }
    }
}
