using Microsoft.AspNetCore.Components;
using Flowbite.Base;

namespace Flowbite.Components;

/// <summary>
/// A skeleton text placeholder component that mimics text lines.
/// </summary>
/// <remarks>
/// Use SkeletonText to create placeholder lines that represent text content.
/// You can specify width and height to match the expected text layout.
/// </remarks>
/// <example>
/// <code>
/// &lt;SkeletonText Width="w-48" Height="h-2.5" /&gt;
/// &lt;SkeletonText Width="max-w-[480px]" Height="h-2" /&gt;
/// </code>
/// </example>
public partial class SkeletonText : FlowbiteComponentBase
{
    /// <summary>
    /// The width of the skeleton text line. Can be a Tailwind width class or a custom width.
    /// </summary>
    /// <remarks>
    /// Common values: "w-48", "w-full", "max-w-[480px]", etc.
    /// If not specified, defaults to "w-full".
    /// </remarks>
    [Parameter]
    public string? Width { get; set; }

    /// <summary>
    /// The height of the skeleton text line. Can be a Tailwind height class.
    /// </summary>
    /// <remarks>
    /// Common values: "h-2" (8px), "h-2.5" (10px), "h-3" (12px), etc.
    /// If not specified, defaults to "h-2".
    /// </remarks>
    [Parameter]
    public string? Height { get; set; }

    /// <summary>
    /// Additional margin bottom spacing after this skeleton element.
    /// </summary>
    /// <remarks>
    /// Use Tailwind margin classes like "mb-2.5", "mb-4", etc.
    /// </remarks>
    [Parameter]
    public string? MarginBottom { get; set; }

    /// <summary>
    /// Additional attributes that will be merged with the component's native attributes.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private string GetHeightClass() => !string.IsNullOrWhiteSpace(Height) ? Height : "h-2";

    private string GetWidthClass() => !string.IsNullOrWhiteSpace(Width) ? Width : "w-full";
}
