using SilkyUIFramework.Attributes;

namespace PointShop.UserInterfaces;

[XmlElementMapping("CommonHeader")]
public partial class SUICommonHeader : SUIDraggableView
{
    public SUICommonHeader() : base()
    {
        InitializeComponent();
        InitializeComponent2();
    }

    public SUICommonHeader(UIElementGroup group, string name) : base(group)
    {
        InitializeComponent();
        InitializeComponent2();

        Title.Text = name;
    }

    private void InitializeComponent2()
    {
        Title.UseDeathText();

        CloseButton.CrossBorderColor = SUIColor.Border * 0.75f;
        CloseButton.CrossBackgroundColor = SUIColor.Warn * 0.75f;
        CloseButton.CrossBorderHoverColor = SUIColor.Highlight;
        CloseButton.CrossBackgroundHoverColor = SUIColor.Warn;
        CloseButton.LeftMouseDown += delegate { PointShopUI.ShowUI = false; };
    }
}


//public interface IUpdateTask
//{
//    bool IsCompleted { get; }

//    event UpdateTaskHandler OnUpdate;

//    void Update(GameTime gameTime);
//}

//public interface IUpdateTaskManager
//{
//    void Add(IUpdateTask updateTask);

//    void Remove(IUpdateTask updateTask);

//    void Update(GameTime gameTime);
//}


//public delegate void UpdateTaskHandler(IUpdateTask sender);