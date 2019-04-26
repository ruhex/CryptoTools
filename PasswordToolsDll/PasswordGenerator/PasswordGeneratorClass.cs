using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordGenerator
{
    public class PasswordGenerator : IPasswordGenerator
    {
        private const int GenCharArrayCount = 512;
        private int Length { get; set; }
        private bool Number { get; set; }
        private bool Lower { get; set; }
        private bool Upper { get; set; }
        private bool SpecialSymbols { get; set; }

        public PasswordGenerator() { }

        public PasswordGenerator(int _length)
        {
            this.Length = _length;
        }
        public PasswordGenerator(int _length, bool _number, bool _lower, bool _upper, bool _special_symbols)
        {

            this.Length = _length;
            this.Number = _number;
            this.Lower = _lower;
            this.Upper = _upper;
            this.SpecialSymbols = _special_symbols;
        }


        private string PassGen()
        {
            string passwd;

            do
            {
                passwd = "";
                Collection<char> charArray = StrongGenCharArray();
                Random _random = new Random();

                int x, y;
                char temp;

                for (int i = 0; i < charArray.Count; i++)
                {
                    x = _random.Next(charArray.Count);
                    y = _random.Next(charArray.Count);

                    temp = charArray[x];
                    charArray[x] = charArray[y];
                    charArray[y] = temp;
                }

                for (int i = 0; i < Length; i++)
                {
                    passwd += charArray[_random.Next(charArray.Count)];
                }
            } while (!CheckPassword(passwd));

            return passwd;

        }

        private Collection<char> StrongGenCharArray()
        {
            Collection<char> charArray = new Collection<char>();
            Random _random = new Random();
            

            for (int i = 0; i < GenCharArrayCount; i++)
            {
                if (this.Number)
                    charArray.Add((char)_random.Next(48, 57));

                if (this.Upper)
                    charArray.Add((char)_random.Next(65, 90));

                if (this.Lower)
                    charArray.Add((char)_random.Next(97, 122));

                if (this.SpecialSymbols)
                {
                    byte[] _spec_chars = { 33, 35, 36, 37, 40, 41, 91, 93, 95, 123, 125 };
                    charArray.Add((char)_spec_chars[_random.Next(_spec_chars.Length)]);
                }
            }

            return charArray;
        }

        private Collection<char> GenCharArray()
        {
            Collection<char> charArray = new Collection<char>();



            if (this.Number)
                for (byte i = 48; i <= 57; i++)
                    charArray.Add((char)i);

            if (this.Upper)
                for (byte i = 65; i <= 90; i++)
                    charArray.Add((char)i);

            if (this.Lower)
                for (byte i = 97; i <= 122; i++)
                    charArray.Add((char)i);

            if (this.SpecialSymbols)
            {                
                byte[] _chars = { 33, 35, 36, 37, 40, 41, 91, 93, 95, 123, 125 };

                foreach (byte tmp in _chars)
                {
                    charArray.Add((char)tmp);
                }
            }

            
            return charArray;
        }

        private bool CheckPassword(string _pass)
        {
            bool[] controlTest = { this.Number, this.Lower, this.Upper, this.SpecialSymbols };
            bool[] test = { false, false, false, false };

            List<char> massiv = new List<char>(_pass);
            List<char> massivNumber = new List<char>();
            List<char> massivUpper = new List<char>();
            List<char> massivLower = new List<char>();
            List<char> massivSpecialSymbols = new List<char>();

            byte[] _chars = { 33, 35, 36, 37, 40, 41, 91, 93, 95, 123, 125 };

            for (byte i = 48; i <= 57; i++)
                massivNumber.Add((char)i);

            for (byte i = 65; i <= 90; i++)
                massivUpper.Add((char)i);

            for (byte i = 97; i <= 122; i++)
                massivLower.Add((char)i);

            foreach (char tmp in _chars)
                massivSpecialSymbols.Add(tmp);

            foreach (char tmp in massivNumber)
            {
                if (massiv.Contains(tmp))
                    test[0] = true;
            }

            foreach (char tmp in massivLower)
            {
                if (massiv.Contains(tmp))
                    test[1] = true;
            }

            foreach (char tmp in massivUpper)
            {
                if (massiv.Contains(tmp))
                    test[2] = true;
            }

            foreach (char tmp in massivSpecialSymbols)
            {
                if (massiv.Contains(tmp))
                    test[3] = true;
            }

            if (test.SequenceEqual(controlTest))
                return true;
            else
                return false;

        }


        public string GetPassword()
        {
            return PassGen();
        }

        public void SetNumber(bool _number)
        {
            this.Number = _number;
        }

        public void SetLength(int _length)
        {
            this.Length = _length;
        }

        public void SetLower(bool _lower)
        {
            this.Lower = _lower;
        }

        public void SetUpper(bool _upper)
        {
            this.Upper = _upper;
        }

        public void SetSpecialSymbols(bool _special_symbols)
        {
            this.SpecialSymbols = _special_symbols;
        }
    }
}