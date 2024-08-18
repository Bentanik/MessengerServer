namespace MessengerServer.Src.Contracts.MessagesList;

public enum MessagesList
{
    [Message("Full Name is required.", "fn01")]
    MissingFullName,

    [Message("This full name already exists in the system.", "fn02")]
    FullNameExsits,

    [Message("Password is required.", "psd01")]
    MissingPassword,

    [Message("Password must be at least 6 characters long.", "psd02")]
    PasswordMinimumLengthSix,

    [Message("Password does not match.", "psd03")]
    PasswordNotMatch,

    [Message("Email is required.", "em01")]
    MissingEmail,

    [Message("Invalid email format.", "em02")]
    NotFormatEmail,

    [Message("This email has been registered in the system.", "em03")]
    EmailExsits,

    [Message("Please check account email and send again!", "sem01")]
    EmailSendingFailed,

    [Message("Account of you only exists in 12 hours, please check email to sign up.", "rg01")]
    Register,

    [Message("Activation failed, please re-register to activate!", "rg02")]
    ActiveRegisterFail,

    [Message("Sign Up Success.", "rg03")]
    RegisterSuccess,

    [Message("Registration failed, please register again!", "rg04")]
    RegisterFail,

    [Message("User does not exist in the system!", "us01")]
    UserNotExist,

    [Message("Please login again!", "lg01")]
    LoginAgain,

    [Message("Login timeout expired, please login again", "lg02")]
    LoginTimeout,

    [Message("Update email successfully", "ue01")]
    UpdateEmailSuccess,

    [Message("Update email fail, please again!", "ue02")]
    UpdateEmailFail,
}