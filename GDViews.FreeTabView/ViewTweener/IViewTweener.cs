using Godot;

namespace GodotViews;

/// <summary>
/// Defines the behavior for view transitions.
/// </summary>
public interface IViewTweener
{
    /// <summary>
    /// This sets the default visual appearance for a view.
    /// </summary>
    /// <param name="view">The target view.</param>
    void Init(Control view);
    
    /// <summary>
    /// This async method manages the behavior when the view is showing up.
    /// </summary>
    /// <param name="view">The target view.</param>
    void Show(Control view);
    
    /// <summary>
    /// This async method manages the behavior when the view is hiding out.
    /// </summary>
    /// <param name="view">The target view.</param>
    void Hide(Control view);
}