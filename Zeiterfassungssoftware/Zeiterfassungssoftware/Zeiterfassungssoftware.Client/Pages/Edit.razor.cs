using Microsoft.AspNetCore.Components;
using Zeiterfassungssoftware.SharedData.Times;

namespace Zeiterfassungssoftware.Client.Pages
{
    public partial class Edit
    {
        public readonly string TIME_FORMAT = @"HH\:mm\.ss dd\.MM\.yyyy";

        [Parameter]
        public Guid Id { get; set; }

        [Inject]
        public ITimeEntryProvider TimeEntrySource { get; set; }

        [Inject]
        private NavigationManager Navigation { get; set; }

        public TimeEntryDto? Entry { get; set; }
        public DateOnly StartDate { get; set; }
        public TimeOnly StartTime { get; set; }
        public DateOnly? EndDate { get; set; }
        public TimeOnly? EndTime { get; set; }


        public Timer? Timer { get; set; }

        public bool DisplayDeleteModal { get; set; }

        public bool DisplayJustificationModal { get; set; }

        private DateOnly _originalStartDate;
        private TimeOnly _originalStartTime;
        private DateOnly? _originalEndDate;
        private TimeOnly? _originalEndTime;
        private string _originalTitle = string.Empty;
        private string _originalDescription = string.Empty;

        private bool HasChanges =>
            StartDate != _originalStartDate ||
            StartTime != _originalStartTime ||
            EndDate != _originalEndDate ||
            EndTime != _originalEndTime ||
            !string.Equals(Entry?.Title, _originalTitle, StringComparison.Ordinal) ||
            !string.Equals(Entry?.Description, _originalDescription, StringComparison.Ordinal);

        protected override async Task OnInitializedAsync()
        {
            Entry = await TimeEntrySource.GetEntryById(Id);
            StartDate = DateOnly.FromDateTime(Entry.Start);
            StartTime = TimeOnly.FromDateTime(Entry.Start);
            
            DateTime End = Entry.End.GetValueOrDefault(DateTime.Now);
            EndDate = DateOnly.FromDateTime(End);
            EndTime = TimeOnly.FromDateTime(End);
            _originalStartDate = StartDate;
            _originalStartTime = StartTime;
            _originalEndDate = EndDate;
            _originalEndTime = EndTime;
            _originalTitle = Entry.Title;
            _originalDescription = Entry.Description;
        }

        private void UpdateEntry()
        {
            if (Entry is null)
                return;

            Entry.Start = new DateTime(StartDate, StartTime);

            if (EndDate is null || EndTime is null)
                Entry.End = DateTime.Now;
            else
                Entry.End = EndDate.Value.ToDateTime(EndTime.Value);
        }

        private void SaveChanges()
        {
            if (!HasChanges)
            {
                Navigation.NavigateTo("/history");
                return;
            }
            DisplayJustificationModal = true;
        }

        private void DeleteClicked()
        {
            DisplayDeleteModal = true;
        }

        private async void DeleteEntry()
        {
            if (Entry is null)
                return;

            await TimeEntrySource.DeleteEntry(Entry.Id);
            Navigation.NavigateTo("/history");
        }
        private async Task UpdateEntry_Confirm(string justification)
        {
            if (Entry is null || string.IsNullOrWhiteSpace(justification))
                return;

            UpdateEntry();
            Entry.Justification = justification;

            await TimeEntrySource.UpdateEntry(Entry.Id, Entry);
            DisplayJustificationModal = false;
            Navigation.NavigateTo("/history");
        }

    }
}
