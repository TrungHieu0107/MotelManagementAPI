using BussinessObject.DTO;
using FluentValidation;
using System;

namespace DataAccess.Validator
{
    public class WaterCostValidator : AbstractValidator<WaterRequestDTO>
    {

        public WaterCostValidator()
        {
            RuleFor(p => p.Price).GreaterThan(0).WithMessage("Giá tiền phải lớn hơn 0")
                                  .NotNull().WithMessage("Giá tiền phải lớn hơn 0");
            //RuleFor(p => p.AppliedYear).GreaterThanOrEqualTo(DateTime.Today.Year).WithMessage("Year must be equal or greater current year");
            RuleFor(p => p.AppliedMonth).GreaterThan(0).LessThan(13).WithMessage("Tháng phải trong khoảng từ 1 tới 12");
            RuleFor(p => p).Must((p) => ValidateDate(p)).WithMessage("Thời gian áp dụng phải lớn hơn thời gian hiện tại và bắt đầu vào ngày 5 của mỗi tháng.\".");
        }



        private bool ValidateDate(WaterRequestDTO obj)
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
