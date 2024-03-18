# GDViews.FreeTabView [ WIP ]

## Introduction

Supports in `Godot 4.1+` with .Net module.  
***GDViews.FreeTabView*** is a `Godot 4` UI Component that's useful for creating highly customizable tab views.

## Installation

For .Net CLI

```txt
dotnet add package GDViews.FreeTabView
```

For Package Manager Console

```txt
NuGet\Install-Package GDViews.FreeTabView
```

For `csproj` PackageReference

```xml
<PackageReference Include="GDViews.FreeTabView" Version="*" />
```

---

<!-- START doctoc generated TOC please keep comment here to allow auto update -->
<!-- DON'T EDIT THIS SECTION, INSTEAD RE-RUN doctoc TO UPDATE -->
## Table of Contents

- [Glossarys](#glossarys)
- [API Usage](#api-usage)
  - [Creating a `ViewItem`](#creating-a-viewitem)
    - [A Simple Example](#a-simple-example)
    - [A Complex Example](#a-complex-example)
  - [Creating a `TabView`](#creating-a-tabview)
    - [Create from existing `ViewItem` Instances](#create-from-existing-viewitem-instances)
    - [Create from `PackedScene`s](#create-from-packedscenes)
- [Component Documentation](#component-documentation)
  - [The `FreeTabView`](#the-freetabview)
    - [Static Factory Methods](#static-factory-methods)
    - [Instance Methods](#instance-methods)
  - [The `FreeTabViewItem` / `FreeTabViewItemT`](#the-freetabviewitem--freetabviewitemt)
    - [Event Methods Diagram](#event-methods-diagram)
  - [ViewItemTweeners](#viewitemtweeners)
    - [Built-in Tweeners](#built-in-tweeners)
    - [Customize Tweeners](#customize-tweeners)

<!-- END doctoc generated TOC please keep comment here to allow auto update -->

---

## Glossarys

`FreeTabView / TabView`: The C# component that controls a group of associated `TabViewItem` and handles the tab view behavior.
`FreeTabViewItem / TabViewItem`: The script(s) inheriting the `FreeTabViewItem` or `FreeTabViewItemT`, developer may create a `FreeTabView` from a collection of `FreeTabViewItem`s.

## API Usage

### Creating a `ViewItem`

Attach the following script to a `Control` to make it a view item.  

#### A Simple Example

This view item displays the current system time.

```csharp
using Godot;
using GodotViews;

/// <summary>
/// Attach this script to a <see cref="Control"/> to make it a ViewItem.
/// </summary>
public partial class MyViewItem : FreeTabViewItem
{
    [Export] private Label _text;

    public override void _Process(double delta)
    {
        base._Process(delta);
        _text.Text = Time.GetTimeStringFromSystem();
    }
}
```

#### A Complex Example

This view item displays `Hello World!` when shown, and shows `Click: Number` when clicking the `_pressButton`.

```csharp
using Godot;

/// <summary>
/// Attach this script to a <see cref="Control"/> to make it a ViewItem.
/// </summary>
public partial class MyViewItem2 : FreeTabViewItem
{
    [Export] private Label _text;
    [Export] private Button _pressButton;

    private int _clickCount;
    
    /// <summary>
    /// Called when the <see cref="FreeTabView"/> is initializing the view item.
    /// </summary>
    protected override void _OnViewItemInitialize()
    {
        _pressButton.Pressed += () => _text.Text = $"Clicked: {_clickCount++}";
    }

    /// <summary>
    /// Called when the <see cref="FreeTabView"/> is showing the view item.
    /// </summary>
    protected override void _OnViewShow()
    {
        _text.Text = "Hello World!";
        _pressButton.GrabFocus();
    }
}
```

### Creating a `TabView`

The `FreeTabView` is pure C# implementation, so instead of attaching a script to a node in scene tree, developers need to create and use it in scripts, there are two ways for constructing a `FreeTabView` instance.

#### Create from existing `ViewItem` Instances

For use cases where developer wish to instantiate their own intance of `ViewItem`, or simply leaving them in the scene tree, `FreeTabView.CreateFromInstance` can be used to construct the `FreeTabView`.

```csharp
using Godot;

/// <summary>
/// Attached to a node in scene tree.
/// </summary>
public partial class Main : Node
{
    // Assigned in Godot Editor, through inspector.
    [Export] private MyViewItem _viewItem1;
    [Export] private MyViewItem2 _viewItem2;

    [Export] private CheckButton _tab1;
    [Export] private CheckButton _tab2;

    private FreeTabView _tabView;

    public override void _Ready()
    {
        // Construct a tab view on ready.
        _tabView = FreeTabView.CreateFromInstance(
            [
                // Associate a tab to its corresponding view item instance.
                new TabInstanceSetup(_tab1, _viewItem1), 
                new TabInstanceSetup(_tab2, _viewItem2), 
            ]
        );
        
        // Make the tab view displays the first view item.
        _tabView.Show(0);
    }

    public override void _Process(double delta)
    {
        // Developer may use their own preferred way to handle switching between tabs.
        if (Input.IsActionJustPressed("ui_left")) _tabView.ShowPrevious();
        if (Input.IsActionJustPressed("ui_right")) _tabView.ShowPrevious();
    }
}
```

#### Create from `PackedScene`s

For use cases where developer wish to store the `ViewItem`s as `PackedScene`s, `FreeTabView.CreateFromPrefab` can be used to construct the `FreeTabView`.

```csharp
using Godot;

/// <summary>
/// Attached to a node in scene tree.
/// </summary>
public partial class Main : Node
{
    // Assigned in Godot Editor, through inspector.
    [Export] private PackedScene _viewItem1;
    [Export] private PackedScene _viewItem2;

    [Export] private CheckButton _tab1;
    [Export] private CheckButton _tab2;

    // Required for storing the instances.
    [Export] private Control _container;

    private FreeTabView _tabView;

    public override void _Ready()
    {
        // Construct a tab view on ready.
        _tabView = FreeTabView.CreateFromPrefab(
            [
                // Associate a tab to a instance for the provided packed scene.
                new TabPrefabSetup(_tab1, _viewItem1), 
                new TabPrefabSetup(_tab2, _viewItem2), 
            ],
            _container
        );
        
        // Make the tab view displays the first view item.
        _tabView.Show(0);
    }

    public override void _Process(double delta)
    {
        // Developer may use their own preferred way to handle switching between tabs.
        if (Input.IsActionJustPressed("ui_left")) _tabView.ShowPrevious();
        if (Input.IsActionJustPressed("ui_right")) _tabView.ShowPrevious();
    }
}
```

## Component Documentation

### The `FreeTabView`

#### Static Factory Methods

> WIP:Introduction to the Static Factory Methods  
> WIP:CreateFromPrefab  
> WIP:CreateFromInstance  

#### Instance Methods

> WIP:Introduction to the Instance Methods  
> WIP:Show  
> WIP:ShowNext  
> WIP:ShowPrevious  

### The `FreeTabViewItem` / `FreeTabViewItemT`

> WIP:Introduction to the two view item types  

#### Event Methods Diagram

> WIP:Introduction to the event methods  
> WIP:_OnViewItemInitialize  
> WIP:_OnViewItemShow  
> WIP:_OnViewItemHide  
> WIP:_OnViewItemNotification  
> WIP:_OnViewItemPredelete  

### ViewItemTweeners

> WIP:Introduction to the tweeners  

#### Built-in Tweeners

> WIP:Introduction to the built-in tweeners  
> WIP:FadeViewItemTweener  
> WIP:NoneViewItemTweener  

#### Customize Tweeners

> WIP:Introduction to the customised tweeners  
> WIP:IViewItemTweener  
