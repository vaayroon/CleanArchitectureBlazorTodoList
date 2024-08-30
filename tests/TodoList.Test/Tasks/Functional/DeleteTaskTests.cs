using System;
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using MongoDB.Bson;
using TodoList.Application.DTOs;
using TodoList.Test.Abstractions;

namespace TodoList.Test.Tasks.Functional;

[Trait("Category", "FunctionalD")]
public class DeleteTaskTests : BaseFunctionalTest
{
    public DeleteTaskTests(FunctionalTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnOk_WhenTaskIsDeleted()
    {
        // Arrange
        TaskDto taskDto = await CreateTaskAsync();

        // Act
        HttpResponseMessage response = await HttpClient.DeleteAsync($"/api/Tasks/{taskDto.Id}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task Should_ReturnBadRequest_whenTaskDoNotExist()
    {
        // Arrange
        string bsonId = ObjectId.GenerateNewId().ToString();

        // Act
        HttpResponseMessage response = await HttpClient.DeleteAsync($"/api/Tasks/{bsonId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    /* [Fact]
    public async Task Should_ReturnBadRequest_WhenIdIsMissing()
    {
        // Arrange
        string bsonId = "   ";

        // Act
        HttpResponseMessage response = await HttpClient.DeleteAsync($"/api/Tasks/{bsonId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    } */

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
