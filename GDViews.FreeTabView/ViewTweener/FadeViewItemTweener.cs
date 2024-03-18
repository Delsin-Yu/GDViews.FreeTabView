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

    private void KillAndCreateNewTween(Control viewItem, in Color color, bool showAfterTween)
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
                        viewItem.Visible = showAfterTween;
                        if (!_activeTween.Remove(viewItem, out var tween)) return;
                        tween.Kill();
                    }
                )
            )
            .Dispose();
    }

    /// <inheritdoc/>
    public void Init(Control viewItem, ref object? additionalData)
    {
        additionalData = new Dictionary<Control, NodeUtils.CachedControlInfo>();
        viewItem.Modulate = Colors.Transparent;
    }

    /// <inheritdoc/>
    public void Show(Control viewItem, object? additionalData)
    {
        NodeUtils.SetNodeChildAvailability(viewItem, (Dictionary<Control, NodeUtils.CachedControlInfo>)additionalData!, true);
        KillAndCreateNewTween(viewItem, Colors.White, true);
    }

    /// <inheritdoc/>
    public void Hide(Control viewItem, object? additionalData)
    {
        NodeUtils.SetNodeChildAvailability(viewItem, (Dictionary<Control, NodeUtils.CachedControlInfo>)additionalData!, false);
        KillAndCreateNewTween(viewItem, Colors.Transparent, false);
    }
}