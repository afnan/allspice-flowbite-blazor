using Microsoft.AspNetCore.Components;
using Flowbite.Base;
using System.Linq;

namespace Flowbite.Components;

/// <summary>
/// A responsive navigation bar component with configurable options.
/// </summary>
/// <remarks>
/// Supports fluid width, rounded corners, border, and mobile menu toggling.
/// </remarks>
public partial class Navbar : FlowbiteComponentBase
{
    /// <summary>
    /// Determines if the navbar should use fluid container width, allowing it to stretch across the entire screen.
    /// </summary>
    /// <remarks>
    /// When set to true, the navbar will have full-width padding instead of being constrained to a fixed container.
    /// </remarks>
    /// <example>
    /// <code>
    /// &lt;Navbar Fluid="true"&gt;
    ///     &lt;!-- Navbar content --&gt;
    /// &lt;/Navbar&gt;
    /// </code>
    /// </example>
    [Parameter]
    public bool Fluid { get; set; }

    /// <summary>
    /// Determines if the navbar should have rounded corners.
    /// </summary>
    /// <remarks>
    /// Adds visual softness to the navbar's appearance by applying border-radius.
    /// </remarks>
    /// <example>
    /// <code>
    /// &lt;Navbar Rounded="true"&gt;
    ///     &lt;!-- Navbar with rounded corners --&gt;
    /// &lt;/Navbar&gt;
    /// </code>
    /// </example>
    [Parameter]
    public bool Rounded { get; set; }

    /// <summary>
    /// Determines if the navbar should have a border.
    /// </summary>
    /// <remarks>
    /// Adds a border around the navbar, which can help define its boundaries.
    /// </remarks>
    /// <example>
    /// <code>
    /// &lt;Navbar Border="true"&gt;
    ///     &lt;!-- Navbar with a border --&gt;
    /// &lt;/Navbar&gt;
    /// </code>
    /// </example>
    [Parameter]
    public bool Border { get; set; }

    /// <summary>
    /// Determines if the navbar should be fixed to the top of the viewport.
    /// </summary>
    /// <remarks>
    /// When set to true, the navbar will be fixed at the top of the page and remain visible when scrolling.
    /// </remarks>
    /// <example>
    /// <code>
    /// &lt;Navbar Sticky="true"&gt;
    ///     &lt;!-- Fixed navbar --&gt;
    /// &lt;/Navbar&gt;
    /// </code>
    /// </example>
    [Parameter]
    public bool Sticky { get; set; }

    /// <summary>
    /// Custom background color classes. Default uses Flowbite v4 design tokens (bg-neutral-primary).
    /// </summary>
    [Parameter]
    public string? BackgroundColor { get; set; }

    /// <summary>
    /// Custom border color classes. Default uses Flowbite v4 design tokens (border-default).
    /// </summary>
    [Parameter]
    public string? BorderColor { get; set; }

    /// <summary>
    /// Gets or sets the ID for the collapse element (for ARIA controls).
    /// </summary>
    [Parameter]
    public string CollapseId { get; set; } = "navbar-default";

    /// <summary>
    /// Additional attributes to be applied to the navbar element.
    /// </summary>
    [Parameter(CaptureUnmatchedValues = true)]
    public Dictionary<string, object>? AdditionalAttributes { get; set; }

    /// <summary>
    /// Gets or sets whether the mobile menu is open.
    /// </summary>
    /// <remarks>
    /// Controls the visibility of the mobile navigation menu.
    /// Useful for responsive designs and mobile-friendly navigation.
    /// </remarks>
    /// <example>
    /// <code>
    /// &lt;Navbar MenuOpen="@isMobileMenuOpen"&gt;
    ///     &lt;!-- Navbar content --&gt;
    /// &lt;/Navbar&gt;
    /// </code>
    /// </example>
    [Parameter]
    public bool MenuOpen { get; set; }

    /// <summary>
    /// Callback event triggered when the mobile menu's open state changes.
    /// </summary>
    /// <remarks>
    /// Allows parent components to respond to changes in the mobile menu's visibility.
    /// Provides the new open state as a boolean value.
    /// </remarks>
    /// <accessibility>
    /// Useful for managing focus states and updating aria attributes for screen readers.
    /// </accessibility>
    /// <example>
    /// <code>
    /// &lt;Navbar MenuOpenChanged="@HandleMobileMenuToggle"&gt;
    ///     &lt;!-- Navbar content --&gt;
    /// &lt;/Navbar&gt;
    /// 
    /// @code {
    ///     private void HandleMobileMenuToggle(bool isOpen)
    ///     {
    ///         // Handle mobile menu state change
    ///     }
    /// }
    /// </code>
    /// </example>
    [Parameter]
    public EventCallback<bool> MenuOpenChanged { get; set; }

    /// <summary>
    /// Event callback fired when the navbar menu is collapsed.
    /// </summary>
    /// <remarks>
    /// Equivalent to Flowbite JavaScript's `onCollapse` callback.
    /// </remarks>
    [Parameter]
    public EventCallback OnCollapse { get; set; }

    /// <summary>
    /// Event callback fired when the navbar menu is expanded.
    /// </summary>
    /// <remarks>
    /// Equivalent to Flowbite JavaScript's `onExpand` callback.
    /// </remarks>
    [Parameter]
    public EventCallback OnExpand { get; set; }

    /// <summary>
    /// Event callback fired when the navbar menu is toggled.
    /// </summary>
    /// <remarks>
    /// Equivalent to Flowbite JavaScript's `onToggle` callback.
    /// </remarks>
    [Parameter]
    public EventCallback<bool> OnToggle { get; set; }

    /// <summary>
    /// Child content to be rendered inside the navbar.
    /// </summary>
    /// <remarks>
    /// Allows flexible composition of navbar content, such as brand logos, navigation links, and toggle buttons.
    /// Supports responsive design by allowing dynamic content rendering.
    /// </remarks>
    /// <accessibility>
    /// Ensure that navbar content is keyboard navigable and screen reader friendly.
    /// Use appropriate ARIA attributes for complex navigation structures.
    /// </accessibility>
    /// <example>
    /// <code>
    /// &lt;Navbar&gt;
    ///     &lt;NavbarBrand&gt;My App&lt;/NavbarBrand&gt;
    ///     &lt;NavbarToggle /&gt;
    ///     &lt;NavbarCollapse&gt;
    ///         &lt;NavbarLink&gt;Home&lt;/NavbarLink&gt;
    ///         &lt;NavbarLink&gt;About&lt;/NavbarLink&gt;
    ///     &lt;/NavbarCollapse&gt;
    /// &lt;/Navbar&gt;
    /// </code>
    /// </example>
    [Parameter]
    public RenderFragment? ChildContent { get; set; }

    private string NavbarClasses => CombineClasses(string.Join(" ", new[]
    {
        BackgroundColor ?? "bg-neutral-primary",
        Sticky ? "fixed w-full z-20 top-0 start-0" : "",
        Border ? $"border-b {BorderColor ?? "border-default"}" : "",
        Rounded ? "rounded" : ""
    }.Where(c => !string.IsNullOrEmpty(c))));

    private string ContainerClasses => CombineClasses(string.Join(" ", new[]
    {
        Fluid ? "" : "max-w-screen-xl",
        "flex flex-wrap items-center justify-between",
        "mx-auto p-4"
    }.Where(c => !string.IsNullOrEmpty(c))));

    protected override void OnInitialized()
    {
        base.OnInitialized();
    }

    /// <summary>
    /// Expands the navbar menu programmatically.
    /// Equivalent to Flowbite JavaScript's `collapse.expand()` method.
    /// </summary>
    public async Task ExpandAsync()
    {
        if (!MenuOpen)
        {
            MenuOpen = true;
            await MenuOpenChanged.InvokeAsync(true);
            await OnExpand.InvokeAsync();
            await OnToggle.InvokeAsync(true);
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Collapses the navbar menu programmatically.
    /// Equivalent to Flowbite JavaScript's `collapse.collapse()` method.
    /// </summary>
    public async Task CollapseAsync()
    {
        if (MenuOpen)
        {
            MenuOpen = false;
            await MenuOpenChanged.InvokeAsync(false);
            await OnCollapse.InvokeAsync();
            await OnToggle.InvokeAsync(false);
            await InvokeAsync(StateHasChanged);
        }
    }

    /// <summary>
    /// Toggles the navbar menu programmatically.
    /// Equivalent to Flowbite JavaScript's `collapse.toggle()` method.
    /// </summary>
    public async Task ToggleAsync()
    {
        MenuOpen = !MenuOpen;
        var isOpen = MenuOpen;
        await MenuOpenChanged.InvokeAsync(isOpen);
        await OnToggle.InvokeAsync(isOpen);
        
        if (isOpen)
        {
            await OnExpand.InvokeAsync();
        }
        else
        {
            await OnCollapse.InvokeAsync();
        }
        
        await InvokeAsync(StateHasChanged);
    }

    internal async Task HandleMenuToggle()
    {
        await ToggleAsync();
    }
}
