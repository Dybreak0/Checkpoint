using MvvmCross.Logging;
using System;
using System.Diagnostics;
using MobileJO.Core.Utilities;

namespace MobileJO.Core
{
    public class DebugTrace : IMvxLog
    {

        public bool IsLogLevelEnabled(MvxLogLevel logLevel) => true;
        
        //for debug purposes only
        public bool Log(MvxLogLevel logLevel, Func<string> messageFunc, Exception exception = null, params object[] formatParameters)
        {
            Debug.WriteLine(logLevel + Constants.SpecialCharacters.Colon + messageFunc());

            return true;
        }
    }
}