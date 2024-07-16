using System;
using System.Collections.Generic;
using NAudio.Wave;

namespace MediaPortal.MVVM.Model.FileReaders.MusicFileReaders
{
    internal class WavFileReader
    {
        public static (Dictionary<string, string> metadata, TimeSpan duration)? ReadWavFile(string filePath)
        {
            Dictionary<string, string> metadata = new Dictionary<string, string>();
            TimeSpan duration = TimeSpan.Zero;

            try
            {
                // Use NAudio to get duration
                using (var audioFileReader = new WaveFileReader(filePath))
                {
                    duration = audioFileReader.TotalTime;
                }

                // Log duration
                Console.WriteLine($"Duration: {duration}");

                return (metadata, duration);
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., file not found, invalid file format, etc.)
                Console.WriteLine($"Error reading WAV file: {ex.Message}");
                return null;
            }
        }
    }
}
