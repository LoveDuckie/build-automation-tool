using System.Runtime.CompilerServices;

namespace BuildAutomationTool.Interfaces;

public interface IAwaitable<out TResult>
{
    IAwaiter<TResult> GetAwaiter();
}

