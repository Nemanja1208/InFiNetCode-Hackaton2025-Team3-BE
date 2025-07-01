using FluentValidation;

namespace Application_Layer.IdeaSessions.Commands.UpdateIdeaSessionTitle;

public class UpdateIdeaSessionTitleValidator : AbstractValidator<UpdateIdeaSessionTitleCommand>
{
    public UpdateIdeaSessionTitleValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MinimumLength(3).WithMessage("Title must be at least 3 characters.")
            .MaximumLength(100).WithMessage("Title must be at most 100 characters.");
    }
}
