using Godot;

namespace GodotViews;

/// <summary>
/// Defines the behavior for view transitions.
/// </summary>
public interface IViewItemTweener
{
    /// <summary>
    /// This sets the default visual appearance for a view item.
    /// </summary>
    /// <param name="viewItem">The target view item.</param>
    void Init(Control viewItem);
    
    /// <summary>
    /// This async method manages the behavior when the view item is showing up.
    /// </summary>
    /// <param name="viewItem">The target view item.</param>
    void Show(Control viewItem);
    
    /// <summary>
    /// This async method manages the behavior when the view item is hiding out.
    /// </summary>
    /// <param name="viewItem">The target view item.</param>
    void Hide(Control viewItem);
}