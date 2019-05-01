using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CryptFiles
{
    public class CryptFiles : ICryptFiles
    {
        private string Name { get; set; }
        private byte[] CryptKey { get; set; }
        public CryptFiles() { }
        public CryptFiles(byte[] _cryptKey) { }
        public void EncryptAsync(byte[] _key, string[] _files)
        {
            foreach (string tmp in _files)
                AESEncryptFileAsync(tmp, AESGenEncryptKey(_key));
        }
        public void Decrypt(byte[] _key, string[] _files)
        {
            foreach (string tmp in _files)
                AESDecryptFileAsync(tmp, AESGenEncryptKey(_key));
        }
        private async Task<byte[]> AESEncryptDataAsync(byte[] _data, byte[] _key)
        {
            return await Task.Run(() => AESEncryptData(_data, _key));
        }
        private byte[] AESEncryptData(byte[] _data, byte[] _key)
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
                        csEncrypt.WriteAsync(_data, 0, _data.Length);

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
        private byte[] AESDecryptData(byte[] _data, byte[] _key)
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
        private string GetFileName(string _file)
        {
            return _file.Remove(_file.LastIndexOf('.'));
        }
        public string[] GetFiles(string _path)
        {
            List<string> massiv = new List<string>();
            FileInfo[] files = new DirectoryInfo(_path).GetFiles();
            foreach (FileInfo tmp in files)
                massiv.Add(tmp.FullName);
            return massiv.ToArray();
        }
        private byte[] AESGenEncryptKey(byte[] _password)
        {
            return SHA256.Create().ComputeHash(_password);
        }

    }
}
