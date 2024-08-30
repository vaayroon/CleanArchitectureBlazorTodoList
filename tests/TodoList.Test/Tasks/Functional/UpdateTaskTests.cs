using System;
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MongoDB.Bson;
using TodoList.Application.DTOs;
using TodoList.Test.Abstractions;

namespace TodoList.Test.Tasks.Functional;

public class UpdateTaskTests : BaseFunctionalTest
{
    public UpdateTaskTests(FunctionalTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnBadRequest_WhenTitleIsMissing()
    {
        // Arrange
        string bsonId = ObjectId.GenerateNewId().ToString();
        var taskDto = new TaskDto
        {
            Title = string.Empty,
            Description = "Some description",
            IsCompleted = false
        };

        // Act
        HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"/api/Tasks/{bsonId}", taskDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_ReturnBadRequest_WhenTitleIsTooLong()
    {
        // Arrange
        string bsonId = ObjectId.GenerateNewId().ToString();
        var taskDto = new TaskDto
        {
            Title = new string('a', 101),
            Description = new string('a', 499),
            IsCompleted = false
        };

        // Act
        HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"/api/Tasks/{bsonId}", taskDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_ReturnBadRequest_WhenDescriptionIsTooLong()
    {
        // Arrange
        string bsonId = ObjectId.GenerateNewId().ToString();
        var taskDto = new TaskDto
        {
            Title = new string('a', 99),
            Description = new string('a', 501),
            IsCompleted = false
        };

        // Act
        HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"/api/Tasks/{bsonId}", taskDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_ReturnOk_WhenRequestIsValid()
    {
        // Arrange
        TaskDto taskDto = await CreateTaskAsync();

        // Act
        HttpResponseMessage response = await HttpClient.PutAsJsonAsync($"/api/Tasks/{taskDto.Id}", taskDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    private async Task<TaskDto> CreateTaskAsync()
    {
        var taskDto = new TaskDto
        {
            Title = "Some title",
            Description = "Some description",
            IsCompleted = false
        };

        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("/api/Tasks", taskDto);

        return await response.Content.ReadFromJsonAsync<TaskDto>();
    }
}
