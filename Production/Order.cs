using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Production
{

    class Order : INotifyPropertyChanged
    {
        private Guid id;
        private DateTime orderDate;
        private string state;
        private int totalPrice;
        private Guid customerId;
        private string customerName;
        private string tel;
        private Guid shopId;
        private string shopName;
        private Dictionary<Guid, int> ProductList { get; set; }


        public Guid Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged("Id");
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

        public int TotalPrice
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

    }
}