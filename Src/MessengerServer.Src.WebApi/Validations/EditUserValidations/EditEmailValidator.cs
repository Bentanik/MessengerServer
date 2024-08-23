using FluentValidation;
using MessengerServer.Src.Contracts.Abstractions.EditUserRequests;
using MessengerServer.Src.Contracts.MessagesList;

namespace MessengerServer.Src.WebApi.Validations.EditUserValidations;

public class EditEmailValidator : AbstractValidator<EditEmailRequest>
{
    public EditEmailValidator()
    {
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
