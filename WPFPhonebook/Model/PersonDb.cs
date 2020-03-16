using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WPFPhonebook.Database;
using System.Collections.ObjectModel;
using System.Data;

namespace WPFPhonebook.Model
{
    public class PersonDb
    {
        static PersonDBMaster p;
        public static void Initialize()
        {
            p = PersonDBMaster.CreateInstance();
            p.Initialize();
        }
        public static ObservableCollection<BasePerson> Insert_GetAllPersons(string fname=null, string mname = null, string lname = null, string contactno = null, string emailid = null, DateTime bdate = new DateTime(), string address = null, string gender = null, string country = null, string state = null, int pincode = 0, int? pid = null)
        {
            p.CreateStoredProcedureCommand(ConfigurationManager.AppSettings["SP_INSERT"]);
            p.AddSPParameters(fname, mname, lname, contactno, emailid, bdate, address, gender, country, state, pincode);
            return p.Execute_GetAllPersons();
        }
        public static ObservableCollection<BasePerson> GetPersonsAny(string searchString = null)
        {
            p.CreateStoredProcedureCommand(ConfigurationManager.AppSettings["SP_SEARCHANY"]);
            p.AddSPParameters(searchString);
            return p.Execute_GetAllPersons();
        }
        public static BasePerson GetPerson(int pid)
        {
            p.CreateStoredProcedureCommand(ConfigurationManager.AppSettings["SP_SELECTPERSON"]);
            p.AddSPParameters(pid);
            return p.Execute_GetAllPersons().FirstOrDefault();
        }
        public static ObservableCollection<BasePerson> DeletePersons(DataTable table)
        {
            p.CreateStoredProcedureCommand(ConfigurationManager.AppSettings["SP_DELETEMULTIPLE"]);
            p.AddSPParameters(table);
            return p.Execute_GetAllPersons();
        }
        public static ObservableCollection<BasePerson> UpdatePerson(int pid, string fname = null, string mname = null, string lname = null, string contactno = null, string emailid = null, DateTime bdate = new DateTime(), string address = null, string gender = null, string country = null, string state = null, int pincode = 0)
        {
            p.CreateStoredProcedureCommand(ConfigurationManager.AppSettings["SP_UPDATEPERSON"]);
            p.AddSPParameters(fname, mname, lname, contactno, emailid, bdate, address, gender, country, state, pincode, pid);
            return p.Execute_GetAllPersons();
        }
    }
}
