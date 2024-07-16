using System;
using System.Collections.Generic;
using Microsoft.WindowsAPICodePack.Shell;

namespace MediaPortal.MVVM.Model.FileReaders.VideoFileReaders
{
    public class MovFileReader
    {
        public static Dictionary<string, string> ExtractMetadata(string filePath)
        {
            var metadata = new Dictionary<string, string>();

            try
            {
                // Create ShellFile object for the MOV file
                ShellFile shellFile = ShellFile.FromFilePath(filePath);

                // Access properties to extract metadata
                metadata["Title"] = shellFile.Properties.System.Title.Value;
                metadata["Duration"] = shellFile.Properties.System.Media.Duration.Value.ToString();
                // You can access more properties here as needed

                return metadata;
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine($"Error extracting MOV metadata: {ex.Message}");
                return null;
            }
        }
    }
}
