using System;

namespace SyncCollege;

public class StudentDetails
{
    private static int s_studentID = 3000;
    public string StudentID { get; set; }
    public string StudentName { get; set; }
    public string FatherName { get; set; }
    public DateTime DOB { get; set; }
    public Gender Gender { get; set; }
    public int Physics { get; set; }
    public int Chemistry { get; set; }
    public int Maths { get; set; }

    public bool IsEligible()
    {
        int totalMarks = Physics + Chemistry + Maths;
        if (totalMarks / 3 >= 75.0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public StudentDetails(string studentName, string fatherName, DateTime dob, Gender gender, int physics, int chemistry, int maths)
    {
        StudentID = $"SF{s_studentID++}";
        StudentName = studentName;
        FatherName = fatherName;
        DOB = dob;
        Gender = gender;
        Physics = physics;
        Chemistry = chemistry;
        Maths = maths;
    }
}

public enum Gender
{
    Male,
    Female,
    Transgender
}
