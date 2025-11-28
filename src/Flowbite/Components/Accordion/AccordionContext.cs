using Microsoft.AspNetCore.Components;

namespace Flowbite.Components.Accordion;

/// <summary>
/// Context class for sharing accordion state between accordion components.
/// </summary>
public class AccordionContext
{
    /// <summary>
    /// The behavior mode of the accordion.
    /// </summary>
    public AccordionMode Mode { get; set; } = AccordionMode.Collapse;

    /// <summary>
    /// The visual style variant of the accordion.
    /// </summary>
    public AccordionStyle Style { get; set; } = AccordionStyle.Default;

    /// <summary>
    /// The color variant of the accordion.
    /// </summary>
    public AccordionColor Color { get; set; } = AccordionColor.Default;

    /// <summary>
    /// Dictionary to track which items are open by their ID.
    /// </summary>
    private readonly HashSet<string> _openItems = new();

    /// <summary>
    /// List of item IDs in order to track position.
    /// </summary>
    private readonly List<string> _itemIds = new();

    /// <summary>
    /// Event callback fired when an item is opened.
    /// </summary>
    public EventCallback<string>? OnItemOpened { get; set; }

    /// <summary>
    /// Event callback fired when an item is closed.
    /// </summary>
    public EventCallback<string>? OnItemClosed { get; set; }

    /// <summary>
    /// Event callback fired when an item is toggled.
    /// </summary>
    public EventCallback<string>? OnItemToggled { get; set; }

    /// <summary>
    /// Checks if an item with the specified ID is currently open.
    /// </summary>
    /// <param name="itemId">The ID of the accordion item</param>
    /// <returns>True if the item is open, false otherwise</returns>
    public bool IsItemOpen(string itemId)
    {
        return _openItems.Contains(itemId);
    }

    /// <summary>
    /// Toggles the open state of an accordion item.
    /// </summary>
    /// <param name="itemId">The ID of the accordion item to toggle</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task ToggleItemAsync(string itemId)
    {
        var wasOpen = _openItems.Contains(itemId);
        var isOpen = !wasOpen;

        if (Mode == AccordionMode.Collapse && isOpen)
        {
            // Close all other items in collapse mode
            var itemsToClose = _openItems.Where(id => id != itemId).ToList();
            foreach (var id in itemsToClose)
            {
                _openItems.Remove(id);
                if (OnItemClosed.HasValue)
                {
                    await OnItemClosed.Value.InvokeAsync(id);
                }
            }
        }

        if (isOpen)
        {
            _openItems.Add(itemId);
            if (OnItemOpened.HasValue)
            {
                await OnItemOpened.Value.InvokeAsync(itemId);
            }
        }
        else
        {
            _openItems.Remove(itemId);
            if (OnItemClosed.HasValue)
            {
                await OnItemClosed.Value.InvokeAsync(itemId);
            }
        }

        if (OnItemToggled.HasValue)
        {
            await OnItemToggled.Value.InvokeAsync(itemId);
        }
    }

    /// <summary>
    /// Opens an accordion item by ID.
    /// </summary>
    /// <param name="itemId">The ID of the accordion item to open</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task OpenItemAsync(string itemId)
    {
        if (!_openItems.Contains(itemId))
        {
            await ToggleItemAsync(itemId);
        }
    }

    /// <summary>
    /// Closes an accordion item by ID.
    /// </summary>
    /// <param name="itemId">The ID of the accordion item to close</param>
    /// <returns>A task representing the asynchronous operation</returns>
    public async Task CloseItemAsync(string itemId)
    {
        if (_openItems.Contains(itemId))
        {
            await ToggleItemAsync(itemId);
        }
    }

    /// <summary>
    /// Sets the initial open state of an item.
    /// </summary>
    /// <param name="itemId">The ID of the accordion item</param>
    /// <param name="isOpen">Whether the item should be open initially</param>
    public void SetInitialState(string itemId, bool isOpen)
    {
        if (isOpen)
        {
            _openItems.Add(itemId);
        }
        else
        {
            _openItems.Remove(itemId);
        }
    }

    /// <summary>
    /// Registers an item and returns its position information.
    /// </summary>
    /// <param name="itemId">The ID of the accordion item</param>
    /// <returns>Tuple containing (isFirst, isLast, index)</returns>
    public (bool IsFirst, bool IsLast, int Index) RegisterItem(string itemId)
    {
        if (!_itemIds.Contains(itemId))
        {
            _itemIds.Add(itemId);
        }
        
        var index = _itemIds.IndexOf(itemId);
        var isFirst = index == 0;
        var isLast = index == _itemIds.Count - 1;
        
        return (isFirst, isLast, index);
    }
}

