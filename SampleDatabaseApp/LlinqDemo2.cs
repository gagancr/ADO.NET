using System.IO;
using System;
using System.Collections.Generic;
using System.Linq;
/*
 * LINQ stands for Language Integrated Queries. 
 * LINQ allows to perform SQL like Queries on Collection objects of .NET.
 * System.Linq is the namespace for working with Collections. 
 * With this namespace, u get new keywords of query like from, in, where orderby group by, join, select for performing queries. 
 * The Query executes when the iteration happens. 
 */
namespace SampleDatabaseApp
{
    class Employe
    {
        public int EmpId { get; set; }
        public string EmpName { get; set; }
        public string EmpCity { get; set; }
        public int EmpSalary { get; set; }
        public int DeptId { get; set; }
    }

    class Departments
    {
        public int Deptid { get; set; }
        public string DeptName { get; set; }

    }

    static class DataComponents
    {
        const string fileName = "../../SampleData.csv";


        
        private static List<Departments> getAllDepts()
        {
        List<Departments> dept=new List<Departments>();

            dept.Add(new Departments { Deptid = 1, DeptName = "Development" });
            dept.Add(new Departments { Deptid = 2, DeptName = "Testing" });
            dept.Add(new Departments { Deptid = 3, DeptName = "IT HelpDesk" });
            dept.Add(new Departments { Deptid = 4, DeptName = "RCM" });
            dept.Add(new Departments { Deptid = 5, DeptName = "Medical Coding" });


            return dept;
        }
        private static List<Employe> getAll()
        {
            var list = new List<Employe>();
            var lines = File.ReadAllLines(fileName);
            foreach (var line in lines)
            {
                var words = line.Split(',');
                var newEmp = new Employe
                {
                    EmpId = int.Parse(words[0]),
                    EmpName = words[1],
                    EmpCity = words[2],
                    EmpSalary = int.Parse(words[3]),
                    DeptId = int.Parse(words[4])
                };
                list.Add(newEmp);
            }
            return list;
        }
        public static List<Employe> AllRecords => getAll();
        public static List<Departments> AllDepartments => getAllDepts();
    }

    class LINQProgram
    {
        static List<Employe> data = DataComponents.AllRecords;
        static List<Departments> dept = DataComponents.AllDepartments;
        static void Main()
        {
            //displayAllNames();
            //displayNamesAndAddresses();
            //displayNamesFromCity("Springdale");'
            //displayNamesWithSalariesGreaterThan(300000);
            //displayNamesOrderbyName();
            //displayUniqueCities();

            //avgData();

            //displayEmployeesAboveAvgSal();

            // displayNamesGroupedByCity();
            // displayGroupedPeople();
            // getEmployeeWithDeptName();

            Console.WriteLine("available departments");
            foreach (var item in dept)
            {
                Console.WriteLine(item.DeptName);
            }
            getEmployeeOnDeptName(Utilities.Prompt("enter the name of the department to check employees").ToLower());

        }

        private static void getEmployeeOnDeptName(string name)
        {
            var query= from emp in data
                       from dpt in dept
                       where dpt.DeptName.ToLower()==name&& emp.DeptId == dpt.Deptid
                       select new { emp.EmpName, emp.DeptId, dpt.DeptName };

            foreach (var item in query)
            {
                Console.WriteLine($"{item.EmpName}  {item.DeptId}  {item.DeptName}");
            }

        }

        private static void getEmployeeWithDeptName()
        {
            //var deptquery = from dpt in dept select dpt.Deptid;
            var query = from emp in data from dpt in dept where emp.DeptId == dpt.Deptid select new { emp.EmpName, emp.DeptId, dpt.DeptName };
            foreach (var item in query)
            {
                Console.WriteLine($"{item.EmpName}  {item.DeptId}  {item.DeptName}");
            }
        }
        
        private static void displayNamesGroupedByCity()
        {
            var query = from emp in data group emp.EmpName by emp.EmpCity into grp orderby grp.Key ascending select grp;
            foreach (var item in query)
            {
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine(item.Key);
                Console.WriteLine("-------------------------------------------");
                foreach (var line in item)
                {
                Console.WriteLine(line);

                }
            }
        }

        private static void displayGroupedPeople()
        {
          var query1 = from em in data group em.EmpName by em.EmpName[0] into grp orderby grp.Key select grp ;
             
           

            foreach (var item in query1)
            {
                Console.WriteLine("-------------------------------------------");
                Console.WriteLine("Names starting with "+item.Key);
                Console.WriteLine("-------------------------------------------");
                foreach (var name in item)
                {
                    Console.WriteLine(name);
                }
               // Console.WriteLine(item.Key);
            }
        }

        private static void avgData()
        {
            var query = (from emp in data select emp.EmpSalary).Average();  //.max .min .average
            Console.WriteLine(query);
        }
        private static void displayEmployeesAboveAvgSal()
        {
            var query = from emp in data where emp.EmpSalary > (from emp1 in data select emp1.EmpSalary).Average() select new { emp.EmpName, emp.EmpSalary };

            foreach (var item in query)
            {
                Console.WriteLine(item.EmpName +" earning "+item.EmpSalary);
            }
        }
        private static void displayUniqueCities()
        {
            var query = (from rec in data
                         select rec.EmpCity).Distinct();
            foreach (var cityName in query)
                Console.WriteLine(cityName);
        }

        private static void displayNamesOrderbyName()
        {
            var query = from rec in data
                        orderby rec.EmpName descending
                        select rec.EmpName;
            foreach (var name in query)
                Console.WriteLine(name);
        }

        private static void displayNamesWithSalariesGreaterThan(int salary)
        {
            var query = from rec in data
                        where rec.EmpSalary >= salary && rec.EmpName.StartsWith("S")
                        select new { rec.EmpName, rec.EmpSalary };
            foreach (var name in query)
                Console.WriteLine(name);
        }

        //Display names of employees whose salary is more than 50000....
        private static void displayNamesFromCity(string city)
        {
            var query = from rec in data
                        where rec.EmpCity == city
                        select rec.EmpName;
            foreach (var name in query)
                Console.WriteLine(name);
        }

        private static void displayNamesAndAddresses()
        {
            var query = from rec in data
                        select new { Name = rec.EmpName, Address = rec.EmpCity };
            foreach (var res in query)
                Console.WriteLine($"{res.Name} from {res.Address}");
        }

        private static void displayAllNames()
        {
            var query = from emp in data
                        select emp.EmpName;
            foreach (var name in query)
                Console.WriteLine(name.ToUpper());
        }
    }
}
