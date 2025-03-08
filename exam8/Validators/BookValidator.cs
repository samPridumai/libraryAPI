using exam8.Models;
using FluentValidation;

namespace exam8.Validators;

public class BookValidator : AbstractValidator<Book>
{
    public BookValidator()
    {
        RuleFor(b => b.Title)
            .NotEmpty().WithMessage("название книги обязательно")
            .MaximumLength(255).WithMessage("не больше 255");
        
        RuleFor(b => b.Author)
            .NotEmpty().WithMessage("автор книги обязательно")
            .MaximumLength(255).WithMessage("не больше 255");
    }
}