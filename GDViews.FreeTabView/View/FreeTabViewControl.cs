namespace GodotViews;

public abstract partial class FreeTabViewControl : FreeTabViewControlT<object>
{
    protected sealed override void _OnViewShow(object? optionalArg) => _OnViewShow();

    protected virtual void _OnViewShow()
    {
    }
}