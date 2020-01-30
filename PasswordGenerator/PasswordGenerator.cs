using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordGenerator
{
    public class PasswGenerator
    {
        private string number_symb = "0123456789";
        private string lowercase_symb = "abcdefghijklmnopqrstvwxyz";
        private string uppercase_symb = "ABCDEFGHIJKLMNOPQRSTVWXYZ";
        private string special_symb = "!@#$%^&*()~_+=-";
        private string accesible_symbols;

        public string Generate(int passwordSize)
        {
            string password = "";
            int as_amount = accesible_symbols.Length; // accessible symbols amount

            Random me = new Random();
            for (int i = 0; i < passwordSize; i++)
            {
                int rand_index = me.Next(0, as_amount - 1);
                password += accesible_symbols[rand_index];
            }
            return password;
        }
    }
}
