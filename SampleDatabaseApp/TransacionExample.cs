using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/*
     * Requirement: Insert a new Employee with Dept Details. if the Dept name exists,
     * extract the id and insert into Employee Table, else create a new record in the DeptTable for the new DeptName,
     * and extract the newly created DeptID and add the Employee.
     * 
     * InsertEmployee StoredProc should be there. 
     * A Function to Extract the DeptID based on DeptName.
     * A statement to insert the DeptName into the DeptTable. 
     */


namespace SampleDatabaseApp
{

    class TransactionDemo
    {
        static readonly string strConnection = ConfigurationManager.ConnectionStrings["myConnection"].ConnectionString;

        private static void addEmployee(string name,string address,int salary,string deptName)
        {
            SqlTransaction transaction = null;
            SqlConnection connection = new SqlConnection(strConnection);
            string cmdGetDeptId = $"select dbo.GetDept('{deptName} as DeptId')";
            string cmdInsertDept = "InsertDept";
            int deptId = 0;

            try
            {
                connection.Open();
                transaction = connection.BeginTransaction();
                SqlCommand cmd1 = new SqlCommand(cmdGetDeptId, connection, transaction);
                deptId = (int)cmd1.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }

        }
    }
    class TransacionExample
    {
    }
}
