using ExcelReader.Infrasturcture.Data;
using System;
using System.Collections.Generic;
using System.Text;
using ExcelReader.Domain.Models;

namespace ExcelReader.Application.Services
{
    public class AttendanceService
    {
        public DBContext DBContext { get; set; }

        public AttendanceService() 
        {
            this.DBContext = new DBContext();
        }

        public void MarkAttendance(
          Student student,
          DateTime enterDate,
          DateTime exitDate,
          int participationMinutes,
          int waitingMinutes)
        {
            if (student == null)
            {
                Console.WriteLine("Student not found.");
                return;
            }

            Attendance attendance = new Attendance
            {
                Student = student,
                EnterDate = enterDate,
                ExitDate = exitDate,
                ParticipationMinutes = participationMinutes,
                WaitingMinutes = waitingMinutes
            };

            this.DBContext.Attendances.Add(attendance);

            Console.WriteLine(
                $"Attendance marked for {student.FirstName} {student.LastName}"
            );
        }

        public List<Attendance> GetAttendance()
        {
            return this.DBContext.Attendances;
        }
    }
}
