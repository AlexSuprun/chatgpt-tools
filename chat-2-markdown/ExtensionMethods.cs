namespace ChatToMarkdown;

public static class ExtensionMethods
{
    public static DateTime? ToDateTime(this double? valueInMilliseconds)
    {
        return valueInMilliseconds.HasValue ?
            DateTimeOffset.FromUnixTimeMilliseconds((long)valueInMilliseconds * 1000).DateTime:
            default(DateTime?) ;
    }
}