using System.Text.Json;
using System.Text.Json.Nodes;

namespace Licenses_Test_Winforms_App;

/// <summary>
/// Reads/writes optional keys in <c>licenpro.settings.json</c> next to the app without removing SDK secrets.
/// </summary>
internal static class LicenProLocalSettings
{
    public static string SettingsFilePath =>
        Path.Combine(AppContext.BaseDirectory, "licenpro.settings.json");

    public static bool ReadAutoCheckProductUpdates()
    {
        try
        {
            if (!File.Exists(SettingsFilePath))
                return true;
            using var doc = JsonDocument.Parse(File.ReadAllText(SettingsFilePath));
            if (doc.RootElement.TryGetProperty("AutoCheckProductUpdates", out var p) &&
                p.ValueKind == JsonValueKind.False)
                return false;
            return true;
        }
        catch
        {
            return true;
        }
    }

    public static void WriteAutoCheckProductUpdates(bool enabled)
    {
        try
        {
            JsonObject root;
            if (File.Exists(SettingsFilePath))
            {
                var text = File.ReadAllText(SettingsFilePath);
                root = string.IsNullOrWhiteSpace(text)
                    ? new JsonObject()
                    : JsonNode.Parse(text)?.AsObject() ?? new JsonObject();
            }
            else
            {
                root = new JsonObject();
            }

            root["AutoCheckProductUpdates"] = enabled;
            var opts = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(SettingsFilePath, root.ToJsonString(opts));
        }
        catch
        {
            /* ignore disk errors in sample app */
        }
    }
}
