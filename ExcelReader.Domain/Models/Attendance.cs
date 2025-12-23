using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelReader.Domain.Models
{
    public class Attendance
    {
        public Student Student { get; set; }
        public DateTime EnterDate { get; set; }
        public DateTime ExitDate { get; set; }
        public int ParticipationMinutes { get; set; }
        public int WaitingMinutes { get; set; }
    }
}
