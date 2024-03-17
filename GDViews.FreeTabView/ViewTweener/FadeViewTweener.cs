using System;
using System.Collections.Generic;
using Godot;

namespace GodotViews.ViewTweeners;

/// <summary>
/// Fades views between solid and transparent for transitions.
/// </summary>
public class FadeViewTweener : IViewTweener
{
    private readonly Dictionary<Control, Tween> _activeTween = new();

    private static readonly NodePath ModulatePath = new(Control.PropertyName.Modulate);

    /// <summary>
    /// The duration for fading.
    /// </summary>
    public float FadeTime { get; set; } = 0.1f;

    private void KillAndCreateNewTween(Control view, in Color color, string methodName)
    {
        if (_activeTween.TryGetValue(view, out var runningTween))
        {
            runningTween.Kill();
            runningTween.Dispose();
        }

        runningTween = view.CreateTween();
        _activeTween[view] = runningTween;

        runningTween
            .TweenProperty(view, ModulatePath, color, FadeTime)
            .Dispose();
        runningTween
            .TweenCallback(
                Callable.From(
                    () =>
                    {
                        if (!_activeTween.Remove(view, out var tween)) return;
                        tween.Kill();
                    }
                )
            )
            .Dispose();
    }

    /// <inheritdoc/>
    public void Init(Control view) => 
        view.Modulate = Colors.Transparent;

    /// <inheritdoc/>
    public void Show(Control view) => 
        KillAndCreateNewTween(view, Colors.White, "Show");

    /// <inheritdoc/>
    public void Hide(Control view) => 
        KillAndCreateNewTween(view, Colors.Transparent, "Hide");
}