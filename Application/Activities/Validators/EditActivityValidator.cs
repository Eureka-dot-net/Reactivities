using Application.Activities.Commands;
using Application.Activities.DTO;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Activities.Validators
{
    public class EditActivityValidator : BaseActivityValidator<EditActivity.Command, EditActivityDto>
    {
        public EditActivityValidator() : base(x => x.ActivityDto)
        {
            RuleFor(x => x.ActivityDto.Id)
                .NotEmpty().WithMessage("Activity ID is required")
                .Must(id => Guid.TryParse(id, out _)).WithMessage("Activity ID must be a valid GUID");
        }
    }
}
