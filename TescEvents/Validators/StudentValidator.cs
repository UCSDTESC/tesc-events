using FluentValidation;
using TescEvents.Models;
using TescEvents.Repositories;

namespace TescEvents.Validators; 

public class StudentValidator : AbstractValidator<Student> {
    public StudentValidator(IStudentRepository studentRepository) {
        RuleFor(u => u.Username)
            .EmailAddress()
            .Must(u => studentRepository.GetUserByUsername(u) == null)
            .WithMessage("User already exists with that username");
    }
}