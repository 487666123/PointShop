using System.Text;
using YamlDotNet.Serialization;

namespace PointShop.Helpers;

public static class FileHelper
{
    /// <summary>
    /// 商店数据路径
    /// </summary>
    public static string ShopDataPath => "PointShop/ShopData/data.yaml";

    /// <summary>
    /// 捐赠者数据路径
    /// </summary>
    public static string DonorDataPath => "PointShop/ShopData/DonorData.yaml";

    // Keep this for compatibility with existing call sites.
    public static IDeserializer YamlDeserializer { get; } = new DeserializerBuilder().Build();
    public static IResourceReader ResourceReader { get; } = new ModResourceReader();

    /// <summary>
    /// 获取 MOD 内文件的字符串
    /// </summary>
    public static string GetString(string path) => ResourceReader.ReadString(path);

    /// <summary>
    /// 将 YAML 字符串反序列化为指定类型
    /// </summary>
    public static T DeserializeYaml<T>(string yaml) => YamlDeserializer.Deserialize<T>(yaml);
}

internal sealed class ModResourceReader : IResourceReader
{
    public string ReadString(string path) => Encoding.UTF8.GetString(ModContent.GetFileBytes(path)).TrimStart('\uFEFF');
}

public interface IResourceReader
{
    string ReadString(string path);
}
