using System;
using System.Collections.Generic;
using System.Text;
using ExcelReader.Domain.Models;

namespace ExcelReader.Infrasturcture.Data
{
    public class DBContext
    {
        public DBContext() 
        {
            Students = new List<Student>();
            Attendances = new List<Attendance>();
        }

        public List<Student> Students { get; set; }

        public List<Attendance> Attendances { get; set; }
    }
}
