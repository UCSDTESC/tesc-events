using FluentValidation;
using TescEvents.Models;
using TescEvents.Repositories;

namespace TescEvents.Validators; 

public class UserValidator : AbstractValidator<User> {
    public UserValidator(IUserRepository userRepository) {
        RuleFor(u => u.Username)
            .EmailAddress()
            .Must(u => userRepository.GetUserByUsername(u) == null)
            .WithMessage("User already exists with that username");
    }
}