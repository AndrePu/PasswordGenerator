using System;
using System.Text;
using PasswordGenerator.Interfaces;

namespace PasswordGenerator.Helpers
{
    public class GeneralPasswordGenerator : IPasswordGenerator
    {
        public string Generate(int passwordSize, string accessibleSymbols)
        {
            var passwordBuilder = new StringBuilder(passwordSize);

            Random rand = new Random();

            for (int i = 0; i < passwordSize; i++)
            {
                int randNumber = rand.Next(0, accessibleSymbols.Length - 1);

                passwordBuilder.Append(accessibleSymbols[randNumber]);
            }

            return passwordBuilder.ToString();
        }
    }
}
