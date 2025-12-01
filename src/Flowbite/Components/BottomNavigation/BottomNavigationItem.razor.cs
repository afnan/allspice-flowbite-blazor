using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Flowbite.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flowbite.Components.BottomNavigation;

/// <summary>
/// Individual item within a bottom navigation component.
/// </summary>
public partial class BottomNavigationItem : FlowbiteComponentBase
{
    [CascadingParameter]
    private BottomNavigation? Parent { get; set; }

    /// <summary>
    /// Optional URL that the item links to. If provided, renders as an anchor tag.
    /// </summary>
    [Parameter]
    public string? Href { get; set; }

    /// <summary>
    /// Optional icon to display in the item.
    /// </summary>
    [Parameter]
    public IconBase? Icon { get; set; }

    /// <summary>
    /// Text label to display below the icon.
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    /// Whether this item is active/selected.
    /// </summary>
    [Parameter]
    public bool Active { get; set; }

    /// <summary>
    /// Whether the item is disabled.
    /// </summary>
    [Parameter]
    public bool Disabled { get; set; }

    /// <summary>
    /// Custom CSS classes for the item button/link.
    /// </summary>
    [Parameter]
    public string? ItemClass { get; set; }

    /// <summary>
    /// Custom CSS classes for the icon when active.
    /// </summary>
    [Parameter]
    public string? ActiveIconColor { get; set; }

    /// <summary>
    /// Custom CSS classes for the label when active.
    /// </summary>
    [Parameter]
    public string? ActiveLabelColor { get; set; }

    /// <summary>
    /// Child content to display as the label (alternative to Label parameter).
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Event callback fired when the item is clicked.
    /// </summary>
    [Parameter]
    public EventCallback<MouseEventArgs> OnClick { get; set; }

    /// <summary>
    /// Additional attributes to be applied to the item.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private BottomNavigationStyle Style => Parent?.Style ?? BottomNavigationStyle.Default;

    private string GetItemClasses()
    {
        var classes = new List<string>
        {
            "inline-flex",
            "flex-col",
            "items-center",
            "justify-center",
            "px-5",
            "hover:bg-neutral-secondary-medium",
            "dark:hover:bg-gray-700",
            "group",
            "transition-colors"
        };

        // Add border if style is WithBorder (except for last item)
        // Note: Last item detection would require parent context tracking
        // For now, we'll add border to all items except the last one
        // Users can override with ItemClass if needed
        if (Style == BottomNavigationStyle.WithBorder)
        {
            classes.Add("border-r border-gray-200 dark:border-gray-700 last:border-r-0");
        }

        // Active state
        if (Active)
        {
            classes.Add("bg-gray-100 dark:bg-gray-700");
        }

        // Disabled state
        if (Disabled)
        {
            classes.Add("opacity-50 cursor-not-allowed");
        }

        // Custom classes
        if (!string.IsNullOrEmpty(ItemClass))
        {
            classes.Add(ItemClass);
        }

        return CombineClasses(string.Join(" ", classes));
    }

    private string GetIconClasses()
    {
        var classes = new List<string> { "w-6 h-6 mb-1" };

        if (Active)
        {
            if (!string.IsNullOrEmpty(ActiveIconColor))
            {
                classes.Add(ActiveIconColor);
            }
            else
            {
                classes.Add("text-primary-600 dark:text-primary-400");
            }
        }
        else
        {
            classes.Add("text-gray-500 dark:text-gray-400 group-hover:text-primary-600 dark:group-hover:text-primary-400");
        }

        return string.Join(" ", classes);
    }

    private string GetLabelClasses()
    {
        var classes = new List<string> { "text-sm" };

        if (Active)
        {
            if (!string.IsNullOrEmpty(ActiveLabelColor))
            {
                classes.Add(ActiveLabelColor);
            }
            else
            {
                classes.Add("text-primary-600 dark:text-primary-400");
            }
        }
        else
        {
            classes.Add("text-gray-500 dark:text-gray-400 group-hover:text-primary-600 dark:group-hover:text-primary-400");
        }

        return string.Join(" ", classes);
    }

    private async Task HandleClick(MouseEventArgs e)
    {
        if (Disabled) return;

        if (OnClick.HasDelegate)
        {
            await OnClick.InvokeAsync(e);
        }
    }
}

