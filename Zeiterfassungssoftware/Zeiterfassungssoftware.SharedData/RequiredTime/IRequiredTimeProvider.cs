namespace Zeiterfassungssoftware.SharedData.RequiredTimes
{
    public interface IRequiredTimeProvider
    {
        public bool IsLoaded { get; set; }

        public Task<RequiredTimeDto> CreateRequiredTime(RequiredTimeDto RequiredTime);
        public Task<RequiredTimeDto> UpdateRequiredTime(Guid id, RequiredTimeDto RequiredTime);
        public Task DeleteRequiredTime(Guid id);
        public Task<RequiredTimeDto> GetRequiredTimeById(Guid id);
        public List<RequiredTimeDto> GetRequiredTimes();

    }
}
