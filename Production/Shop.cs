using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Production
{
    public class Shop : INotifyPropertyChanged
    {
        private Guid id;
        private string name;
        private string adress;
        private string postCode;

        public Guid Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged("Id");
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

        public string Adress
        {
            get => adress;
            set
            {
                adress = value;
                OnPropertyChanged("Adress");
            }
        }

        public string PostCode
        {
            get => postCode;
            set
            {
                postCode = value;
                OnPropertyChanged("PostCode");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
