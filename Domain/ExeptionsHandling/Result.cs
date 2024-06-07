using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Domain.Models;

public record Result<T>
{
    public string Message { get; private set; }
    public string Code {  get; private set; }
    public bool IsSuccess { get; private set; }
    public T? Data { get; private set; }

    public static Result<T> Failed(string code,string massage)
    {
        return new Result<T> { Code = code,Message = massage, IsSuccess = false };
    }
    public static Result<T> Success(T? data, string massage = "") =>
        new Result<T> { Data = data, Message = massage, IsSuccess = true };

    public static Result<T> SuccesfullyUpdated<T>(int saved, T? data)
    {
        if (saved > 0)
        {
            return new Result<T> { Data = data, Message = "Updated Succesfully", IsSuccess = true };
        }

        return new Result<T> { Data = data, IsSuccess = true, Message = "No Changes" };
    }


}
