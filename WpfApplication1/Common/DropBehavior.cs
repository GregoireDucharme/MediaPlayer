﻿using System.Windows.Input;
using System.Windows;

public static class DropBehavior
{
    private static readonly DependencyProperty PreviewDropCommandProperty =
                DependencyProperty.RegisterAttached
                (
                    "PreviewDropCommand",
                    typeof(ICommand),
                    typeof(DropBehavior),
                    new PropertyMetadata(PreviewDropCommandPropertyChangedCallBack)
                );

    public static void SetPreviewDropCommand(this UIElement inUIElement, ICommand inCommand)
    {
        inUIElement.SetValue(PreviewDropCommandProperty, inCommand);
    }

    private static ICommand GetPreviewDropCommand(UIElement inUIElement)
    {
        return (ICommand)inUIElement.GetValue(PreviewDropCommandProperty);
    }

    private static void PreviewDropCommandPropertyChangedCallBack(DependencyObject inDependencyObject, DependencyPropertyChangedEventArgs inEventArgs)
    {
        UIElement uiElement = inDependencyObject as UIElement;
        if (null == uiElement)
            return;
        uiElement.Drop += (sender, args) =>
        {
            GetPreviewDropCommand(uiElement).Execute(args.Data);
            args.Handled = true;
        };
    }
}
