using ItemTime.ProcessUsage;
using System.Diagnostics;

namespace ItemTime.ProcessUsageApp;
class ProcessViewer
{
    ProcessUsageInfo? pUsageInfo;
    public event Action<string>? WriteHandler = (text) => Console.Write(text);
    public ProcessViewer(Process process) : base()  => pUsageInfo = new ProcessUsageInfo(process);
    private ProcessViewer() => Init();
    private void Init()
    {
        //handlers
    }
    public void WriteUsageInfo(SizeType size = SizeType.MB) => WriteHandler?.Invoke(GetUsageInfo(size));
    public string GetUsageInfo(SizeType size = SizeType.MB)
    {
        string info = $"cpu: {pUsageInfo?.Cpu:f2}%\n";
        info += $"ram: {pUsageInfo?.Ram / (int)size} {size}\n";
        info += $"privated ram: {pUsageInfo?.PrivatedRam / (int)size} {size}\n";
        info += $"handle opened: {pUsageInfo?.HandleCount}\n\n";
        return info;
    }
    
}