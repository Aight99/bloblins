using UnityEngine;

public static class DebugHelper
{
    public enum MessageType
    {
        Movement,
        Yippee
    }

    private static readonly Color Purple = new(0.47f, 0.34f, 0.94f);
    private static readonly Color Yellow = new(0.98f, 0.84f, 0.34f);

    public static void Log(MessageType type, string message)
    {
        string prefix;
        string colorHex;

        switch (type)
        {
            case MessageType.Movement:
                prefix = "ДВИЖЕНИЕ";
                colorHex = ColorUtility.ToHtmlStringRGB(Purple);
                break;
            case MessageType.Yippee:
                prefix = "ЯПИ!";
                colorHex = ColorUtility.ToHtmlStringRGB(Yellow);
                break;
            default:
                prefix = "";
                colorHex = ColorUtility.ToHtmlStringRGB(Color.white);
                break;
        }

        Debug.Log($"<color=#{colorHex}>{prefix}</color>: {message}");
    }

    public static void LogMovement(string message)
    {
        Log(MessageType.Movement, message);
    }

    public static void LogYippee(string message)
    {
        Log(MessageType.Yippee, message);
    }
}
