using System;
using System.Security.Cryptography;


namespace Crypt
{
    public class Crypt : ICrypt
    {
        public virtual void DecryptAsync(byte[] _key, string[] _files)
        {
            throw new NotImplementedException();
        }

        public virtual void EncryptAsync(byte[] _key, string[] _files)
        {
            throw new NotImplementedException();
        }

        protected byte[] AESGenEncryptKey(byte[] _password)
        {
            return SHA256.Create().ComputeHash(_password);
        }
    }
}
