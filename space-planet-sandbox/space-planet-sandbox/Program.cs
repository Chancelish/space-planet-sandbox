using System;

namespace space_planet_sandbox
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new SandboxGame())
                game.Run();
        }
    }
}
