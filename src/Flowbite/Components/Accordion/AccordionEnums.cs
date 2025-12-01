namespace Flowbite.Components.Accordion;

/// <summary>
/// Defines the behavior mode of the accordion.
/// </summary>
public enum AccordionMode
{
    /// <summary>
    /// Collapse mode: only one item can be open at a time.
    /// </summary>
    Collapse,

    /// <summary>
    /// Always open mode: multiple items can be open simultaneously.
    /// </summary>
    AlwaysOpen
}

/// <summary>
/// Defines the visual style variant of the accordion.
/// </summary>
public enum AccordionStyle
{
    /// <summary>
    /// Default accordion style with borders and rounded corners.
    /// </summary>
    Default,

    /// <summary>
    /// Separated cards style where each item appears as a separate card.
    /// </summary>
    Separated,

    /// <summary>
    /// Flush style with no borders or rounded corners.
    /// </summary>
    Flush
}

/// <summary>
/// Defines the color variants available for the accordion.
/// </summary>
public enum AccordionColor
{
    /// <summary>
    /// Default color scheme.
    /// </summary>
    Default,

    /// <summary>
    /// Primary color scheme.
    /// </summary>
    Primary,

    /// <summary>
    /// Gray color scheme.
    /// </summary>
    Gray
}

