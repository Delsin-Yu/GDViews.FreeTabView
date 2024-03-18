using System;
using System.Collections.Generic;
using Godot;

namespace GodotViews.ViewTweeners;

/// <summary>
/// Fades views between solid and transparent for transitions.
/// </summary>
public class FadeViewItemTweener : IViewItemTweener
{
    private readonly Dictionary<Control, Tween> _activeTween = new();

    private static readonly NodePath ModulatePath = new(Control.PropertyName.Modulate);

    /// <summary>
    /// The duration for fading.
    /// </summary>
    public float FadeTime { get; set; } = 0.1f;

    private void KillAndCreateNewTween(Control viewItem, in Color color, string methodName)
    {
        if (_activeTween.TryGetValue(viewItem, out var runningTween))
        {
            runningTween.Kill();
            runningTween.Dispose();
        }

        runningTween = viewItem.CreateTween();
        _activeTween[viewItem] = runningTween;

        runningTween
            .TweenProperty(viewItem, ModulatePath, color, FadeTime)
            .Dispose();
        runningTween
            .TweenCallback(
                Callable.From(
                    () =>
                    {
                        if (!_activeTween.Remove(viewItem, out var tween)) return;
                        tween.Kill();
                    }
                )
            )
            .Dispose();
    }

    /// <inheritdoc/>
    public void Init(Control viewItem) => 
        viewItem.Modulate = Colors.Transparent;

    /// <inheritdoc/>
    public void Show(Control viewItem) => 
        KillAndCreateNewTween(viewItem, Colors.White, "Show");

    /// <inheritdoc/>
    public void Hide(Control viewItem) => 
        KillAndCreateNewTween(viewItem, Colors.Transparent, "Hide");
}