using System.Text.Json;

namespace TodoList.WebUIServer.Infrastructure.Extensions;

public static class HttpContentExtensions
{
    private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public static HttpContent ToJsonContent(this object obj)
    {
        return JsonContent.Create(obj);
    }

    public static async Task<Dictionary<string, List<string>>> ReadAsErrorDictionaryAsync(this HttpContent content)
    {
        string jsonResponse = await content.ReadAsStringAsync();

        // Try to deserialize as Dictionary<string, List<string>>
        try
        {
            Dictionary<string, List<string>>? errors = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(jsonResponse);
            if (errors != null)
            {
                return errors;
            }
        }
        catch (JsonException) { }

        // Try to deserialize as ErrorResponse
        try
        {
            ErrorResponse? errorResponse = JsonSerializer.Deserialize<ErrorResponse>(jsonResponse, _jsonSerializerOptions);
            if (errorResponse != null)
            {
                return new Dictionary<string, List<string>>
                {
                    { errorResponse.Code, new List<string> { errorResponse.Message } }
                };
            }
        }
        catch (JsonException) { }

        // Try to deserialize as ErrorResponse[]
        try
        {
            ErrorResponse[]? errorResponses = JsonSerializer.Deserialize<ErrorResponse[]>(jsonResponse, _jsonSerializerOptions);
            if (errorResponses != null)
            {
                var errorDict = new Dictionary<string, List<string>>();
                foreach (ErrorResponse error in errorResponses)
                {
                    if (!errorDict.TryGetValue(error.Code, out List<string>? value))
                    {
                        value = [];
                        errorDict[error.Code] = value;
                    }

                    value.Add(error.Message);
                }
                return errorDict;
            }
        }
        catch (JsonException) { }

        // If all deserialization attempts fail, return a generic error
        return new Dictionary<string, List<string>>
        {
            { "UnknownError", new List<string> { "An unknown error occurred." } }
        };
    }
}

public class ErrorResponse
{
    public string Code { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public int Type { get; set; }
}
