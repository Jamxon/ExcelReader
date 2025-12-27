using ExcelReader.Domain.Models;
using ExcelReader.Infrasturcture.Data;
using OfficeOpenXml;
using System.Globalization;

namespace ExcelReader.Application.Services
{
    public class AttendanceService
    {
        public DBContext DBContext { get; set; }

        public AttendanceService()
        {
            this.DBContext = new DBContext();
        }

        public List<ExternalAttendance> ReadExcelToExternalAttendance(string path)
        {
            var result = new List<ExternalAttendance>();

            using (var package = new ExcelPackage(new FileInfo(path)))
            {
                var worksheet = package.Workbook.Worksheets.FirstOrDefault();

                if (worksheet is null)
                {
                    throw new Exception("Sheet1 topilmadi. Excel sheet nomini tekshiring.");
                }

                int rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++)
                {
                    result.Add(new ExternalAttendance
                    {
                        FullNameWithCode = worksheet.Cells[row, 1].Text,
                        Email = worksheet.Cells[row, 2].Text,
                        EnterDate = ParseExcelDate(worksheet.Cells[row, 3]),
                        ExitDate = ParseExcelDate(worksheet.Cells[row, 4]),
                        Duration = int.Parse(worksheet.Cells[row, 5].Text),
                        IsHost = worksheet.Cells[row, 6].Text,
                        IsWaiting = worksheet.Cells[row, 7].Text
                    });
                }
            }

            return result;
        }

        public List<ExternalAttendance> FilterExternalAttendance(
            List<ExternalAttendance> list)
        {
            return list
                .Where(x =>
                    x.IsHost == "Да" &&
                    x.Duration > 0
                )
                .ToList();
        }

        public void ImportToAttendance(
    List<ExternalAttendance> externals,
    List<Student> students)
        {
            var grouped = externals
                .GroupBy(x => new
                {
                    StudentCode = ExtractCode(x.FullNameWithCode),
                    Date = x.EnterDate.Date
                });

            foreach (var group in grouped)
            {
                var student = students
                    .FirstOrDefault(s => s.Code == group.Key.StudentCode);

                if (student == null)
                    continue;

                int participation = group
                    .Where(x => x.IsWaiting != "Да")
                    .Sum(x => x.Duration);

                int waiting = group
                    .Where(x => x.IsWaiting == "Да")
                    .Sum(x => x.Duration);

                var enterDate = group.Min(x => x.EnterDate);
                var exitDate = group.Max(x => x.ExitDate);

                var attendance = new Attendance
                {
                    Student = student,
                    EnterDate = enterDate,
                    ExitDate = exitDate,
                    ParticipationMinutes = participation,
                    WaitingMinutes = waiting
                };

                DBContext.Attendances.Add(attendance);
            }
        }

        private string ExtractCode(string fullNameWithCode)
        {
            // misol: "Ali Valiyev (123)"
            var digits = new string(fullNameWithCode.Where(char.IsDigit).ToArray());
            return digits;
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


        private DateTime ParseExcelDate(ExcelRange cell)
        {
            // 1️⃣ Agar Excel ichida Date bo‘lsa
            if (cell.Value is DateTime dt)
                return dt;

            // 2️⃣ Agar Excel numeric (OADate) bo‘lsa
            if (cell.Value is double oa)
                return DateTime.FromOADate(oa);

            // 3️⃣ Agar TEXT bo‘lsa
            var text = cell.Text?.Trim();

            string[] formats =
            {
                "dd.MM.yyyy hh:mm:ss tt",
                "dd.MM.yyyy HH:mm:ss",
                "dd.MM.yyyy",
                "MM/dd/yyyy hh:mm:ss tt"
            };

            if (DateTime.TryParseExact(
                    text,
                    formats,
                    CultureInfo.InvariantCulture,
                    DateTimeStyles.None,
                    out DateTime parsed))
            {
                return parsed;
            }

            throw new FormatException($"Invalid date format: {text}");
        }
    }
}
