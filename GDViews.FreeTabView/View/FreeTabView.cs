using System;
using System.Collections.Generic;
using Godot;

namespace GodotViews;

/// <summary>
/// Defines a Tab - View setup.
/// </summary>
/// <param name="TabButton">The check button that's used as a tab.</param>
/// <param name="ViewPrefab">The packed scene that contains a <see cref="FreeTabViewControlT{TOptionalArg}"/>/<see cref="FreeTabViewControl"/> class.</param>
public record struct TabPrefabSetup(CheckButton TabButton, PackedScene ViewPrefab);

/// <summary>
/// Defines a Tab - View setup.
/// </summary>
/// <param name="TabButton">The check button that's used as a tab.</param>
/// <param name="ViewInstance">A <see cref="FreeTabViewControlT{TOptionalArg}"/>/<see cref="FreeTabViewControl"/> instance.</param>
public record struct TabInstanceSetup(CheckButton TabButton, IFreeTabViewControl ViewInstance);

public partial class FreeTabView
{
    public event Action<IFreeTabViewControl>? OnViewShow;
    public event Action<IFreeTabViewControl>? OnViewHide;
    
    private readonly CheckButton[] _checkButtons;
    private readonly IInternalFreeTabViewControl[] _views;
    private Func<IFreeTabViewControl, object?>? _defaultArgumentResolver;

    private readonly ButtonGroup _buttonGroup;

    private record struct CachedControlInfo(Control.MouseFilterEnum CachedMouseFilter, Control.FocusModeEnum CachedFocusMode);

    private int? _current;

    private void TryHideCurrentView()
    {
        if (_current == null) return;
        var view = _views[_current.Value];
        view.HideView();
        OnViewHide?.Invoke(view);
    }

    private void OpenWithArgumentResolver(Func<IFreeTabViewControl, object?>? argumentResolver)
    {
        var view = _views[_current!.Value];
        object? argument = null;
        if (argumentResolver != null) argument = argumentResolver(view);
        view.ShowView(argument);
        OnViewShow?.Invoke(view);
        UpdateTabs();
    }

    private void UpdateTabs()
    {
        for (var i = 0; i < _checkButtons.Length; i++)
        {
            var checkButton = _checkButtons[i];
            var isCurrent = _current == i;
            checkButton.SetPressedNoSignal(isCurrent);
        }
    }
    
    public void Show(int index) => Show<object>(index);
    public void Show<TOptionalArg>(int index, TOptionalArg? optionalArg = default)
    {
        TryHideCurrentView();
        _current = index;
        var view = _views[index];
        view.ShowView(optionalArg ?? _defaultArgumentResolver?.Invoke(view));
        OnViewShow?.Invoke(view);
        UpdateTabs();
    }

    public void ShowNext(Func<IFreeTabViewControl, object?>? argumentResolver = null)
    {
        TryHideCurrentView();

        if (_current == null)
        {
            _current = 0;
        }
        else
        {
            _current++;
            if (_current >= _views.Length) _current = 0;
        }

        OpenWithArgumentResolver(argumentResolver ?? _defaultArgumentResolver);
    }

    public void ShowPrevious(Func<IFreeTabViewControl, object?>? argumentResolver = null)
    {
        TryHideCurrentView();

        if (_current == null)
        {
            _current = 0;
        }
        else
        {
            _current--;
            if (_current < 0) _current = _views.Length - 1;
        }

        OpenWithArgumentResolver(argumentResolver ?? _defaultArgumentResolver);
    }
}