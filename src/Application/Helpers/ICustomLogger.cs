using System;

namespace JMS.Application.Helpers;

public interface ICustomLogger : IDisposable
{
    void Log(string message);
    void Log(string message, Exception? e);
}
