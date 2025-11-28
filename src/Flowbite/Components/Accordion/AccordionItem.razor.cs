using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Flowbite.Base;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Flowbite.Components.Accordion;

/// <summary>
/// Individual accordion item component that can be expanded or collapsed.
/// </summary>
public partial class AccordionItem : FlowbiteComponentBase
{
    [CascadingParameter] private AccordionContext? _context { get; set; }

    private bool _isOpen;
    private string? _itemId;

    /// <summary>
    /// Unique identifier for this accordion item. If not provided, one will be generated.
    /// </summary>
    [Parameter]
    public string? Id { get; set; }

    /// <summary>
    /// The header content of the accordion item.
    /// </summary>
    [Parameter]
    public RenderFragment? HeaderContent { get; set; }

    /// <summary>
    /// The body content of the accordion item.
    /// </summary>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    /// <summary>
    /// Optional custom icon to display instead of the default chevron icon.
    /// </summary>
    [Parameter]
    public IconBase? Icon { get; set; }

    /// <summary>
    /// Custom CSS classes to apply to the header button. These will be merged with the default classes.
    /// </summary>
    [Parameter]
    public string? HeaderClass { get; set; }

    /// <summary>
    /// Custom CSS classes to apply to the body/content area. These will be merged with the default classes.
    /// </summary>
    [Parameter]
    public string? BodyClass { get; set; }

    /// <summary>
    /// Custom CSS classes for the header text color. Overrides the default color scheme.
    /// Example: "text-blue-600 dark:text-blue-400"
    /// </summary>
    [Parameter]
    public string? HeaderTextColor { get; set; }

    /// <summary>
    /// Custom CSS classes for the header hover state. Overrides the default hover colors.
    /// Example: "hover:bg-blue-50 hover:text-blue-700"
    /// </summary>
    [Parameter]
    public string? HeaderHoverColor { get; set; }

    /// <summary>
    /// Custom CSS classes for the header active/open state. Overrides the default active colors.
    /// Example: "bg-blue-100 text-blue-900 dark:bg-blue-900/20 dark:text-blue-300"
    /// </summary>
    [Parameter]
    public string? HeaderActiveColor { get; set; }

    /// <summary>
    /// Whether this item is open by default (uncontrolled mode).
    /// </summary>
    [Parameter]
    public bool DefaultOpen { get; set; }

    /// <summary>
    /// Whether this item is open (controlled mode). Use with OpenChanged for two-way binding.
    /// </summary>
    [Parameter]
    public bool? Open { get; set; }

    /// <summary>
    /// Event callback fired when the open state changes.
    /// </summary>
    [Parameter]
    public EventCallback<bool> OpenChanged { get; set; }

    /// <summary>
    /// Additional attributes to be applied to the accordion item.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    private AccordionStyle Style => _context?.Style ?? AccordionStyle.Default;
    private AccordionColor Color => _context?.Color ?? AccordionColor.Default;
    private bool Animate => _context?.Animate ?? true;

    private string GetItemId() => _itemId ?? string.Empty;
    private bool GetIsOpen() => _context?.IsItemOpen(GetItemId()) ?? DefaultOpen;
    private string GetHeaderId() => $"{GetItemId()}-heading";
    private string GetBodyId() => $"{GetItemId()}-body";

    private (bool IsFirst, bool IsLast, int Index) GetPosition()
    {
        if (_context is null || _itemId is null)
            return (false, false, -1);
        
        return _context.RegisterItem(_itemId);
    }

    protected override void OnInitialized()
    {
        _itemId = Id ?? $"accordion-item-{Guid.NewGuid()}";
        
        if (Open.HasValue)
        {
            _isOpen = Open.Value;
        }
        else
        {
            _isOpen = DefaultOpen;
        }

        if (_context is not null)
        {
            _context.SetInitialState(_itemId, _isOpen);
        }
    }

    protected override void OnParametersSet()
    {
        if (Open.HasValue)
        {
            _isOpen = Open.Value;
            if (_context is not null)
            {
                if (_isOpen && !_context.IsItemOpen(_itemId!))
                {
                    _context.SetInitialState(_itemId!, true);
                }
                else if (!_isOpen && _context.IsItemOpen(_itemId!))
                {
                    _context.SetInitialState(_itemId!, false);
                }
            }
        }
        else
        {
            _isOpen = _context?.IsItemOpen(_itemId!) ?? DefaultOpen;
        }
    }

    private async Task HandleToggle()
    {
        if (_context is null || _itemId is null)
            return;

        await _context.ToggleItemAsync(_itemId);
        _isOpen = _context.IsItemOpen(_itemId);

        if (OpenChanged.HasDelegate)
        {
            await OpenChanged.InvokeAsync(_isOpen);
        }

        StateHasChanged();
    }

    private async Task HandleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter" || e.Key == " ")
        {
            await HandleToggle();
        }
    }

    private string GetItemClasses()
    {
        var classes = new List<string> { "mb-2" };
        return CombineClasses(string.Join(" ", classes));
    }

    private string GetHeaderClasses()
    {
        var classes = new List<string>
        {
            "flex items-center justify-between w-full p-5 font-medium rtl:text-right",
            "gap-3 transition-colors"
        };

        // Apply custom colors if provided, otherwise use color enum
        if (!string.IsNullOrEmpty(HeaderTextColor))
        {
            classes.Add(HeaderTextColor);
        }
        else
        {
            // Apply color variants from enum
            switch (Color)
            {
                case AccordionColor.Primary:
                    classes.Add("text-gray-500 dark:text-gray-400");
                    break;
                case AccordionColor.Gray:
                case AccordionColor.Default:
                default:
                    classes.Add("text-gray-500 dark:text-gray-400");
                    break;
            }
        }

        // Apply custom hover colors if provided
        if (!string.IsNullOrEmpty(HeaderHoverColor))
        {
            classes.Add(HeaderHoverColor);
            classes.Add("focus:outline-none focus:ring-2 focus:ring-gray-200 dark:focus:ring-gray-800");
        }
        else
        {
            // Apply color variants from enum
            switch (Color)
            {
                case AccordionColor.Primary:
                    classes.Add("hover:bg-primary-50 hover:text-primary-700 focus:outline-none focus:ring-2 focus:ring-primary-200");
                    classes.Add("dark:hover:bg-primary-900/20 dark:hover:text-primary-300 dark:focus:ring-primary-800");
                    break;
                case AccordionColor.Gray:
                case AccordionColor.Default:
                default:
                    classes.Add("hover:bg-gray-100 hover:text-gray-900 focus:outline-none focus:ring-2 focus:ring-gray-200");
                    classes.Add("dark:hover:bg-gray-800 dark:hover:text-white dark:focus:ring-gray-800");
                    break;
            }
        }

        var (isFirst, isLast, _) = GetPosition();

        switch (Style)
        {
            case AccordionStyle.Separated:
                classes.Add("rounded-lg border border-gray-200 shadow-sm dark:border-gray-700");
                break;
            case AccordionStyle.Flush:
                classes.Add("border-0 border-b border-gray-200 dark:border-gray-700");
                break;
            case AccordionStyle.Default:
            default:
                if (isFirst)
                {
                    // First item: rounded top, no top border, no side borders, only bottom border
                    classes.Add("rounded-t-lg border border-t-0 border-x-0 border-b border-gray-200 dark:border-gray-700");
                }
                else if (isLast)
                {
                    // Last item: no borders
                    // No border classes needed
                }
                else
                {
                    // Middle items: no top, no sides, only bottom border
                    classes.Add("border border-x-0 border-b border-gray-200 border-t-0 dark:border-gray-700");
                }
                break;
        }

        // Add active state classes
        if (GetIsOpen())
        {
            if (!string.IsNullOrEmpty(HeaderActiveColor))
            {
                classes.Add(HeaderActiveColor);
            }
            else
            {
                // Apply color variants from enum
                switch (Color)
                {
                    case AccordionColor.Primary:
                        classes.Add("text-primary-700 dark:text-primary-300 bg-primary-50 dark:bg-primary-900/20");
                        break;
                    case AccordionColor.Gray:
                    case AccordionColor.Default:
                    default:
                        classes.Add("text-gray-900 dark:text-white bg-gray-100 dark:bg-gray-800");
                        break;
                }
            }
        }

        // Add custom header classes if provided
        if (!string.IsNullOrEmpty(HeaderClass))
        {
            classes.Add(HeaderClass);
        }

        return CombineClasses(string.Join(" ", classes));
    }

    private string GetBodyAnimationClasses()
    {
        if (!Animate)
        {
            // No animation - use simple display toggle
            return string.Empty;
        }

        var classes = new List<string> { "transition-all duration-300 ease-in-out" };
        
        if (GetIsOpen())
        {
            classes.Add("max-h-[2000px] opacity-100");
        }
        else
        {
            classes.Add("max-h-0 opacity-0");
        }
        
        return string.Join(" ", classes);
    }

    private string GetBodyStyle()
    {
        if (!Animate)
        {
            // No animation - use simple display toggle
            if (GetIsOpen())
            {
                return "display: block;";
            }
            else
            {
                return "display: none;";
            }
        }
        
        // With animation, ensure no space when closed - use overflow hidden on body
        return string.Empty;
    }

    private string GetBodyClasses()
    {
        var classes = new List<string>();
        var (isFirst, isLast, _) = GetPosition();

        switch (Style)
        {
            case AccordionStyle.Separated:
                classes.Add("border border-t-0 border-gray-200 rounded-b-lg dark:border-gray-700");
                break;
            case AccordionStyle.Flush:
                classes.Add("border-0 border-b border-gray-200 dark:border-gray-700");
                break;
            case AccordionStyle.Default:
            default:
                if (isLast)
                {
                    // Last item: no border on body div itself
                    // Border will be on content div instead
                }
                else
                {
                    // First and middle items: no side borders, no top border, only bottom border
                    classes.Add("border border-x-0 border-t-0 border-b border-gray-200 dark:border-gray-700");
                }
                break;
        }

        // Add custom body classes if provided
        if (!string.IsNullOrEmpty(BodyClass))
        {
            classes.Add(BodyClass);
        }

        return string.Join(" ", classes);
    }

    private string GetContentClasses()
    {
        var classes = new List<string> { "p-4 md:p-5" };
        var (isFirst, isLast, _) = GetPosition();

        if (Style == AccordionStyle.Default && isLast)
        {
            // Last item's content div needs top border
            classes.Add("border border-t border-b-0 border-x-0 border-gray-200 dark:border-gray-700");
        }

        return string.Join(" ", classes);
    }

    private string GetIconClasses()
    {
        var classes = new List<string> { "w-5 h-5 shrink-0" };
        
        if (Animate)
        {
            classes.Add("transition-transform duration-200");
        }
        
        if (_isOpen)
        {
            classes.Add("rotate-180");
        }

        return string.Join(" ", classes);
    }
}

