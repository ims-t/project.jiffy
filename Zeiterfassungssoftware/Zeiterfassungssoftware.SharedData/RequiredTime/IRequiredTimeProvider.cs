namespace Zeiterfassungssoftware.SharedData.RequiredTimes
{
    public interface IRequiredTimeProvider
    {
        public bool IsLoaded { get; set; }

        public Task<RequiredTimeDto> CreateRequiredTime(RequiredTimeDto requiredTime);
        public Task<RequiredTimeDto> UpdateRequiredTime(Guid id, RequiredTimeDto requiredTime);
        public Task DeleteRequiredTime(Guid id);
        public Task<RequiredTimeDto> GetRequiredTimeById(Guid id);
        public List<RequiredTimeDto> GetRequiredTimes();

    }
}
