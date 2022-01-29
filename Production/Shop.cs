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
    public class Shop : INotifyPropertyChanged
    {
        private Guid id;
        private string name;
        private string adress;
        private string postCode;
        private string first = "";
        private string second = "";
        private string third = "";
        private string ellipsis = "";
        //public ObservableCollection<Guid> EmployeeGuidList { get; set; }
        public ObservableCollection<string> EmployeeList { get; set; }
        

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

        public string First
        {
            get => first;
            set
            {
                first = value;
                OnPropertyChanged("First");
            }
        }

        public string Second
        {
            get => second;
            set
            {
                second = value;
                OnPropertyChanged("Second");
            }
        }

        public string Third
        {
            get => third;
            set
            {
                third = value;
                OnPropertyChanged("Third");
            }
        }

        public string Ellipsis
        {
            get => ellipsis;
            set
            {
                ellipsis = value;
                OnPropertyChanged("Ellipsis");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

    }
}
