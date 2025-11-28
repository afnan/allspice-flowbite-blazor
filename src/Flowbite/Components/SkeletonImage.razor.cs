using Microsoft.AspNetCore.Components;
using Flowbite.Base;

namespace Flowbite.Components;

/// <summary>
/// A skeleton image placeholder component that mimics an image loading state.
/// </summary>
/// <remarks>
/// Use SkeletonImage to create a placeholder for images that are being loaded.
/// It displays a background with an optional image icon in the center.
/// </remarks>
/// <example>
/// <code>
/// &lt;SkeletonImage Width="w-full" Height="h-48" ShowIcon="true" /&gt;
/// </code>
/// </example>
public partial class SkeletonImage : FlowbiteComponentBase
{
    /// <summary>
    /// The width of the skeleton image placeholder.
    /// </summary>
    /// <remarks>
    /// Common values: "w-full", "w-48", "sm:w-96", etc.
    /// Defaults to "w-full".
    /// </remarks>
    [Parameter]
    public string? Width { get; set; } = "w-full";

    /// <summary>
    /// The height of the skeleton image placeholder.
    /// </summary>
    /// <remarks>
    /// Common values: "h-48", "h-64", "h-96", etc.
    /// Defaults to "h-48".
    /// </remarks>
    [Parameter]
    public string? Height { get; set; } = "h-48";

    /// <summary>
    /// Whether to show the image icon in the center of the placeholder.
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
