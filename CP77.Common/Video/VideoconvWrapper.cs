﻿using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Catel.IoC;
using WolvenKit.Common.Model;
using WolvenKit.Common.Services;

namespace WolvenKit.Common.Tools.Video
{
    public enum EVideoExtension
    {
        bik,
        mp4,
        avi
    }

    public static class AudioconvWrapper
    {
        public static string Convert(string outDir, string filepath, EVideoExtension filetype)
        {
            string textconvpath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "avconv/ffmpeg.exe");
            var logger = ServiceLocator.Default.ResolveType<ILoggerService>();

            var filename = Path.Combine(outDir, $"{Path.GetFileNameWithoutExtension(filepath)}.{filetype}");

            var proc = new ProcessStartInfo(textconvpath)
            {
                WorkingDirectory = Path.GetDirectoryName(textconvpath),
                Arguments = $" -i \"{filepath}\" \"{outDir}\"",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true,
            };

            using (var p = Process.Start(proc))
            {
                p.WaitForExit();
            }

            var fi = new FileInfo(filename);
            if (!fi.Exists)
            {
                logger.LogString($"Could not convert {fi.FullName}.", Logtype.Error);
            }

            return fi.FullName;
        }
    }
}
