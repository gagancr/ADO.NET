using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


/*linq allows to perform sql like queries on collection objects of .net
 * 
 * 
 */

namespace SampleDatabaseApp
{

    class Employees
    {
        public int empid { get; set; }
        public string  empName { get; set; }
        public string empCity { get; set; }
        public string empContact { get; set; }

      
    }
    class Datacomponnet
    {
        const string file= "../../Employees.csv";

       public static List<Employees> getAllEmployees()
        {
            List<Employees> emp = new List<Employees>();

            var lines= File.ReadAllLines(file);

            foreach (var item in lines)
            {
                string[] words = item.Split(',');
                Employees e = new Employees
                {
                    empid = int.Parse(words[0]),
                    empName = words[1],
                    empCity = words[2],
                    empContact = words[3]
                };
                emp.Add(e);
            }
            return emp;
        }
        
        
    }
    class LinqDemo
    {
           static List<Employees> data = Datacomponnet.getAllEmployees();
        private static void displayAllNames()
        {
            var query = from emp in data select emp.empContact;
            foreach (var item in query)
            {
                Console.WriteLine(item);
            }
        }

       private static void displayNamesAndAddresses()
        {
            var query = from rec in data select new { rec.empName, rec.empCity ,rec.empContact};

            foreach (var item in query)
            {
                Console.WriteLine($"{item.empName} from {item.empCity} phone number {item.empContact} ");
            }
        }

        private static void displayNamesFromCity(string city)
        {
            var query = from emp in data where emp.empCity.ToLower() == city.ToLower() select new { emp.empName, emp.empCity };

            foreach (var item in query)
            {
                Console.WriteLine($"{item.empName} from {item.empCity}");
            }
        }

        private static void displayNameWithSalary(int id ,string str)//and name starts with s
        {

            var query = from employee in data where employee.empid > id && employee.empName.ToLower().StartsWith(str) select new { employee.empid, employee.empCity, employee.empName };
            Console.WriteLine($"greater than id -{id} and name starts with {str} are");
            foreach (var item in query)
            {
                Console.WriteLine($" {item.empid}  {item.empName}  {item.empCity}");
            }
        }
        private static void displayOrderByName()
        {
            var query = from emp in data orderby emp.empName select emp.empName;
            foreach (var item in query)
            {
                Console.WriteLine(item);
            }
        }
        private static void uniqueData()
        {
            var query = (from emp in data orderby emp.empCity select emp.empCity ).Distinct();
            foreach (var item in query)
            {
                Console.WriteLine(item);
            }
        }

        private static void maximumData()
        {
            var query = (from emp in data select emp.empid).Average();  //.max .min .average
            Console.WriteLine(query);
        }


        static void Main(string[] args)
        {
            //displayAllNames();
            //displayNamesAndAddresses();
            //displayNamesFromCity("MAndYa");
            //  displayNameWithSalary(Utilities.GetNumber("enter the id greater than which to fetch data"),Utilities.Prompt("enter a letter from which starting names you want to fetch").ToLower());
            //displayOrderByName();
            // uniqueData();
            maximumData();

        }
    }
}
