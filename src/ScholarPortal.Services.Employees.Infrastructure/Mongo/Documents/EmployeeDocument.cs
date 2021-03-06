using System;
using Convey.Types;
using ScholarPortal.Services.Employees.Core.Entities;
using ScholarPortal.Services.Employees.Core.Repositories;

namespace ScholarPortal.Services.Employees.Infrastructure.Mongo.Documents
{
	public class EmployeeDocument : IEmployeeDocument, IIdentifiable<Guid>
	{
		public Guid Id { get; set; }
		public string Title { get; set; }
		public DateTime CreatedAt { get; set; }
		public EmployeeState State { get; set; }
		public Guid UserId { get; set; }

		public EmployeeDocument(Guid id, string title, DateTime createdAt, EmployeeState state, Guid userId)
		{
			Id = id;
			Title = title;
			CreatedAt = createdAt;
			State = state;
			UserId = userId;
		}
	}
}