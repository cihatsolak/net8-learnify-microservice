namespace Learnify.Shared;

public class ServiceResult
{
    [JsonIgnore] public int Status { get; set; }
    public ProblemDetails Fail { get; set; }

    [JsonIgnore] public bool IsSuccess => Fail is null;
    [JsonIgnore] public bool IsFailed => !IsSuccess;

    //Static Factory Methods
    public static ServiceResult SuccessAsNoContent()
    {
        return new ServiceResult
        {
            Status = StatusCodes.Status204NoContent
        };
    }

    public static ServiceResult ErrorAsNotFound()
    {
        return new ServiceResult
        {
            Status = StatusCodes.Status404NotFound,
            Fail = new ProblemDetails
            {
                Title = "Not Found",
                Status = StatusCodes.Status404NotFound,
                Detail = "The requested resource was not found"
            }
        };
    }

    public static ServiceResult ErrorFromProblemDetails(Refit.ApiException apiException)
    {
        if (string.IsNullOrWhiteSpace(apiException.Content))
        {
            return new ServiceResult()
            {
                Fail = new ProblemDetails
                {
                    Title = "Error",
                    Status = apiException.StatusCode.GetHashCode(),
                    Detail = apiException.Message
                },
                Status = apiException.StatusCode.GetHashCode()
            };
        }

        var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(apiException.Content,
            new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

        return new ServiceResult()
        {
            Fail = problemDetails,
            Status = apiException.StatusCode.GetHashCode()
        };
    }

    public static ServiceResult Error(ProblemDetails problemDetails, int statusCode)
    {
        return new ServiceResult
        {
            Fail = problemDetails,
            Status = statusCode
        };
    }

    public static ServiceResult Error(string title, int statusCode)
    {
        return new ServiceResult
        {
            Status = statusCode,
            Fail = new ProblemDetails
            {
                Title = title,
                Status = statusCode
            }
        };
    }

    public static ServiceResult Error(string title, string description, int statusCode)
    {
        return new ServiceResult
        {
            Status = statusCode,
            Fail = new ProblemDetails
            {
                Title = title,
                Detail = description,
                Status = statusCode
            }
        };
    }

    public static ServiceResult ErrorFromValidation(IDictionary<string, object?> errors)
    {
        return new ServiceResult
        {
            Status = StatusCodes.Status400BadRequest,
            Fail = new ProblemDetails
            {
                Title = "Validation errors occured",
                Detail = "One or more validation errors occured",
                Status = StatusCodes.Status400BadRequest,
                Extensions = errors
            }
        };
    }
}

public class ServiceResult<T> : ServiceResult where T : class
{
    public T Data { get; set; }
    [JsonIgnore] public string UrlAsCreated { get; set; }

    public static ServiceResult<T> SuccessAsOk(T data)
    {
        return new ServiceResult<T>
        {
            Status = StatusCodes.Status200OK,
            Data = data
        };
    }

    public static ServiceResult<T> SuccessAsCreated(T data, string urlAsCreated)
    {
        return new ServiceResult<T>
        {
            Status = StatusCodes.Status201Created,
            Data = data,
            UrlAsCreated = urlAsCreated
        };
    }

    public new static ServiceResult<T> ErrorFromProblemDetails(Refit.ApiException apiException)
    {
        if (string.IsNullOrWhiteSpace(apiException.Content))
        {
            return new ServiceResult<T>()
            {
                Fail = new ProblemDetails
                {
                    Title = "Error",
                    Status = apiException.StatusCode.GetHashCode(),
                    Detail = apiException.Message
                },
                Status = apiException.StatusCode.GetHashCode()
            };
        }

        var problemDetails = JsonSerializer.Deserialize<ProblemDetails>(apiException.Content,
            new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            });

        return new ServiceResult<T>()
        {
            Fail = problemDetails,
            Status = apiException.StatusCode.GetHashCode()
        };
    }

    public new static ServiceResult<T> Error(ProblemDetails problemDetails, int statusCode)
    {
        return new ServiceResult<T>
        {
            Fail = problemDetails,
            Status = statusCode
        };
    }

    public new static ServiceResult<T> Error(string title, int statusCode)
    {
        return new ServiceResult<T>
        {
            Status = statusCode,
            Fail = new ProblemDetails
            {
                Title = title,
                Status = statusCode
            }
        };
    }

    public new static ServiceResult<T> Error(string title, string description, int statusCode)
    {
        return new ServiceResult<T>
        {
            Status = statusCode,
            Fail = new ProblemDetails
            {
                Title = title,
                Detail = description,
                Status = statusCode
            }
        };
    }

    public new static ServiceResult<T> ErrorFromValidation(IDictionary<string, object?> errors)
    {
        return new ServiceResult<T>
        {
            Status = StatusCodes.Status400BadRequest,
            Fail = new ProblemDetails
            {
                Title = "Validation errors occured",
                Detail = "One or more validation errors occured",
                Status = StatusCodes.Status400BadRequest,
                Extensions = errors
            }
        };
    }
}
