namespace PointShop.UserInterfaces.Components;

/// <summary>
/// 提供彩虹文本的颜色插值能力，根据归一化进度返回对应的渐变颜色。
/// </summary>
class RainbowTextEffect
{
    /// <summary>
    /// 按顺序定义彩虹渐变的关键颜色列表，至少包含一个颜色。
    /// </summary>
    /// <exception cref="ArgumentException">当颜色列表为空时抛出。</exception>
    public required Color[] RainbowColors
    {
        get; init
        {
            if (value.Length < 1)
                throw new ArgumentException("颜色数量不能小于一。");
            field = value;
        }
    }

    /// <summary>
    /// 区间钳制插值算法：在 <c>[0, 1]</c> 区间内从首色过渡到尾色，超过区间会被钳制。
    /// </summary>
    /// <param name="amount">归一化进度。有效区间为 <c>[0, 1]</c>，超出范围会自动钳制。</param>
    /// <returns>按当前进度计算得到的插值颜色。</returns>
    public Color GetColorClamped(float amount)
    {
        var colors = RainbowColors.AsSpan();

        if (colors.Length == 1)
            return colors[0];

        amount = MathHelper.Clamp(amount, 0f, 1f);

        var maxIndex = colors.Length - 1;

        var scaled = amount * maxIndex;
        var startIndex = (int)scaled;
        var endIndex = startIndex + 1;

        // 到达边界时，直接返回最后一个颜色，避免访问越界。
        if (endIndex > maxIndex) return colors[maxIndex];

        return Color.Lerp(colors[startIndex], colors[endIndex], scaled - startIndex);
    }

    /// <summary>
    /// 循环闭环插值算法：进度会按周期循环，并在末尾自动衔接回首色。
    /// </summary>
    /// <param name="amount">归一化进度。任意实数都可输入，会先折算到 <c>[0, 1)</c>。</param>
    /// <returns>按循环进度计算得到的插值颜色。</returns>
    public Color GetColorLooped(float amount)
    {
        var colors = RainbowColors.AsSpan();

        if (colors.Length == 1)
            return colors[0];

        amount -= MathF.Floor(amount);

        var colorCount = colors.Length;
        var scaled = amount * colorCount;
        var startIndex = (int)scaled;
        var endIndex = (startIndex + 1) % colorCount;

        return Color.Lerp(colors[startIndex], colors[endIndex], scaled - startIndex);
    }

    /// <summary>
    /// 默认彩虹效果配置（用于区间钳制算法），按赤橙黄绿青蓝紫排列，
    /// 并在末尾回到赤色以形成首尾视觉连续。
    /// </summary>
    public static RainbowTextEffect Default => new()
    {
        RainbowColors = [
            new(255, 0, 0),    // 赤
            new(255, 127, 0),  // 橙
            new(255, 255, 0),  // 黄
            new(0, 255, 0),    // 绿
            new(0, 255, 255),  // 青
            new(0, 0, 255),    // 蓝
            new(148, 0, 211),  // 紫
            new(255, 0, 0),    // 赤（末尾重复以保证视觉连续）
        ]
    };

    /// <summary>
    /// 闭环算法推荐配置（用于 <see cref="GetColorLooped(float)"/>），
    /// 不需要在末尾重复第一个颜色。
    /// </summary>
    public static RainbowTextEffect LoopedDefault => new()
    {
        RainbowColors = [
            new(255, 0, 0),    // 赤
            new(255, 127, 0),  // 橙
            new(255, 255, 0),  // 黄
            new(0, 255, 0),    // 绿
            new(0, 255, 255),  // 青
            new(0, 0, 255),    // 蓝
            new(148, 0, 211),  // 紫
        ]
    };
}
