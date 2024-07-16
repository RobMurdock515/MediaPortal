using System;
using System.Collections.Generic;
using Microsoft.WindowsAPICodePack.Shell;

namespace MediaPortal.MVVM.Model.FileReaders.VideoFileReaders
{
    internal class MkvFileReader
    {
        public static Dictionary<string, string> ExtractMetadata(string filePath)
        {
            var metadata = new Dictionary<string, string>();

            try
            {
                ShellFile shellFile = ShellFile.FromFilePath(filePath);

                // Extract basic metadata
                metadata["Title"] = shellFile.Properties.System.Title.Value;
                metadata["Duration"] = shellFile.Properties.System.Media.Duration.Value.ToString();
                // Add more metadata fields as needed

                return metadata;
            }
            catch (Exception ex)
            {
                // Handle exception (e.g., file not found, invalid file format, etc.)
                Console.WriteLine($"Error extracting MKV metadata: {ex.Message}");
                return null;
            }
        }
    }
}
