namespace GodotViews;

public interface IFreeTabViewControl { }

internal interface IInternalFreeTabViewControl : IFreeTabViewControl
{
    public void InitializeView();
    public void ShowView(object? optionalArg);
    public void HideView();
}