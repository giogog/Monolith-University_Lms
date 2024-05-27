using Domain.Dtos;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class ExamResults
    {
        public int Id { get; set; }
        [Range(0, 1)]
        public double Grant { get; set; }
        [NotMapped]
        public IEnumerable<ExamResultsDto> Results => GetResults();
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

        private ICollection<ExamResultsDto> GetResults()
        {
            ICollection<ExamResultsDto> results = new List<ExamResultsDto>();
            if (SubjectGrades.Count() == SubjectNames.Count())
            {
                for (int i = 0; i < SubjectGrades.Count(); i++)
                {
                    results.Add(new ExamResultsDto(SubjectNames[i], SubjectGrades[i]));
                }
            }    
            return results;
        }
    }
}
