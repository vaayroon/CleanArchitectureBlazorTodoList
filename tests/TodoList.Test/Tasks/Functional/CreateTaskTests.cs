using System;
using System.ComponentModel;
using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using TodoList.Application.DTOs;
using TodoList.Test.Abstractions;

namespace TodoList.Test.Tasks.Functional;

[Trait("Category", "FunctionalC")]
public class CreateTaskTests : BaseFunctionalTest
{
    public CreateTaskTests(FunctionalTestWebAppFactory factory)
        : base(factory)
    {
    }

    [Fact]
    public async Task Should_ReturnBadRequest_WhenTitleIsMissing()
    {
        // Arrange
        var taskDto = new TaskDto
        {
            Title = string.Empty,
            Description = "Some description",
            IsCompleted = false
        };

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("/api/Tasks", taskDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_ReturnBadRequest_WhenTitleIsTooLong()
    {
        // Arrange
        var taskDto = new TaskDto
        {
            Title = new string('a', 101),
            Description = new string('a', 499),
            IsCompleted = false
        };

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("/api/Tasks", taskDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_ReturnBadRequest_WhenDescriptionIsTooLong()
    {
        // Arrange
        var taskDto = new TaskDto
        {
            Title = new string('a', 99),
            Description = new string('a', 501),
            IsCompleted = false
        };

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("/api/Tasks", taskDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Should_ReturnOk_WhenTaskIsValid()
    {
        // Arrange
        var taskDto = new TaskDto
        {
            Title = "Some title",
            Description = "Some description",
            IsCompleted = false
        };

        // Act
        HttpResponseMessage response = await HttpClient.PostAsJsonAsync("/api/Tasks", taskDto);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
