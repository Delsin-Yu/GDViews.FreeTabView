namespace GodotViews.FreeTab;

/// <summary>
/// Use to represent a view item instance inheriting <see cref="FreeTabViewItemT{TOptionalArg}"/> or <see cref="FreeTabViewItem"/>.
/// </summary>
public interface IFreeTabViewItem;

internal interface IInternalFreeTabViewItem : IFreeTabViewItem
{
    public void InitializeViewItem();
    public void ShowViewItem(object? optionalArg);
    public void HideViewItem();
}