using System;

namespace Labb3_Datorgrafik
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Labb3())
                game.Run();
        }
    }
}
