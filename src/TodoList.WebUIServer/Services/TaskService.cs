using System;
using System.Net;
using TodoList.Domain.Entities;
using TodoList.Domain.SharedKernel.Constants;
using TodoList.Domain.SharedKernel.Extensions;
using TodoList.Domain.SharedKernel.Primitives;
using TodoList.WebUIServer.Infrastructure.Extensions;

namespace TodoList.WebUIServer.Services;

public class TaskService
{
    private readonly HttpClient _httpClient;

    public TaskService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<IEnumerable<TaskItem>>> GetTasksAsync(string status)
    {
        HttpResponseMessage response = await _httpClient.GetAsync(new Uri($"api/Tasks?status={status}", UriKind.Relative));
        if (!response.IsSuccessStatusCode)
        {
            return Result
                .Failure<IEnumerable<TaskItem>>(
                    new Error(
                        response.StatusCode.ToString(),
                        response.ReasonPhrase ?? "An error occurred while retrieving tasks",
                        ErrorType.Problem)); 
        }

        return Result.Successs(
            await response.Content.ReadFromJsonAsync<IEnumerable<TaskItem>>() ??
            Array.Empty<TaskItem>());
    }

    public async Task<TaskItem> GetTaskByIdAsync(string id)
    {
        return await _httpClient.GetFromJsonAsync<TaskItem>($"api/Tasks/{id}") ?? new TaskItem();
    }

    public async Task<object> CreateTaskAsync(TaskItem task)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(
            new Uri("api/Tasks", UriKind.Relative),
            task);


        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            return await response.Content.ReadAsErrorDictionaryAsync();
        } else if (response.IsSuccessStatusCode)
        {
            TaskItem? taskItem = await response.Content.ReadFromJsonAsync<TaskItem>() ?? null;
            if (taskItem == null)
            {
                return Result
                    .Failure(
                        new Error(
                            response.StatusCode.ToString(),
                            response.ReasonPhrase ?? "An error occurred while creating a task",
                            ErrorType.Problem));
            }
            return Result.Successs(taskItem);
        }
        else
        {
            return Result
                .Failure(
                    new Error(
                        response.StatusCode.ToString(),
                        response.ReasonPhrase ?? "An error occurred while creating a task",
                        ErrorType.Problem));
        }
    }

    public async Task<object> UpdateTaskAsync(TaskItem task)
    {
        HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/Tasks/{task.Id}", task);

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            return await response.Content.ReadAsErrorDictionaryAsync();
        } else if (response.IsSuccessStatusCode)
        {
            return Result.Successs();
        }
        else
        {
            return Result
                .Failure(
                    new Error(
                        response.StatusCode.ToString(),
                        response.ReasonPhrase ?? "An error occurred while updating a task",
                        ErrorType.Problem));
        }
    }

    public async Task<Result> ToggleStatusTaskAsync(string id, bool markAsComplete)
    {
        HttpResponseMessage response = await _httpClient.PatchAsJsonAsync(
            new Uri($"api/Tasks/{id}/toggle-completion", UriKind.Relative),
            new { MarkAsComplete = markAsComplete });
        if (!response.IsSuccessStatusCode)
        {
            return Result
                .Failure(
                    new Error(
                        response.StatusCode.ToString(),
                        response.ReasonPhrase ?? "An error occurred while toggling task status",
                        ErrorType.Problem)); 
        }

        return Result.Successs();
    }

    public async Task<Result> DeleteTaskAsync(string id)
    {
        HttpResponseMessage response = await _httpClient.DeleteAsync(new Uri($"api/Tasks/{id}", UriKind.Relative));
        if (!response.IsSuccessStatusCode)
        {
            return Result
                .Failure(
                    new Error(
                        response.StatusCode.ToString(),
                        response.ReasonPhrase ?? "An error occurred while deleting a task",
                        ErrorType.Problem)); 
        }

        return Result.Successs();
    }
}
