using ExcelReader.Domain.Models;
using ExcelReader.Infrasturcture.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelReader.Application.Services.AttendanceServices
{
    internal interface IAttendanceService
    {
        DBContext DBContext { get; set; }

        List<ExternalAttendance> ReadExcelToExternalAttendance(string path);

        List<ExternalAttendance> FilterExternalAttendance(
            List<ExternalAttendance> list);

        void ImportToAttendance(
            List<ExternalAttendance> externalAttendances,
            List<Student> students);
    }
}
