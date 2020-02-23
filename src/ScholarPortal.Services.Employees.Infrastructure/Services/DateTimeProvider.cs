using System;
using ScholarPortal.Services.Employees.Application.Services;

namespace ScholarPortal.Services.Employees.Infrastructure.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now  => DateTime.UtcNow;
    }
}