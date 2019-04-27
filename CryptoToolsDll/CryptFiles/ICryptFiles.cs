using System;
using System.Collections.Generic;
using System.Text;

namespace CryptFiles
{
    public interface ICryptFiles
    {
        void Encrypt(byte[] _key, string[] _files);
        void Decrypt(byte[] _key, string[] _files);
        string[] GetFiles(string _path);
    }
}
