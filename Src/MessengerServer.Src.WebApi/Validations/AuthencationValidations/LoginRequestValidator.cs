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
             .WithErrorCode(MessagesList.MissingFullName.GetErrorMessage().Code)
             .WithMessage(MessagesList.MissingPassword.GetErrorMessage().Message).DependentRules(() =>
             {
                 RuleFor(x => x.Password)
                      .MinimumLength(6)
                      .WithErrorCode(MessagesList.PasswordMinimumLengthSix.GetErrorMessage().Code)
                      .WithMessage(MessagesList.PasswordMinimumLengthSix.GetErrorMessage().Message);
             });

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithErrorCode(MessagesList.MissingEmail.GetErrorMessage().Code)
            .WithMessage(MessagesList.MissingEmail.GetErrorMessage().Message)
            .DependentRules(() =>
            {
                RuleFor(x => x.Email)
                    .EmailAddress()
                    .WithErrorCode(MessagesList.NotFormatEmail.GetErrorMessage().Code)
                    .WithMessage(MessagesList.NotFormatEmail.GetErrorMessage().Message);
            });
    }
}
