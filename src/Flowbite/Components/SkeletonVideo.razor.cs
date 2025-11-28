using Microsoft.AspNetCore.Components;
using Flowbite.Base;

namespace Flowbite.Components;

/// <summary>
/// A skeleton video placeholder component that mimics a video loading state.
/// </summary>
/// <remarks>
/// Use SkeletonVideo to create a placeholder for videos that are being loaded.
/// It displays a background with an optional video icon in the center.
/// </remarks>
/// <example>
/// <code>
/// &lt;SkeletonVideo Width="max-w-sm" Height="h-56" ShowIcon="true" /&gt;
/// </code>
/// </example>
public partial class SkeletonVideo : FlowbiteComponentBase
{
    /// <summary>
    /// The width of the skeleton video placeholder.
    /// </summary>
    /// <remarks>
    /// Common values: "max-w-sm", "w-full", "w-96", etc.
    /// Defaults to "max-w-sm".
    /// </remarks>
    [Parameter]
    public string? Width { get; set; } = "max-w-sm";

    /// <summary>
    /// The height of the skeleton video placeholder.
    /// </summary>
    /// <remarks>
    /// Common values: "h-56", "h-64", "h-96", etc.
    /// Defaults to "h-56".
    /// </remarks>
    [Parameter]
    public string? Height { get; set; } = "h-56";

    /// <summary>
    /// Whether to show the video icon in the center of the placeholder.
    /// </summary>
    [Parameter]
    public bool ShowIcon { get; set; } = true;

    /// <summary>
    /// The size of the icon. Defaults to "w-11 h-11".
    /// </summary>
    [Parameter]
    public string? IconSize { get; set; } = "w-11 h-11";

    /// <summary>
    /// Additional attributes that will be merged with the component's native attributes.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
