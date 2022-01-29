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
    /// Interaction logic for UserList.xaml
    /// </summary>
    public partial class UserList : Window
    {
        public UserList(string sql = "", Guid id = default)
        {
            InitializeComponent();
            if (string.IsNullOrEmpty(sql))
            {
                DataContext = new UserListViewModel(true, true, true);
            }
            else
            {
                DataContext = new UserListViewModel(false, true, false, sql, id);
            }
            _ = ShowDialog();
        }
    }
}
