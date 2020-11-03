using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Newtonsoft.Json;

namespace ComAp_Assignment
{
    class Program
    {
        private static List<Student> AllStudents = new List<Student>();
        private static List<string> Errors = new List<string>();

        static void Main(string[] args)
        {
            string fileName = "examination.txt";
            string resultFileName = "ExaminationResult.json";
            ReadFile(fileName);
            Console.WriteLine("File loaded");
            CreateResults(resultFileName);
            Console.WriteLine("Result Saved");
            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }

        private static void CreateResults(string fileName)
        {
            JsonResult result = new JsonResult(AllStudents, Errors);
            string JSONresult = JsonConvert.SerializeObject(result);

            using (var tw = new StreamWriter(fileName, false))
            {
                tw.WriteLine(JSONresult);
                tw.Close();
            }
        }

        private static void ReadFile(string fileName)
        {
            StreamReader reader = new StreamReader(fileName);
            string currentGroup = "";
            List<string> row;
            do
            {
                row = reader.ReadLine()?.Split(';').ToList();
                if (row == null) continue;
                if (string.IsNullOrEmpty(row[0]) || row.Contains("Entrance examination"))
                    continue;
                if (row.Where(x => x.Contains("Group")).Any() && row.Count == 1)
                {
                    currentGroup = row[0];
                    continue;
                }
                else
                {
                    string studentName = row[0];
                    string math = row[1].Substring(5);
                    string physics = row[2].Substring(8);
                    string english = row[3].Substring(8);
                    
                    if (ValidateAndCreateStudent(currentGroup, studentName, math, physics, english, out Student student))
                        AllStudents.Add(student);                    
                }

            } while (row != null);

        }
        
        private static bool ValidateAndCreateStudent(string groupName, string studentName, string math, string physics, string english, out Student student)
        {
            student = null;
            bool isValid = true;

            if (int.TryParse(math, out int mathRating)) {
                if (!IsInRange(mathRating)) {
                    LogError(studentName, "Math");
                    isValid = false;
                }
            }
            else {
                LogError(studentName, "Math");
                isValid = false;
            }

            if (int.TryParse(physics, out int physicsRating)) {
                if (!IsInRange(physicsRating)) {
                    LogError(studentName, "Physics");
                    isValid = false;
                }
            }
            else {
                LogError(studentName, "Physics");
                isValid = false;
            }

            if (int.TryParse(english, out int englishRating)) {
                if (!IsInRange(englishRating)) {
                    LogError(studentName, "English");
                    isValid = false;
                }
            }
            else {
                LogError(studentName, "English");
                isValid = false;
            }   
            
            if (isValid)
                student = new Student(mathRating, physicsRating, englishRating, groupName, studentName);

            return isValid;

        }

        private static bool IsInRange(int num)
        {
            return (num >= 0 && num <= 100);
        }

        private static void LogError(string studentName, string examType)
        {
            Errors.Add($"Student '{studentName}' has incorect input from {examType} exam.");
        }
    }
}
