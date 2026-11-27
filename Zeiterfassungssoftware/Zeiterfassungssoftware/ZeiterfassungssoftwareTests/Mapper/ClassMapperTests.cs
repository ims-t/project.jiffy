using Zeiterfassungssoftware.Data.Jiffy.Models;
using Zeiterfassungssoftware.SharedData.Classes;
using Zeiterfassungssoftware.SharedData.RequiredTimes;

namespace Zeiterfassungssoftware.Mapper.Tests
{
    [TestClass()]
    public class ClassMapperTests
    {
        #region FromDTO Tests

        [TestMethod()]
        public void FromDTONullArgumentTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => ClassMapper.FromDTO(null));
        }

        [TestMethod()]
        public void FromDTOValid()
        {
            var requiredTimeDto = new RequiredTimeDto()
            {
                Id = Guid.NewGuid(),
                ClassId = Guid.NewGuid(),
                DayOfWeek = DayOfWeek.Monday,
                Required = TimeSpan.FromHours(8)
            };

            var classDto = new ClassDto()
            {
                Id = Guid.NewGuid(),
                Name = "Computer Science 101",
                RequiredTimes = new List<RequiredTimeDto> { requiredTimeDto }
            };

            var requiredTime = new RequiredTime()
            {
                Id = requiredTimeDto.Id,
                ClassId = requiredTimeDto.ClassId,
                DayOfWeek = requiredTimeDto.DayOfWeek,
                Required = requiredTimeDto.Required
            };

            var classEntity = new Class()
            {
                Id = classDto.Id,
                Name = classDto.Name,
                RequiredTimes = new List<RequiredTime> { requiredTime }
            };

            Assert.AreEqual(classEntity, ClassMapper.FromDTO(classDto));
        }

        [TestMethod()]
        public void FromDTOValidWithEmptyCollection()
        {
            var classDto = new ClassDto()
            {
                Id = Guid.NewGuid(),
                Name = "Computer Science 101",
                RequiredTimes = new List<RequiredTimeDto>()
            };

            var classEntity = new Class()
            {
                Id = classDto.Id,
                Name = classDto.Name,
                RequiredTimes = new List<RequiredTime>()
            };

            var result = ClassMapper.FromDTO(classDto);
            Assert.AreEqual(classEntity.Id, result.Id);
            Assert.AreEqual(classEntity.Name, result.Name);
            Assert.AreEqual(0, result.RequiredTimes.Count);
        }

        [TestMethod()]
        public void FromDTOValidWithMultipleRequiredTimes()
        {
            var classId = Guid.NewGuid();
            var requiredTimeDto1 = new RequiredTimeDto()
            {
                Id = Guid.NewGuid(),
                ClassId = classId,
                DayOfWeek = DayOfWeek.Monday,
                Required = TimeSpan.FromHours(8)
            };

            var requiredTimeDto2 = new RequiredTimeDto()
            {
                Id = Guid.NewGuid(),
                ClassId = classId,
                DayOfWeek = DayOfWeek.Tuesday,
                Required = TimeSpan.FromHours(6)
            };

            var classDto = new ClassDto()
            {
                Id = classId,
                Name = "Computer Science 101",
                RequiredTimes = new List<RequiredTimeDto> { requiredTimeDto1, requiredTimeDto2 }
            };

            var result = ClassMapper.FromDTO(classDto);
            Assert.AreEqual(classDto.Id, result.Id);
            Assert.AreEqual(classDto.Name, result.Name);
            Assert.AreEqual(2, result.RequiredTimes.Count);
        }

        [TestMethod()]
        public void FromDTONotEqual()
        {
            var classDto = new ClassDto()
            {
                Id = Guid.NewGuid(),
                Name = "Computer Science 101",
                RequiredTimes = new List<RequiredTimeDto>()
            };

            var classEntity = new Class()
            {
                Id = Guid.NewGuid(),
                Name = "Mathematics 201",
                RequiredTimes = new List<RequiredTime>()
            };

            Assert.AreNotEqual(classEntity, ClassMapper.FromDTO(classDto));
        }

        #endregion

        #region ToDTO Tests

        [TestMethod()]
        public void ToDTONullArgumentTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => ClassMapper.ToDTO(null));
        }

        [TestMethod()]
        public void ToDTOValid()
        {
            var classId = Guid.NewGuid();
            var requiredTime = new RequiredTime()
            {
                Id = Guid.NewGuid(),
                ClassId = classId,
                DayOfWeek = DayOfWeek.Monday,
                Required = TimeSpan.FromHours(8),
                ValidUntil = DateTime.Today.AddYears(1)
            };

            var classEntity = new Class()
            {
                Id = classId,
                Name = "Computer Science 101",
                RequiredTimes = new List<RequiredTime> { requiredTime }
            };

            var requiredTimeDto = new RequiredTimeDto()
            {
                Id = requiredTime.Id,
                ClassId = requiredTime.ClassId,
                DayOfWeek = requiredTime.DayOfWeek,
                Required = requiredTime.Required
            };

            var classDto = new ClassDto()
            {
                Id = classEntity.Id,
                Name = classEntity.Name,
                RequiredTimes = new List<RequiredTimeDto> { requiredTimeDto }
            };

            Assert.AreEqual(classDto, ClassMapper.ToDTO(classEntity));
        }

        [TestMethod()]
        public void ToDTOValidWithEmptyCollection()
        {
            var classEntity = new Class()
            {
                Id = Guid.NewGuid(),
                Name = "Computer Science 101",
                RequiredTimes = new List<RequiredTime>()
            };

            var classDto = new ClassDto()
            {
                Id = classEntity.Id,
                Name = classEntity.Name,
                RequiredTimes = new List<RequiredTimeDto>()
            };

            var result = ClassMapper.ToDTO(classEntity);
            Assert.AreEqual(classDto.Id, result.Id);
            Assert.AreEqual(classDto.Name, result.Name);
            Assert.AreEqual(0, result.RequiredTimes.Count);
        }

        [TestMethod()]
        public void ToDTOValidWithMultipleRequiredTimes()
        {
            var classId = Guid.NewGuid();
            var requiredTime1 = new RequiredTime()
            {
                Id = Guid.NewGuid(),
                ClassId = classId,
                DayOfWeek = DayOfWeek.Monday,
                Required = TimeSpan.FromHours(8),
                ValidUntil = DateTime.Today.AddYears(1)
            };

            var requiredTime2 = new RequiredTime()
            {
                Id = Guid.NewGuid(),
                ClassId = classId,
                DayOfWeek = DayOfWeek.Tuesday,
                Required = TimeSpan.FromHours(6),
                ValidUntil = DateTime.Today.AddYears(1)
            };

            var classEntity = new Class()
            {
                Id = classId,
                Name = "Computer Science 101",
                RequiredTimes = new List<RequiredTime> { requiredTime1, requiredTime2 }
            };

            var result = ClassMapper.ToDTO(classEntity);
            Assert.AreEqual(classEntity.Id, result.Id);
            Assert.AreEqual(classEntity.Name, result.Name);
            Assert.AreEqual(2, result.RequiredTimes.Count);
        }

        [TestMethod()]
        public void ToDTONotEqual()
        {
            var classEntity = new Class()
            {
                Id = Guid.NewGuid(),
                Name = "Computer Science 101",
                RequiredTimes = new List<RequiredTime>()
            };

            var classDto = new ClassDto()
            {
                Id = Guid.NewGuid(),
                Name = "Mathematics 201",
                RequiredTimes = new List<RequiredTimeDto>()
            };

            Assert.AreNotEqual(classDto, ClassMapper.ToDTO(classEntity));
        }

        #endregion

        #region ValidateDTO Tests

        [TestMethod()]
        public void ValidateDTONullTest()
        {
            Assert.IsFalse(ClassMapper.ValidateDTO(null));
        }

        [TestMethod()]
        public void ValidateDTOValidTest()
        {
            var classDto = new ClassDto()
            {
                Id = Guid.NewGuid(),
                Name = "Computer Science 101",
                RequiredTimes = new List<RequiredTimeDto>()
            };

            Assert.IsTrue(ClassMapper.ValidateDTO(classDto));
        }

        [TestMethod()]
        public void ValidateDTOEmptyNameTest()
        {
            var classDto = new ClassDto()
            {
                Id = Guid.NewGuid(),
                Name = string.Empty,
                RequiredTimes = new List<RequiredTimeDto>()
            };

            Assert.IsFalse(ClassMapper.ValidateDTO(classDto));
        }

        [TestMethod()]
        public void ValidateDTONullNameTest()
        {
            var classDto = new ClassDto()
            {
                Id = Guid.NewGuid(),
                Name = null,
                RequiredTimes = new List<RequiredTimeDto>()
            };

            Assert.IsFalse(ClassMapper.ValidateDTO(classDto));
        }

        [TestMethod()]
        public void ValidateDTOWhitespaceNameTest()
        {
            var classDto = new ClassDto()
            {
                Id = Guid.NewGuid(),
                Name = "   ",
                RequiredTimes = new List<RequiredTimeDto>()
            };

            Assert.IsFalse(ClassMapper.ValidateDTO(classDto));
        }

        #endregion
    }
}