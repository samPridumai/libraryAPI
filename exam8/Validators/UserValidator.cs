using System.Text.RegularExpressions;
using exam8.Models;
using FluentValidation;

namespace exam8.Validators;

public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("заполнить")
            .EmailAddress().WithMessage("неверный формат");

        RuleFor(u => u.PhoneNumber.ToString())
            .NotEmpty().WithMessage("заполнить")
            .Must(ValidPhoneRegex).WithMessage("неверный формат");
    }

    private bool ValidPhoneRegex(string phoneNumber)
    {
        var phoneRegex = new Regex(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$");
        return phoneRegex.IsMatch(phoneNumber);
    }
}