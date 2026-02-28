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

        #region - Timer -

        private Stopwatch _stopwatch = new Stopwatch();
        private string LoadingTime = "00:00:000";

        private void StartTimer() => _stopwatch.Start();

        private void StopTimer()
        {
            _stopwatch.Stop();
            LoadingTime = FormatTime(_stopwatch.Elapsed);
        }

        private void ResetTimer() => _stopwatch.Reset();

        private string FormatTime(TimeSpan timeSpan)
        {
            return $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}:{timeSpan.Milliseconds:D3}";
        }

        #endregion

        #region - Dialog -

        [CascadingParameter]
        public IMudDialogInstance? MudDialog { get; set; }

        public void Close() => MudDialog?.Close();

        public void Close(DialogResult result) => MudDialog?.Close(result);

        #endregion

        // ***************************************************

        [Parameter]
        public Type? DialogContentType { get; set; }

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
