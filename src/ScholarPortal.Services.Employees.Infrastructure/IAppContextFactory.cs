using ScholarPortal.Services.Employees.Application;

namespace ScholarPortal.Services.Employees.Infrastructure
{
	public interface IAppContextFactory
	{
		IAppContext Create();
	}
}