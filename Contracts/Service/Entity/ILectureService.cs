using Domain.Dtos;
using Domain.Models;

namespace Contracts;

public interface ILectureService
{
    Task<Result<bool>> RegisterLecture(int subjectId, LectureDto lectureDto);
}
