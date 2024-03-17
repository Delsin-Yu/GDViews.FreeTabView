using System;
using Godot;

namespace GodotViews.ViewTweeners;

/// <summary>
/// This is the default tweener that does not have any animated transition properties, it instantly hides and shows the view.
/// </summary>
public class NoneViewTweener : IViewTweener
{
    /// <summary>
    /// The unified instance of this <see cref="NoneViewTweener"/>.
    /// </summary>
    public static readonly NoneViewTweener Instance = new();
    
    private NoneViewTweener() { }

    /// <inheritdoc/>
    public void Init(Control view) { }

    /// <inheritdoc/>
    public void Show(Control view) => view.Show();

    /// <inheritdoc/>
    public void Hide(Control view) => view.Hide();
}