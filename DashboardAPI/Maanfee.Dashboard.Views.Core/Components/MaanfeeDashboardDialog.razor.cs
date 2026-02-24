using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using MudBlazor;
using System.Diagnostics;

namespace Maanfee.Dashboard.Views.Core
{
    public partial class MaanfeeDashboardDialog : _BaseView
    {
        protected override async Task OnInitializedAsync()
        {
            StartTimer();
            await base.OnInitializedAsync();

            IsLoaded = true;
            StopTimer();
        }

        // ********************* Timer *********************

        private Stopwatch _stopwatch = new Stopwatch();
        private string LoadingTime = "00:00:000";

        public void StartTimer() => _stopwatch.Start();
        
        public void StopTimer()
        {
            _stopwatch.Stop();
            LoadingTime = TimeSpan.FromSeconds(_stopwatch.ElapsedMilliseconds).ToString(@"mm\:ss");
        }

        public void ResetTimer() => _stopwatch.Reset();

        // ***************************************************

        [Parameter]
        public Type? DialogContentType { get; set; }

        [CascadingParameter]
        public IMudDialogInstance? MudDialog { get; set; }

        public void Close() => MudDialog?.Close();

        public void Close(DialogResult result) => MudDialog?.Close(result);

        // ***************************************************

        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object>? AdditionalAttributes { get; set; }

        [Parameter]
        public bool IsDialogLoaded { get; set; }

        [Parameter]
        public string? Title { get; set; }

        // ***************************************************

        [Parameter]
        public RenderFragment? MaanfeeDashboardDialogHeaderContent { get; set; }

        [Parameter]
        public RenderFragment? MaanfeeDashboardDialogHeaderAction { get; set; }

        [Parameter]
        public RenderFragment? MaanfeeDashboardDialogContent { get; set; }

        [Parameter]
        public RenderFragment? MaanfeeDashboardDialogActionContent { get; set; }

        // ********************* Events *********************

        [Parameter]
        public EventCallback<KeyboardEventArgs> OnKeyDown { get; set; }

        // ***************************************************

        private async Task OpenInformationDialog()
        {
            DialogParameters DialogParameters = new DialogParameters();
            DialogParameters.Add("DialogContentType", DialogContentType != null ? DialogContentType : null);

            await Dialog!.ShowAsync<DialogInformation>(string.Empty, DialogParameters, new DialogOptions()
            {
                NoHeader = true,
                MaxWidth = MaxWidth.Small,
                FullWidth = true,
                Position = DialogPosition.Center,
                BackgroundClass = "Dialog-Blur",
                CloseOnEscapeKey = true,
            });
        }
    }
}
