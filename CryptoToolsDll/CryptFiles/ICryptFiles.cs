using System;
using System.Collections.Generic;
using System.Text;

namespace CryptFiles
{
    interface ICryptFiles
    {
        bool Crypt(byte[] _key);
    }
}
