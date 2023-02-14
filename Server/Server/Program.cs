using GameServer;
using System.Runtime.InteropServices;

public class Program
{
    private static bool _isRunning;

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
    public struct Class
    {
        public int test;
    }

    public static void Main()
    {
        _isRunning = true;

        Thread mainThread = new Thread(new ThreadStart(MainThread));
        mainThread.Start();

        Server.Create(7777, 10);
    }

    private static void MainThread()
    {
        Console.WriteLine($"Main thread started. Running at {30} ticks per second.");
        DateTime _nextLoop = DateTime.Now;

        while (_isRunning)
        {
            while (_nextLoop < DateTime.Now)
            {
                // Update game logic

                _nextLoop = _nextLoop.AddMilliseconds(1000f);

                if (_nextLoop > DateTime.Now)
                {
                    Thread.Sleep(_nextLoop - DateTime.Now);
                }
            }
        }
    }
}