using System;
using System.Collections.Generic;
using System.Text;
using ExcelReader.Infrasturcture.Data;
using ExcelReader.Domain.Models;

namespace ExcelReader.Application.Services.StudentServices
{
    public class StudentService : IStudentService
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

        public void UpdateStudent(
            Student student,
            string firstName,
            string lastName,
            string email,
            string code)
        {
            if (student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }
            student.FirstName = firstName;
            student.LastName = lastName;
            student.Email = email;
            student.Code = code;
            Console.WriteLine(
                $"Student {student.FirstName} {student.LastName} updated successfully."
            );
        }
        public List<Student> SearchStudentByCode(string code)
        {
            List<Student> foundStudents = new List<Student>();
            foreach (var student in this.DBContext.Students)
            {
                if (student.Code.Equals(code, StringComparison.OrdinalIgnoreCase))
                {
                    foundStudents.Add(student);
                }
            }
            return foundStudents;
        }

        public List<Student> SearchStudentByName(string name)
        {
            List<Student> foundStudents = new List<Student>();
            foreach (var student in this.DBContext.Students)
            {
                if (student.FirstName.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    foundStudents.Add(student);
                }
            }
            return foundStudents;
        }

        public void DeleteStudent(Student student)
        {
            if (student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }
            this.DBContext.Students.Remove(student);
            Console.WriteLine(
                $"Student {student.FirstName} {student.LastName} deleted successfully."
            );
        }
    }
}
