﻿using Microsoft.AspNetCore.Components;

namespace Flowbite.Components;

/// <summary>
/// ProgressBar component for displaying progress indicators.
/// </summary>
public partial class ProgressBar
{
    /// <summary>
    /// Current progress value (0-100)
    /// </summary>
    [Parameter]
    public int Value { get; set; } = 0;

    /// <summary>
    /// Label text for the progress bar
    /// </summary>
    [Parameter]
    public string? Label { get; set; }

    /// <summary>
    /// Whether to show a label
    /// </summary>
    [Parameter]
    public bool ShowLabel { get; set; } = true;

    /// <summary>
    /// Whether to show percentage value
    /// </summary>
    [Parameter]
    public bool ShowPercentage { get; set; } = true;

    /// <summary>
    /// Color variant of the progress bar
    /// </summary>
    [Parameter]
    public ProgressColor Color { get; set; } = ProgressColor.Blue;

    /// <summary>
    /// Size variant of the progress bar
    /// </summary>
    [Parameter]
    public ProgressSize Size { get; set; } = ProgressSize.Medium;

    /// <summary>
    /// Corner radius style
    /// </summary>
    [Parameter]
    public ProgressCorner Corner { get; set; } = ProgressCorner.Rounded;

    /// <summary>
    /// Whether the progress bar has stripes
    /// </summary>
    [Parameter]
    public bool Striped { get; set; }

    /// <summary>
    /// Whether stripes should animate
    /// </summary>
    [Parameter]
    public bool AnimateStripes { get; set; }

    /// <summary>
    /// Position of the label relative to progress bar
    /// </summary>
    [Parameter]
    public LabelPosition LabelPos { get; set; } = LabelPosition.Above;

    /// <summary>
    /// Additional content to be rendered with the progress bar
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Additional attributes to be applied to the progress bar container
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private string GetProgressBarClasses()
    {
        var classes = new List<string> { "w-full" };

        if (AdditionalAttributes != null && AdditionalAttributes.TryGetValue("class", out var additionalClass))
        {
            classes.Add(additionalClass.ToString()!);
        }

        return string.Join(" ", classes);
    }

    private string GetTrackClasses()
    {
        var classes = new List<string>
        {
            "w-full overflow-hidden",
            GetSizeClass(),
            GetCornerClass(),
            "bg-gray-200 dark:bg-gray-700"
        };

        return string.Join(" ", classes);
    }

    private string GetFillClasses()
    {
        var classes = new List<string>
    {
        "h-full transition-all duration-300",
        GetColorClass(),
        GetCornerClass()
    };

        if (Striped)
        {
            classes.Add("bg-[linear-gradient(45deg,rgba(255,255,255,.15)_25%,transparent_25%,transparent_50%,rgba(255,255,255,.15)_50%,rgba(255,255,255,.15)_75%,transparent_75%,transparent)]");
            classes.Add("bg-[length:1rem_1rem]");

            if (AnimateStripes)
            {
                classes.Add("animate-pulse");
            }
        }

        return string.Join(" ", classes);
    }

    private string GetLabelClasses()
    {
        var classes = new List<string>
        {
            "flex justify-between text-sm font-medium text-gray-700 dark:text-gray-300"
        };

        if (LabelPos == LabelPosition.Above)
        {
            classes.Add("mb-1");
        }
        else if (LabelPos == LabelPosition.Below)
        {
            classes.Add("mt-1");
        }

        return string.Join(" ", classes);
    }

    private string GetInsideLabelClasses()
    {
        var classes = new List<string>
        {
            "flex items-center justify-center h-full px-2 text-xs font-semibold",
            GetInsideLabelColorClass()
        };

        return string.Join(" ", classes);
    }

    private string GetSizeClass() => Size switch
    {
        ProgressSize.ExtraSmall => "h-1",
        ProgressSize.Small => "h-2",
        ProgressSize.Medium => "h-4",
        ProgressSize.Large => "h-6",
        ProgressSize.ExtraLarge => "h-8",
        _ => "h-4"
    };

    private string GetColorClass() => Color switch
    {
        ProgressColor.Blue => "bg-primary-600",

        ProgressColor.Gray => "bg-gray-600",
        ProgressColor.Green => "bg-green-600",
        ProgressColor.Red => "bg-red-600",
        ProgressColor.Yellow => "bg-yellow-400",
        ProgressColor.Purple => "bg-purple-600",
        ProgressColor.Pink => "bg-pink-500",
        ProgressColor.Indigo => "bg-indigo-600",
        ProgressColor.Teal => "bg-teal-500",
        ProgressColor.Orange => "bg-orange-600",
        ProgressColor.Cyan => "bg-cyan-500",
        ProgressColor.Lime => "bg-lime-500",

        _ => "bg-primary-600"
    };

    private string GetInsideLabelColorClass() => Color switch
    {
        ProgressColor.Gray => "text-gray-100",
        ProgressColor.Blue => "text-blue-100",
        ProgressColor.Green => "text-green-100",
        ProgressColor.Red => "text-red-100",
        ProgressColor.Yellow => "text-yellow-900",
        ProgressColor.Purple => "text-purple-100",
        ProgressColor.Pink => "text-pink-100",
        ProgressColor.Indigo => "text-indigo-100",
        ProgressColor.Teal => "text-teal-100",
        ProgressColor.Orange => "text-orange-100",
        ProgressColor.Cyan => "text-cyan-100",
        ProgressColor.Lime => "text-lime-900",
        _ => "text-blue-100"
    };


    private string GetCornerClass() => Corner switch
    {
        ProgressCorner.Square => "rounded-none",
        ProgressCorner.Rounded => "rounded",
        ProgressCorner.Pill => "rounded-full",
        _ => "rounded"
    };
}

/// <summary>
/// Defines the color variants for progress bars
/// </summary>
public enum ProgressColor
{
    Gray,
    Blue,
    Green,
    Red,
    Yellow,
    Purple,
    Pink,
    Indigo,
    Teal,
    Orange,
    Cyan,
    Lime
}

/// <summary>
/// Defines the size variants for progress bars
/// </summary>
public enum ProgressSize
{
    ExtraSmall,
    Small,
    Medium,
    Large,
    ExtraLarge
}

/// <summary>
/// Defines the corner radius variants for progress bars
/// </summary>
public enum ProgressCorner
{
    Square,
    Rounded,
    Pill
}

/// <summary>
/// Defines the label position variants for progress bars
/// </summary>
public enum LabelPosition
{
    Above,
    Below,
    Inside,
    None
}