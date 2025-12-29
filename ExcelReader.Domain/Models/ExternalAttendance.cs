using System;
using System.Collections.Generic;
using System.Text;

namespace ExcelReader.Domain.Models
{
    public class ExternalAttendance
    {
        public string FullNameWithCode { get; set; }
        public string Email { get; set; }
        public DateTime EnterDate { get; set; }
        public DateTime ExitDate { get; set; }
        public int Duration { get; set; }
        public string IsGuest { get; set; }
        public string IsWaiting { get; set; }
    }
}
