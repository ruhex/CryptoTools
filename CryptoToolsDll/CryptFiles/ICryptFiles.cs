﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CryptFiles
{
    public interface ICryptFiles
    {
        void EncryptAsync(byte[] _key, string[] _files);
        void Decrypt(byte[] _key, string[] _files);
        string[] GetFiles(string _path);
    }
}
