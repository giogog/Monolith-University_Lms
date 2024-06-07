﻿using Contracts;
using Domain.Dtos;
using Domain.Enums;
using Domain.Extensions;
using Domain.Models;

namespace Application.Services;

public class GradeService:IGradeService
{
    private readonly IRepositoryManager _repositoryManager;

    public GradeService(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }
    public Result<string[]> SetGradeSystem(string[] grades)
    {
        string[] gradeSystem = Enum.GetNames(typeof(GradeSystem));
        bool containsAll = grades.All(item => gradeSystem.Contains(item));

        if (!containsAll) 
            return Result<string[]>.Failed("GradeError", "Ivalid Grade Type");

        Array.Sort(grades);
        return Result<string[]>.Success(grades.ChangeRepeatedItemsNames());
    }

    public Result<Dictionary<string, double>> GetGradeSystem(double[] grades,string[] gradeTypes)
    {
        

        if (gradeTypes.Count() == null)
            return Result<Dictionary<string, double>>.Failed("NotFound","Subject not found");
        if(gradeTypes.Count() != grades.Count())
            return Result<Dictionary<string, double>>.Failed("NotMatch", $"Subject grade types and enrollment grades do not Match: {gradeTypes.Count()} : {grades.Count()}");

        Dictionary<string,double> GradeSystem = new Dictionary<string,double>();
        for(int i = 0;i< gradeTypes.Count();i++)
        {
            GradeSystem.Add(gradeTypes[i], grades[i]);
        }

        return Result<Dictionary<string, double>>.Success(GradeSystem);

    }

    public Result<double[]> AddGradeToStudent(GradeDto gradeDto)
    {
        if (!gradeDto.Grades.ContainsKey(gradeDto.GradeType))
            return Result<double[]>.Failed("NotFound",$"Grade Type {gradeDto.GradeType} doesn't Exists");

        gradeDto.Grades[gradeDto.GradeType] = gradeDto.Grade;
        if(gradeDto.Grades.Values.Sum() > 100)
            return Result<double[]>.Failed("GradeMistake", $"Sum of the grade is greater than 100: {gradeDto.Grades.Values.Sum()}");

        double[] updatedGrades = new double[gradeDto.Grades.Count()];

        int i = 0;
        foreach (var grade in gradeDto.Grades) 
        {
            updatedGrades[i] = grade.Value;
            i++;
        }
        

        return Result<double[]>.Success(updatedGrades);

    }
}
