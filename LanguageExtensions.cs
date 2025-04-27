namespace PointShop;

public static class LanguageHelper
{
    /// <summary>
    /// 获取文本，通过 <see cref="Language.GetText"/> arg0: $"Mods.PointShop.{key}"
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static LocalizedText GetTextByPointShop(string key)
    {
        return Language.GetText($"Mods.PointShop.{key}");
    }
}