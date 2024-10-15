using System;
using System.Collections.Generic;

namespace SyncCollege;

public class Operations
{
    private static List<StudentDetails> studentsList = new List<StudentDetails>();
    private static List<DepartmentDetails> departmentsList = new List<DepartmentDetails>();
    private static List<AdmissionDetails> admissionsList = new List<AdmissionDetails>();
    private static StudentDetails currentLoggedInStudent;

    public static void MainMenu()
    {
        bool flag = true;

        do
        {
            Console.WriteLine("1. Student Registration\n2. Student Login\n3. Exit");
            string userChoice = Console.ReadLine().Trim();

            switch (userChoice)
            {
                case "1":
                    {
                        StudentRegistration();
                        break;
                    }
                case "2":
                    {
                        StudentLogin();
                        break;
                    }
                case "3":
                    {
                        flag = false;
                        Console.WriteLine("Thank you for visiting... GOODBYE");
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Invalid choice. Please try again");
                        break;
                    }
            }
        } while (flag);
    }

    public static void StudentRegistration()
    {
        Console.WriteLine("Student Name");
        string studentName = Console.ReadLine().Trim();

        Console.WriteLine("Father Name");
        string fatherName = Console.ReadLine().Trim();

        DateTime dob;
        do
        {
            Console.WriteLine("Date of Birth (dd/MM/yyyy)");
        } while (!DateTime.TryParseExact(Console.ReadLine().Trim(), "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out dob) || dob > DateTime.Now);

        Console.WriteLine("Gender (Male, Female, Transgender)");
        Gender gender;
        while (!Enum.TryParse<Gender>(Console.ReadLine().Trim(), true, out gender) || !Enum.IsDefined(typeof(Gender), gender))
        {
            Console.WriteLine("Invalid Gender. Please enter 'Male', 'Female', or 'Transgender')");
        }

        Console.WriteLine("Physics Marks");
        int physics;
        while (!int.TryParse(Console.ReadLine(), out physics) || physics > 100 || physics < 0)
        {
            Console.WriteLine("Please enter a valid number (0 - 100)");
        }

        Console.WriteLine("Chemistry Marks");
        int chemistry;
        while (!int.TryParse(Console.ReadLine(), out chemistry) || chemistry > 100 || chemistry < 0)
        {
            Console.WriteLine("Please enter a valid number (0 - 100)");
        }

        Console.WriteLine("Maths Marks");
        int maths;
        while (!int.TryParse(Console.ReadLine(), out maths) || maths > 100 || maths < 0)
        {
            Console.WriteLine("Please enter a valid number (0 - 100)");
        }

        StudentDetails newStudent = new(studentName, fatherName, dob, gender, physics, chemistry, maths);
        studentsList.Add(newStudent);
        Console.WriteLine($"Student Registered Successfully and StudentID is {newStudent.StudentID}");
    }

    public static void StudentLogin()
    {
        Console.WriteLine("Enter Student ID");
        string studentID = Console.ReadLine().ToUpper().Trim();

        bool flag = true;
        foreach (StudentDetails student in studentsList)
        {
            if (student.StudentID == studentID)
            {
                flag = false;
                currentLoggedInStudent = student;
                SubMenu();
                break;
            }
        }
        if (flag)
        {
            Console.WriteLine("Invalid Student ID");
        }
    }

    public static void SubMenu()
    {
        bool flag = true;

        do
        {
            Console.WriteLine("a. Check Eligibility\nb. Show Student Details\nc. Take Admission\nd. Cancel Amission\ne. Show Admission Details\nf. Department-wise Seat Availability\ng. Exit");
            string userChoice = Console.ReadLine().ToLower().Trim();

            switch (userChoice)
            {
                case "a":
                    {
                        CheckEligibility();
                        break;
                    }
                case "b":
                    {
                        ShowDetails();
                        break;
                    }
                case "c":
                    {
                        TakeAdmission();
                        break;
                    }
                case "d":
                    {
                        CancelAdmission();
                        break;
                    }
                case "e":
                    {
                        ShowAdmissionDetails();
                        break;
                    }
                case "f":
                    {
                        DepartmentSeatAvailability();
                        break;
                    }
                case "g":
                    {
                        flag = false;
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Invalid choice. Please try again");
                        break;
                    }
            }
        } while (flag);
    }

    public static void CheckEligibility()
    {
        if (currentLoggedInStudent.IsEligible())
        {
            Console.WriteLine("Student is Eligible");
        }
        else
        {
            Console.WriteLine("Student is not Eligible");
        }
    }

    public static void ShowDetails()
    {
        Console.WriteLine($"Student ID: {currentLoggedInStudent.StudentID} | Student Name: {currentLoggedInStudent.StudentName + " " + currentLoggedInStudent.FatherName} | DOB: {currentLoggedInStudent.DOB.ToString("dd/MM/yyyy")} | Gender: {currentLoggedInStudent.Gender} | Physics: {currentLoggedInStudent.Physics} | Chemistry: {currentLoggedInStudent.Chemistry} | Maths: {currentLoggedInStudent.Maths}");
    }

    public static void TakeAdmission()
    {
        DepartmentSeatAvailability();

        Console.WriteLine("Enter Department ID to Enroll");
        string departmentID = Console.ReadLine().Trim().ToUpper();

        bool flag = true;
        foreach (DepartmentDetails department in departmentsList)
        {
            if (department.DepartmentID == departmentID)
            {
                flag = false;
                if (currentLoggedInStudent.IsEligible())
                {
                    if (department.NumberOfSeats > 0)
                    {
                        // check if student has already enrolled in any other admission
                        int admissionCount = 0;
                        foreach (AdmissionDetails admission in admissionsList)
                        {
                            if (admission.StudentID == currentLoggedInStudent.StudentID && admission.AdmissionStatus == AdmissionStatus.Admitted)
                            {
                                admissionCount++;
                            }
                        }

                        bool flag1 = true;
                        foreach (AdmissionDetails admission1 in admissionsList)
                        {
                            if (admission1.DepartmentID == department.DepartmentID)
                            {
                                flag1 = false;
                                Console.WriteLine("Student has already enrolled in this course");
                                break;
                            }
                        }
                        if (flag1)
                        {
                            if (admissionCount >= 2)
                            {
                                Console.WriteLine("Enrollment failed. Student is already enrolled in two courses");
                            }
                            else
                            {
                                AdmissionDetails newAdmission = new(currentLoggedInStudent.StudentID, department.DepartmentID, DateTime.Now, AdmissionStatus.Admitted);
                                admissionsList.Add(newAdmission);
                                department.NumberOfSeats--;
                                Console.WriteLine($"Admission taken successfully. Your admission ID - {newAdmission.AdmissionID}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Seats are not available for the selected Department");
                    }
                }
                else
                {
                    Console.WriteLine("Enrollment Failed. Student is not Eligible");
                }
                break;
            }
        }
        if (flag)
        {
            Console.WriteLine("Invalid Department ID");
        }
    }

    public static void CancelAdmission()
    {
        if (ShowEnrolledAdmissionDetails())
        {


            Console.WriteLine("Enter Admission ID to cancel");
            string admissionID = Console.ReadLine().ToUpper().Trim();

            bool flag = true;
            foreach (AdmissionDetails admission in admissionsList)
            {
                if (admission.AdmissionID == admissionID)
                {
                    flag = false;
                    if (admission.StudentID == currentLoggedInStudent.StudentID && admission.AdmissionStatus == AdmissionStatus.Admitted)
                    {
                        Console.WriteLine($"Admission ID: {admission.AdmissionID} | Student ID: {admission.StudentID} | Department ID: {admission.DepartmentID} | Admission Date: {admission.AdmissionDate} | Admission Status: {admission.AdmissionStatus}");

                        admission.AdmissionStatus = AdmissionStatus.Cancelled;

                        foreach (DepartmentDetails department in departmentsList)
                        {
                            if (department.DepartmentID == admission.DepartmentID)
                            {
                                department.NumberOfSeats++;
                            }
                        }
                        Console.WriteLine("Admission cancelled successfully");
                    }
                    else
                    {
                        Console.WriteLine("Admission not found");
                    }
                }
            }
            if (flag)
            {
                Console.WriteLine("Invalid Amission ID");
            }
        }
    }

    public static void ShowAdmissionDetails()
    {
        bool flag = true;
        foreach (AdmissionDetails admission in admissionsList)
        {
            if (admission.StudentID == currentLoggedInStudent.StudentID)
            {
                flag = false;
                Console.WriteLine($"Admission ID: {admission.AdmissionID} | Student ID: {admission.StudentID} | Department ID: {admission.DepartmentID} | Admission Date: {admission.AdmissionDate} | Admission Status: {admission.AdmissionStatus}");
            }
        }
        if (flag)
        {
            Console.WriteLine("No Enrollment History found");
        }
    }

    public static bool ShowEnrolledAdmissionDetails()
    {
        bool isEnrolled = false;

        foreach (AdmissionDetails admission in admissionsList)
        {
            if (admission.StudentID == currentLoggedInStudent.StudentID && admission.AdmissionStatus == AdmissionStatus.Admitted)
            {
                isEnrolled = true;
                Console.WriteLine($"Admission ID: {admission.AdmissionID} | Student ID: {admission.StudentID} | Department ID: {admission.DepartmentID} | Admission Date: {admission.AdmissionDate} | Admission Status: {admission.AdmissionStatus}");
            }
        }
        if (!isEnrolled)
        {
            Console.WriteLine("No active admission found");
        }
        return isEnrolled;
    }

    public static void DepartmentSeatAvailability()
    {
        foreach (DepartmentDetails department in departmentsList)
        {
            Console.WriteLine($"Department ID: {department.DepartmentID} | Department Name: {department.DepartmentName} | Number of Seats: {department.NumberOfSeats}");
        }
    }

    public static void DefaultData()
    {
        departmentsList.Add(new(DepartmentName.EEE, 29));
        departmentsList.Add(new(DepartmentName.CSE, 29));
        departmentsList.Add(new(DepartmentName.MECH, 30));
        departmentsList.Add(new(DepartmentName.ECE, 30));
    }
}
