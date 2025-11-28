using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
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
            "text-gray-500 dark:text-gray-400",
            "hover:bg-gray-100 hover:text-gray-900 focus:outline-none focus:ring-2 focus:ring-gray-200",
            "dark:hover:bg-gray-800 dark:hover:text-white dark:focus:ring-gray-800",
            "gap-3 transition-colors"
        };

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
        if (_isOpen)
        {
            classes.Add("text-gray-900 dark:text-white bg-gray-100 dark:bg-gray-800");
        }

        return CombineClasses(string.Join(" ", classes));
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
        
        if (_isOpen)
        {
            classes.Add("rotate-180");
        }

        return string.Join(" ", classes);
    }
}

