using System;
using System.Collections.Generic;
using System.Linq;

namespace EZGrade
{
    public class Grade
    {
        public int NumberMissed { get; set; }

        public int GradeValue { get; set; }

        public Grade(int i, int p)
        {
            // TODO: Complete member initialization
            NumberMissed = i;
            GradeValue = p;
        }
    }
}