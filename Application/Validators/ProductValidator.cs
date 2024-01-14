using Domain.Constants;
using Domain.Models;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Validators
{
    public class ProductValidator : AbstractValidator<ProductModel>
    {
        public ProductValidator()
        {
            RuleFor(p => p.Name).NotNull().NotEmpty().MaximumLength(Lengths.MaxNameLength);
            RuleFor(p => p.Description).NotNull().NotEmpty().MaximumLength(Lengths.MaxDescriptionLength);
            RuleFor(x => x.Price).NotNull().NotEmpty();
        }
    }
}
