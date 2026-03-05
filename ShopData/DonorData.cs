namespace PointShop.ShopData;

/// <summary>
/// 捐赠数据文件的根对象。
/// </summary>
internal record DonorData
{
    /// <summary>
    /// 数据更新日期。
    /// </summary>
    public DateOnly UpdateDate { get; init; }

    /// <summary>
    /// 捐赠者列表。
    /// </summary>
    public Donor[] Donors { get; init; } = [];
}

/// <summary>
/// 单个捐赠者信息。
/// </summary>
internal record Donor
{
    /// <summary>
    /// 捐赠者名称。
    /// </summary>
    public string Name { get; init; } = "";

    /// <summary>
    /// 该捐赠者的捐赠记录。
    /// </summary>
    public Donation[] Donations { get; init; } = [];

    /// <summary>
    /// 效果
    /// </summary>
    public string Effect { get; set; } = string.Empty;
}

/// <summary>
/// 单笔捐赠记录。
/// </summary>
internal record Donation
{
    /// <summary>
    /// 捐赠日期。
    /// </summary>
    public DateOnly Date { get; init; }

    /// <summary>
    /// 捐赠金额。
    /// </summary>
    public decimal Amount { get; init; }
}