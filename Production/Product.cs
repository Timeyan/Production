using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Production
{
    public class Product: INotifyPropertyChanged, ICloneable
    {
        private Guid id;
        private string name;
        private string type;
        private string serialNumberData;
        private string serialNumber;
        private bool isProducedData;
        private string isProduced;
        private decimal priceData;
        private string price;
        private int stockQuantityData;
        private string stockQuantity;

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
        public string Type
        {
            get { return type; }
            set
            {
                type = value;
                OnPropertyChanged("Type");
            }
        }

        public string SerialNumberData
        {
            get { return serialNumberData; }
            set
            {
                serialNumberData = value;
            }
        }

        public string SerialNumber
        {
            get { return serialNumber; }
            set
            {
                serialNumber = value;
                OnPropertyChanged("SerialNumber");
            }
        }

        public bool IsProducedData
        {
            get { return isProducedData; }
            set
            {
                isProducedData = value;
            }
        }

        public string IsProduced
        {
            get { return isProduced; }
            set
            {
                isProduced = value;
                OnPropertyChanged("IsProduced");
            }
        }

        public int StockQuantityData
        {
            get { return stockQuantityData; }
            set
            {
                stockQuantityData = value;
            }
        }

        public string StockQuantity
        {
            get { return stockQuantity; }
            set
            {
                stockQuantity = value;
                OnPropertyChanged("StockQuantity");
            }
        }

        public decimal PriceData
        {
            get { return priceData; }
            set
            {
                priceData = value;
            }
        }

        public string Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged("Price");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public object Clone()
        {
            return MemberwiseClone();
        }

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
    }
}
