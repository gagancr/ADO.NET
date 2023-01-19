using SampleDatabaseApp.PatientDocLayer;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDatabaseApp
{
    class NewAssignment
    {
        const string FileName = "../../Customers.csv";
       static  string STRCONN = ConfigurationManager.ConnectionStrings[1].ConnectionString;
        static string query = "select * from employees";

        static string joinQuery = "select * from Patients p,Doctors d where p.DoctorId=d.DoctorId;";

       static SqlConnection conn = new SqlConnection(STRCONN);
       static SqlCommand cmdd = new SqlCommand(query, conn);


        public void addEmployee(int id ,string name)
        {
            try
            {
                conn.Open();
                int i =cmdd.ExecuteNonQuery();
            }
            catch (Exception e)
            {

                throw e;
            }
            finally
            {
                conn.Close();

            }
        }

        static public DataTable GetAllDetails()
        {
            SqlConnection con = new SqlConnection(STRCONN);
            SqlCommand cmd = new SqlCommand(joinQuery, con);

            try
            {
                con.Open();
               var reader= cmd.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);
                return dt;

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

        static public void displayitems()
        {

            try
            {
            var data = GetAllDetails();

            foreach (DataRow row in data.Rows)
            {
                Console.WriteLine(row[0]+" "+row[1]+" "+row[2]+"   "+row[4]+"   "+row[5]+"   "+row[6]);
            }

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
        }

        static List<Patient> ReadFromCsv()
        {

            List<Patient> pat = new List<Patient>();
            string[] allLines = File.ReadAllLines(FileName);

            foreach (string line in allLines)
            {
                var word = line.Split(',');
                Patient p = new Patient
                {
                    patientId = Convert.ToInt32(word[0]),
                    patientName = word[1],
                    patientAddress = word[2],
                    doctorId = Convert.ToInt32(word[3])

                };
                pat.Add(p);
            }
            return pat;
        }

        static void Main(string[] args)
        {
            // displayitems();

            var read=ReadFromCsv();

            foreach (var item in ReadFromCsv())
            {
                Console.WriteLine(item.patientName +" from "+item.patientAddress);
            }
        }
    }
}
