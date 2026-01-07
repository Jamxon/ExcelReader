using ExcelReader.Infrasturcture.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelReader.Application.Services.StudentServices
{
    internal interface IStudentService
    {
        DBContext DBContext { get; set; }
        void AddStudent(string firstName, string lastName, string email, string code);
        List<Domain.Models.Student> GetStudent();
        void UpdateStudent(
            Domain.Models.Student student,
            string firstName,
            string lastName,
            string email,
            string code);
        List<Domain.Models.Student> SearchStudentByCode(string code);

        void DeleteStudent(Domain.Models.Student student);
    }
}
