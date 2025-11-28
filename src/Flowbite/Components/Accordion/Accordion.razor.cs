using Microsoft.AspNetCore.Components;
using System.Collections.Generic;

namespace Flowbite.Components.Accordion;

/// <summary>
/// Accordion component for displaying collapsible content sections.
/// </summary>
public partial class Accordion : FlowbiteComponentBase
{
    private AccordionContext _context = null!;

    /// <summary>
    /// The behavior mode of the accordion.
    /// </summary>
    [Parameter]
    public AccordionMode Mode { get; set; } = AccordionMode.Collapse;

    /// <summary>
    /// The visual style variant of the accordion.
    /// </summary>
    [Parameter]
    public AccordionStyle Style { get; set; } = AccordionStyle.Default;

    /// <summary>
    /// The color variant of the accordion.
    /// </summary>
    [Parameter]
    public AccordionColor Color { get; set; } = AccordionColor.Default;

    /// <summary>
    /// Child content containing AccordionItem components.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Event callback fired when an accordion item is opened.
    /// </summary>
    [Parameter]
    public EventCallback<string> OnItemOpened { get; set; }

    /// <summary>
    /// Event callback fired when an accordion item is closed.
    /// </summary>
    [Parameter]
    public EventCallback<string> OnItemClosed { get; set; }

    /// <summary>
    /// Event callback fired when an accordion item is toggled.
    /// </summary>
    [Parameter]
    public EventCallback<string> OnItemToggled { get; set; }

    /// <summary>
    /// Additional attributes to be applied to the accordion container.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    protected override void OnInitialized()
    {
        _context = new AccordionContext
        {
            Mode = Mode,
            Style = Style,
            Color = Color,
            OnItemOpened = OnItemOpened,
            OnItemClosed = OnItemClosed,
            OnItemToggled = OnItemToggled
        };
    }

    protected override void OnParametersSet()
    {
        if (_context is not null)
        {
            _context.Mode = Mode;
            _context.Style = Style;
            _context.Color = Color;
            _context.OnItemOpened = OnItemOpened;
            _context.OnItemClosed = OnItemClosed;
            _context.OnItemToggled = OnItemToggled;
        }
    }

    /// <summary>
    /// Opens an accordion item by ID programmatically.
    /// </summary>
    /// <param name="itemId">The ID of the accordion item to open</param>
    public async Task OpenItemAsync(string itemId)
    {
        if (_context is not null)
        {
            await _context.OpenItemAsync(itemId);
            StateHasChanged();
        }
    }

    /// <summary>
    /// Closes an accordion item by ID programmatically.
    /// </summary>
    /// <param name="itemId">The ID of the accordion item to close</param>
    public async Task CloseItemAsync(string itemId)
    {
        if (_context is not null)
        {
            await _context.CloseItemAsync(itemId);
            StateHasChanged();
        }
    }

    /// <summary>
    /// Toggles an accordion item by ID programmatically.
    /// </summary>
    /// <param name="itemId">The ID of the accordion item to toggle</param>
    public async Task ToggleItemAsync(string itemId)
    {
        if (_context is not null)
        {
            await _context.ToggleItemAsync(itemId);
            StateHasChanged();
        }
    }

    private string GetAccordionClasses()
    {
        var classes = new List<string> { "flowbite-accordion" };

        switch (Style)
        {
            case AccordionStyle.Separated:
                // Separated cards style - each item will have its own card styling
                break;
            case AccordionStyle.Flush:
                // Flush style - no borders
                classes.Add("divide-y divide-gray-200 dark:divide-gray-700");
                break;
            case AccordionStyle.Default:
            default:
                // Match official Flowbite: rounded-base border border-default overflow-hidden shadow-xs
                classes.Add("rounded-lg border border-gray-200 overflow-hidden shadow-sm dark:border-gray-700");
                break;
        }

        return CombineClasses(string.Join(" ", classes));
    }
}

