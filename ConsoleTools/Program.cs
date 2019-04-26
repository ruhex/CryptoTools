﻿using System;
using System.Linq;
using System.Collections.ObjectModel;
using PasswordGenerator;
using PasswordSave;
using CryptFiles;
using System.Collections.Generic;

namespace ConsoleTools
{
    class Program
    {
        static void Main(string[] args)
        {
            Init init = new Init(
                new PasswordGenerator.PasswordGenerator(8, true, true, true, true),
                new CryptFiles.CryptFiles()
                );
           


            //ToolsMenu.massiv.Add(new ToolsMenu("Password generation", init._passwordGeneratior));


            Console.WriteLine("Please select tool:");
            Console.WriteLine("");
            Console.WriteLine("1. Password generation");
            Console.WriteLine("2. AES encrypt / decrypt files");


            try
            {
                char _tool = Console.ReadKey().KeyChar;
                switch (_tool)
                {
                    case (char)49:
                        // PASSWORD FUNCTION
                        GetPass(init._passwordGeneratior);
                        break;

                    case (char)50:
                        // CRYPTO FUNCTION
                        CryptoFile(init._cryptFiles);
                        break;
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }





            for (byte i = 0; i < ToolsMenu.massiv.Count; i++)
                Console.WriteLine($"{i} {ToolsMenu.massiv[i].Title}");

            //switch(ToolsMenu.massiv[0].Obj.GetType())
            //{
            //    case Type.GetType(""):
            //        Console.WriteLine("1");
            //        break;
            //}
            //Console.WriteLine(init._passwordGeneratior.GetPassword());

            Console.ReadKey();

        }

        static void CryptoFile(ICryptFiles _cryptFiles)
        {
            Console.Write("Please enter file name: ");
            string file_name = Console.ReadLine();

            Console.Write("Please enter password: ");
            string password = Console.ReadLine();

            Console.WriteLine("1. Encrypt file");
            Console.WriteLine("2. Decrypt file");

            char e = Console.ReadKey().KeyChar;

            switch (e)
            {
                case (char)49:
                    Console.WriteLine($"Start encrypt file: {file_name}");
                    _cryptFiles.Encrypt(PasswordToByte(password), file_name);
                    Console.WriteLine($"File - {file_name}, encrypted!");
                    break;
                case (char)50:
                    Console.WriteLine($"Start decrypt file: {file_name}");
                    _cryptFiles.Decrypt(PasswordToByte(password), file_name);
                    break;
            }
        }
        
        static void GetPass(IPasswordGenerator _passwordGenerator)
        {

            Console.Write("Password length (default 8)?: ");
            int count;
            try
            {
                count = Convert.ToInt32(Console.ReadLine());
                _passwordGenerator.SetLength(count);
            }
            catch
            {
                _passwordGenerator.SetLength(8);
            }

            Console.WriteLine("");

            Console.Write("Use numbers (default yes)?: ");
            string useNumber = Console.ReadLine();
            if (useNumber == "no")
                _passwordGenerator.SetNumber(false);
            else
                _passwordGenerator.SetNumber(true);
            Console.WriteLine("");

            Console.Write("Use lower char (default yes)?: ");
            string useLower = Console.ReadLine();
            if (useLower == "no")
                _passwordGenerator.SetLower(false);
            else
                _passwordGenerator.SetLower(true);
            Console.WriteLine("");

            Console.Write("Use upper char (default yes)?: ");
            string useUpper = Console.ReadLine();
            if (useUpper == "no")
                _passwordGenerator.SetUpper(false);
            else
                _passwordGenerator.SetUpper(true);
            Console.WriteLine("");

            Console.Write("Use special symbols  (default yes)?: ");
            string useSpecialSymbols = Console.ReadLine();
            if (useSpecialSymbols == "no")
                _passwordGenerator.SetSpecialSymbols(false);
            else
                _passwordGenerator.SetSpecialSymbols(true);
            Console.WriteLine("");




            Console.WriteLine("");
            Console.WriteLine("------------------------------------");
            Console.WriteLine(_passwordGenerator.GetPassword());

            for (int i = 0; i < 1000; i++)
            {
                Console.WriteLine(_passwordGenerator.GetPassword());
            }


            Console.WriteLine("------------------------------------");
        }

        static byte[] PasswordToByte(string _passwd)
        {
            byte[] key = new byte[_passwd.Length];

            for (int i = 0; i < _passwd.Length; i++)
            {
                key[i] = (byte)_passwd[i];
            }
            return key;
        }


    }

    class Init
    {
        public IPasswordGenerator _passwordGeneratior;
        public ICryptFiles _cryptFiles;

        public Init() { }
        public Init(IPasswordGenerator _passwordGeneratior, ICryptFiles _cryptFiles)
        {
            this._passwordGeneratior = _passwordGeneratior;
            this._cryptFiles = _cryptFiles;
        }
    }

    class ToolsMenu
    {
        public string Title { get; set; }
        public Object Obj { get; set; }

        public static Collection<ToolsMenu> massiv = new Collection<ToolsMenu>();

        public ToolsMenu(string _title)
        {
            this.Title = _title;
        }

        public ToolsMenu(string _title, object _obj)
        {
            this.Title = _title;
            this.Obj = _obj;
        }

    }
}
