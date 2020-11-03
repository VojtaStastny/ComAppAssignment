using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComAp_Assignment
{
    public class Student
    {
        public string Name;
        public string GroupName;
        public int MathRating;
        public int PhysicsRating;
        public int EnglishRating;
        
        public Student(int math, int physics, int english, string groupName, string studentName)
        {
            MathRating = math;
            PhysicsRating = physics;
            EnglishRating = english;
            GroupName = groupName;
            Name = studentName;
        }

        public double GetWeightedAverage()
        {
            return (MathRating * 0.4) + (PhysicsRating * 0.35) + (EnglishRating * 0.25);
        }
    }
}
