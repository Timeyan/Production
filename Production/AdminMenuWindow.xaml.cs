﻿using System;
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
    }
}
