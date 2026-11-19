using Microsoft.AspNetCore.Components;

namespace Zeiterfassungssoftware.Client.Components
{
    public partial class JustificationModal
    {
        [Parameter]
        public EventCallback<string> OnConfirm { get; set; }

        [Parameter]
        public EventCallback OnCancel { get; set; }

        private string Justification { get; set; } = string.Empty;

        private async Task OnConfirmClicked()
        {
            await OnConfirm.InvokeAsync(Justification);
        }

        private async Task OnCancelClicked()
        {
            await OnCancel.InvokeAsync();
        }
    }
}
