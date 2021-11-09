using Domain.Constants;
using Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainServices.Validators
{
    public class ReactionValidator : AbstractValidator<Reactions>
    {
        private const string PROPERTY_REQUIRED_MESSAGE = "{PropertyName} es requerido.";
        public ReactionValidator() {
            RuleFor(r => r.Likes)
                .NotNull().WithMessage(PROPERTY_REQUIRED_MESSAGE)
                .NotEmpty().WithMessage(PROPERTY_REQUIRED_MESSAGE)
                .GreaterThan(Numerical.MinValueReaction)
                .LessThanOrEqualTo(Numerical.MaxValueReaction);
            RuleFor(r => r.Dislikes)
                .NotNull().WithMessage(PROPERTY_REQUIRED_MESSAGE)
                .NotEmpty().WithMessage(PROPERTY_REQUIRED_MESSAGE)
                .GreaterThan(Numerical.MinValueReaction)
                .LessThanOrEqualTo(Numerical.MaxValueReaction);
            RuleFor(r => r.Id)
                .GreaterThanOrEqualTo(Numerical.MinValue)
                .NotNull().WithMessage(PROPERTY_REQUIRED_MESSAGE)
                .NotEmpty().WithMessage(PROPERTY_REQUIRED_MESSAGE);
        }
    }
}
