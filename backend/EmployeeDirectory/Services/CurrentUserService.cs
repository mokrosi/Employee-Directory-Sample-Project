using System;
using System.Security.Claims;
using EmployeeDirectory.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace EmployeeDirectory.Services;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public Guid UserId
    {
        get
        {
            var user = _httpContextAccessor.HttpContext?.User;
            if (user == null) return Guid.Empty;

            var claim = user.FindFirst(ClaimTypes.NameIdentifier)
                     ?? user.FindFirst("sub")
                     ?? user.FindFirst("id")
                     ?? user.FindFirst("nameid");

            if (claim != null && Guid.TryParse(claim.Value, out var userId))
            {
                return userId;
            }

            return Guid.Empty;
        }
    }
}