using System;

namespace NS_Shaft_Mono
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new DYMono.DYMonoApp())
                game.Run();
        }
    }
}
