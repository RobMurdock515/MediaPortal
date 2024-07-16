using System;
using System.Collections.Generic;
using MediaToolkit;
using MediaToolkit.Model;
using MediaToolkit.Options;

namespace MediaPortal.MVVM.Model.FileReaders.VideoFileReaders
{
    public class Mp4FileReader
    {
        public static Dictionary<string, string> ExtractMetadata(string filePath)
        {
            var metadata = new Dictionary<string, string>();

            try
            {
                using (var engine = new Engine())
                {
                    var inputFile = new MediaFile { Filename = filePath };
                    engine.GetMetadata(inputFile);

                    // Access metadata properties from the MediaFile object
                    metadata["Title"] = inputFile.Metadata.Duration.ToString();
                    // Add more metadata fields as needed

                    return metadata;
                }
            }
            catch (Exception ex)
            {
                // Handle exception
                Console.WriteLine($"Error extracting MP4 metadata: {ex.Message}");
                return null;
            }
        }
    }
}
