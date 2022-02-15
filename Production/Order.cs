using Production;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Production
{
    public class Prod : INotifyPropertyChanged
    {
        public Guid Id { get; set; }
        private string name;
        private string number;
        private int quantity;
        private decimal price;
        public string Number
        {
            get => number;
            set
            {
                number = value;
                OnPropertyChanged("Number");
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

        public int Quantity
        {
            get => quantity;
            set
            {
                quantity = value;
                OnPropertyChanged("Quantity");
            }
        }
        public decimal Price
        {
            get => price;
            set
            {
                price = value;
                OnPropertyChanged("Price");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    };
    public class Order : INotifyPropertyChanged
    {
        private Guid id;
        private int number;
        private DateTime orderDate;
        private string state;
        private Decimal totalPrice;
        private Guid customerId;
        private string customerName;
        private string tel;
        private Guid shopId;
        private string shopName;
        private ObservableCollection<Prod> productList;

        public ObservableCollection<Prod> ProductList
        {
            get => productList;
            set
            {
                productList = value;
                OnPropertyChanged("ProductList");
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

        public int Number
        {
            get => number;
            set
            {
                number = value;
                OnPropertyChanged("Number");
            }
        }

        public DateTime OrderDate
        {
            get => orderDate;
            set
            {
                orderDate = value;
                OnPropertyChanged("OrderDate");
            }
        }

        public string State
        {
            get => state;
            set
            {
                state = value;
                OnPropertyChanged("State");
            }
        }

        public Decimal TotalPrice
        {
            get => totalPrice;
            set
            {
                totalPrice = value;
                OnPropertyChanged("TotalPrice");
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

        public string CustomerName
        {
            get => customerName;
            set
            {
                customerName = value;
                OnPropertyChanged("CustomerName");
            }
        }

        public string Tel
        {
            get => tel;
            set
            {
                tel = value;
                OnPropertyChanged("Tel");
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

        public string ShopName
        {
            get => shopName;
            set
            {
                shopName = value;
                OnPropertyChanged("ShopName");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        /*private RelayCommand clickList;

        public ICommand ClickList
        {
            get
            {
                if (clickList == null)
                {
                    clickList = new RelayCommand(PerformClickList);
                }

                return clickList;
            }
        }

        private void PerformClickList(object commandParameter)
        {
            ProductList = new Dictionary<Guid, int>();
            //_ = new OrderView(id, number, orderDate, state, totalPrice, customerId,
                //customerName, tel, shopId, shopName, ProductList);
        }*/
    }
}