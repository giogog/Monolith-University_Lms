using System.ComponentModel.DataAnnotations;

namespace Domain.Models
{
    public class ExamResults
    {
        public int Id { get; set; }
        [Range(0, 1)]
        public double Grant { get; set; }
        public string[] SubjectNames { get; set; }
        public int[] SubjectGrades { get; set; }

        public ExamResults()
        {
            
        }
        public ExamResults(int size)
        {
            SubjectGrades = new int[size];
            SubjectNames = new string[size];
        }
    }
}
