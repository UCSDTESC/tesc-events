using FluentValidation;
using TescEvents.Models;
using TescEvents.Repositories;

namespace TescEvents.Validators; 

public class UserValidator : AbstractValidator<User> {
    public UserValidator(IUserRepository userRepository) {
        // TODO: Insert user validation rules
        RuleFor(u => userRepository.GetUserByUuid(u.Id))
            .Null();
    }
}