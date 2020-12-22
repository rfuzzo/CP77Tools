﻿using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using Catel.IoC;
using WolvenKit.Common.Model;
using WolvenKit.Common.Services;

namespace WolvenKit.Common.Tools.Audio
{
    public enum EAudioExtension
    {
        wem,
        ogg, 
        wav, 
        mp3, 
        aac, 
        flac 
    }

    public static class AudioconvWrapper
    {

        public static string Convert(string outDir, string filepath, EAudioExtension filetype)
        {
            string ffmpegpath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "avconv/ffmpeg.exe");
            var logger = ServiceLocator.Default.ResolveType<ILoggerService>();

            var filename = Path.Combine(outDir, $"{Path.GetFileNameWithoutExtension(filepath)}.{filetype}");

            var proc = new ProcessStartInfo(ffmpegpath)
            {
                WorkingDirectory = Path.GetDirectoryName(ffmpegpath),
                Arguments = $" -i \"{filepath}\" \"{filename}\"",
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
