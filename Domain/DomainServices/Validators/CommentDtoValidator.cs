using Domain.Constants;
using Domain.Models.Dtos;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainServices.Validators
{
    public class CommentDtoValidator : AbstractValidator<CommentDto>
    {
        private const string PROPERTY_REQUIRED_MESSAGE = "{PropertyName} es requerido.";
        public CommentDtoValidator() {
            RuleFor(r => r.Content)
                .NotNull().WithMessage(PROPERTY_REQUIRED_MESSAGE)
                .NotEmpty().WithMessage(PROPERTY_REQUIRED_MESSAGE)
                .MaximumLength(ModelConstraints.MaxLengthContent);
            RuleFor(r => r.ArticleId)
                .GreaterThan(Numerical.MinValue)
                .NotNull().WithMessage(PROPERTY_REQUIRED_MESSAGE)
                .NotEmpty().WithMessage(PROPERTY_REQUIRED_MESSAGE);
        }
    }
}
