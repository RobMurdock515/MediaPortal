namespace MediaPortal.MVVM.Model
{
    /// <summary>
    /// Represents user settings related to media library locations.
    /// </summary>
    public class UserSettings
    {
        /// <summary>
        /// Gets or sets the location of the music library.
        /// </summary>
        public string MusicLibraryLocation { get; set; }

        /// <summary>
        /// Gets or sets the location of the video library.
        /// </summary>
        public string VideoLibraryLocation { get; set; }
    }
}
