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
        public CryptFiles(byte[] _cryptKey)
        {

        }

        public void EncryptAsync(byte[] _key, string[] _files)
        {
            AESEncryptFileAsync(@"D:\test\ww\_NJplHNvL8M.jpg", AESGenEncryptKey(_key));
            foreach (string tmp in _files)
            {
                //AESEncryptFileAsync(tmp, AESGenEncryptKey(_key));
                //WriteFileAsync(AESEncryptDataAsync(ReadFile(tmp), AESGenEncryptKey(_key)).Result, $"{tmp}_encrypt");
                //WriteFile(AESEncryptData(ReadFile(tmp), AESGenEncryptKey(_key)), $"{tmp}_encrypt");
            }

        }

        public void Decrypt(byte[] _key, string[] _files)
        {
            AESDecryptFile(@"D:\test\ww\_NJplHNvL8M.jpg", AESGenEncryptKey(_key));
            foreach (string tmp in _files)
            {
               // AESDecryptFile(tmp, AESGenEncryptKey(_key));
                //WriteFile(AESDecryptData(ReadFile(tmp), AESGenEncryptKey(_key)), $"{tmp}");
            }

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

        private void WriteIV(byte[] _iv, string _fileName)
        {
            using (FileStream writeStream = File.Create($"{_fileName}.iv"))
            {
                writeStream.Write(_iv);
            }
        }

        private byte[] ReadIV(string _fileName)
        {
            using (FileStream readStream = File.Open($"{_fileName}.iv", FileMode.Open))
            {
                byte[] data = new byte[readStream.Length];
                readStream.Read(data, 0, data.Length);
                readStream.Close();
                return data;
            }
        }
        private async Task AESEncryptFileAsync(string _fileName, byte[] _key)
        {
            await Task.Run(() => AESEncryptFile(_fileName, _key));
        }
        private async Task AESEncryptFile(string _fileName, byte[] _key)
        {
            using (Aes _aes = Aes.Create())
            {
                byte[] iv;
                _aes.KeySize = 256;
                _aes.Mode = CipherMode.CBC;

                _aes.Key = _key;
                _aes.GenerateIV();
                iv = _aes.IV;
                WriteIV(iv, _fileName);
                ICryptoTransform encryptor = _aes.CreateEncryptor(_aes.Key, _aes.IV);

                using (FileStream SourceStream = File.Create($"{_fileName}.encrypt"))
                {
                    using (CryptoStream csEncrypt = new CryptoStream(SourceStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (FileStream DestinationStream = File.Open(_fileName, FileMode.Open))
                        {
                            DestinationStream.CopyTo(csEncrypt);
                        }
                    }
                }

            }
        }

        private void AESDecryptFile(string _fileName, byte[] _key)
        {
            byte[] iv = ReadIV(_fileName);

            using (Aes _aes = Aes.Create())
            {
                _aes.KeySize = 256;
                _aes.Mode = CipherMode.CBC;

                _aes.Key = _key;
                _aes.IV = iv;

                ICryptoTransform decryptor = _aes.CreateDecryptor(_aes.Key, _aes.IV);

                using (FileStream DestinationStream = File.Open($"{ _fileName}.encrypt", FileMode.Open))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(DestinationStream, decryptor, CryptoStreamMode.Read))
                    {
                        using (FileStream SourceStream = File.Create($"{_fileName}"))
                        {
                            csDecrypt.CopyTo(SourceStream);
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
            return Path.GetFileNameWithoutExtension(_file);
        }
        public string[] GetFiles(string _path)
        {
            List<string> massiv = new List<string>();
            FileInfo[] files = new DirectoryInfo(_path).GetFiles();
            foreach (FileInfo tmp in files)
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


        private async Task WriteFileAsync(byte[] _data, string _fileName)
        {
            await Task.Run(() => WriteFile(_data, _fileName));
        }

        private void WriteFile(byte[] _data, string _fileName)
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
