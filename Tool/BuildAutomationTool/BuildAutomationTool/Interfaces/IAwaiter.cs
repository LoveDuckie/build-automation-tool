using System.Runtime.CompilerServices;

namespace BuildAutomationTool.Interfaces;

public interface IAwaiter<out TResult> : INotifyCompletion // or ICriticalNotifyCompletion
{
    bool IsCompleted { get; }

    TResult GetResult();
}