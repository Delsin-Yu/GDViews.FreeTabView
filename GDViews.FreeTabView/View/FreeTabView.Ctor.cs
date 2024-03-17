using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot;

namespace GodotViews;

public partial class FreeTabView
{
    private void InitializeTab(int i, CheckButton checkButton, IInternalFreeTabViewControl view)
    {
        _checkButtons[i] = checkButton;
        _views[i] = view;
        var localI = i;
        checkButton.Toggled += pressed =>
        {
            if (pressed) Show(localI);
        };
        checkButton.ButtonGroup = _buttonGroup;
        DelegateRunner.RunProtected(view.InitializeView, "View Initialization", ((Control)view).Name);
        view.HideView();
    }

    private static void BoundsCheck(int count, [CallerArgumentExpression(nameof(count))] string? parameterName = null)
    {
#if NET8_0_OR_GREATER
        ArgumentOutOfRangeException.ThrowIfZero(count, parameterName);
#else
        if (count == 0) throw new ArgumentOutOfRangeException(parameterName, count, null);
#endif
    }

    public static FreeTabView CreateFromPrefab(in ReadOnlySpan<TabPrefabSetup> tabSetups, Control viewsContainer, Func<IFreeTabViewControl, object?>? defaultArgumentResolver = null)
    {
        var length = tabSetups.Length;
        BoundsCheck(length);
        var instance = new FreeTabView(length, defaultArgumentResolver);

        for (var i = 0; i < tabSetups.Length; i++)
        {
            var (checkButton, viewPrefab) = tabSetups[i];
            var view = viewPrefab.Instantiate<IInternalFreeTabViewControl>();
            viewsContainer.AddChild((Control)view);
            instance.InitializeTab(i, checkButton, view);
        }
        
        return instance;
    }

    public static FreeTabView CreateFromPrefab(in IReadOnlyList<TabPrefabSetup> tabSetups, Control viewsContainer, Func<IFreeTabViewControl, object?>? defaultArgumentResolver = null)
    {
        var length = tabSetups.Count;
        BoundsCheck(length);
        var instance = new FreeTabView(length, defaultArgumentResolver);

        for (var i = 0; i < tabSetups.Count; i++)
        {
            var (checkButton, viewPrefab) = tabSetups[i];
            var view = viewPrefab.Instantiate<IInternalFreeTabViewControl>();
            viewsContainer.AddChild((Control)view);
            instance.InitializeTab(i, checkButton, view);
        }
        
        return instance;
    }

    public static FreeTabView CreateFromInstance(in ReadOnlySpan<TabInstanceSetup> tabSetups, Func<IFreeTabViewControl, object?>? defaultArgumentResolver = null)
    {
        var length = tabSetups.Length;
        BoundsCheck(length);
        var instance = new FreeTabView(length, defaultArgumentResolver);

        for (var i = 0; i < tabSetups.Length; i++)
        {
            var (checkButton, viewInstance) = tabSetups[i];
            instance.InitializeTab(i, checkButton, (IInternalFreeTabViewControl)viewInstance);
        }
        
        return instance;
    }

    public static FreeTabView CreateFromInstance(in IReadOnlyList<TabInstanceSetup> tabSetups, Func<IFreeTabViewControl, object?>? defaultArgumentResolver = null)
    {
        var length = tabSetups.Count;
        BoundsCheck(length);
        var instance = new FreeTabView(length, defaultArgumentResolver);

        for (var i = 0; i < tabSetups.Count; i++)
        {
            var (checkButton, viewInstance) = tabSetups[i];
            instance.InitializeTab(i, checkButton, (IInternalFreeTabViewControl)viewInstance);
        }
        
        return instance;
    }

    private FreeTabView(Func<IFreeTabViewControl, object?> defaultArgumentResolver)
    {
        _defaultArgumentResolver = defaultArgumentResolver;
        _views = Array.Empty<IInternalFreeTabViewControl>();
        _checkButtons = Array.Empty<CheckButton>();
        _buttonGroup = new();
    }

    private FreeTabView(int length, Func<IFreeTabViewControl, object?> defaultArgumentResolver)
    {
        _defaultArgumentResolver = defaultArgumentResolver;
        _views = new IInternalFreeTabViewControl[length];
        _checkButtons = new CheckButton[length];
        _buttonGroup = new()
        {
            AllowUnpress = false
        };
    }
}