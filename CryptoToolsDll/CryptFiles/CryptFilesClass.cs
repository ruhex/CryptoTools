using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

namespace CryptFiles
{
    public class CryptFiles : ICryptFiles
    {
        private string Name { get; set; }
        private byte[] CryptKey { get; set; }
        public CryptFiles() { }
        public CryptFiles(byte[] _cryptKey)
        {

        }

        public void Encrypt(byte[] _key, string[] _files)
        {
            foreach (string tmp in _files)
                WriteFile(AESEncryptData(ReadFile(tmp), AESGenEncryptKey(_key)), $"{tmp}_encrypt");
        }

        public void Decrypt(byte[] _key, string[] _files)
        {
            foreach (string tmp in _files)
                WriteFile(AESDecryptData(ReadFile(tmp), AESGenEncryptKey(_key)), $"{tmp}");


        }

        byte[] AESEncryptData(byte[] _data, byte[] _key)
        {
            byte[] encrypted, tmp;


            using (Aes _aes = Aes.Create())
            {
                byte[] iv;
                _aes.KeySize = 256;
                _aes.Mode = CipherMode.CBC;

                _aes.Key = _key;
                _aes.GenerateIV();
                iv = _aes.IV;

                ICryptoTransform encryptor = _aes.CreateEncryptor(_aes.Key, _aes.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(_data, 0, _data.Length);

                    }
                    encrypted = msEncrypt.ToArray();
                }

                tmp = new byte[iv.Length + encrypted.Length];
                iv.CopyTo(tmp, 0);
                encrypted.CopyTo(tmp, iv.Length);
                //encrypted = tmp;

            }
            return tmp;
        }

        byte[] AESDecryptData(byte[] _data, byte[] _key)
        {
            byte[] decrypt;
            List<byte> tmp_list = new List<byte>(_data);
            byte[] iv = new byte[16];
            tmp_list.RemoveRange(0, 16);


            for (int i = 0; i < 16; i++)
                iv[i] = _data[i];

            _data = tmp_list.ToArray();


            using (Aes _aes = Aes.Create())
            {
                _aes.KeySize = 256;
                _aes.Mode = CipherMode.CBC;

                _aes.Key = _key;
                _aes.IV = iv;

                ICryptoTransform decryptor = _aes.CreateDecryptor(_aes.Key, _aes.IV);

                using (MemoryStream msDecrypt = new MemoryStream(_data))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        csDecrypt.Read(_data, 0, _data.Length);
                    }
                    decrypt = msDecrypt.ToArray();
                }
            }
            return decrypt;
        }

        public string[] GetFiles(string _path)
        {
            
            FileInfo[] fileInfo = new DirectoryInfo(_path).GetFiles();

            List<string> massiv = new List<string>();

            foreach (FileInfo tmp in fileInfo)
                massiv.Add(tmp.FullName);

            return massiv.ToArray();

        }

        private byte[] ReadFile(string _fileName)
        {
            FileStream fileStream = new FileStream(_fileName, FileMode.Open);

            byte[] test = new byte[fileStream.Length];

            fileStream.Read(test, 0, test.Length);
            fileStream.Close();
            return test;
        }
        void WriteFile(byte[] _data, string _fileName)
        {
            FileStream new_file = new FileStream($"{_fileName}", FileMode.Create);

            new_file.Write(_data, 0, _data.Length);
            new_file.Close();
        }

        private byte[] AESGenEncryptKey(byte[] _password)
        {
            return SHA256.Create().ComputeHash(_password);
        }

    }
}
