using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Console = Colorful.Console;

namespace DiscordBanDetector
{
    class Visuals
    {
        public static void WriteLine(string Content, Color color)
        {
            Console.Write("[", Color.LimeGreen);
            Console.Write(DateTime.Now.ToString("HH:mm:ss"), Color.DeepSkyBlue);
            Console.Write("]", Color.LimeGreen);
            Console.Write(" >  ", Color.White);
            Console.WriteLine(Content, color);
        }
        public static void WriteLine()
        {
            Console.Write("[", Color.LimeGreen);
            Console.Write(DateTime.Now.ToString("HH:mm:ss"), Color.DeepSkyBlue);
            Console.Write("]", Color.LimeGreen);
            Console.Write(" >  ", Color.White);
        }
        public static string ReadLine()
        {
            Console.Write("[", Color.LimeGreen);
            Console.Write(DateTime.Now.ToString("HH:mm:ss"), Color.DeepSkyBlue);
            Console.Write("]", Color.LimeGreen);
            Console.Write(" >  ", Color.White);
            return Console.ReadLine();
        }
        public static string ReadLine(string Content, Color color)
        {
            Console.Write("[", Color.LimeGreen);
            Console.Write(DateTime.Now.ToString("HH:mm:ss"), Color.DeepSkyBlue);
            Console.Write("]", Color.LimeGreen);
            Console.Write(" >  ", Color.White);
            Console.Write(Content, color);
            return Console.ReadLine();
        }
        public static void Log(string Content)
        {
            Console.Write("[", Color.LimeGreen);
            Console.Write(DateTime.Now.ToString("HH:mm:ss"), Color.DeepSkyBlue);
            Console.Write("]", Color.LimeGreen);
            Console.Write(" >  ", Color.White);
            Console.WriteLine("[LOG] " + Content, Color.LightBlue);
        }
        public static void Error(string Content)
        {
            Console.Write("[", Color.LimeGreen);
            Console.Write(DateTime.Now.ToString("HH:mm:ss"), Color.DeepSkyBlue);
            Console.Write("]", Color.LimeGreen);
            Console.Write(" >  ", Color.White);
            Console.WriteLine("[ERROR] " + Content, Color.IndianRed);
        }
    }
}
