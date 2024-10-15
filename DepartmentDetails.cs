using System;

namespace SyncCollege;

public class DepartmentDetails
{
    private static int s_departmentID = 101;
    public string DepartmentID { get; set; }
    public DepartmentName DepartmentName { get; set; }
    public int NumberOfSeats { get; set; }

    public DepartmentDetails(DepartmentName departmentName, int numberOfSeats)
    {
        DepartmentID = $"DID{s_departmentID++}";
        DepartmentName = departmentName;
        NumberOfSeats = numberOfSeats;
    }
}

public enum DepartmentName
{
    EEE,
    CSE,
    MECH,
    ECE
}
