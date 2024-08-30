using System;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;

namespace TodoList.WebUIServer.Infrastructure;

public class CustomErrorBoundary : ErrorBoundary
{
    [Inject]
    private IWebHostEnvironment _hostEnvironment { get; set; }

    protected override Task OnErrorAsync(Exception exception)
    {
        if (_hostEnvironment.IsDevelopment())
        {
            return base.OnErrorAsync(exception);
        }

        return Task.CompletedTask;
    }
}
