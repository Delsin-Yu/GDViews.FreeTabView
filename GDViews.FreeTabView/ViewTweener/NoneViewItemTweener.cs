using System;
using Godot;

namespace GodotViews.ViewTweeners;

/// <summary>
/// This is the default tweener that does not have any animated transition properties, it instantly hides and shows the view.
/// </summary>
public class NoneViewItemTweener : IViewItemTweener
{
    /// <summary>
    /// The unified instance of this <see cref="NoneViewItemTweener"/>.
    /// </summary>
    public static readonly NoneViewItemTweener Instance = new();
    
    private NoneViewItemTweener() { }

    /// <inheritdoc/>
    public void Init(Control viewItem) { }

    /// <inheritdoc/>
    public void Show(Control viewItem) => viewItem.Show();

    /// <inheritdoc/>
    public void Hide(Control viewItem) => viewItem.Hide();
}