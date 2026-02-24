namespace PointShop;

/// <summary>
/// 弱事件管理器。
/// 使用方需要在游戏更新循环中持续调用 <see cref="Update"/>，以便定期清理失效引用。
/// </summary>
public class WeakEventManager<TEventArgs> where TEventArgs : EventArgs
{
    /// <summary>
    /// 清理间隔（毫秒）。
    /// </summary>
    public double CleanupIntervalMilliseconds { get; set; } = 5000D;

    /// <summary>
    /// 是否需要在下一次更新时执行清理。
    /// </summary>
    private bool _needsCleanup = false;

    /// <summary>
    /// 上次清理发生的时间（毫秒）。
    /// </summary>
    private double _lastCleanupTimeMilliseconds = 0;

    /// <summary>
    /// 事件处理器的弱引用集合。
    /// </summary>
    private readonly List<WeakReference<EventHandler<TEventArgs>>> _eventBindings = [];

    /// <summary>
    /// 添加事件处理器。
    /// </summary>
    /// <param name="source">处理器持有者，用于维持处理器的强引用。</param>
    /// <param name="handler">要添加的处理器。</param>
    public void AddHandler(IEventHandlerHolder source, EventHandler<TEventArgs> handler)
    {
        ArgumentNullException.ThrowIfNull(source);

        source.ActiveHandlers.Add(handler);
        _eventBindings.Add(new WeakReference<EventHandler<TEventArgs>>(handler));
    }

    /// <summary>
    /// 移除事件处理器，并清理已失效的引用。
    /// </summary>
    /// <param name="source">处理器持有者。</param>
    /// <param name="handler">要移除的处理器。</param>
    public void RemoveHandler(IEventHandlerHolder source, EventHandler<TEventArgs> handler)
    {
        ArgumentNullException.ThrowIfNull(source);

        source.ActiveHandlers.Remove(handler);
        _eventBindings.RemoveAll(b => !b.TryGetTarget(out var h) || h == handler);
    }

    /// <summary>
    /// 触发事件并通知当前存活的处理器。
    /// </summary>
    /// <param name="eventArgs">事件参数。</param>
    public void Raise(TEventArgs eventArgs)
    {
        foreach (var handlerRef in _eventBindings)
        {
            if (handlerRef.TryGetTarget(out var handler))
            {
                handler.Invoke(this, eventArgs);
            }
            else
            {
                _needsCleanup = true;
            }
        }
    }

    /// <summary>
    /// 在更新循环中执行周期清理。
    /// </summary>
    /// <param name="gameTime">当前游戏时间。</param>
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

    /// <summary>
    /// 清理所有已失效的处理器引用。
    /// </summary>
    private void Cleanup() => _eventBindings.RemoveAll(handlerRef => !handlerRef.TryGetTarget(out _));
}

/// <summary>
/// 事件处理器持有者。
/// 用于通过强引用托管处理器，避免过早被 GC 回收。
/// </summary>
public interface IEventHandlerHolder
{
    /// <summary>
    /// 用于保持事件处理器强引用的列表。
    /// </summary>
    List<object> ActiveHandlers { get; }
}

/// <summary>
/// 泛型事件参数包装器。
/// </summary>
/// <typeparam name="T">值类型。</typeparam>
public class EventArgs<T>(T value) : EventArgs
{
    /// <summary>
    /// 事件携带的值。
    /// </summary>
    public T Value { get; } = value;
}
