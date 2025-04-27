namespace PointShop;

/// <summary>
/// 如果你要使用, 就必须将 Update 放在一个循环中
/// </summary>
public class WeakEventManager<TEventArgs> where TEventArgs : EventArgs
{
    public double CleanupIntervalMilliseconds { get; set; } = 5000D;

    private bool _needsCleanup = false;
    private double _lastCleanupTimeMilliseconds = 0;

    // 使用元组同时保存弱引用和事件源
    private readonly List<WeakReference<EventHandler<TEventArgs>>> _eventBindings = [];

    /// <summary>
    /// 订阅事件（必须通过此方法添加）
    /// </summary>
    public void AddHandler(IEventHandlerHolder source, EventHandler<TEventArgs> handler)
    {
        ArgumentNullException.ThrowIfNull(source);
        source.ActiveHandlers.Add(handler);
        _eventBindings.Add(new WeakReference<EventHandler<TEventArgs>>(handler));
    }

    /// <summary>
    /// 取消订阅（自动清理）
    /// </summary>
    public void RemoveHandler(IEventHandlerHolder source, EventHandler<TEventArgs> handler)
    {
        if (source == null) return;
        source.ActiveHandlers.Remove(handler);
        _eventBindings.RemoveAll(b => !b.TryGetTarget(out var h) || h == handler);
    }

    public void Raise(TEventArgs eventArgs)
    {
        foreach (var handlerRef in _eventBindings)
        {
            if (handlerRef.TryGetTarget(out var handler)) // 确保源对象存活
            {
                handler.Invoke(this, eventArgs);
            }
            else
            {
                _needsCleanup = true;
            }
        }
    }

    public void Update(GameTime gameTime)
    {
        var currentTime = gameTime.TotalGameTime.TotalMilliseconds;

        if (_needsCleanup || currentTime - _lastCleanupTimeMilliseconds >= CleanupIntervalMilliseconds)
        {
            Cleanup();
            _needsCleanup = false;
            _lastCleanupTimeMilliseconds = currentTime;
        }
    }

    private void Cleanup() => _eventBindings.RemoveAll(handlerRef => !handlerRef.TryGetTarget(out _));
}

public interface IEventHandlerHolder
{
    /// <summary>
    /// 用于保持事件处理程序引用的列表
    /// </summary>
    List<object> ActiveHandlers { get; }
}

public class EventArgs<T>(T value) : EventArgs
{
    public T Value { get; } = value;
}