using System;

namespace ScholarPortal.Services.Employees.Core.Entities
{
	public abstract class AuditableEntity
	{
		public DateTime CreatedAt { get; set; }
		//public Employee CreatedBy { get; set; }
		public DateTime? UpdatedAt { get; set; }
		//public Employee UpdatedBy { get; set; }

		public AuditableEntity()
		{
			CreatedAt = DateTime.UtcNow;
		}

		protected void SetUpdatedDate()
			=> UpdatedAt = DateTime.UtcNow;
	}
}