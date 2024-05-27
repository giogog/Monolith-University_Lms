using System.Collections.Generic;
using System.Text.Json;
using Client.Dtos;

namespace Client.Services
{
    public class ValidationService
    {
        public Dictionary<string, List<string>> ParseErrors(string responseContent)
        {
            var fieldErrors = new Dictionary<string, List<string>>();

            try
            {
                var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (errorResponse?.Errors != null)
                {
                    foreach (var error in errorResponse.Errors)
                    {
                        fieldErrors[error.Key] = error.Value;
                    }
                }
            }
            catch (JsonException)
            {
                // Handle custom error messages directly
                var customErrors = JsonSerializer.Deserialize<List<CustomError>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                foreach (var error in customErrors)
                {
                    var field = MapErrorCodeToField(error.Code);
                    if (!string.IsNullOrEmpty(field))
                    {
                        if (!fieldErrors.ContainsKey(field))
                        {
                            fieldErrors[field] = new List<string>();
                        }
                        fieldErrors[field].Add(error.Description);
                    }
                }
            }

            return fieldErrors;
        }

        private string MapErrorCodeToField(string errorCode)
        {
            return errorCode switch
            {
                "DuplicateUserName" => nameof(RegisterDto.PersonalID),
                "DuplicateEmail" => nameof(RegisterDto.Email),
                "WrongPersonalId" => nameof(LoginDto.PersonalId),
                "WrongPassword" => nameof(LoginDto.Password),
                "UserDoesNotExist" => nameof(LoginDto.PersonalId),
                "IncorrectPassword" => nameof(LoginDto.Password),
                "MailisNotConfirmed" => nameof(LoginDto.PersonalId),
                _ => string.Empty
            };
        }

        public class ErrorResponse
        {
            public Dictionary<string, List<string>> Errors { get; set; }
        }

        public class CustomError
        {
            public string Code { get; set; }
            public string Description { get; set; }
        }
    }
}


//using System.Text.Json;
//using System.Collections.Generic;
//using Client.Dtos;

//namespace Client.Services;
//public class ValidationService
//{
//    public Dictionary<string, List<string>> ParseErrors(string responseContent)
//    {
//        var fieldErrors = new Dictionary<string, List<string>>();

//        try
//        {
//            var errorResponse = JsonSerializer.Deserialize<ErrorResponse>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
//            if (errorResponse?.Errors != null)
//            {
//                foreach (var error in errorResponse.Errors)
//                {
//                    fieldErrors[error.Key] = error.Value;
//                }
//            }
//        }
//        catch (JsonException)
//        {
//            // Handle custom error messages directly
//            var customErrors = JsonSerializer.Deserialize<List<CustomError>>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
//            foreach (var error in customErrors)
//            {
//                var field = MapErrorCodeToField(error.Code);
//                if (!string.IsNullOrEmpty(field))
//                {
//                    if (!fieldErrors.ContainsKey(field))
//                    {
//                        fieldErrors[field] = new List<string>();
//                    }
//                    fieldErrors[field].Add(error.Description);
//                }
//            }
//        }

//        return fieldErrors;
//    }

//    private string MapErrorCodeToField(string errorCode)
//    {
//        return errorCode switch
//        {
//            "DuplicateUserName" => nameof(RegisterDto.PersonalID),
//            "DuplicateEmail" => nameof(RegisterDto.Email),
//            "WrongPersonalId" => nameof(RegisterDto.PersonalID),
//            _ => string.Empty
//        };
//    }

//    public class ErrorResponse
//    {
//        public Dictionary<string, List<string>> Errors { get; set; }
//    }

//    public class CustomError
//    {
//        public string Code { get; set; }
//        public string Description { get; set; }
//    }
//}
