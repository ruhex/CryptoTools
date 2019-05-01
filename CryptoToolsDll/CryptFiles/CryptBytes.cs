using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Crypt
{
    class CryptBytes : Crypt
    {
        public CryptBytes() { }
        public CryptBytes(byte[] _cryptKey) { }

        public override void EncryptAsync(byte[] _key, string[] _files)
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
    }
}
