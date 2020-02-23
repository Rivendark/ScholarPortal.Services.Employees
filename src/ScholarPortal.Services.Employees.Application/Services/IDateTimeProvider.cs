using System;

namespace ScholarPortal.Services.Employees.Application.Services
{
	public interface IDateTimeProvider
	{
		DateTime Now { get; }
	}
}