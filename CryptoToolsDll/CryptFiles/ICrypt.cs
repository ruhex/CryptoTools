namespace Crypt
{
    public interface ICrypt
    {
        void EncryptAsync(byte[] _key, string[] _files);
        void DecryptAsync(byte[] _key, string[] _files);
    }
}
