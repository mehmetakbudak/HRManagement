using System.ComponentModel;

namespace Chinook.Storage.Enums
{
    public enum SourceType
    {
        [Description("Yazar Yazıları")]
        Article = 1,
        [Description("Blog")]
        Blog,
        [Description("Foto Galeri")]
        PhotoGallery,
        [Description("Video Galeri")]
        VideoGallery,
        [Description("Kariyer Fırsatları")]
        Job
    }
}
