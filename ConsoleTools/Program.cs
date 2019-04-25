using System;
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
                new PasswordGenerator.PasswordGenerator(8, true, true, false, false),
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
                        Console.WriteLine();
                        Console.WriteLine("------------------------------------");
                        Console.WriteLine(init._passwordGeneratior.GetPassword());

                        for (int i = 0; i < 1000; i++)
                        {
                            Console.WriteLine(PassTest(init._passwordGeneratior.GetPassword()));
                        }


                        Console.WriteLine("------------------------------------");
                        break;
                    case (char)50:
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
                                init._cryptFiles.Encrypt(PasswordToByte(password), file_name);
                                Console.WriteLine($"File - {file_name}, encrypted!");
                                break;
                            case (char)50:
                                Console.WriteLine($"Start decrypt file: {file_name}");
                                init._cryptFiles.Decrypt(PasswordToByte(password), file_name);
                                break;
                        }
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


        static string PassTest(string _pass)
        {
            char test = '0';
            int y = 0;
            List<char> chekChar = new List<char>();
            List<char> chekPass = new List<char>();

            foreach (char tmp in _pass)
                chekPass.Add(tmp);

            for (byte i = 48; i <= 57; i++)
                test += chekPass.Find(delegate (char _char) { return _char == (char)i; });
            if (test != '0')
                y += 1;
            test = '0';

            for (byte i = 65; i <= 90; i++)
                test += chekPass.Find(delegate (char _char) { return _char == (char)i; });
            if (test != '0')
                y += 1;
            test = '0';

            for (byte i = 97; i <= 122; i++)
                test += chekPass.Find(delegate (char _char) { return _char == (char)i; });
            if (test != '0')
                y += 1;
            test = '0';

            byte[] _chars = { 33, 35, 36, 37, 40, 41, 91, 93, 95, 123, 125 };

            foreach (byte tmp in _chars)
            {
                test += chekPass.Find(delegate (char _char) { return _char == (char)tmp; });
                if (test != '0')
                    y += 1;
            }

            if (y == 4)
                return "ok " + _pass;
            else
                return _pass;


        }




        static string GetPass(IPasswordGenerator _passwordGeneratior)
        {
            return _passwordGeneratior.GetPassword();
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
