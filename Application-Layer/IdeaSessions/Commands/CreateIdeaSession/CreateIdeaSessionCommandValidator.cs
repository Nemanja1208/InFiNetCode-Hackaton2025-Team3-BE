using FluentValidation;

namespace Application_Layer.IdeaSessions.Commands.CreateIdeaSession
{
    public class CreateIdeaSessionCommandValidator : AbstractValidator<CreateIdeaSessionCommand>
    {
        public CreateIdeaSessionCommandValidator()
        {
            RuleFor(x => x.Dto.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MinimumLength(3).WithMessage("Title must be at least 3 characters long.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");
        }
    }
}