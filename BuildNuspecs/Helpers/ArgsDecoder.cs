// Copyright (c) 2021 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;

namespace BuildNuspecs.Helpers
{
    /// <summary>
    /// This decodes the arguments provided
    /// </summary>
    public static class ArgsDecoder
    {


        public static void DecodeArgsAndUpdateSettings(this string[] args, ConsoleOutput consoleOut, Settings settings)
        {
            var debugMode = false;
#if DEBUG
            debugMode = true;
#endif
            string message = null;
            //If there is a argument, then this overrides
            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "U" : 
                        settings.OverwriteCachedVersion = true;
                        message = "Building a NuGet package using Debug code and updating existing cached version";
                        settings.DebugMode = true;
                        break;
                    case "D":
                        settings.DebugMode = true;
                        break;
                    case "R":
                        settings.DebugMode = false;
                        break;
                    default:
                        consoleOut.LogMessage("If you provide an argument it must be D (for Debug), R (for Release) or U (for direct update)", LogLevel.Error);
                        break;
                }
            }
            else
            {
                settings.DebugMode = debugMode;
            }

            if (message == null)
                message = debugMode
                ? "Building a NuGet package using Debug code"
                : "Building a NuGet package using Release code";

            consoleOut.LogMessage(message, LogLevel.Information);

        }

    }
}