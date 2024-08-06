namespace MessengerServer.Src.Contracts.MessagesList;

[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
sealed class MessageAttribute(string message, string code) : Attribute
{
    public string Message { get; } = message;
    public string Code { get; } = code;
}

public static class MessageAttributeExtension
{
    public static (string Message, string Code) GetErrorMessage(this MessagesList errorCode)
    {
        var type = typeof(MessagesList);
        var field = type.GetField(errorCode.ToString());

        var attribute = (MessageAttribute?)field?.GetCustomAttributes(typeof(MessageAttribute), false).FirstOrDefault();
        return attribute != null ? (attribute.Message, attribute.Code) : (string.Empty, string.Empty);
    }
}