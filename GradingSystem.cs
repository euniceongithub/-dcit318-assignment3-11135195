using System;
using System.Collections.Generic;
using System.IO;

namespace SchoolGradingSystem
{
    // a. Student Class
    public class Student
    {
        public int Id { get; }
        public string FullName { get; }
        public int Score { get; }

        public Student(int id, string fullName, int score)
        {
            Id = id;
            FullName = fullName;
            Score = score;
        }

        public string GetGrade()
        {
            if (Score >= 80 && Score <= 100) return "A";
            if (Score >= 70 && Score <= 79) return "B";
            if (Score >= 60 && Score <= 69) return "C";
            if (Score >= 50 && Score <= 59) return "D";
            return "F";
        }
    }

    // b. Custom Exceptions
    public class InvalidScoreFormatException : Exception
    {
        public InvalidScoreFormatException(string message) : base(message) { }
    }

    public class MissingFieldException : Exception
    {
        public MissingFieldException(string message) : base(message) { }
    }

    // d. StudentResultProcessor Class
    public class StudentResultProcessor
    {
        public List<Student> ReadStudentsFromFile(string inputFilePath)
        {
            var students = new List<Student>();

            using (var reader = new StreamReader(inputFilePath))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    var parts = line.Split(',');

                    // Check for missing fields
                    if (parts.Length != 3)
                        throw new MissingFieldException($"Missing field(s) in line: {line}");

                    string idStr = parts[0].Trim();
                    string fullName = parts[1].Trim();
                    string scoreStr = parts[2].Trim();

                    // Check if ID can be parsed
                    if (!int.TryParse(idStr, out int id))
                        throw new FormatException($"Invalid ID format in line: {line}");

                    // Check if score can be parsed
                    if (!int.TryParse(scoreStr, out int score))
                        throw new InvalidScoreFormatException($"Invalid score format in line: {line}");

                    students.Add(new Student(id, fullName, score));
                }
            }

            return students;
        }

        public void WriteReportToFile(List<Student> students, string outputFilePath)
        {
            using (var writer = new StreamWriter(outputFilePath))
            {
                foreach (var student in students)
                {
                    writer.WriteLine($"{student.FullName} (ID: {student.Id}): Score = {student.Score}, Grade = {student.GetGrade()}");
                }
            }
        }
    }

    // e. Main Program
    public class Program
    {
        public static void Main()
        {
            var processor = new StudentResultProcessor();
            string inputPath = "students.txt";      // Input file
            string outputPath = "report.txt";       // Output file

            try
            {
                var students = processor.ReadStudentsFromFile(inputPath);
                processor.WriteReportToFile(students, outputPath);

                Console.WriteLine($"Report generated successfully: {outputPath}");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("Error: Input file not found.");
            }
            catch (InvalidScoreFormatException ex)
            {
                Console.WriteLine($"Score format error: {ex.Message}");
            }
            catch (MissingFieldException ex)
            {
                Console.WriteLine($"Missing field error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
