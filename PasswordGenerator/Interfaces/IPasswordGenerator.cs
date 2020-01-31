
namespace PasswordGenerator.Interfaces
{
    public interface IPasswordGenerator
    {
        string Generate(int passwordSize, string accessibleSymbols);
    }
}
