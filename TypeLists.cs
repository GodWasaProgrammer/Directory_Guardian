namespace DirectoryGuardian;

public static class TypeLists
{
    private static readonly Dictionary<SortTypes, List<string>> _ExtensionsMap = new()
    {
        { SortTypes.Videos, new List<string>() {".mp4", ".avi", ".mkv", ".mov", ".wmv", ".flv", ".webm", ".vob", ".ogv", ".3gp", ".3g2", ".m4v", ".f4v", ".f4p", ".f4a", ".f4b", ".mpeg", ".mpg", ".m2ts", ".mts", ".ts", ".ogm", ".divx", ".xvid", ".rmvb", ".rm", ".asf", ".amv", ".svi", ".drc", ".mxf", ".gxf" } },
        { SortTypes.Images, new List<string>(){ ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".tif", ".webp", ".svg", ".ico", ".heic", ".heif", ".raw", ".psd", ".ai", ".eps", ".indd", ".cdr", ".dwg", ".dxf", ".xcf", ".bmp", ".dib", ".rle", ".pict", ".pct", ".pic", ".tga", ".icns", ".jfif", ".jp2", ".jpx", ".j2k", ".j2c", ".hdp", ".wdp", ".jxr" } },
        { SortTypes.Audio, new List<string>(){ ".mp3", ".wav", ".flac", ".aac", ".m4a", ".aiff", ".ape", ".ogg", ".oga", ".wma", ".cda", ".ac3", ".dts", ".mka", ".m3u", ".pls", ".wpl", ".alac", ".wv", ".cda", ".aiff", ".m4a", ".m4b", ".tta", ".flac", ".ape", ".wv", ".dts", ".mka", ".wv", ".tta", ".dsf", ".ra", ".rm", ".ram", ".cda", ".dts", ".mka", ".wv", ".tta", ".ape", ".m4a", ".m4b", ".tta", ".flac", ".ape", ".wv", ".dts", ".mka", ".wv", ".tta", ".ds" } },
        { SortTypes.Executables, new List<string>() { ".exe", ".bat", ".sh", ".app", ".msi", ".com", ".bin", ".cmd", ".wsf", ".vbs", ".ps1", ".jar", ".scr", ".run", ".apk", ".gadget", ".reg", ".cgi", ".cpl", ".air", ".ksh", ".inf", ".msc" } },
        { SortTypes.Archives, new List<string>(){ ".zip", ".rar", ".7z", ".tar", ".gz", ".xz", ".bz2", ".cab", ".iso", ".dmg", ".ova", ".wim", ".xar", ".cpio", ".arj", ".lha", ".rz", ".z", ".jar", ".war", ".ear", ".ear.zip", ".apk", ".ipa", ".deb", ".rpm", ".msi", ".pkg", ".nupkg", ".msi", ".msp", ".cab", ".wim", ".xar", ".cpio", ".arj", ".lha", ".rz", ".z", ".jar", ".war", ".ear", ".ear.zip", ".apk", ".ipa", ".deb", ".rpm", ".msi", ".pkg", ".nupkg", ".msi", ".msp", ".cab", ".wim", ".xar" } },
        { SortTypes.Documents, new List<string>(){ ".txt", ".doc", ".docx", ".pdf", ".rtf", ".xls", ".xlsx", ".ppt", ".pptx", ".odt", ".ods", ".odp", ".epub", ".mobi", ".azw", ".azw3", ".epub", ".mobi", ".azw", ".azw3", ".epub", ".mobi", ".azw", ".azw" } },
    };
    public static Dictionary<SortTypes, List<string>> ExtensionsMap { get { return _ExtensionsMap; } }
}