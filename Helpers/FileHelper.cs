using System.Text;
using YamlDotNet.Serialization;

namespace PointShop.Helpers;

public static class FileHelper
{
    public static string ShopDataPath => "PointShop/ShopData/data.yaml";

    public static IDeserializer YamlDeserializer { get; } = new DeserializerBuilder().Build();

    /// <summary>
    /// 获取 MOD 内文件的字符串
    /// </summary>
    public static string GetString(string path) => Encoding.UTF8.GetString(ModContent.GetFileBytes(path));
}