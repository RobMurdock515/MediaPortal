using System;
using System.Collections.Generic;
using TagLib;

namespace MediaPortal.MVVM.Model.FileReaders.MusicFileReaders
{
    internal class FlacFileReader
    {
        public static (Dictionary<string, string> metadata, TimeSpan duration)? ReadFlacFile(string filePath)
        {
            Dictionary<string, string> metadata = new Dictionary<string, string>();
            TimeSpan duration = TimeSpan.Zero;

            try
            {
                // Use TagLib# to read metadata
                using (var tagLibFile = TagLib.File.Create(filePath))
                {
                    metadata["Title"] = tagLibFile.Tag.Title;
                    metadata["Artist"] = tagLibFile.Tag.FirstPerformer;
                    metadata["Album"] = tagLibFile.Tag.Album;
                    metadata["Year"] = tagLibFile.Tag.Year.ToString();
                    // Add more metadata fields as needed
                }

                // Log metadata
                Console.WriteLine($"Title: {metadata["Title"]}");
                Console.WriteLine($"Artist: {metadata["Artist"]}");
                Console.WriteLine($"Album: {metadata["Album"]}");
                Console.WriteLine($"Year: {metadata["Year"]}");
                // Log duration if available
                Console.WriteLine($"Duration: {duration}");

                return (metadata, duration);
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., file not found, invalid file format, etc.)
                Console.WriteLine($"Error reading FLAC file: {ex.Message}");
                return null;
            }
        }
    }
}
