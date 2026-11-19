using Zeiterfassungssoftware.Data.Jiffy.Models;
using Zeiterfassungssoftware.SharedData.Classes;

namespace Zeiterfassungssoftware.Mapper
{
    public class ClassMapper
    {
        public static Class FromDTO(ClassDto classDto)
        {
            if (classDto is null)
                throw new ArgumentNullException();

            return new()
            {
                Id = classDto.Id,
                Name = classDto.Name,
                RequiredTimes = classDto.RequiredTimes.Select(e => RequiredTimeMapper.FromDTO(e)).ToList()
            };
        }

        public static ClassDto ToDTO(Class _class)
        {
            if (_class is null)
                throw new ArgumentNullException();

            return new()
            {
                Id = _class.Id,
                Name = _class.Name,
                RequiredTimes = _class.RequiredTimes.Where(e => e.ValidUntil > DateTime.Now).Select(e => RequiredTimeMapper.ToDTO(e)).ToList()
            };
        }

        public static bool ValidateDTO(ClassDto classDto)
        {
            if (classDto is null)
                return false;

            return (!string.IsNullOrWhiteSpace(classDto.Name));
        }
    }
}
