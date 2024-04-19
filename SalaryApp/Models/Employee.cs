using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalaryApp.Models
{
    public class Employee
    {
        public Entry NameEntry { get; set; }

        public Entry Entry { get; set; }

        public Employee(Entry name, Entry entry)
        {
            NameEntry = name;
            Entry = entry;
        }
    }
}
