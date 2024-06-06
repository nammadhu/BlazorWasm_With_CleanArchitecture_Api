using SharedResponse.DTOs;

namespace SharedResponse;

public interface ITranslator
{
    string this[string name]
    {
        get;
    }

    string GetString(string name);
    string GetString(TranslatorMessageDto input);
}
