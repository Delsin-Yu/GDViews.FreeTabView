using System;
using Godot;
using GodotViews.ViewTweeners;

namespace GodotViews;

/// <summary>
/// Inherit this type to create a view item that's used by <see cref="FreeTabView"/>.
/// </summary>
/// <typeparam name="TOptionalArg">The type for the optional argument.</typeparam>
[GlobalClass]
public abstract partial class FreeTabViewItemT<TOptionalArg> : Control, IInternalFreeTabViewItem
{
    /// <summary>
    /// The tweener associated to this view item.
    /// </summary>
    protected IViewItemTweener ViewItemTweener
    {
        get => _tweener ?? NoneViewItemTweener.Instance;
        set => _tweener = value;
    }

    private string? _cachedName;
    private IViewItemTweener? _tweener;
    private object? _tweenerData;

    internal string LocalName => _cachedName ??= Name;

    /// <summary>
    /// Called when the <see cref="FreeTabView"/> is initializing the view item.
    /// </summary>
    /// <remarks>
    /// This method is considered "Protected", that is, throwing an exception inside the override of this method will not cause the component to malfunction.
    /// </remarks>
    protected virtual void _OnViewItemInitialize()
    {
    }

    /// <summary>
    /// Called when the <see cref="FreeTabView"/> is showing the view item.
    /// </summary>
    /// <param name="optionalArg">An optional argument passes to the view item.</param>
    /// <remarks>
    /// This method is considered "Protected", that is, throwing an exception inside the override of this method will not cause the component to malfunction.
    /// </remarks>
    protected virtual void _OnViewItemShow(TOptionalArg? optionalArg)
    {
    }

    /// <summary>
    /// Called when the <see cref="FreeTabView"/> is hiding the view item.
    /// </summary>
    /// <remarks>
    /// This method is considered "Protected", that is, throwing an exception inside the override of this method will not cause the component to malfunction.
    /// </remarks>
    protected virtual void _OnViewItemHide()
    {
    }

    /// <inheritdoc cref="GodotObject._Notification"/>
    /// <remarks>
    /// This method is considered "Protected", that is, throwing an exception inside the override of this method will not cause the component to malfunction.
    /// </remarks>
    protected virtual void _OnViewItemNotification(int what)
    {
    }

    /// <summary>
    /// Called when Godot is deleting the view item (<see cref="GodotObject.NotificationPredelete"/>).
    /// </summary>
    /// <remarks>
    /// This method is considered "Protected", that is, throwing an exception inside the override of this method will not cause the component to malfunction.
    /// </remarks>
    protected virtual void _OnViewItemPredelete()
    {
    }

    /// <summary>
    /// Override <see cref="_OnViewItemNotification"/> instead, this method is used for raising <see cref="_OnViewItemPredelete"/> at the appropriate time.
    /// </summary>
    public sealed override void _Notification(int what)
    {
        base._Notification(what);

        DelegateRunner.RunProtected(_OnViewItemNotification, what, "View Notification", LocalName);

        if (what != NotificationPredelete) return;

        DelegateRunner.RunProtected(_OnViewItemPredelete, "Delete View Item", LocalName);
    }

    void IInternalFreeTabViewItem.ShowViewItem(object? optionalArg)
    {
        ViewItemTweener.Show(this, _tweenerData);
        if (optionalArg == null)
        {
            DelegateRunner.RunProtected<TOptionalArg?>(_OnViewItemShow, default, "Show View Item", LocalName);
            return;
        }

        if (optionalArg is not TOptionalArg typedOpenArg)
        {
            throw new InvalidCastException($"Unable to cast from {optionalArg.GetType()} to {typeof(TOptionalArg)}");
        }

        DelegateRunner.RunProtected<TOptionalArg?>(_OnViewItemShow, typedOpenArg, "Show View Item", LocalName);
    }

    void IInternalFreeTabViewItem.HideViewItem()
    {
        ViewItemTweener.Hide(this, _tweenerData);
        DelegateRunner.RunProtected(_OnViewItemHide, "Hide View Item", LocalName);
    }

    void IInternalFreeTabViewItem.InitializeViewItem()
    {
        DelegateRunner.RunProtected(_OnViewItemInitialize, "View Item Initialization", LocalName);
        ViewItemTweener.Init(this, ref _tweenerData);
    }
}