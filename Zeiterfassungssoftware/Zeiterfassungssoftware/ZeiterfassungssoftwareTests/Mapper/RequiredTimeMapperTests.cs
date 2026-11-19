using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Zeiterfassungssoftware.Client.Pages;
using Zeiterfassungssoftware.Data;
using Zeiterfassungssoftware.Data.Jiffy.Models;
using Zeiterfassungssoftware.Mapper;
using Zeiterfassungssoftware.SharedData.RequiredTimes;
using Zeiterfassungssoftware.SharedData.Users;

namespace Zeiterfassungssoftware.Mapper.Tests
{
    [TestClass()]
    public class RequiredTimeMapperTests
    {
        [TestMethod()]
        public void ToDTONullArgumentTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => RequiredTimeMapper.ToDTO(null));
        }

        [TestMethod()]
        public void FromDTONullArgumentTest()
        {
            Assert.ThrowsException<ArgumentNullException>(() => RequiredTimeMapper.FromDTO(null));
        }

        [TestMethod()]
        public void ToDTOValid()
        {
            var Id = Guid.NewGuid();
            var ClassId = Guid.NewGuid();

            var RequiredTime = new RequiredTime()
            {
                Id = Id,
                ClassId = ClassId,
                DayOfWeek = DayOfWeek.Wednesday,
                Should = new TimeSpan(2, 0, 0),
                ValidUntil = DateTime.Now
            };

            var RequiredTimeDto = new RequiredTimeDto()
            {
                Id = Id,
                ClassId = ClassId,
                DayOfWeek = DayOfWeek.Wednesday,
                Should = new TimeSpan(2, 0, 0)
            };

            Assert.AreEqual(RequiredTimeDto, RequiredTimeMapper.ToDTO(RequiredTime));
        }

        [TestMethod()]
        public void FromDTOValid()
        {
            var Id = Guid.NewGuid();
            var ClassId = Guid.NewGuid();

            var RequiredTime = new RequiredTime()
            {
                Id = Id,
                ClassId = ClassId,
                DayOfWeek = DayOfWeek.Wednesday,
                Should = new TimeSpan(2, 0, 0),
                ValidUntil = DateTime.Now
            };

            var RequiredTimeDto = new RequiredTimeDto()
            {
                Id = Id,
                ClassId = ClassId,
                DayOfWeek = DayOfWeek.Wednesday,
                Should = new TimeSpan(2, 0, 0)
            };

            Assert.AreEqual(RequiredTime, RequiredTimeMapper.FromDTO(RequiredTimeDto));
        }

        [TestMethod()]
        public void FromDTONotEqual()
        {
            var RequiredTime = new RequiredTime()
            {
                Id = Guid.NewGuid(),
                ClassId = Guid.NewGuid(),
                DayOfWeek = DayOfWeek.Wednesday,
                Should = new TimeSpan(2, 0, 0),
                ValidUntil = DateTime.Now
            };

            var RequiredTimeDto = new RequiredTimeDto()
            {
                Id = Guid.NewGuid(),
                ClassId = Guid.NewGuid(),
                DayOfWeek = DayOfWeek.Wednesday,
                Should = new TimeSpan(2, 0, 0)
            };

            Assert.AreNotEqual(RequiredTime, RequiredTimeMapper.FromDTO(RequiredTimeDto));
        }

        [TestMethod()]
        public void ToDTONotEqual()
        {
            var RequiredTime = new RequiredTime()
            {
                Id = Guid.NewGuid(),
                ClassId = Guid.NewGuid(),
                DayOfWeek = DayOfWeek.Wednesday,
                Should = new TimeSpan(2, 0, 0),
                ValidUntil = DateTime.Now
            };

            var RequiredTimeDto = new RequiredTimeDto()
            {
                Id = Guid.NewGuid(),
                ClassId = Guid.NewGuid(),
                DayOfWeek = DayOfWeek.Wednesday,
                Should = new TimeSpan(2, 0, 0)
            };

            Assert.AreNotEqual(RequiredTimeDto, RequiredTimeMapper.ToDTO(RequiredTime));
        }
    }
}