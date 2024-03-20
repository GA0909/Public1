using AteaScraper.Logging;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AteaScraper.Validators
{
    public class LogQueryValidator : IValidator<LogQueryRequest>
    {
        public bool Validate(LogQueryRequest input)
        {
            // Check if 'From' and 'To' parameters are not null or empty
            if (string.IsNullOrEmpty(input.From) || string.IsNullOrEmpty(input.To))
            {
                return false;
            }

            // Attempt to parse 'From' and 'To' parameters as DateTime
            if (!DateTime.TryParse(input.From, out _) || !DateTime.TryParse(input.To, out _))
            {
                return false;
            }

            return true; // Validation successful
        }

        bool IValidator.CanValidateInstancesOfType(Type type)
        {
            throw new NotImplementedException();
        }

        IValidatorDescriptor IValidator.CreateDescriptor()
        {
            throw new NotImplementedException();
        }

        ValidationResult IValidator<LogQueryRequest>.Validate(LogQueryRequest instance)
        {
            throw new NotImplementedException();
        }

        ValidationResult IValidator.Validate(IValidationContext context)
        {
            throw new NotImplementedException();
        }

        Task<ValidationResult> IValidator<LogQueryRequest>.ValidateAsync(LogQueryRequest instance, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        Task<ValidationResult> IValidator.ValidateAsync(IValidationContext context, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }
    }
}
