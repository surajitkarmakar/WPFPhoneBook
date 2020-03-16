using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.ComponentModel;
using WPFPhonebook.Model;
using WPFPhonebook.Command;
using System.Windows.Controls;
using System.Data;
using System.Windows;

namespace WPFPhonebook.ViewModel
{
    public class PersonViewModel : INotifyPropertyChanged
    {
        private string searchString;
        public string SearchString
        {
            get 
            { 
                return searchString; 
            }
            set 
            { 
                searchString = value;
                PeopleCollection = PersonDb.GetPersonsAny(searchString);
                OnPropertyChanged("SearchString");
            }
        }

        public RelayCommand InsertCommand { get; set; }
        public RelayCommand EditCommand { get; set; }
        public RelayCommand DeleteCommand { get; set; }
        public RelayCommand UpdateCommand { get; set; }

        private BasePerson person;
        public BasePerson _Person
        {
            get 
            { 
                if (person == null)
                    person = new Person();
                return person; 
            }
            set { person = value;  OnPropertyChanged("_Person"); }
        }
        private ObservableCollection<BasePerson> peopleCollection;
        public ObservableCollection<BasePerson> PeopleCollection
        {
            get 
            {
                return peopleCollection;
            }
            set
            {
                peopleCollection = value;
                OnPropertyChanged("PeopleCollection");
            }
        }
        private bool IsUpdateEnabled { get; set; }
        public PersonViewModel()
        {
            person = new Person();
            person.BirthDate = DateTime.Today;
            peopleCollection = new ObservableCollection<BasePerson>();
            PersonDb.Initialize();
            PeopleCollection = PersonDb.Insert_GetAllPersons(_Person.FName);
            InsertCommand = new RelayCommand(InsertPersonCommand, IsInsertCommandEnabled, false);
            EditCommand = new RelayCommand(EditPersonCommand, IsEditCommandEnabled, false);
            UpdateCommand = new RelayCommand(EditPersonCommand, IsUpdateCommandEnabled, false);
            DeleteCommand = new RelayCommand(DeletePersonCommand, IsDeleteCommandEnabled, false);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public void InsertPersonCommand(object parameter)
        {
            PeopleCollection= PersonDb.Insert_GetAllPersons(_Person.FName, _Person.MName, _Person.LName, _Person.ContactNo, _Person.EmailId, _Person.BirthDate, _Person.Address, _Person.Gender.ToString(), _Person.Country.ToString(), _Person.State, _Person.PinCode);
            _Person = new Person();
        }
        public void EditPersonCommand(object parameter)
        {
            if (parameter is DataGrid)
            {
                DataGrid dtGridPerson = (DataGrid)parameter;
                if (dtGridPerson.SelectedItem == null)
                {
                    MessageBox.Show("Please select a record to be deleted from the datagrid ");
                    return;
                }
                _Person = (BasePerson)dtGridPerson.SelectedItem;
                IsUpdateEnabled = true;
            }
            else if(IsUpdateEnabled)
            {
                PeopleCollection = PersonDb.UpdatePerson(_Person.Id,_Person.FName, _Person.MName, _Person.LName, _Person.ContactNo, _Person.EmailId, _Person.BirthDate, _Person.Address, _Person.Gender.ToString(), _Person.Country.ToString(), _Person.State, _Person.PinCode);
                _Person = new Person();
                IsUpdateEnabled = false;
            }
            else MessageBox.Show("Please select a record to be edited from the datagrid ");
        }
        public void DeletePersonCommand(object parameter)
        {
            if (parameter is DataGrid)
            {
                List<int> ids = new List<int>();
                DataGrid dtGridPerson = (DataGrid)parameter;
                if (dtGridPerson.SelectedItem == null)
                {
                    MessageBox.Show("Please select a record to be deleted from the datagrid ");
                    return;
                }

                DataTable table = new DataTable();
                table.Columns.Add("id", typeof(int));
                foreach (var person in dtGridPerson.SelectedItems)
                {
                    int id = ((BasePerson)person).Id;
                    table.Rows.Add(id);
                }
                PeopleCollection = PersonDb.DeletePersons(table);
            }
            else MessageBox.Show("Please select a record to be deleted from the datagrid ");
        }
        public bool IsInsertCommandEnabled(object parameter)
        {
            if(_Person.IsValid && !IsUpdateEnabled)
                return true;
            return false;
        }
        public bool IsEditCommandEnabled(object parameter)
        {
            //if(IsUpdateEnabled)
                return true;
            //return false;
        }
        public bool IsUpdateCommandEnabled(object parameter)
        {
            if (IsUpdateEnabled)
                return true;
            return false;
        }
        public bool IsDeleteCommandEnabled(object parameter)
        {
            return true;
        }
    }
}
