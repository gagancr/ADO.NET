using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SampleDatabaseApp.DataLayer;
using SampleDatabaseApp.PatientDocLayer;
namespace SampleDatabaseApp
{
    class DisconnectedPatientDoce2E : IDataAccessComp
    {

        static string StrConnection = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
        const string query = "select * from Patients;select * from Doctors";

        static DataSet disconnectedObj = new DataSet("AllRecords");
        static SqlDataAdapter adapter = null;

        static void fillRecords()
        {
            SqlConnection con = new SqlConnection(StrConnection);
            SqlCommand cmd = new SqlCommand(query, con);
            adapter = new SqlDataAdapter(cmd);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(disconnectedObj);
            disconnectedObj.Tables[0].TableName = "PatientsList";

            //setting primary key
            if (disconnectedObj.Tables[0].PrimaryKey.Length == 0)
                disconnectedObj.Tables[0].PrimaryKey = new DataColumn[]
                {
                    disconnectedObj.Tables[0].Columns[0]
                };



            disconnectedObj.Tables[1].TableName = "DoctorsList";
            Trace.WriteLine("connection state " + con.State);

        }
        public void addPatient(Patient p)
        {
            DataRow newRow = disconnectedObj.Tables[0].NewRow();
            newRow[0] = 0;
            newRow[1] = p.patientName;
            newRow[2] = p.patientAddress;
            newRow[3] = p.doctorId;
           

            disconnectedObj.Tables[0].Rows.Add(newRow);
            adapter.Update(disconnectedObj, "PatientsList");
        }
        public void addDoctor(Doctor d)
        {
            DataRow newrow = disconnectedObj.Tables[1].NewRow();
            newrow[0] = 0;
            newrow[1] = d.doctorName;
            newrow[2] = d.specialization;

            disconnectedObj.Tables[1].Rows.Add(newrow);
            adapter.Update(disconnectedObj, "DoctorsList");
        }

        public void deletePatient(int id)
        {
            foreach (DataRow row in disconnectedObj.Tables[0].Rows)
            {
                if (row[0].ToString() == id.ToString())
                {
                    row.Delete();
                    break;
                }
            }
            adapter.Update(disconnectedObj, "PatientsList");
        }

        public List<Doctor> GetDoctors()
        {
            List<Doctor> doctors = new List<Doctor>();
            foreach (DataRow row in disconnectedObj.Tables["DoctorsList"].Rows)
            {

                Doctor doc = new Doctor
                {
                    doctorId = (int)row[0],
                    doctorName = row[1].ToString(),
                    specialization = row[2].ToString()
                };
                doctors.Add(doc);
           }
            return doctors;
        }

        public List<Patient> GetPatients()
        {
            List<Patient> patients = new List<Patient>();
            foreach (DataRow row in disconnectedObj.Tables["PatientsList"].Rows)
            {

               
                patients.Add(new Patient
                {
                    patientId = (int)row[0],
                    patientName = row[1].ToString(),
                    patientAddress = row[2].ToString(),
                    doctorId = (int)row[3]
                });
             }
            return patients;
        }
        public void searchPatient(string name)
        {
            ArrayList al = new ArrayList();
            foreach (DataRow  item in disconnectedObj.Tables[0].Rows)
            {
                
                if (name == item[1].ToString())
                {
                    al.Add(item);
                }
            }
            int count = 0;
            foreach (var item in al)
            {
                DataRow x = (DataRow)al[count];
                Console.WriteLine(x[0]+" "+x[1]+" from "+x[2]);
                count++;
            }
        }
        public void updatePatient(Patient p)
        {
            foreach (DataRow row in disconnectedObj.Tables[0].Rows)
            {
                if (row[0].ToString() == p.patientId.ToString())
                {
                    row[1] = p.patientName;
                    row[2] = p.patientAddress;
                    row[3] = p.doctorId;
                   

                }
            }
            // disconnectedObj.Tables[0].Rows.Add(newRow);
            adapter.Update(disconnectedObj, "PatientsList");
        }
        static void Main(string[] args)
        {
            
            DisconnectedPatientDoce2E d = new DisconnectedPatientDoce2E();
            fillRecords();
            //Patient p = new Patient
            //{
            //    //patientId = Utilities.GetNumber("enter the id to update"),
            //    patientName = Utilities.Prompt("enter the patient name "),
            //    patientAddress = Utilities.Prompt("enter the patient address"),
            //    doctorId = Utilities.GetNumber("enter the doctors id")
            //};
            // d.addPatient(p);


            //Doctor doc = new Doctor
            //{

            //    doctorName = Utilities.Prompt("enter the doctors name "),
            //    specialization = Utilities.Prompt("enter the specialization")

            //};

            //d.addDoctor(doc);


            d.searchPatient("vilas");
            



            // d.deletePatient(15);
            //var doctors = d.GetDoctors();
            //foreach (var item in doctors)
            //{
            //    Console.WriteLine(item.doctorName + "  from  " + item.specialization);
            //}

            //var patients = d.GetPatients();
            //foreach (var item in patients)
            //{
            //    Console.WriteLine(item.patientId+" "+item.patientName+" "+item.doctorId );
            //}
            // Console.WriteLine("Patient added successfully");
        }

       
    }
}
