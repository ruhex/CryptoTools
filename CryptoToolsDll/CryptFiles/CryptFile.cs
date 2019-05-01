using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Crypt
{
    public class CryptFile : Crypt
    {
        public CryptFile() { }
        public CryptFile(byte[] _cryptKey) { }
        public override void EncryptAsync(byte[] _key, string[] _files)
        {
            foreach (string tmp in _files)
                AESEncryptFileAsync(tmp, AESGenEncryptKey(_key));
        }
        public override void DecryptAsync(byte[] _key, string[] _files)
        {
            foreach (string tmp in _files)
                AESDecryptFileAsync(tmp, AESGenEncryptKey(_key));
        }
        
        private async Task AESEncryptFileAsync(string _fileName, byte[] _key)
        {
            using (Aes _aes = Aes.Create())
            {
                _aes.KeySize = 256;
                _aes.Mode = CipherMode.CBC;

                _aes.Key = _key;
                _aes.GenerateIV();

                ICryptoTransform encryptor = _aes.CreateEncryptor(_aes.Key, _aes.IV);

                using (FileStream creteStream = File.Create($"{_fileName}.encrypt"))
                {
                    using (CryptoStream csEncrypt = new CryptoStream(creteStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (FileStream openStream = File.Open(_fileName, FileMode.Open))
                        {
                            await csEncrypt.WriteAsync(_aes.IV, 0, _aes.IV.Length);
                            await openStream.CopyToAsync(csEncrypt);
                        }
                    }
                }

            }
        }
        private async Task AESDecryptFileAsync(string _fileName, byte[] _key)
        {
            byte[] iv = new byte[16];

            using (Aes _aes = Aes.Create())
            {
                _aes.KeySize = 256;
                _aes.Mode = CipherMode.CBC;

                _aes.Key = _key;


                using (FileStream openStream = File.Open($"{ _fileName}", FileMode.Open))
                {

                    await openStream.ReadAsync(iv, 0, iv.Length);
                    ICryptoTransform decryptor = _aes.CreateDecryptor(_aes.Key, iv);

                    using (CryptoStream csDecrypt = new CryptoStream(openStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (FileStream createStream = File.Create($"{GetFileName(_fileName)}"))
                        {
                            await csDecrypt.CopyToAsync(createStream);
                        }
                    }
                   
                        
                }
            }
        }
        
        private string GetFileName(string _file)
        {
            return _file.Remove(_file.LastIndexOf('.'));
        }
        
        

    }
}
