namespace GodotViews.FreeTab;

/// <summary>
/// Inherit this type to create a view item that's used by <see cref="FreeTabView"/>,
/// this variant does not have optional argument support.
/// </summary>
public abstract partial class FreeTabViewItem : FreeTabViewItemT<object>
{
    /// <summary>
    /// Override the parameterless version <see cref="_OnViewShow()"/> instead.
    /// </summary>
    protected sealed override void _OnViewItemShow(object? optionalArg) => _OnViewShow();

    /// <summary>
    /// Called when the <see cref="FreeTabView"/> is showing the view item.
    /// </summary>
    /// <remarks>
    /// This method is considered "Protected", that is, throwing an exception inside the override of this method will not cause the component to malfunction.
    /// </remarks>
    protected virtual void _OnViewShow()
    {
    }
}