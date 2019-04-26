using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordGenerator
{
    public interface IPasswordGenerator
    {
        string GetPassword();
        void SetLength(int _length);
        void SetLower(bool _lower);
        void SetUpper(bool _upper);
        void SetNumber(bool _number);
        void SetSpecialSymbols(bool _special_symbols);
    }
}