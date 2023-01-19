using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Diagnostics;

namespace SampleDatabaseApp
{
    class DisconnectedModelDemo
    {
        static string StrConnection = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;
        const string query = "select * from employees;select * from tblDept";
        
           static DataSet disconnectedObj = new DataSet("AllRecords");
           static SqlDataAdapter adapter = null;

        static void fillRecords()
        {
            SqlConnection con = new SqlConnection(StrConnection);
            SqlCommand cmd = new SqlCommand(query, con);
           adapter = new SqlDataAdapter(cmd);
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            adapter.Fill(disconnectedObj);
            disconnectedObj.Tables[0].TableName = "EmployeeList";

            //setting primary key
            if (disconnectedObj.Tables[0].PrimaryKey.Length == 0)
                disconnectedObj.Tables[0].PrimaryKey = new DataColumn[]
                {
                    disconnectedObj.Tables[0].Columns[0]
                };
            


            disconnectedObj.Tables[1].TableName = "DeptList";
            Trace.WriteLine("connection state " + con.State);

        }

        static void insertEmployee(string name,string address,int salary,int deptId,int mgrid)
        {
            DataRow newRow = disconnectedObj.Tables[0].NewRow();
            newRow[0] = 0;
            newRow[1] = name;
            newRow[2] = address;
            newRow[3] = salary;
            newRow[4] = deptId;
            newRow[5] = mgrid;
            
                disconnectedObj.Tables[0].Rows.Add(newRow);
           
            
            adapter.Update(disconnectedObj, "EmployeeList");
            
        }

        //1st way
        static void updateEmployee(int id ,string name, string address, int salary, int deptId, int mgrid)
        {
           
            foreach (DataRow row in disconnectedObj.Tables[0].Rows)
            {
                if (row[0].ToString()==id.ToString())
                {
                         row[1] = name;
                         row[2] = address;
                         row[3] = salary;
                         row[4] = deptId;
                         row[5] = mgrid;

                }
            }
           // disconnectedObj.Tables[0].Rows.Add(newRow);
            adapter.Update(disconnectedObj, "EmployeeList");
        }

        //way 2
        static void updateEmployee2(int id, string name, string address, int salary, int deptId, int mgrid)
        {
            var selectedRow = disconnectedObj.Tables[0].Rows.Find(id);
            selectedRow[1] = name;
            selectedRow[2] = address;
            selectedRow[3] = salary;
            selectedRow[4] = deptId;
            selectedRow[5] = mgrid;
 
            // disconnectedObj.Tables[0].Rows.Add(newRow);
            adapter.Update(disconnectedObj, "EmployeeList");

        }
        private static void deleteEmployee(int id)
        {
            foreach (DataRow row in disconnectedObj.Tables[0].Rows)
            {
                if (row[0].ToString()==id.ToString())
                {
                    row.Delete();
                    break;
                }
            }
            adapter.Update(disconnectedObj, "EmployeeList");
        }

        static void Main(string[] args)
        {

            fillRecords();
            //foreach (DataRow row in disconnectedObj.Tables[0].Rows) //you can also write table["firstTable"]
            //{
            //    Console.WriteLine(row[1]);          //row["empname"]
            //}

            //  DisplayEmployeesOfDept("Development");

            insertEmployee("nani", "kgf", 35000, 4, 1020);
            //deleteEmployee(1056);
            //updateEmployee2(1055, "Mahadev", "Seehalli", 65000, 5, 1019);
        }

        static void DisplayEmployeesOfDept(string deptName)
        {
            int deptId = 0;
            foreach (DataRow row in disconnectedObj.Tables["deptList"].Rows)
            {
                if (row["deptName"].ToString()==deptName)
                {
                    deptId = (int)row["deptId"];
                    break;
                }
            }
            foreach (DataRow row in disconnectedObj.Tables[0].Rows)
            {
                if (Convert.ToInt32(row["deptId"]) == deptId)
                {
                    Console.WriteLine(row["empName"]);
                }
            }
        }
    }
}
