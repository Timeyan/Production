using System;
using System.Collections.Generic;
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
    /// Interaction logic for AdminMenuWindow.xaml
    /// </summary>
    public partial class AdminMenuWindow : Window
    {
        public AdminMenuWindow()
        {
            InitializeComponent();
            menegerName.Text = UserInfo.UserName + " " + UserInfo.UserLastName;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ProductList productList = new ProductList();
            productList.Show();
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UserList userList = new UserList();
            userList.Show();
            Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            OrderList orderList = new OrderList();
            orderList.Show();
            Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            ShopList shopList = new ShopList();
            shopList.Show();
            Close();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            EmlpoyeeList emlpoyeeList = new EmlpoyeeList();
            emlpoyeeList.Show();
            Close();
        }
    }
}
