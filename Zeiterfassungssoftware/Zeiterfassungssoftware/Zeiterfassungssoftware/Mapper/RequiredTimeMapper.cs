using Zeiterfassungssoftware.Data.Jiffy.Models;
using Zeiterfassungssoftware.SharedData.RequiredTimes;

namespace Zeiterfassungssoftware.Mapper
{
    public class RequiredTimeMapper
    {
        public static RequiredTime FromDTO(RequiredTimeDto requiredTimeDto)
        {
            if (requiredTimeDto is null)
                throw new ArgumentNullException();

            return new()
            {
                Id = requiredTimeDto.Id,
                ClassId = requiredTimeDto.ClassId,
                Required = requiredTimeDto.Required,
                DayOfWeek = requiredTimeDto.DayOfWeek,
            };
        }

        public static RequiredTimeDto ToDTO(RequiredTime requiredTime)
        {
            if (requiredTime is null)
                throw new ArgumentNullException();

            return new()
            {
                Id = requiredTime.Id,
                ClassId = requiredTime.ClassId,
                Required = requiredTime.Required,
                DayOfWeek = requiredTime.DayOfWeek,
            };
        }

        public static bool ValidateDto(RequiredTimeDto requiredTimeDto)
        {
            if (requiredTimeDto is null)
                return false;

            if(requiredTimeDto.Required <= TimeSpan.FromSeconds(0))
                return false;

            return true;
        }
    }
}
