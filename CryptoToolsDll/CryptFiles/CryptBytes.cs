using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Crypt
{
    class CryptBytes : Crypt
    {
<<<<<<< HEAD:CryptoToolsDll/CryptFiles/CryptFilesClass.cs
        Stopwatch sw;
        private string Name { get; set; }
        private byte[] CryptKey { get; set; }
        public CryptFiles() { }
        public CryptFiles(byte[] _cryptKey)
=======
        public CryptBytes() { }
        public CryptBytes(byte[] _cryptKey) { }

        public override void EncryptAsync(byte[] _key, string[] _files)
>>>>>>> async:CryptoToolsDll/CryptFiles/CryptBytes.cs
        {

        }

        public override void DecryptAsync(byte[] _key, string[] _files)
        {

        }

        private async Task<byte[]> AESEncryptDataAsync(byte[] _data, byte[] _key)
        {
            return await Task.Run(() => AESEncryptData(_data, _key));
        }
        private async Task<byte[]> AESDecryptDataAsync(byte[] _data, byte[] _key)
        {
            return await Task.Run(() => AESDecryptData(_data, _key));
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
<<<<<<< HEAD:CryptoToolsDll/CryptFiles/CryptFilesClass.cs

        public string[] GetFiles(string _path)
        {
            
            FileInfo[] fileInfo = new DirectoryInfo(_path).GetFiles();

            List<string> massiv = new List<string>();

            foreach (FileInfo tmp in fileInfo)
                massiv.Add(tmp.FullName);

            return massiv.ToArray();

        }


        private async Task<byte[]> ReadFileAsync(string _fileName)
        {
            return await Task.Run(() => ReadFile(_fileName));
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

        ~CryptFiles()
        {
            sw.Stop();
        }

=======
>>>>>>> async:CryptoToolsDll/CryptFiles/CryptBytes.cs
    }
}
