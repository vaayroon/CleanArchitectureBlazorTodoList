using System;
using FluentAssertions;
using Moq;
using TodoList.Application.DTOs;
using TodoList.Application.Tasks.Commands.CreateTask;
using TodoList.Domain.Entities;
using TodoList.Domain.Repositories;
using TodoList.Domain.SharedKernel.Extensions;

namespace TodoList.Test.Tasks.Commands.CreateTask;

[Trait("Category", "Unit")]
public class CreateTask_CreateTaskCommandHandlerTest
{
    private readonly CreateTaskCommandHandler _handler;
    private readonly Mock<ITaskRepository> _taskRepositoryMock;

    public CreateTask_CreateTaskCommandHandlerTest()
    {
        _taskRepositoryMock = new Mock<ITaskRepository>();
        _handler = new CreateTaskCommandHandler(
            _taskRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess_CreationSucced()
    {
        // Arrange
        var command = new CreateTaskCommand(
            "Task 1",
            "Task 1 description",
            false);

        // Act
        Result<TaskDto> result = await _handler.Handle(command, default);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        _taskRepositoryMock.Verify(
            x => x.CreateAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }
}
