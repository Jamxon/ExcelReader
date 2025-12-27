using ExcelReader.Application.Services;
using ExcelReader.Domain.Models;
using OfficeOpenXml;
using System;
using System.Diagnostics.Metrics;

namespace ExcelReader.Client
{
    class Program
    {
        static StudentService studentService = new StudentService();
        static AttendanceService attendanceService = new AttendanceService();

        static void Main(string[] args)
        {
            int counter = 0;
            while (counter < 5)
            {
                counter++;
                Console.Clear();
                DrawHeader("STUDENT ATTENDANCE SYSTEM");

                Console.WriteLine("1. Talaba qo‘shish");
                Console.WriteLine("2. Talabalar ro‘yxati");
                Console.WriteLine("3. Attendance belgilash");
                Console.WriteLine("4. Attendance ro‘yxati");
                Console.WriteLine("5. Attendance update qilish");
                Console.WriteLine("6. Talaba tahrirlash");
                Console.WriteLine("7. Code bo'yicha qidirish");
                Console.WriteLine("8. Ism bo'yicha qidirish");
                Console.WriteLine("9. Talaba o‘chirish");
                Console.WriteLine("10. Excel Read");
                Console.WriteLine("0. Chiqish");

                Console.Write("\nTanlang: ");
                string choice = Console.ReadLine() ?? "";

                switch (choice)
                {
                    case "1": AddStudent(); break;
                    case "2": ShowStudents(); break;
                    case "3": MarkAttendance(); break;
                    case "4": ShowAttendances(); break;
                    case "5": UpdateAttendance(); break;
                    case "6": UpdateStudent(); break;
                    case "7": SearchStudentByCode(); break;
                    case "8": SearchStudentByName(); break;
                    case "9": DeleteStudent(); break;
                    case "10": ExcelRead(); break;
                    case "0": return;
                }
            }
        }

        // ================= STUDENT =================

        static void AddStudent()
        {
            Console.Clear();
            DrawHeader("TALABA QO‘SHISH");

            Console.Write("Ism: ");
            string firstName = Console.ReadLine() ?? "";

            Console.Write("Familiya: ");
            string lastName = Console.ReadLine() ?? "";

            Console.Write("Email: ");
            string email = Console.ReadLine() ?? "";

            Console.Write("Code: ");
            string code = Console.ReadLine() ?? "";

            studentService.AddStudent(firstName, lastName, email, code);

            Success("Talaba muvaffaqiyatli qo‘shildi!");
        }

        static void UpdateStudent()
        {
            Console.Clear();
            DrawHeader("TALABA TAHRIRLASH");
            var students = studentService.GetStudent();
            if (students.Count == 0)
            {
                Error("Talabalar mavjud emas!");
                return;
            }
            for (int i = 0; i < students.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {students[i].FirstName} {students[i].LastName}");
            }
            Console.Write("\nTalabani tanlang: ");
            int index = int.Parse(Console.ReadLine() ?? "1") - 1;
            Console.Write("Yangi Ism: ");
            string firstName = Console.ReadLine() ?? "";
            Console.Write("Yangi Familiya: ");
            string lastName = Console.ReadLine() ?? "";
            Console.Write("Yangi Email: ");
            string email = Console.ReadLine() ?? "";
            Console.Write("Yangi Code: ");
            string code = Console.ReadLine() ?? "";
            studentService.UpdateStudent(
                students[index],
                firstName,
                lastName,
                email,
                code
            );
            Success("Talaba muvaffaqiyatli tahrirlandi!");
        }

        static void SearchStudentByCode()
        {
            Console.Clear();
            DrawHeader("CODE BO'YICHA QIDIRISH");
            Console.Write("Qidirilayotgan Code ni kiriting: ");
            string code = Console.ReadLine() ?? "";
            var foundStudents = studentService.SearchStudentByCode(code);
            if (foundStudents.Count == 0)
            {
                Error("Hech qanday talaba topilmadi!");
                return;
            }
            Console.WriteLine("\nTopilgan talabalar:");
            foreach (var s in foundStudents)
            {
                Console.WriteLine($"{s.Code} | {s.FirstName} {s.LastName}");
            }
            Pause();
        }

        static void SearchStudentByName()
        {
            Console.Clear();
            DrawHeader("ISM BO'YICHA QIDIRISH");
            Console.Write("Qidirilayotgan Ism ni kiriting: ");
            string name = Console.ReadLine() ?? "";
            var foundStudents = studentService.SearchStudentByName(name);
            if (foundStudents.Count == 0)
            {
                Error("Hech qanday talaba topilmadi!");
                return;
            }
            Console.WriteLine("\nTopilgan talabalar:");
            foreach (var s in foundStudents)
            {
                Console.WriteLine($"{s.Code} | {s.FirstName} {s.LastName}");
            }
            Pause();
        }

        static void DeleteStudent()
        {
            Console.Clear();
            DrawHeader("TALABA O‘CHIRISH");
            var students = studentService.GetStudent();
            if (students.Count == 0)
            {
                Error("Talabalar mavjud emas!");
                return;
            }
            for (int i = 0; i < students.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {students[i].FirstName} {students[i].LastName}");
            }

            Console.Write("\nTalabani tanlang (code): ");
            int code= int.Parse(Console.ReadLine() ?? "1");

            var student = students.Find(s => s.Code == code.ToString());
            if (student == null)
            {
                Error("Talaba topilmadi!");
                return;
            }

            studentService.DeleteStudent(student);
            
            Success("Talaba muvaffaqiyatli o‘chirildi!");
            Pause();
        }

        static void ShowStudents()
        {
            Console.Clear();
            DrawHeader("TALABALAR RO‘YXATI");

            var students = studentService.GetStudent();

            foreach (var s in students)
            {
                Console.WriteLine($"{s.Code} | {s.FirstName} {s.LastName}");
            }

            Pause();
        }

        // ================= ATTENDANCE =================

        static void MarkAttendance()
        {
            Console.Clear();
            DrawHeader("ATTENDANCE BELGILASH");

            var students = studentService.GetStudent();

            if (students.Count == 0)
            {
                Error("Talabalar mavjud emas!");
                return;
            }

            for (int i = 0; i < students.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {students[i].FirstName} {students[i].LastName}");
            }

            Console.Write("\nTalabani tanlang: ");
            int index = int.Parse(Console.ReadLine() ?? "1") - 1;

            Console.Write("Participation minutes: ");
            int participation = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Waiting minutes: ");
            int waiting = int.Parse(Console.ReadLine() ?? "0");

            attendanceService.MarkAttendance(
                students[index],
                DateTime.Now,
                DateTime.Now.AddHours(2),
                participation,
                waiting
            );

            Success("Attendance belgilandi!");
        }

        static void ShowAttendances()
        {
            Console.Clear();
            DrawHeader("ATTENDANCE RO‘YXATI");

            var attendances = attendanceService.GetAttendance();

            foreach (var a in attendances)
            {
                Console.WriteLine(
                    $"{a.Student.FirstName} {a.Student.LastName} | " +
                    $"Participation: {a.ParticipationMinutes} | Waiting: {a.WaitingMinutes}"
                );
            }

            Pause();
        }

        static void UpdateAttendance()
        {
            Console.Clear();
            DrawHeader("ATTENDANCE UPDATE");

            var attendances = attendanceService.GetAttendance();

            if (attendances.Count == 0)
            {
                Error("Attendance topilmadi!");
                return;
            }

            for (int i = 0; i < attendances.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {attendances[i].Student.FirstName} {attendances[i].Student.LastName}");
            }

            Console.Write("\nAttendance tanlang: ");
            int index = int.Parse(Console.ReadLine() ?? "1") - 1;

            Console.Write("Yangi Participation minutes: ");
            attendances[index].ParticipationMinutes = int.Parse(Console.ReadLine() ?? "0");

            Console.Write("Yangi Waiting minutes: ");
            attendances[index].WaitingMinutes = int.Parse(Console.ReadLine() ?? "0");

            Success("Attendance yangilandi!");
        }
        static void ExcelRead()
        {
            Console.Clear();
            DrawHeader("EXCEL IMPORT");

            string path = @"C:\Users\ThinkPad\Desktop\ExcelReader\assets\fixed_attendance.xlsx";

            var externalList = attendanceService
                .ReadExcelToExternalAttendance(path);

            Console.WriteLine($"Exceldan o‘qildi: {externalList.Count}");

            var filtered = attendanceService
                .FilterExternalAttendance(externalList);

            Console.WriteLine($"Filterdan o‘tdi: {filtered.Count}");

            var students = studentService.GetStudent();

            attendanceService.ImportToAttendance(filtered, students);

            Success("Attendance muvaffaqiyatli import qilindi!");
        }

        static void DrawHeader(string text)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("======================================");
            Console.WriteLine($"   {text}");
            Console.WriteLine("======================================");
            Console.ResetColor();
        }

        static void Success(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n✔ {msg}");
            Console.ResetColor();
            Pause();
        }

        static void Error(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n✖ {msg}");
            Console.ResetColor();
            Pause();
        }

        static void Pause()
        {
            Console.WriteLine("\nDavom etish uchun tugma bosing...");
            Console.ReadKey();
        }
    }
}
