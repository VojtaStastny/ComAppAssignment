using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComAp_Assignment
{
    public class JsonResult
    {
        public List<StudentResult> StudentResults;

        public List<string> Errors;

        public GroupResult GlobalResults;

        public List<GroupResult> GroupResults;

        public JsonResult(List<Student> allStudents, List<string> errors)
        {
            StudentResults = GetStudentResults(allStudents);
            GlobalResults = GetGlobalResults(allStudents);
            GroupResults = GetGroupResults(allStudents);
            Errors = errors;
        }

        private List<GroupResult> GetGroupResults(List<Student> allStudents)
        {
            List<string> GroupNames = allStudents.Select(x => x.GroupName).Distinct().ToList();
            List<GroupResult> result = new List<GroupResult>();

            foreach (string groupName in GroupNames)
            {
                List<Student> groupStudents = allStudents.Where(x => x.GroupName == groupName).ToList();

                SubjectResult Math = new SubjectResult();
                Math.ExamType = "Math";
                Math.Average = groupStudents.Sum(x => x.MathRating) / groupStudents.Count;
                Math.Median = GetMedian(groupStudents, "Math");
                Math.Modus = GetModus(groupStudents, "Math");

                SubjectResult Physics = new SubjectResult();
                Physics.ExamType = "Physics";
                Physics.Average = groupStudents.Sum(x => x.PhysicsRating) / groupStudents.Count;
                Physics.Median = GetMedian(groupStudents, "Physics");
                Physics.Modus = GetModus(groupStudents, "Physics");

                SubjectResult English = new SubjectResult();
                English.ExamType = "English";
                English.Average = groupStudents.Sum(x => x.EnglishRating) / groupStudents.Count;
                English.Median = GetMedian(groupStudents, "English");
                English.Modus = GetModus(groupStudents, "English");

                result.Add(new GroupResult() {
                                Name = groupName,
                                SubjectResults = new List<SubjectResult>() {
                                    Math,
                                    Physics,
                                    English
                                }
                });
            }

            return result;
        }

        private List<StudentResult> GetStudentResults(List<Student> allStudents)
        {
            List<StudentResult> result = new List<StudentResult>();
            foreach (Student s in allStudents)
            {
                result.Add(new StudentResult()
                {
                    Name = s.Name,
                    WeightedAverageRating = s.GetWeightedAverage()
                });
            }
            return result;
        }

        private GroupResult GetGlobalResults(List<Student> allStudents)
        {
            GroupResult result = new GroupResult();
            result.Name = "Global";

            SubjectResult Math = new SubjectResult();
            Math.ExamType = "Math";
            Math.Average = allStudents.Sum(x => x.MathRating) / allStudents.Count;
            Math.Median = GetMedian(allStudents, "Math");
            Math.Modus = GetModus(allStudents, "Math");

            SubjectResult Physics = new SubjectResult();
            Physics.ExamType = "Physics";
            Physics.Average = allStudents.Sum(x => x.PhysicsRating) / allStudents.Count;
            Physics.Median = GetMedian(allStudents, "Physics");
            Physics.Modus = GetModus(allStudents, "Physics");

            SubjectResult English = new SubjectResult();
            English.ExamType = "English";
            English.Average = allStudents.Sum(x => x.EnglishRating) / allStudents.Count;
            English.Median = GetMedian(allStudents, "English");
            English.Modus = GetModus(allStudents, "English");

            return new GroupResult() { Name = "Global", SubjectResults = new List<SubjectResult>() { Math, Physics, English } };
        }

        private static double GetMedian(List<Student> students, string examType)
        {
            bool isEven = students.Count % 2 == 0;
            int midIndex = (students.Count / 2);
            List<Student> orderedStudents = null;
            double median = 0;

            switch (examType)
            {
                case "Math":
                    orderedStudents = students.OrderBy(x => x.MathRating).ToList();
                    if (isEven)
                        median = (orderedStudents[midIndex].MathRating + orderedStudents[midIndex - 1].MathRating) / 2;
                    else median = orderedStudents[midIndex].MathRating;
                    break;

                case "Physics":
                    orderedStudents = students.OrderBy(x => x.PhysicsRating).ToList();
                    if (isEven)
                        median = (orderedStudents[midIndex].PhysicsRating + orderedStudents[midIndex - 1].PhysicsRating) / 2;
                    else median = orderedStudents[midIndex].PhysicsRating;
                    break;

                case "English":
                    orderedStudents = students.OrderBy(x => x.EnglishRating).ToList();
                    if (isEven)
                        median = (orderedStudents[midIndex].EnglishRating + orderedStudents[midIndex - 1].EnglishRating) / 2;
                    else median = orderedStudents[midIndex].EnglishRating;
                    break;

            }

            return median;
        }

        private static List<int> GetModus(List<Student> students, string examType)
        {
            List<int> ratings = null;
            List<int> ratingsDist = null;
            List<int> modus = null;
            List<Tuple<int, int>> appearance = new List<Tuple<int, int>>();

            switch (examType)
            {
                case "Math":
                    ratings = students.Select(x => x.MathRating).ToList();
                    ratingsDist = ratings.Distinct().ToList();
                    foreach (int r1 in ratingsDist)
                    {
                        appearance.Add(new Tuple<int, int>(r1, ratings.Where(x => x == r1).Count()));
                    }
                    modus = appearance.Where(x => x.Item2 == appearance.Max(y => y.Item2)).Select(x => x.Item1).ToList();
                    break;

                case "Physics":
                    ratings = students.Select(x => x.PhysicsRating).ToList();
                    ratingsDist = ratings.Distinct().ToList();
                    foreach (int r1 in ratingsDist)
                    {
                        appearance.Add(new Tuple<int, int>(r1, ratings.Where(x => x == r1).Count()));
                    }
                    modus = appearance.Where(x => x.Item2 == appearance.Max(y => y.Item2)).Select(x => x.Item1).ToList();
                    break;

                case "English":
                    ratings = students.Select(x => x.EnglishRating).ToList();
                    ratingsDist = ratings.Distinct().ToList();
                    foreach (int r1 in ratingsDist)
                    {
                        appearance.Add(new Tuple<int, int>(r1, ratings.Where(x => x == r1).Count()));
                    }
                    modus = appearance.Where(x => x.Item2 == appearance.Max(y => y.Item2)).Select(x => x.Item1).ToList();
                    break;
            }

            return modus;
        }

    }
}
