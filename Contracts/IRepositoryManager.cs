﻿using Microsoft.EntityFrameworkCore;

namespace Contracts;

public interface IRepositoryManager
{
    IUserRepository UserRepository { get; }
    IUniversityRepository UniversityRepository { get; }
    IFacultyRepository FacultyRepository { get; }
    IUserRoleRepository UserRoleRepository { get; }
    IRoleRepository RoleRepository { get; }
    IStudentRepository StudentRepository { get; }
    Task<int> SaveAsync();
}
