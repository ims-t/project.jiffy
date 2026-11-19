using Microsoft.AspNetCore.Components;
using Zeiterfassungssoftware.SharedData.Classes;
using Zeiterfassungssoftware.SharedData.RequiredTimes;

namespace Zeiterfassungssoftware.Client.Pages
{
    public partial class Classes : ComponentBase, IDisposable
    {
        [Inject]
        public IClassProvider ClassSource { get; set; }
        [Inject]
        public IRequiredTimeProvider RequiredTimeSource { get; set; }

        public ClassDto SelectedClass { get; set; } = new();
        public Guid SelectedGuid { get; set; }

        public List<RequiredTimeDto> Original { get; set; } = new();

        public Timer? Timer { get; set; }

        public bool DisplayDeleteModal { get; set; }

        protected override void OnInitialized()
        {
            Timer = new Timer(UpdateTimer, null, 0, 200);
        }
        public void UpdateTimer(object? obj)
        {
            InvokeAsync(StateHasChanged);
        }

        public void SelectedClassChanged(ChangeEventArgs args)
        {
            SelectedGuid = Guid.Parse(args.Value.ToString());
            SelectedClass = SelectedGuid.Equals(Guid.Empty) ? new() : ClassSource.GetClasses().FirstOrDefault(e => e.Id.Equals(SelectedGuid)) ?? new();
            Original = new(SelectedClass.RequiredTimes);
        }

        public async Task SaveClass()
        {
            if (SelectedGuid == Guid.Empty)
            {
                var ConfirmedClass = await ClassSource.CreateClass(SelectedClass);
                for (int i = 0; i < SelectedClass.RequiredTimes.Count; i++)
                {
                    var RequiredTime = SelectedClass.RequiredTimes[i];
                    RequiredTime.ClassId = ConfirmedClass.Id;
                    var ConfirmedRequiredTime = await RequiredTimeSource.CreateRequiredTime(RequiredTime);
                    SelectedClass.RequiredTimes[i] = ConfirmedRequiredTime;
                }
                // ???
                SelectedClass = await ClassSource.UpdateClass(ConfirmedClass.Id, SelectedClass);
                SelectedClassChanged(new ChangeEventArgs { Value = Guid.Empty.ToString() });
            }
            else
            {
                // Temporary Fix
                foreach (var RequiredTime in Original)
                {
                    await RequiredTimeSource.DeleteRequiredTime(RequiredTime.Id);
                }

                for (int i = 0; i < SelectedClass.RequiredTimes.Count; i++)
                {
                    var RequiredTime = SelectedClass.RequiredTimes[i];
                    RequiredTime.ClassId = SelectedClass.Id;
                    var ConfirmedRequiredTime = await RequiredTimeSource.CreateRequiredTime(RequiredTime);
                    SelectedClass.RequiredTimes[i] = ConfirmedRequiredTime;
                }

                SelectedClass = await ClassSource.UpdateClass(SelectedClass.Id, SelectedClass);

                SelectedClassChanged(new ChangeEventArgs()
                {
                    Value = Guid.Empty.ToString()
                });
            }
        }
        private void DeleteClicked()
        {
            DisplayDeleteModal = true;
        }

        public void DeleteClass()
        {

            ClassSource.DeleteClass(SelectedClass.Id);

            SelectedClassChanged(new ChangeEventArgs()
            {
                Value = Guid.Empty.ToString()
            });

            DisplayDeleteModal = false;
        }

        void IDisposable.Dispose()
        {
            Timer?.Dispose();
        }

    }
}