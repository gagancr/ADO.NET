using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDatabaseApp
{

    static class Database
    {
        const string STRCONNECTION = "Data Source=192.168.171.36;Initial Catalog=3310;Integrated Security=True";

        const string STRQUERY = "select * from dbo.employees";

        const string DEPTQUERY = "select * from dbo.tblDept";

        const string STREMPINSERT= "insert into dbo.employees values(@empname,@empadress,@empsalary,@deptid,@managerid)";

        const string STRFIND = "select * from dbo.books where bookName=@name";

        const string STRINSERT = "insert into dbo.books values(@bookid,@bookname,@bookprice,@bookauthor)";

        const string GETID = "select deptid from dbo.tblDept where DeptName=@deptname";

        const string STRINSERTPROC = "InsertEmployees";
        
            public static DataTable getDeptRecords()
        {
            SqlConnection con = new SqlConnection(STRCONNECTION);
            SqlCommand cmd = new SqlCommand(DEPTQUERY, con);
            try
            {
                con.Open();
                var reader = cmd.ExecuteReader();
                DataTable table = new DataTable("dbo.tbldept");
                
                table.Load(reader);
                return table;
            }
            catch (Exception e)
            {

                throw e;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }
        public static void displayDeptTable()
        {
            try
            {
                var table = Database.getDeptRecords();
                Console.WriteLine("Available departments ");
                foreach (DataRow row in table.Rows)
                {
                    Console.WriteLine($"{row[1]} ");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    public static DataTable GetAllRecords()
    {
            SqlConnection con = new SqlConnection(STRCONNECTION);
            SqlCommand cmd = new SqlCommand(STRQUERY, con);
            
            try
            {
                con.Open();
                var reader = cmd.ExecuteReader();
                DataTable table=    new DataTable("dbo.employees");
                table.Load(reader);
                return table;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
    }
        public static void displayAsTable()
        {
            try
            {
                var table = Database.GetAllRecords();
                foreach (DataRow row in table.Rows)
                {
                    Console.WriteLine($"Name  :{row[1]} from  {row[2]}");
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
        }
        public static void displayDetails(string name)
        {
             string NEWSQLQRY = $"select * from dbo.books where bookName like '%{name}%'" ;
            SqlConnection con = new SqlConnection(STRCONNECTION);
            SqlCommand cmd = new SqlCommand(NEWSQLQRY, con);
            try
            {
                con.Open();
                var reader = cmd.ExecuteReader();
                DataTable table = new DataTable("dbo.books");
                table.Load(reader);
                while (reader.Read())
                {
                    Console.WriteLine($"{reader[1]}");
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);
            }
        }
        public static void displayDetailsUsingParameters(string name)
        {
            SqlCommand cmd = new SqlCommand(STRFIND, new SqlConnection(STRCONNECTION));
            try
            {
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Console.WriteLine($"{reader[1]} of rupees {reader[2]} written by {reader[3]}");
                }
            }
            catch (SqlException e)
            {

                Console.WriteLine(e.Message);
            }
            finally
            {
                cmd.Connection.Close();
            }
        }

        public static void addNewRecord(int bookid,string bookName, int bookPrice,string bookAuthor)
        {
            SqlConnection sqlcon = new SqlConnection(STRCONNECTION);
            SqlCommand sqlcmd = new SqlCommand(STRINSERT, sqlcon);
            sqlcmd.Parameters.AddWithValue("@bookname", bookName);
            sqlcmd.Parameters.AddWithValue("@bookid", bookid);
            sqlcmd.Parameters.AddWithValue("@bookprice", bookPrice);
            sqlcmd.Parameters.AddWithValue("@bookauthor", bookAuthor);

            try
            {
                sqlcon.Open();
                var rowsaffected = sqlcmd.ExecuteNonQuery();
                if (rowsaffected!=1)
                {
                    throw new Exception("failed to add record");
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
         
            }
            finally
            {
                sqlcmd.Connection.Close();
            }
        }
        public static void addNewRecordEmp(string empName,string empAdress, int empSalary, int deptId, int ManagerId)
        {
            SqlConnection sqlcon = new SqlConnection(STRCONNECTION);
            SqlCommand sqlcmd = new SqlCommand(STREMPINSERT, sqlcon);
            sqlcmd.Parameters.AddWithValue("@empname", empName);
            sqlcmd.Parameters.AddWithValue("@empadress", empAdress);
            sqlcmd.Parameters.AddWithValue("@empsalary", empSalary);
            sqlcmd.Parameters.AddWithValue("@deptid", deptId);
            sqlcmd.Parameters.AddWithValue("@managerid", ManagerId);

            try
            {
                sqlcon.Open();
                var rowsaffected = sqlcmd.ExecuteNonQuery();
                if (rowsaffected != 1)
                {
                    throw new Exception("failed to add record");
                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);

            }
            finally
            {
                sqlcmd.Connection.Close();
            }
        }

        public static int GetDeptId(string name)
        {
            SqlCommand cmd = new SqlCommand(GETID, new SqlConnection(STRCONNECTION));
            try
            {
                cmd.Parameters.AddWithValue("@deptname", name);
                cmd.Connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    return Convert.ToInt32( reader[0]);
                    //Console.WriteLine($"{reader[1]} of rupees {reader[2]} written by {reader[3]}");
                }
                return 0;
            }
            catch (SqlException e)
            {

                throw e;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }
        
        public static void addNewRecordFromInputs()
        {
           

            string empName = Utilities.Prompt("enter the name of the employee");
            string empadress = Utilities.Prompt("enter the adress of the employee");
            int sal = Utilities.GetNumber("enter the salary");
            displayDeptTable();
            string deptname = Utilities.Prompt("enter the dept name from above");
            //name = Utilities.Prompt("select the dept name from the above list");
            int mgrid = Utilities.GetNumber("enter the managerid");
            int deptid = GetDeptId(deptname);
            addNewRecordEmp(empName, empadress, sal, deptid, mgrid);

            Console.WriteLine("added successfully");


        }

        public static void addNewRecordUsingProc(string name , string address,int salary,int deptId,int mgrid)
        {
            int empid = 0;
            SqlConnection sqlcon = new SqlConnection(STRCONNECTION);
            SqlCommand sqlcmd = new SqlCommand(STRINSERTPROC, sqlcon);
            sqlcmd.CommandType = CommandType.StoredProcedure;
            sqlcmd.Parameters.AddWithValue("@empName", name);
            sqlcmd.Parameters.AddWithValue("@empAdress", address);
            sqlcmd.Parameters.AddWithValue("@empSalary", salary);
            sqlcmd.Parameters.AddWithValue("@deptId", deptId);
            sqlcmd.Parameters.AddWithValue("@ManagerId", mgrid);
            sqlcmd.Parameters.AddWithValue("@empId", empid);

            sqlcmd.Parameters[5].Direction = ParameterDirection.Output;

            try
            {
                sqlcon.Open();
                sqlcmd.ExecuteNonQuery();
                empid = Convert.ToInt32(sqlcmd.Parameters[5].Value);
                Console.WriteLine("the empid of newly added emplotyee is"+empid);
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message); 
            }
            finally
            {
                sqlcmd.Connection.Close();
            }


        }
    }
    class Ex02_UsingClassAndMethods
    {
        static void Main(string[] args)
        {
            //Database.displayAsTable();
            //Database.displayDetails("bbb");
            // Database.displayDetailsUsingParameters("house of the dragons");
            //Database.addNewRecord(123, "Economics", 250, "Ramesh Singh");

            // Database.displayDeptTable();

            // Database.addNewRecordEmp("mahesh", "up", 30000, 2, 1020);



            Database.addNewRecordFromInputs();
           //Database.addNewRecordUsingProc("phani", "banglaore", 70000, 4, 1020);

        }
    }
}
