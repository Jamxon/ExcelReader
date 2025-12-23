using System;
using System.Collections.Generic;
using System.Text;
using ExcelReader.Infrasturcture.Data;
using ExcelReader.Domain.Models;

namespace ExcelReader.Application.Services
{
    public class StudentService
    {
        public DBContext DBContext { get; set; }

        public StudentService() 
        { 
            this.DBContext = new DBContext();
        }

        public void AddStudent(string firstName, string lastName, string email, string code) 
        {
            Student newStudent = new Student()
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Code = code ?? new Random().Next(1000, 2000).ToString(),
            };

            this.DBContext.Students.Add(newStudent);
        }

        public List<Student> GetStudent()
        {
            return this.DBContext.Students;
        }
    }
}
