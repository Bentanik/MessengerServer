using FluentValidation;
using MessengerServer.Src.Contracts.Abstractions.EditUserRequests;
using MessengerServer.Src.Contracts.MessagesList;

namespace MessengerServer.Src.WebApi.Validations.EditUserValidations;

public class EditFullNameValidator : AbstractValidator<EditFullNameRequest>
{
    public EditFullNameValidator()
    {
        RuleFor(x => x.FullName)
          .NotEmpty()
          .WithErrorCode(MessagesList.MissingFullName.GetMessage().Code)
          .WithMessage(MessagesList.MissingFullName.GetMessage().Message);
    }
}
