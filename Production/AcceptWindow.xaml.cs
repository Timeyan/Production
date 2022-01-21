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
    /// Interaction logic for AcceptWindow.xaml
    /// </summary>
    public partial class AcceptWindow : Window
    {

        public AcceptWindow()
        {
            InitializeComponent();
            Warn.Text = "Подтвердите удаление";
        }
        public AcceptWindow(string warning)
        {
            InitializeComponent();
            Warn.Text = warning;
        }

        private void EnterClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        private void EnterClickReg(object sender, RoutedEventArgs e)
        {

        }
    }
}
