using System;

namespace MobileJO.Core.Contracts
{
    public interface IDirectory
    {
        string CreateDirectory(string directoryName);
        void OpenFile(string fileUri);
    }
}