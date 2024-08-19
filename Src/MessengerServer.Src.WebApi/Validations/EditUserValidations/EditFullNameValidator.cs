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
          .WithErrorCode(MessagesList.MissingFullName.GetErrorMessage().Code)
          .WithMessage(MessagesList.MissingFullName.GetErrorMessage().Message);
    }
}
