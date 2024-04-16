using System;
using Godot;

namespace GodotViews.FreeTab;

/// <summary>
/// Defines a Tab/Prefab setup.
/// </summary>
/// <param name="TabButton">The check button that's used as a tab.</param>
/// <param name="ViewItemPrefab">The packed scene containing a script inherits the <see cref="FreeTabViewItemT{TOptionalArg}"/> or <see cref="FreeTabViewItem"/> type.</param>
public record struct TabPrefabSetup(CheckButton TabButton, PackedScene ViewItemPrefab);

/// <summary>
/// Defines a Tab/ViewItem setup.
/// </summary>
/// <param name="TabButton">The check button that's used as a tab.</param>
/// <param name="ViewItemInstance">A script instance instance inherits the <see cref="FreeTabViewItemT{TOptionalArg}"/> or <see cref="FreeTabViewItem"/> type.</param>
public record struct TabInstanceSetup(CheckButton TabButton, IFreeTabViewItem ViewItemInstance);

/// <summary>
/// <para>A view item controller used to achieve the logic of a <see cref="TabContainer"/>.</para>
/// <para><see cref="FreeTabView"/>s are built via code, using scripts inheriting the <see cref="FreeTabViewItemT{TOptionalArg}"/> or <see cref="FreeTabViewItem"/> type to create the view items,
/// and create a <see cref="FreeTabView"/> instance from them using FreeTabView.CreateFromInstance or FreeTabView.CreateFromPrefab.</para>
/// </summary>
public partial class FreeTabView
{
    /// <summary>
    /// Called once when showing a view item.
    /// </summary>
    public event Action<IFreeTabViewItem>? OnViewItemShow;
    
    /// <summary>
    /// Called once when hiding a view item.
    /// </summary>
    public event Action<IFreeTabViewItem>? OnViewItemHide;
    
    private readonly CheckButton[] _checkButtons;
    private readonly IInternalFreeTabViewItem[] _viewItems;
    private readonly Func<IFreeTabViewItem, object?>? _defaultArgumentResolver;
    private readonly ButtonGroup _buttonGroup;

    private int? _currentViewItemIndex;

    private void TryHideCurrentViewItem()
    {
        if (_currentViewItemIndex == null) return;
        var viewItem = _viewItems[_currentViewItemIndex.Value];
        viewItem.HideViewItem();
        OnViewItemHide?.Invoke(viewItem);
    }

    private void OpenWithArgumentResolver(Func<IFreeTabViewItem, object?>? argumentResolver)
    {
        var viewItem = _viewItems[_currentViewItemIndex!.Value];
        object? argument = null;
        if (argumentResolver != null) argument = argumentResolver(viewItem);
        viewItem.ShowViewItem(argument);
        OnViewItemShow?.Invoke(viewItem);
        UpdateTabs();
    }

    private void UpdateTabs()
    {
        for (var i = 0; i < _checkButtons.Length; i++)
        {
            var checkButton = _checkButtons[i];
            var isCurrent = _currentViewItemIndex == i;
            checkButton.SetPressedNoSignal(isCurrent);
        }
    }
    
    /// <summary>
    /// Shows a view item at the given index.
    /// </summary>
    /// <param name="index">The view item index.</param>
    /// <exception cref="ArgumentOutOfRangeException">Throws then the view item index is greater or equal to the view items length.</exception>
    public void Show(int index) => Show(index, null);
    

    /// <summary>
    /// Shows a view item at the given index, and pass an <paramref name="optionalArg"/> to the target view item.
    /// </summary>
    /// <param name="index">The view item index.</param>
    /// <param name="optionalArg">An optional argument pass to the target view item.</param>
    /// <exception cref="ArgumentOutOfRangeException">Throws then the view item index is greater or equal to the view items length.</exception>
    public void Show(int index, object? optionalArg)
    {
        LengthCheck(index, _viewItems.Length);
        TryHideCurrentViewItem();
        _currentViewItemIndex = index;
        var viewItem = _viewItems[index];
        viewItem.ShowViewItem(optionalArg ?? _defaultArgumentResolver?.Invoke(viewItem));
        OnViewItemShow?.Invoke(viewItem);
        UpdateTabs();
    }

    /// <summary>
    /// Shows the next view item.<br/>
    /// If no view item shown at the moment, the first view item will be shown.<br/>
    /// </summary>
    /// <param name="warp">If set to true, the component will show the first view item when the current view item is already the last.</param>
    /// <param name="argumentResolver">Called before showing the selected view item, developers may use this delegate to determine the optional argument passes to the view item.</param>
    public void ShowNext(bool warp = true, Func<IFreeTabViewItem, object?>? argumentResolver = null)
    {
        TryHideCurrentViewItem();

        if (_currentViewItemIndex == null)
        {
            _currentViewItemIndex = 0;
        }
        else
        {
            if (_currentViewItemIndex + 1 >= _viewItems.Length)
            {
                if (warp) _currentViewItemIndex = 0;
            }
            else
            {
                _currentViewItemIndex++;
            }
        }

        OpenWithArgumentResolver(argumentResolver ?? _defaultArgumentResolver);
    }

    /// <summary>
    /// Shows the previous view item.<br/>
    /// If no view item shown at the moment, the first view item will be shown.<br/>
    /// </summary>
    /// <param name="warp">If set to true, the component will show the last view item when the current view item is already the first.</param>
    /// <param name="argumentResolver">Called before showing the selected view item, developers may use this delegate to determine the optional argument passes to the view item.</param>
    public void ShowPrevious(bool warp = true, Func<IFreeTabViewItem, object?>? argumentResolver = null)
    {
        TryHideCurrentViewItem();

        if (_currentViewItemIndex - 1 < 0)
        {
            if (warp) _currentViewItemIndex =  _viewItems.Length - 1;
        }
        else
        {
            _currentViewItemIndex--;
        }

        OpenWithArgumentResolver(argumentResolver ?? _defaultArgumentResolver);
    }
}