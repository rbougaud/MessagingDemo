namespace Infrastructure.Common;

public static class UserContext
{
    private static AsyncLocal<string> _currentUserId = new();

    public static string CurrentUserId
    {
        get => _currentUserId.Value ?? string.Empty;
        set => _currentUserId.Value = value;
    }
}
