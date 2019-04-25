using System;
using System.Collections.Generic;
using System.Text;

namespace CryptFiles
{
    public interface ICryptFiles
    {
        void Encrypt(byte[] _key, string _fileName);
        void Decrypt(byte[] _key, string _fileName);
    }
}
