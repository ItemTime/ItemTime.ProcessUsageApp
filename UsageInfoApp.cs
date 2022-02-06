using System.Diagnostics;

namespace ItemTime.ProcessUsageApp;
class UsageInfoApp
{
    Process? process;
    ProcessViewer? pViewer;
    public string? StartPath { get; set; }
    public int Delay { get; set; }
    private ConsoleKey Key => Console.ReadKey(true).Key;
    private UsageInfoApp() 
    {
        //
    }
    public static UsageInfoApp Init() 
    {
        string path = GetStartPath();
        int delay = GetDelay();
        return new UsageInfoApp() {StartPath = path, Delay = delay};
    }
    public void Run()
    {
        process = Process.Start(StartPath!);
        pViewer = new ProcessViewer(process);
        while (true)
        {
            using var cancelSource = new CancellationTokenSource();
            using var waitEnterTask = Task<ConsoleKey>.Run(() => 
            {
                var key = Key;
                cancelSource.Cancel();
                return key;
            });
            do
            {
                pViewer?.WriteUsageInfo();
                try
                {
                    Task.Delay(Delay, cancelSource.Token).GetAwaiter().GetResult();
                } catch {}
            } while (waitEnterTask.Status == TaskStatus.Running);
            if (waitEnterTask.Result == ConsoleKey.Enter)
                break;
        }
        KillOrNotProcess();
        Close();
    }
    private static string GetStartPath()
    {
        System.Console.Write("Write program path> ");
        var path = Console.ReadLine()!;
        if (!File.Exists(path))
        {
            throw new Exception("Path doesn't exist");
        }
        return path;
    }
    private static int GetDelay()
    {
        System.Console.Write("Write delay>");
        float delay = 2.5f;
        float.TryParse(Console.ReadLine()!, out delay);
        return (int)(delay * 1000);
    }
    private void KillOrNotProcess()
    {
        System.Console.WriteLine("Press Enter to stop started process");
        if (Key == ConsoleKey.Enter)
            process?.Kill();
    }
    private void Close()
    {
        System.Console.WriteLine("Press Enter to close program");
        var key = Key;
        if (key == ConsoleKey.Enter) 
        {
            Process.GetCurrentProcess().Kill();
        }
    }
}