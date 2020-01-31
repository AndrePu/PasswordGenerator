using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordGenerator.Interfaces
{
    public interface IPasswordSaver
    {
        void Save(string password);
    }
}
