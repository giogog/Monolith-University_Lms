﻿using Microsoft.EntityFrameworkCore;

namespace Contracts;

public interface IRepositoryManager : IDisposable
{
    IUserRepository UserRepository { get; }
    IUniversityRepository UniversityRepository { get; }
    IFacultyRepository FacultyRepository { get; }
    IUserRoleRepository UserRoleRepository { get; }
    IRoleRepository RoleRepository { get; }
    IStudentRepository StudentRepository { get; }
    ITeacherRepository TeacherRepository { get; }   
    ISubjectRepository SubjectRepository { get; }
    ILectureRepository LectureRepository { get; }
    ISeminarRepository SeminarRepository { get; }
    IEnrollmentRepository EnrollmentRepository { get; }
    Task BeginTransactionAsync();
    Task CommitTransactionAsync();
    Task RollbackTransactionAsync();
    Task<int> SaveAsync();
}
