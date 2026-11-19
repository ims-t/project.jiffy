using Zeiterfassungssoftware.Data.Jiffy.Models;
using Zeiterfassungssoftware.SharedData.RequiredTimes;

namespace Zeiterfassungssoftware.Mapper
{
    public class RequiredTimeMapper
    {
        public static RequiredTime FromDTO(RequiredTimeDto RequiredTimeDto)
        {
            if (RequiredTimeDto is null)
                throw new ArgumentNullException();

            return new()
            {
                Id = RequiredTimeDto.Id,
                ClassId = RequiredTimeDto.ClassId,
                Should = RequiredTimeDto.Should,
                DayOfWeek = RequiredTimeDto.DayOfWeek,
            };
        }

        public static RequiredTimeDto ToDTO(RequiredTime RequiredTime)
        {
            if (RequiredTime is null)
                throw new ArgumentNullException();

            return new()
            {
                Id = RequiredTime.Id,
                ClassId = RequiredTime.ClassId,
                Should = RequiredTime.Should,
                DayOfWeek = RequiredTime.DayOfWeek,
            };
        }

        public static bool ValidateDto(RequiredTimeDto RequiredTimeDto)
        {
            if (RequiredTimeDto is null)
                return false;

            if(RequiredTimeDto.Should <= TimeSpan.FromSeconds(0))
                return false;

            return true;
        }
    }
}
