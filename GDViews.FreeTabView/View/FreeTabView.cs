using System;
using Godot;

namespace GodotViews;

/// <summary>
/// Defines a Tab/Prefab setup.
/// </summary>
/// <param name="TabButton">The check button that's used as a tab.</param>
/// <param name="ViewPrefab">The packed scene containing a script inherits the <see cref="FreeTabViewItemT{TOptionalArg}"/> or <see cref="FreeTabViewItem"/> type.</param>
public record struct TabPrefabSetup(CheckButton TabButton, PackedScene ViewPrefab);

/// <summary>
/// Defines a Tab/ViewItem setup.
/// </summary>
/// <param name="TabButton">The check button that's used as a tab.</param>
/// <param name="ViewItemInstance">A script instance instance inherits the <see cref="FreeTabViewItemT{TOptionalArg}"/> or <see cref="FreeTabViewItem"/> type.</param>
public record struct TabInstanceSetup(CheckButton TabButton, IFreeTabViewItem ViewItemInstance);

/// <summary>
/// <para>A view controller used to achieve the logic of a <see cref="TabContainer"/>.</para>
/// <para><see cref="FreeTabView"/>s are built via code, using scripts inheriting the <see cref="FreeTabViewItemT{TOptionalArg}"/> or <see cref="FreeTabViewItem"/> type to create the view items,
/// and create a <see cref="FreeTabView"/> instance from them using FreeTabView.CreateFromInstance or FreeTabView.CreateFromPrefab.</para>
/// </summary>
public partial class FreeTabView
{
    /// <summary>
    /// Called once when showing a view.
    /// </summary>
    public event Action<IFreeTabViewItem>? OnViewItemShow;
    
    /// <summary>
    /// Called once when hiding a view.
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
        var view = _viewItems[_currentViewItemIndex.Value];
        view.HideViewItem();
        OnViewItemHide?.Invoke(view);
    }

    private void OpenWithArgumentResolver(Func<IFreeTabViewItem, object?>? argumentResolver)
    {
        var view = _viewItems[_currentViewItemIndex!.Value];
        object? argument = null;
        if (argumentResolver != null) argument = argumentResolver(view);
        view.ShowViewItem(argument);
        OnViewItemShow?.Invoke(view);
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
    /// Shows a view at the given index.
    /// </summary>
    /// <param name="index">The view index.</param>
    /// <exception cref="ArgumentOutOfRangeException">Throws then the view index is greater or equal to the view length.</exception>
    public void Show(int index) => Show(index, null);
    

    /// <summary>
    /// Shows a view at the given index, and pass an <paramref name="optionalArg"/> to the target view.
    /// </summary>
    /// <param name="index">The view index.</param>
    /// <param name="optionalArg">An optional argument pass to the target view.</param>
    /// <exception cref="ArgumentOutOfRangeException">Throws then the view index is greater or equal to the view length.</exception>
    public void Show(int index, object? optionalArg)
    {
        LengthCheck(index, _viewItems.Length);
        TryHideCurrentViewItem();
        _currentViewItemIndex = index;
        var view = _viewItems[index];
        view.ShowViewItem(optionalArg ?? _defaultArgumentResolver?.Invoke(view));
        OnViewItemShow?.Invoke(view);
        UpdateTabs();
    }

    /// <summary>
    /// Shows the next view.<br/>
    /// If no view shown at the moment, the first view will be shown.<br/>
    /// </summary>
    /// <param name="wrapped">If set to true, the system will show the first view when the current view is already the last.</param>
    /// <param name="argumentResolver">Called before showing the selected view, developers may use this delegate to determine the optional argument passes to the view.</param>
    public void ShowNext(bool wrapped = true, Func<IFreeTabViewItem, object?>? argumentResolver = null)
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
                if (wrapped) _currentViewItemIndex = 0;
            }
            else
            {
                _currentViewItemIndex++;
            }
        }

        OpenWithArgumentResolver(argumentResolver ?? _defaultArgumentResolver);
    }

    /// <summary>
    /// Shows the previous view.<br/>
    /// If no view shown at the moment, the first view will be shown.<br/>
    /// </summary>
    /// <param name="wrapped">If set to true, the system will show the last view when the current view is already the first.</param>
    /// <param name="argumentResolver">Called before showing the selected view, developers may use this delegate to determine the optional argument passes to the view.</param>
    public void ShowPrevious(bool wrapped = true, Func<IFreeTabViewItem, object?>? argumentResolver = null)
    {
        TryHideCurrentViewItem();

        if (_currentViewItemIndex - 1 < 0)
        {
            if (wrapped) _currentViewItemIndex =  _viewItems.Length - 1;
        }
        else
        {
            _currentViewItemIndex--;
        }

        OpenWithArgumentResolver(argumentResolver ?? _defaultArgumentResolver);
    }
}