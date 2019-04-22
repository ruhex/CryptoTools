using System;
using System.IO;
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

        public bool Crypt(byte[] _key)
        {
            return true;
        }

        private byte[] GenCryptKey(byte[] _data)
        {
            byte[] tmpKey;

            tmpKey = System.Security.Cryptography.SHA512.Create().ComputeHash(_data);

            return tmpKey;

        }

        private bool GenCryptFile(string _name)
        {
            string line = "";
            FileStream fileStream = new FileStream(_name, FileMode.Open);
            return true;
            
        }

    }
}
