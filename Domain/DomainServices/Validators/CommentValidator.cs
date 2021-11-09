using Domain.Constants;
using Domain.Repository.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainServices.Validators
{
    public class CommentValidator : AbstractValidator<Comment>
    {
        private const string PROPERTY_REQUIRED_MESSAGE = "{PropertyName} es requerido.";
        public CommentValidator() {
            RuleFor(r => r.Content)
                .NotNull().WithMessage(PROPERTY_REQUIRED_MESSAGE)
                .NotEmpty().WithMessage(PROPERTY_REQUIRED_MESSAGE)
                .MaximumLength(Constraints.MaxLengthContent);
            RuleFor(r => r.ArticleId)
                .GreaterThan(Numerical.MinValue)
                .NotNull().WithMessage(PROPERTY_REQUIRED_MESSAGE)
                .NotEmpty().WithMessage(PROPERTY_REQUIRED_MESSAGE);
            RuleFor(r => r.UserId)
                .GreaterThan(Numerical.MinValue)
                .NotNull().WithMessage(PROPERTY_REQUIRED_MESSAGE)
                .NotEmpty().WithMessage(PROPERTY_REQUIRED_MESSAGE);
        }
    }
}
