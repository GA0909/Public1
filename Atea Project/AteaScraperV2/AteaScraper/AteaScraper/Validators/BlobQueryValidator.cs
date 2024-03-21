using AteaScraper.Blobing;
using FluentValidation;
using FluentValidation.Results;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AteaScraper.Validators
{
    public class BlobQueryValidator : IValidator<BlobQueryRequest>
    {
        public bool Validate(BlobQueryRequest input)
        {
            // Check if not null or empty
            if (string.IsNullOrEmpty(input.BlobName))
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

        ValidationResult IValidator<BlobQueryRequest>.Validate(BlobQueryRequest instance)
        {
            throw new NotImplementedException();
        }

        ValidationResult IValidator.Validate(IValidationContext context)
        {
            throw new NotImplementedException();
        }

        Task<ValidationResult> IValidator<BlobQueryRequest>.ValidateAsync(BlobQueryRequest instance, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }

        Task<ValidationResult> IValidator.ValidateAsync(IValidationContext context, CancellationToken cancellation)
        {
            throw new NotImplementedException();
        }
    }
}
