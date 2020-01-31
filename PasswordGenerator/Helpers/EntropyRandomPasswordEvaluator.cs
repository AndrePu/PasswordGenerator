using System;
using PasswordGenerator.Interfaces;
using PasswordGenerator.Enums;

namespace PasswordGenerator.Helpers
{
    public class EntropyRandomPasswordEvaluator : IRandomPasswordEvaluator
    {
        public PasswordRating Evaluate(int passwordSize, string accessibleSymbols)
        {
            double measure = FindEntropyMeasure(passwordSize, accessibleSymbols);

            // desired entropy measures
            double[] desiredEntropy = { 40, 80, 96 };

            if (measure < desiredEntropy[0])
            {
                return PasswordRating.Weak;
            }
            else if (measure < desiredEntropy[1])
            {
                return PasswordRating.Medium;
            }
            else if (measure < desiredEntropy[2])
            {
                return PasswordRating.Strong;
            }
            else
            {
                return PasswordRating.VeryStrong;
            }
        }

        /// <summary>
        /// Evaluates password by Entropy measure.
        /// Detailed(2.1-2.2): https://en.wikipedia.org/wiki/Password_strength
        /// </summary>
        private double FindEntropyMeasure(int passwordSize, string accessibleSymbols)
        {
            if (accessibleSymbols == null || accessibleSymbols.Length == 0)
                return 0;

            double measure = passwordSize * Math.Log(accessibleSymbols.Length, 2);

            return measure;
        }
    }
}
