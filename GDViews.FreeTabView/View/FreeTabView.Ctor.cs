using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;

namespace GodotViews.FreeTab;

public partial class FreeTabView
{
    private void InitializeTab(int i, CheckButton checkButton, IInternalFreeTabViewItem viewItem)
    {
        _checkButtons[i] = checkButton;
        _viewItems[i] = viewItem;
        var localI = i;
        checkButton.Toggled += pressed =>
        {
            if (pressed) Show(localI);
        };
        checkButton.ButtonGroup = _buttonGroup;
        DelegateRunner.RunProtected(viewItem.InitializeViewItem, "View Initialization", ((Control)viewItem).Name);
        viewItem.HideViewItem();
    }

    private static void BoundsCheck(int count, [CallerArgumentExpression(nameof(count))] string? paramName = null)
    {
#if NET8_0_OR_GREATER
        ArgumentOutOfRangeException.ThrowIfZero(count, paramName);
#else
        if (count == 0) throw new ArgumentOutOfRangeException(paramName, count, null);
#endif
    }

    private static void LengthCheck(int count, int total, [CallerArgumentExpression(nameof(count))] string? paramName = null)
    {
#if NET8_0_OR_GREATER
        ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual(count, total, paramName);
#else
        if (count >= total) throw new ArgumentOutOfRangeException(paramName, count, null);
#endif
    }

    /// <summary>
    /// Create an instance of the <see cref="FreeTabView"/> from the given <see cref="TabPrefabSetup"/>s, this overload instantiates the given <see cref="PackedScene"/>s under the <paramref name="viewsContainer"/>.
    /// </summary>
    /// <param name="tabSetups">A collection of <see cref="TabPrefabSetup"/>s to initialize the <see cref="FreeTabView"/>.</param>
    /// <param name="viewsContainer">The container to instantiate the <see cref="PackedScene"/>s under.</param>
    /// <param name="defaultArgumentResolver">An optional delegate to resolve the optional argument passes to a view item when it's shown.</param>
    /// <returns>An instance of the <see cref="FreeTabView"/> that's ready for use.</returns>
    public static FreeTabView CreateFromPrefab(in ReadOnlySpan<TabPrefabSetup> tabSetups, Control viewsContainer, Func<IFreeTabViewItem, object?>? defaultArgumentResolver = null)
    {
        var length = tabSetups.Length;
        BoundsCheck(length);
        var instance = new FreeTabView(length, defaultArgumentResolver);

        for (var i = 0; i < tabSetups.Length; i++)
        {
            var (checkButton, viewItemPrefab) = tabSetups[i];
            var viewItem = viewItemPrefab.Instantiate<IInternalFreeTabViewItem>();
            viewsContainer.AddChild((Control)viewItem);
            instance.InitializeTab(i, checkButton, viewItem);
        }
        
        return instance;
    }

    /// <summary>
    /// Create an instance of the <see cref="FreeTabView"/> from the given <see cref="TabPrefabSetup"/>s, 
    /// this overload instantiates the given <see cref="PackedScene"/>s under the <paramref name="viewsContainer"/>.
    /// </summary>
    /// <param name="tabSetups">A collection of <see cref="TabPrefabSetup"/>s to initialize the <see cref="FreeTabView"/>.</param>
    /// <param name="viewsContainer">The container to instantiate the <see cref="PackedScene"/>s under.</param>
    /// <param name="defaultArgumentResolver">An optional delegate to resolve the optional argument passes to a view item when it's shown.</param>
    /// <returns>An instance of the <see cref="FreeTabView"/> that's ready for use.</returns>
    public static FreeTabView CreateFromPrefab(in IReadOnlyList<TabPrefabSetup> tabSetups, Control viewsContainer, Func<IFreeTabViewItem, object?>? defaultArgumentResolver = null)
    {
        var length = tabSetups.Count;
        BoundsCheck(length);
        var instance = new FreeTabView(length, defaultArgumentResolver);

        for (var i = 0; i < tabSetups.Count; i++)
        {
            var (checkButton, viewItemPrefab) = tabSetups[i];
            var viewItem = viewItemPrefab.Instantiate<IInternalFreeTabViewItem>();
            viewsContainer.AddChild((Control)viewItem);
            instance.InitializeTab(i, checkButton, viewItem);
        }
        
        return instance;
    }

    /// <summary>
    /// Create an instance of the <see cref="FreeTabView"/> from the given <see cref="TabPrefabSetup"/>s, 
    /// this overload references the given <see cref="IFreeTabViewItem"/>s.
    /// </summary>
    /// <param name="tabSetups">A collection of <see cref="TabInstanceSetup"/>s to initialize the <see cref="FreeTabView"/>.</param>
    /// <param name="defaultArgumentResolver">An optional delegate to resolve the optional argument passes to a view item when it's shown.</param>
    /// <returns>An instance of the <see cref="FreeTabView"/> that's ready for use.</returns>
    public static FreeTabView CreateFromInstance(in ReadOnlySpan<TabInstanceSetup> tabSetups, Func<IFreeTabViewItem, object?>? defaultArgumentResolver = null)
    {
        var length = tabSetups.Length;
        BoundsCheck(length);
        var instance = new FreeTabView(length, defaultArgumentResolver);

        for (var i = 0; i < tabSetups.Length; i++)
        {
            var (checkButton, viewItemInstance) = tabSetups[i];
            instance.InitializeTab(i, checkButton, (IInternalFreeTabViewItem)viewItemInstance);
        }
        
        return instance;
    }

    /// <summary>
    /// Create an instance of the <see cref="FreeTabView"/> from the given <see cref="TabPrefabSetup"/>s, 
    /// this overload references the given <see cref="IFreeTabViewItem"/>s.
    /// </summary>
    /// <param name="tabSetups">A collection of <see cref="TabInstanceSetup"/>s to initialize the <see cref="FreeTabView"/>.</param>
    /// <param name="defaultArgumentResolver">An optional delegate to resolve the optional argument passes to a view item when it's shown.</param>
    /// <returns>An instance of the <see cref="FreeTabView"/> that's ready for use.</returns>
    public static FreeTabView CreateFromInstance(in IReadOnlyList<TabInstanceSetup> tabSetups, Func<IFreeTabViewItem, object?>? defaultArgumentResolver = null)
    {
        var length = tabSetups.Count;
        BoundsCheck(length);
        var instance = new FreeTabView(length, defaultArgumentResolver);

        for (var i = 0; i < tabSetups.Count; i++)
        {
            var (checkButton, viewItemInstance) = tabSetups[i];
            instance.InitializeTab(i, checkButton, (IInternalFreeTabViewItem)viewItemInstance);
        }
        
        return instance;
    }

    // ReSharper disable once UnusedMember.Local
    private FreeTabView()
    {
        _defaultArgumentResolver = null;
        _viewItems = Array.Empty<IInternalFreeTabViewItem>();
        _checkButtons = Array.Empty<CheckButton>();
        _buttonGroup = new();
    }

    private FreeTabView(int length, Func<IFreeTabViewItem, object?>? defaultArgumentResolver)
    {
        _defaultArgumentResolver = defaultArgumentResolver;
        _viewItems = new IInternalFreeTabViewItem[length];
        _checkButtons = new CheckButton[length];
        _buttonGroup = new()
        {
            AllowUnpress = false
        };
    }
}