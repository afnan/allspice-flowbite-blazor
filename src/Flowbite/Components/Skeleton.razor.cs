using Microsoft.AspNetCore.Components;
using Flowbite.Base;

namespace Flowbite.Components;

/// <summary>
/// A skeleton component for indicating loading states with placeholder elements that mimic the content being loaded.
/// </summary>
/// <remarks>
/// The Skeleton component provides visual feedback for loading states by displaying placeholder elements
/// that resemble the actual content. This creates a better user experience compared to blank spaces or spinners.
/// It supports various patterns including text, images, videos, and cards.
/// </remarks>
/// <example>
/// <code>
/// &lt;Skeleton&gt;
///     &lt;SkeletonText /&gt;
/// &lt;/Skeleton&gt;
/// </code>
/// </example>
/// <accessibility>
/// The skeleton is automatically assigned appropriate ARIA attributes for accessibility:
/// - role="status" indicates a loading state
/// - "Loading..." text is included for screen readers via sr-only class
/// </accessibility>
public partial class Skeleton : FlowbiteComponentBase
{
    /// <summary>
    /// The content to be displayed as skeleton placeholders.
    /// </summary>
    /// <remarks>
    /// Use this to compose skeleton layouts with various helper components like
    /// SkeletonText, SkeletonImage, SkeletonVideo, etc.
    /// </remarks>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Additional attributes that will be merged with the component's native attributes.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }
}
