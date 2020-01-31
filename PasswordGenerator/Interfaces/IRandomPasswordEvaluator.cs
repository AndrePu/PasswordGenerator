using PasswordGenerator.Enums;

namespace PasswordGenerator.Interfaces
{
    public interface IRandomPasswordEvaluator
    {
        PasswordRating Evaluate(int passwordSize, string accessibleSymbols);
    }
}
