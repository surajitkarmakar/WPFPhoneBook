using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using WPFPhonebook.Model;
using System.Collections.ObjectModel;
using Microsoft.SqlServer.Server;

namespace WPFPhonebook.Database
{
    public class PersonDBMaster
    {
        SqlConnection _sqlConnection;
        SqlCommand _command;
        SqlDataReader _dataReader;
        static PersonDBMaster Instance;
        public static PersonDBMaster CreateInstance()
        {
            object _lock = new object();
            //Singleton pattern
            if (Instance == null)
            { 
                lock (_lock)
                { 
                    Instance = new PersonDBMaster();
                }
            }
            return Instance;
        }
        private PersonDBMaster()
        {
                
        }
        internal void Initialize()
        {
            _sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["phonebook"].ConnectionString);
            _sqlConnection.Open();
        }
        internal void CreateStoredProcedureCommand(string ProcedureName)
        {
            _command = new SqlCommand(ProcedureName, _sqlConnection);
            _command.CommandType = CommandType.StoredProcedure;
        }
        internal void AddSPParameters(string fname,string mname=null,string lname = null, string contactno = null, string emailid = null, DateTime? bdate= null, string address = null, string gender = null, string country = null, string state = null, int pincode = 0,int? pid=null)
        {
            if(pid!=null)
                _command.Parameters.AddWithValue("@pid", pid);
            _command.Parameters.AddWithValue("@fname", fname);
            _command.Parameters.AddWithValue("@mname", mname);
            _command.Parameters.AddWithValue("@lname", lname);
            _command.Parameters.AddWithValue("@contactno", contactno);
            _command.Parameters.AddWithValue("@emailid", emailid);
            if(bdate==DateTime.MinValue)
                bdate = DateTime.Now;
            _command.Parameters.AddWithValue("@bdate", bdate);
            _command.Parameters.AddWithValue("@address", address);
            _command.Parameters.AddWithValue("@gender", gender);
            _command.Parameters.AddWithValue("@country", country);
            _command.Parameters.AddWithValue("@state", state);
            _command.Parameters.AddWithValue("@pincode", pincode);

        }
        internal void AddSPParameters(string searchString)
        {
            _command.Parameters.AddWithValue("@searchstring", searchString);
        }
        internal void AddSPParameters(int pid)
        {
            _command.Parameters.AddWithValue("@pid", pid);
        }
        internal void AddSPParameters(DataTable table)
        {
            _command.Parameters.Add("@idlist", SqlDbType.Structured).Value = table;
        }
        internal ObservableCollection<BasePerson> Execute_GetAllPersons()
        {
            _dataReader = _command.ExecuteReader();
            ObservableCollection<BasePerson> persons = new ObservableCollection<BasePerson>();
            while (_dataReader.Read())
            {
                persons.Add(new Person()
                {
                    Id= Int32.Parse(_dataReader["pid"].ToString()),
                    FName = _dataReader["pfname"].ToString(),
                    MName = _dataReader["pmname"].ToString(),
                    LName = _dataReader["plname"].ToString(),
                    ContactNo = _dataReader["pcontactno"].ToString(),
                    EmailId = _dataReader["pemailid"].ToString(),
                    BirthDate =  Convert.ToDateTime(_dataReader["pbdate"].ToString()),
                    Address = _dataReader["paddress"].ToString(),
                    Gender = (Gender)Enum.Parse(typeof(Gender), _dataReader["pgender"].ToString(),true),
                    Country = (Country)Enum.Parse(typeof(Country),_dataReader["pcountry"].ToString(),true),
                    State = _dataReader["pstate"].ToString(),
                    PinCode = (Int32.Parse(_dataReader["ppincode"].ToString()))
                });
            }
            ClearCommand();
            return persons;
        }
        private void ClearCommand()
        {
            //_command.Parameters.Clear();
            _command = null;
            _dataReader.Close();
        }
    }
}
