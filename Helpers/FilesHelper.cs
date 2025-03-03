using System.Text;
using YamlDotNet.Serialization;

namespace PointShop.Helpers;

public static class FilesHelper
{
    public static string ShopDataPath => "PointShop/ShopData/data.yaml";

    public static IDeserializer YAMLDeserializer { get; } = new DeserializerBuilder().Build();

    /// <summary>
    /// 获取 MOD 内文件的字符串
    /// </summary>
    public static string GetString(string path) =>
        Encoding.UTF8.GetString(ModContent.GetFileBytes(path));
}