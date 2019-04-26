using System;
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
                new PasswordGenerator.PasswordGenerator(8, true, true, true, false, true),
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

                        for (int i = 0; i < 10000; i++)
                        {
                            Console.WriteLine(init._passwordGeneratior.GetPassword());
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
