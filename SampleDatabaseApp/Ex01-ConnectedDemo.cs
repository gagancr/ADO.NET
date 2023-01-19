using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SampleDatabaseApp
{
    class Example01
    {
        const string STRCONNECTION = "Data Source=192.168.171.36;Initial Catalog=3310;Integrated Security=True";

        const string STRQUERY = "select * from dbo.employees";
        static void Main(string[] args)
        {
            SqlConnection sqlcon = new SqlConnection();
            sqlcon.ConnectionString = STRCONNECTION;

            SqlCommand sqlCommand = sqlcon.CreateCommand();
            sqlCommand.CommandText = STRQUERY;

            try
            {
                sqlcon.Open();
                SqlDataReader reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine(reader["empName"] +"  from  "+ reader["empAdress"]);
                   
                    Console.WriteLine();
                }
            }
            catch (SqlException ex)
            {

                Console.WriteLine(ex.Message); 
            }
            finally{
                if (sqlcon.State == System.Data.ConnectionState.Open)
                    sqlcon.Close();
            }
            
           // Console.WriteLine("hai");
        }
    }
}
