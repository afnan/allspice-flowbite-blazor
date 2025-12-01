using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Flowbite.Components.BottomNavigation;

/// <summary>
/// Bottom navigation component for displaying a fixed navigation bar at the bottom of the page.
/// </summary>
public partial class BottomNavigation : FlowbiteComponentBase
{
    /// <summary>
    /// The style variant of the bottom navigation.
    /// </summary>
    [Parameter]
    public BottomNavigationStyle Style { get; set; } = BottomNavigationStyle.Default;

    /// <summary>
    /// Maximum width of the navigation container. Default is max-w-lg.
    /// </summary>
    [Parameter]
    public string? MaxWidth { get; set; }

    /// <summary>
    /// Number of grid columns. Default is 4. Use grid-cols-3, grid-cols-4, grid-cols-5, etc.
    /// </summary>
    [Parameter]
    public int Columns { get; set; } = 4;

    /// <summary>
    /// Background color classes. Default is bg-white dark:bg-gray-800.
    /// </summary>
    [Parameter]
    public string? BackgroundColor { get; set; }

    /// <summary>
    /// Border color classes. Default is border-gray-200 dark:border-gray-700.
    /// </summary>
    [Parameter]
    public string? BorderColor { get; set; }

    /// <summary>
    /// Whether to use fixed positioning (viewport-relative) or absolute positioning (container-relative).
    /// Default is true (fixed). Set to false when used inside a container for previews.
    /// </summary>
    [Parameter]
    public bool Fixed { get; set; } = true;

    /// <summary>
    /// Child content containing BottomNavigationItem components.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Additional attributes to be applied to the navigation container.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private string GetNavigationClasses()
    {
        var classes = new List<string>();
        
        // Only add positioning class when Fixed=true
        // When Fixed=false, we'll use inline style to override
        if (Fixed)
        {
            classes.Add("fixed");
        }
        else
        {
            // Don't add fixed class, use absolute via style instead
            classes.Add("absolute");
        }
        
        classes.Add("fixed bottom-0 left-0 z-50 w-full h-16 bg-neutral-primary-soft border-t border-default");

        // Background color
        if (!string.IsNullOrEmpty(BackgroundColor))
        {
            classes.Add(BackgroundColor);
        }
        else
        {
            classes.Add("bg-white dark:bg-gray-800");
        }

        // Border
        if (!string.IsNullOrEmpty(BorderColor))
        {
            classes.Add($"border-t {BorderColor}");
        }
        else
        {
            classes.Add("border-t border-gray-200 dark:border-gray-700");
        }

        return CombineClasses(string.Join(" ", classes));
    }

    private string GetNavigationStyle()
    {
        // When Fixed=false, ensure absolute positioning is used and override any fixed positioning
        if (!Fixed)
        {
            return "position: absolute !important;";
        }
        return string.Empty;
    }

    private string GetContainerClasses()
    {
        var classes = new List<string>
        {
            "grid",
            "h-full",
            "w-full",
            MaxWidth ?? "max-w-lg",
            "mx-auto",
            "font-medium"
        };

        // Add grid columns class based on Columns parameter
        var gridColsClass = Columns switch
        {
            2 => "grid-cols-2",
            3 => "grid-cols-3",
            4 => "grid-cols-4",
            5 => "grid-cols-5",
            6 => "grid-cols-6",
            _ => $"grid-cols-{Columns}"
        };
        classes.Add(gridColsClass);

        return string.Join(" ", classes);
    }

    private string GetContainerStyle()
    {
        // Ensure grid is applied and set grid-template-columns as fallback
        return $"display: grid; grid-template-columns: repeat({Columns}, minmax(0, 1fr));";
    }
}

