using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Zeiterfassungssoftware.Controller;
using Zeiterfassungssoftware.Data;
using Zeiterfassungssoftware.Data.Jiffy.Models;
using Zeiterfassungssoftware.SharedData.RequiredTimes;

namespace Zeiterfassungssoftware.Controller.Tests
{
    [TestClass]
    public class RequiredTimesControllerTests
    {
        private ApplicationDbContext _context;
        private RequiredTimesController _controller;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _controller = new RequiredTimesController(_context);
        }

        [TestCleanup]
        public void Cleanup()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [TestMethod]
        public async Task GetRequiredTimes_ReturnsEmptyList_WhenNoValidRequiredTimesExist()
        {
            var result = await _controller.GetRequiredTimes();

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var RequiredTimes = okResult.Value as List<RequiredTimeDto>;
            Assert.IsNotNull(RequiredTimes);
            Assert.AreEqual(0, RequiredTimes.Count);
        }

        [TestMethod]
        public async Task GetRequiredTimes_ReturnsOnlyValidRequiredTimes()
        {
            var validRequiredTime = new RequiredTime
            {
                Id = Guid.NewGuid(),
                DayOfWeek = DayOfWeek.Monday,
                ValidUntil = DateTime.Now.AddDays(30)
            };
            var expiredRequiredTime = new RequiredTime
            {
                Id = Guid.NewGuid(),
                DayOfWeek = DayOfWeek.Tuesday,
                ValidUntil = DateTime.Now.AddDays(-30)
            };
            _context.RequiredTimes.AddRange(validRequiredTime, expiredRequiredTime);
            await _context.SaveChangesAsync();

            var result = await _controller.GetRequiredTimes();

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var RequiredTimes = okResult.Value as List<RequiredTimeDto>;
            Assert.IsNotNull(RequiredTimes);
            Assert.AreEqual(1, RequiredTimes.Count);
        }

        [TestMethod]
        public async Task GetRequiredTimes_ReturnsAllValidRequiredTimes()
        {
            var RequiredTime1 = new RequiredTime
            {
                Id = Guid.NewGuid(),
                DayOfWeek = DayOfWeek.Monday,
                ValidUntil = DateTime.Now.AddDays(30)
            };
            var RequiredTime2 = new RequiredTime
            {
                Id = Guid.NewGuid(),
                DayOfWeek = DayOfWeek.Tuesday,
                ValidUntil = DateTime.Now.AddDays(30)
            };
            _context.RequiredTimes.AddRange(RequiredTime1, RequiredTime2);
            await _context.SaveChangesAsync();

            var result = await _controller.GetRequiredTimes();

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var RequiredTimes = okResult.Value as List<RequiredTimeDto>;
            Assert.IsNotNull(RequiredTimes);
            Assert.AreEqual(2, RequiredTimes.Count);
        }

        [TestMethod]
        public async Task GetRequiredTimeById_ReturnsRequiredTime_WhenValidRequiredTimeExists()
        {
            var id = Guid.NewGuid();
            var RequiredTime = new RequiredTime
            {
                Id = id,
                DayOfWeek = DayOfWeek.Monday,
                ValidUntil = DateTime.Now.AddDays(30)
            };
            _context.RequiredTimes.Add(RequiredTime);
            await _context.SaveChangesAsync();

            var result = await _controller.GetRequiredTimeById(id);

            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);
            var RequiredTimeDto = okResult.Value as RequiredTimeDto;
            Assert.IsNotNull(RequiredTimeDto);
        }

        [TestMethod]
        public async Task GetRequiredTimeById_ReturnsNotFound_WhenRequiredTimeDoesNotExist()
        {
            var result = await _controller.GetRequiredTimeById(Guid.NewGuid());

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task GetRequiredTimeById_ReturnsNotFound_WhenRequiredTimeIsExpired()
        {
            var id = Guid.NewGuid();
            var RequiredTime = new RequiredTime
            {
                Id = id,
                DayOfWeek = DayOfWeek.Monday,
                ValidUntil = DateTime.Now.AddDays(-30)
            };
            _context.RequiredTimes.Add(RequiredTime);
            await _context.SaveChangesAsync();

            var result = await _controller.GetRequiredTimeById(id);

            Assert.IsInstanceOfType(result.Result, typeof(NotFoundResult));
        }

        


        [TestMethod]
        public async Task DeleteRequiredTime_ReturnsNotFound_WhenRequiredTimeDoesNotExist()
        {
            var result = await _controller.DeleteRequiredTime(Guid.NewGuid());

            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }

        [TestMethod]
        public async Task DeleteRequiredTime_InvalidatesRequiredTime_WhenRequiredTimeExists()
        {
            var id = Guid.NewGuid();
            var RequiredTime = new RequiredTime
            {
                Id = id,
                DayOfWeek = DayOfWeek.Monday,
                ValidUntil = DateTime.Now.AddDays(30)
            };
            _context.RequiredTimes.Add(RequiredTime);
            await _context.SaveChangesAsync();

            var result = await _controller.DeleteRequiredTime(id);

            Assert.IsInstanceOfType(result, typeof(NoContentResult));

            var deletedRequiredTime = await _context.RequiredTimes.FindAsync(id);
            Assert.IsNotNull(deletedRequiredTime);
            Assert.IsTrue(deletedRequiredTime.ValidUntil <= DateTime.Now);
        }

        [TestMethod]
        public async Task DeleteRequiredTime_DoesNotPhysicallyDeleteRecord()
        {
            var id = Guid.NewGuid();
            var RequiredTime = new RequiredTime
            {
                Id = id,
                DayOfWeek = DayOfWeek.Monday,
                ValidUntil = DateTime.Now.AddDays(30)
            };
            _context.RequiredTimes.Add(RequiredTime);
            await _context.SaveChangesAsync();

            await _controller.DeleteRequiredTime(id);

            var RequiredTimeStillExists = await _context.RequiredTimes.FindAsync(id);
            Assert.IsNotNull(RequiredTimeStillExists);
        }
    }
}