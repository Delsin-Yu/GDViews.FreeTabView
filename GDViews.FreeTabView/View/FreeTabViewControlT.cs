using System;
using Godot;
using GodotViews.ViewTweeners;

namespace GodotViews;

[GlobalClass]
public abstract partial class FreeTabViewControlT<TOptionalArg> : Control, IInternalFreeTabViewControl
{
    protected IViewTweener Tweener
    {
        get => _tweener ?? NoneViewTweener.Instance;
        set => _tweener = value;
    }

    private string? _cachedName;
    private IViewTweener? _tweener;
    
    internal string LocalName => _cachedName ??= Name;

    protected virtual void _OnViewInitialize()
    {
    }

    protected virtual void _OnViewNotification(int what)
    {
    }
    
    protected virtual void _OnViewPredelete()
    {
    }

    protected virtual void _OnViewShow(TOptionalArg? optionalArg)
    {
        
    }

    protected virtual void _OnViewHide()
    {
    }

    /// <summary>
    /// Override <see cref="_OnViewNotification"/> instead, this method is used for raising <see cref="_OnViewPredelete"/> at the appropriate time.
    /// </summary>
    public sealed override void _Notification(int what)
    {
        base._Notification(what);
        
        DelegateRunner.RunProtected(_OnViewNotification, what, "View Notification", LocalName);
        
        if (what != NotificationPredelete) return;
        
        DelegateRunner.RunProtected(_OnViewPredelete, "Delete View", LocalName);
    }

    void IInternalFreeTabViewControl.ShowView(object? optionalArg)
    {
        Tweener.Show(this);
        if (optionalArg == null)
        {
            DelegateRunner.RunProtected<TOptionalArg?>(_OnViewShow, default, "Show View", LocalName);
            return;
        }

        if (optionalArg is not TOptionalArg typedOpenArg)
        {
            throw new InvalidCastException($"Unable to cast from {optionalArg.GetType()} to {typeof(TOptionalArg)}");
        }

        DelegateRunner.RunProtected<TOptionalArg?>(_OnViewShow, typedOpenArg, "Show View", LocalName);
    }

    void IInternalFreeTabViewControl.HideView()
    {
        Tweener.Hide(this);
        DelegateRunner.RunProtected(_OnViewHide, "Hide View", LocalName);
    }

    void IInternalFreeTabViewControl.InitializeView()
    {
        Tweener.Init(this);
        DelegateRunner.RunProtected(_OnViewInitialize, "View Initialization", LocalName);
    }
}