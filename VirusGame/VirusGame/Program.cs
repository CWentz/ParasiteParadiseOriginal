using System;

namespace VirusGame
{
#if WINDOWS || XBOX
    static class Program
    {
        static void Main(string[] args)
        {
            using (VirusGameMain game = new VirusGameMain())
            {
                game.Run();
            }
        }
    }
#endif
}

