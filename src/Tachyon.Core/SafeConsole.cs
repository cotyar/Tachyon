#region copyright
// -----------------------------------------------------------------------
//  <copyright file="SafeConsole.cs" creator="Bartosz Sypytkowski">
//      Copyright (C) 2018 Bartosz Sypytkowski <b.sypytkowski@gmail.com>
//  </copyright>
// -----------------------------------------------------------------------
#endregion

using System;

namespace Tachyon.Core
{
    static class SafeConsole
    {
        private static readonly object syncRoot = new object();
        private static readonly Action<string> write = Console.Write;
        private static readonly Action<string> writeLine = Console.WriteLine;

        public static void Write(string text, ConsoleColor color = ConsoleColor.White)
        {
            Wrapped(color, text, write);
        }

        public static void WriteLine(string text, ConsoleColor color = ConsoleColor.White)
        {
            Wrapped(color, text, writeLine);
        }

        private static void Wrapped(ConsoleColor color, string text, Action<string> print)
        {
            lock (syncRoot)
            {
                try
                {
                    var previousColor = Console.ForegroundColor;
                    try
                    {
                        Console.ForegroundColor = color;
                    }
                    catch (Exception) { }

                    try
                    {
                        print(text);
                    }
                    finally
                    {
                        try
                        {
                            Console.BackgroundColor = previousColor;
                        }
                        catch (Exception) { }
                    }
                }
                catch (ObjectDisposedException) { /* console already disposed */ }
            }
        }
    }
}