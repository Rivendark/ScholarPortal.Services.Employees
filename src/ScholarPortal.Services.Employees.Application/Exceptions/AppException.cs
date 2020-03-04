using System;

namespace ScholarPortal.Services.Employees.Application.Exceptions
{
    public abstract class AppException : Exception
    {
        public abstract string Code { get; }

        protected AppException(string message) : base(message)
        {
        }
    }
}