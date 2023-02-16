using FluentValidation;
using TescEvents.DTOs;
using TescEvents.Entities;
using TescEvents.Services;

namespace TescEvents.Validators; 

public class UserCreateRequestValidator : AbstractValidator<StudentCreateRequestDTO> {
    private IUserService context;
    public UserCreateRequestValidator(IUserService context) {
        this.context = context;
        RuleLevelCascadeMode = CascadeMode.Stop;
        
        RuleFor(u => u.Email)
            .EmailAddress()
            .Must(NotInUse).WithMessage("Email address already in use");
        RuleFor(u => u.Password)
            .NotEmpty()
            .MinimumLength(8);
        RuleFor(u => u.Pid)
            .NotEmpty();
        RuleFor(u => u.FirstName)
            .NotEmpty();
        RuleFor(u => u.LastName)
            .NotEmpty();
    }

    private bool NotInUse(string email) {
        return context.GetStudentByEmail(email) == null;
    }
}