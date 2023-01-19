using SampleDatabaseApp.DataLayer;
using SampleDatabaseApp.PatientDocLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDatabaseApp
{

    namespace PatientDocLayer
    {
        class Patient
        {
            public int patientId { get; set; }
            public string  patientName { get; set; }
            public string patientAddress { get; set; }

            public int doctorId { get; set; }


        }
        class Doctor
        {
            public int doctorId { get; set; }
            public string doctorName { get; set; }
            public string specialization { get; set; }
        }
    }
    namespace DataLayer
    {
        interface IDataAccessComp
        {
            void addPatient(Patient p);
            void updatePatient(Patient p);
            void deletePatient(int id);
            void addDoctor(Doctor d);

            List<Patient> GetPatients();
            List<Doctor> GetDoctors();


        }
        class Datacomp : IDataAccessComp
        {



            private string strCon = string.Empty;

            const string STRINSERT = "insert into patients values(@PatientName,@PatientAddress,@DoctorId)";
            const string STRUPDATE = "update patients set PatientName=@patientName ,PatientAddress=@patientaddress,DoctorId=@doctorid where patientId=@patientid";
            const string STRALLPATIENTS = "SELECT * FROM PATIENTS";
            const string STRALLDOCTORS = "select * from doctors";
            const string STRDELETE = "delete from patients where patientId=@id";

            public Datacomp(string connectionString)
            {
                strCon = connectionString;
            }


            private void NonQueryExecute(string query,SqlParameter[] parameters,CommandType type)
            {
                SqlConnection con = new SqlConnection(strCon);

                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = type;
                if (parameters!=null)
                {
                    foreach (SqlParameter parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);
                    }
                }
                try
                {
                    con.Open();
                    cmd.ExecuteNonQuery();
                   
                }
                catch (Exception e)
                {

                    throw e;
                }
                finally
                {
                    con.Close();

                }
            }

            private DataTable getRecords(string query,SqlParameter[]parameters,CommandType type)
            {
                SqlConnection con = new SqlConnection(strCon);
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = type;
                if (parameters!=null)
                {

                    foreach (SqlParameter parameter in parameters)
                    {
                        cmd.Parameters.Add(parameter);

                    }
                }
                try
                {
                    con.Open();
                    var reader = cmd.ExecuteReader();
                    DataTable table = new DataTable("records");
                    table.Load(reader);
                    return table;
                }
                catch (Exception e)
                {

                    throw e;
                }
                finally
                {
                    con.Close();
                }

            }



            public void addPatient(Patient p)
            {
                List<SqlParameter> parameters= new List<SqlParameter>();
               // parameters.Add(new SqlParameter("@patientid", p.patientId));
                parameters.Add(new SqlParameter("@patientName", p.patientName));
                parameters.Add(new SqlParameter("@patientAddress", p.patientAddress));
                parameters.Add(new SqlParameter("@DoctorId", p.doctorId));

                try
                {
                    NonQueryExecute(STRINSERT, parameters.ToArray(), CommandType.Text);
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }

            }

            public void deletePatient(int id)
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("id", id));
                try
                {
                    NonQueryExecute(STRDELETE, parameters.ToArray(), CommandType.Text);
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }
            }

            public List<Doctor> GetDoctors()
            {
                var table = getRecords(STRALLDOCTORS, null,CommandType.Text);
                List<Doctor> doctorsList = new List<Doctor>();
                foreach (DataRow row in table.Rows)
                {
                    Doctor doc = new Doctor
                    {
                        doctorId = Convert.ToInt32(row[0]),
                        doctorName = row[1].ToString(),
                        specialization = row[2].ToString()
                    };
                    doctorsList.Add(doc);
                }
                return doctorsList;
            }

            public List<Patient> GetPatients()
            {
                var table = getRecords(STRALLPATIENTS, null, CommandType.Text);
                List<Patient> patientsList = new List<Patient>();
                
                foreach (DataRow row in table.Rows)
                {
                    Patient p = new Patient
                    {
                        patientId = (int)row[0],
                        patientName = row[1].ToString(),
                        patientAddress = row[2].ToString(),
                        doctorId = (int)row[3]
                    };
                    patientsList.Add(p);
                }
                return patientsList;
            }

            public void updatePatient(Patient p)
            {
                List<SqlParameter> parameters = new List<SqlParameter>();
                parameters.Add(new SqlParameter("@patientid", p.patientId));
                parameters.Add(new SqlParameter("@patientname", p.patientName));
                parameters.Add(new SqlParameter("@patientaddress", p.patientAddress));
                parameters.Add(new SqlParameter("@doctorid", p.doctorId));

                try
                {
                    NonQueryExecute(STRUPDATE, parameters.ToArray(), CommandType.Text);
                }
                catch (Exception e)
                {

                    Console.WriteLine(e.Message);
                }
            }

            public void addDoctor(Doctor d)
            {
                throw new NotImplementedException();
            }
        }
    }
    
    class PatientDoctorE2E
    {
        static IDataAccessComp component = null;
        static string connecionString = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
        static void Main(string[] args)
        {
            component = new Datacomp(connecionString);

            var data = component.GetPatients();
            foreach (var item in data)
            {
                Console.WriteLine(item.patientId+ " "+item.patientName);
            }
            Console.WriteLine("available patient id are as above");
            component.addPatient(
                new Patient
                {

                    patientName = Utilities.Prompt("enter the patient name "),
                    patientAddress = Utilities.Prompt("enter the patient address"),
                    doctorId = Utilities.GetNumber("enter the doctors id")
                });

            //component.updatePatient(new Patient
            //{
            //    patientId=Utilities.GetNumber("enter the id of the patient to update"),
            //    patientName = Utilities.Prompt("enter the patient name "),
            //    patientAddress = Utilities.Prompt("enter the patient address"),
            //    doctorId = Utilities.GetNumber("enter the doctors id")
            //});

          //  component.deletePatient(Utilities.GetNumber("enter the id of the patient to delete"));
            
        }

    }
}
