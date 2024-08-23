using FluentValidation;
using MessengerServer.Src.Contracts.Abstractions.AuthenticationRequests;
using MessengerServer.Src.Contracts.MessagesList;

namespace MessengerServer.Src.WebApi.Validations.AuthencationValidations;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Password)
             .NotEmpty()
             .WithErrorCode(MessagesList.MissingFullName.GetMessage().Code)
             .WithMessage(MessagesList.MissingPassword.GetMessage().Message).DependentRules(() =>
             {
                 RuleFor(x => x.Password)
                      .MinimumLength(6)
                      .WithErrorCode(MessagesList.PasswordMinimumLengthSix.GetMessage().Code)
                      .WithMessage(MessagesList.PasswordMinimumLengthSix.GetMessage().Message);
             });

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithErrorCode(MessagesList.MissingEmail.GetMessage().Code)
            .WithMessage(MessagesList.MissingEmail.GetMessage().Message)
            .DependentRules(() =>
            {
                RuleFor(x => x.Email)
                    .EmailAddress()
                    .WithErrorCode(MessagesList.NotFormatEmail.GetMessage().Code)
                    .WithMessage(MessagesList.NotFormatEmail.GetMessage().Message);
            });
    }
}
