using YamlDotNet.Serialization;

namespace PointShop.Helpers;

public class FileHelper
{
    public static IDeserializer Deserializer { get; } = new DeserializerBuilder().Build();
}