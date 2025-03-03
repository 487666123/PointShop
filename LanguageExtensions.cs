namespace PointShop;

public static class LanguageHelper
{
    public static LocalizedText GetTextByPointShop(string key)
    {
        return Language.GetText($"Mods.PointShop.{key}");
    }
}