namespace DirectoryGuardian;

public class TypeLists
{
    private static List<string> _VideoExtensions = [".mp4", ".avi", ".mkv", ".mov", ".wmv", ".flv", ".webm", ".vob", ".ogv", ".3gp", ".3g2", ".m4v", ".f4v", ".f4p", ".f4a", ".f4b", ".mpeg", ".mpg", ".m2ts", ".mts", ".ts", ".ogm", ".divx", ".xvid", ".rmvb", ".rm", ".asf", ".amv", ".svi", ".drc", ".mxf", ".gxf"];
    public static List<string> VideoExtensions { get { return _VideoExtensions; } }

    private static List<string> _ImageExtensions = [".jpg", ".jpeg", ".png", ".gif", ".bmp", ".tiff", ".tif", ".webp", ".svg", ".ico", ".heic", ".heif", ".raw", ".psd", ".ai", ".eps", ".pdf", ".indd", ".cdr", ".dwg", ".dxf", ".xcf", ".bmp", ".dib", ".rle", ".pict", ".pct", ".pic", ".tga", ".icns", ".jfif", ".jp2", ".jpx", ".j2k", ".j2c", ".hdp", ".wdp", ".jxr"];
    public static List<string> ImageExtensions { get { return _ImageExtensions; } }

    private static List<string> _AudioExtensions = [".mp3", ".wav", ".flac", ".aac", ".m4a", ".aiff", ".ape", ".ogg", ".oga", ".wma", ".cda", ".ac3", ".dts", ".mka", ".m3u", ".pls", ".wpl", ".alac", ".wv", ".cda", ".aiff", ".m4a", ".m4b", ".tta", ".flac", ".ape", ".wv", ".dts", ".mka", ".wv", ".tta", ".dsf", ".ra", ".rm", ".ram", ".cda", ".dts", ".mka", ".wv", ".tta", ".ape", ".m4a", ".m4b", ".tta", ".flac", ".ape", ".wv", ".dts", ".mka", ".wv", ".tta", ".ds"];
    public static List<string> AudioExtensions { get { return _AudioExtensions; } }

    private static List<string> _DocumentExtensions = [".txt", ".doc", ".docx", ".pdf", ".rtf", ".xls", ".xlsx", ".ppt", ".pptx", ".odt", ".ods", ".odp", ".epub", ".mobi", ".azw", ".azw3", ".epub", ".mobi", ".azw", ".azw3", ".epub", ".mobi", ".azw", ".azw"];
    public static List<string> DocumentExtensions { get { return _DocumentExtensions; } }

    private static List<string> _ArchiveExtensions = [".zip", ".rar", ".7z", ".tar", ".gz", ".xz", ".bz2", ".cab", ".iso", ".dmg", ".ova", ".wim", ".xar", ".cpio", ".arj", ".lha", ".rz", ".z", ".jar", ".war", ".ear", ".ear.zip", ".apk", ".ipa", ".deb", ".rpm", ".msi", ".pkg", ".nupkg", ".msi", ".msp", ".cab", ".wim", ".xar", ".cpio", ".arj", ".lha", ".rz", ".z", ".jar", ".war", ".ear", ".ear.zip", ".apk", ".ipa", ".deb", ".rpm", ".msi", ".pkg", ".nupkg", ".msi", ".msp", ".cab", ".wim", ".xar"];
    public static List<string> ArchiveExtensions { get { return _ArchiveExtensions; } }

    private static List<string> _Executables = [".exe", ".bat", ".sh", ".app", ".msi", ".com", ".bin", ".cmd", ".wsf", ".vbs", ".ps1", ".jar", ".scr", ".run", ".apk", ".gadget", ".reg", ".cgi", ".cpl", ".air", ".ksh", ".inf", ".msc"];
    public static List<string> ExecutableExtensions { get { return _Executables; } }

    private static List<List<string>> _TypeList = [_VideoExtensions, _ImageExtensions, _AudioExtensions, _DocumentExtensions, _ArchiveExtensions, _Executables];
    public static List<List<string>> TypeList { get { return _TypeList; } }
}