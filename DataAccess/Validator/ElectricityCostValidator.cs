using BussinessObject.DTO;
using FluentValidation;
using System;

namespace DataAccess.Validator
{
    public class ElectricityCostValidator : AbstractValidator<ElectricityCostRequestDTO>
    {

        public ElectricityCostValidator()
        {
            RuleFor(p => p.Price).GreaterThan(0).WithMessage("Price must be greater the 0")
                                  .NotNull().WithMessage("Price must be greater the 0");
            //RuleFor(p => p.AppliedYear).GreaterThanOrEqualTo(DateTime.Today.Year).WithMessage("Year must be equal or greater current year");
            RuleFor(p => p.AppliedMonth).GreaterThan(0).LessThan(13).WithMessage("Month must be from 1 to 12");
            RuleFor(p => p).Must((p) => ValidateDate(p)).WithMessage("The applied must be greater or equal the 5th of the month or year");
        }

        private bool ValidateDate(ElectricityCostRequestDTO obj)
        {
            bool check = true;
            DateTime date = DateTime.Today;
            if (obj.AppliedYear < date.Year)
            {
                check = false;
            }
            else if (obj.AppliedYear == date.Year && obj.AppliedMonth <= date.Month)
            {
                check = false;
            }
            return check;

        }
    }
}
