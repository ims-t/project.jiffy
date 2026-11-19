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
            var RequiredTimeDto = new RequiredTimeDto()
            {
                Id = Guid.NewGuid(),
                ClassId = Guid.NewGuid(),
                DayOfWeek = DayOfWeek.Monday,
                Should = TimeSpan.FromHours(8)
            };

            var classDto = new ClassDto()
            {
                Id = Guid.NewGuid(),
                Name = "Computer Science 101",
                RequiredTimes = new List<RequiredTimeDto> { RequiredTimeDto }
            };

            var RequiredTime = new RequiredTime()
            {
                Id = RequiredTimeDto.Id,
                ClassId = RequiredTimeDto.ClassId,
                DayOfWeek = RequiredTimeDto.DayOfWeek,
                Should = RequiredTimeDto.Should
            };

            var classEntity = new Class()
            {
                Id = classDto.Id,
                Name = classDto.Name,
                RequiredTimes = new List<RequiredTime> { RequiredTime }
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
            var RequiredTimeDto1 = new RequiredTimeDto()
            {
                Id = Guid.NewGuid(),
                ClassId = classId,
                DayOfWeek = DayOfWeek.Monday,
                Should = TimeSpan.FromHours(8)
            };

            var RequiredTimeDto2 = new RequiredTimeDto()
            {
                Id = Guid.NewGuid(),
                ClassId = classId,
                DayOfWeek = DayOfWeek.Tuesday,
                Should = TimeSpan.FromHours(6)
            };

            var classDto = new ClassDto()
            {
                Id = classId,
                Name = "Computer Science 101",
                RequiredTimes = new List<RequiredTimeDto> { RequiredTimeDto1, RequiredTimeDto2 }
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
            var RequiredTime = new RequiredTime()
            {
                Id = Guid.NewGuid(),
                ClassId = classId,
                DayOfWeek = DayOfWeek.Monday,
                Should = TimeSpan.FromHours(8),
                ValidUntil = DateTime.Today.AddYears(1)
            };

            var classEntity = new Class()
            {
                Id = classId,
                Name = "Computer Science 101",
                RequiredTimes = new List<RequiredTime> { RequiredTime }
            };

            var RequiredTimeDto = new RequiredTimeDto()
            {
                Id = RequiredTime.Id,
                ClassId = RequiredTime.ClassId,
                DayOfWeek = RequiredTime.DayOfWeek,
                Should = RequiredTime.Should
            };

            var classDto = new ClassDto()
            {
                Id = classEntity.Id,
                Name = classEntity.Name,
                RequiredTimes = new List<RequiredTimeDto> { RequiredTimeDto }
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
            var RequiredTime1 = new RequiredTime()
            {
                Id = Guid.NewGuid(),
                ClassId = classId,
                DayOfWeek = DayOfWeek.Monday,
                Should = TimeSpan.FromHours(8),
                ValidUntil = DateTime.Today.AddYears(1)
            };

            var RequiredTime2 = new RequiredTime()
            {
                Id = Guid.NewGuid(),
                ClassId = classId,
                DayOfWeek = DayOfWeek.Tuesday,
                Should = TimeSpan.FromHours(6),
                ValidUntil = DateTime.Today.AddYears(1)
            };

            var classEntity = new Class()
            {
                Id = classId,
                Name = "Computer Science 101",
                RequiredTimes = new List<RequiredTime> { RequiredTime1, RequiredTime2 }
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