namespace Advexp
{
    public interface ISettingsSerializerWishes
    {
        char Delimeter();
        string RemoveInappropriateSymbols(string text);
    }
}