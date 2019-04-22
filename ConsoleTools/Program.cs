using System;
using System.Collections.ObjectModel;
using PasswordGenerator;
using PasswordSave;

namespace ConsoleTools
{
    class Program
    {
        static void Main(string[] args)
        {
            Init init = new Init(new PasswordGenerator.PasswordGenerator(8, true, true, false, false));


            //ToolsMenu.massiv.Add(new ToolsMenu("Password generation", init._passwordGeneratior));



            Console.WriteLine("Please select tool:");
            Console.WriteLine("");
            Console.WriteLine("1. Password generation");


            try
            {

                char _tool = Console.ReadKey().KeyChar;
                switch (_tool)
                {
                    case (char)49:
                        Console.WriteLine();
                        Console.WriteLine("------------------------------------");
                        Console.WriteLine(init._passwordGeneratior.GetPassword());
                        Console.WriteLine("------------------------------------");
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


    }

    class Init
    {
        public IPasswordGenerator _passwordGeneratior;
        public Init(IPasswordGenerator _passwordGeneratior)
        {
            this._passwordGeneratior = _passwordGeneratior;
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
