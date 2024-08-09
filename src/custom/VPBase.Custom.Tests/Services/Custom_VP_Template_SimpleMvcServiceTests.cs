using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using VPBase.Custom.Core.Data.Entities;
using VPBase.Custom.Core.Models.VP_Template_SimpleMvc;
using VPBase.Custom.Core.Services.VP_Template_SimpleMvcService;
using VPBase.Custom.Tests.Helpers;
using VPBase.Shared.Core.Helpers.Validation;

namespace VPBase.Custom.Tests.Services
{
    [TestFixture]
    public class Custom_VP_Template_SimpleMvcServiceTests : MemoryCustomDbTestBase
    {
        private VP_Template_SimpleMvcService _custom_VP_Template_SimpleMvcService;

        private string _custom_VP_Template_SimpleMvcId = "Custom_VP_Template_SimpleMvc_1";

        private VP_Template_SimpleMvc _dbItemTemplate;

        [SetUp]
        public new void SetUp()
        {
            _custom_VP_Template_SimpleMvcService = ServiceProvider.GetService<VP_Template_SimpleMvcService>();

            _dbItemTemplate = new VP_Template_SimpleMvc(TenantTestId)
            {
                Title = "Test",
                VP_Template_SimpleMvcId = "Custom_VP_Template_SimpleMvc_",
                CreatedUtc = DateTimeProvider.Now().AddDays(-1),
                ModifiedUtc = DateTimeProvider.Now().AddDays(-1)
            };
        }

        [Test]
        public void When_deleting_existing_Custom_VP_Template_SimpleMvc__entity_will_be_marked_with_deleted_date()
        {
            _dbItemTemplate.VP_Template_SimpleMvcId = _custom_VP_Template_SimpleMvcId;
            Storage.Add(_dbItemTemplate);
            Storage.SaveChanges();
            var modifiedBeforeDeleted = _dbItemTemplate.ModifiedUtc;

            var serverResponse = _custom_VP_Template_SimpleMvcService.Delete(_custom_VP_Template_SimpleMvcId, TenantTestId);

            Console.WriteLine("Custom_VP_Template_Basic deleted: " + serverResponse);

            Assert.That(serverResponse.IsValid, Is.True);

            var item = Storage.VP_Template_SimpleMvcs.First(x => x.VP_Template_SimpleMvcId == _custom_VP_Template_SimpleMvcId);

            Console.WriteLine("Custom_VP_Template_Basic deleted date utc: " + item.DeletedUtc);

            Assert.That(item.DeletedUtc, Is.Not.Null);
            Assert.That(item.ModifiedUtc, Is.GreaterThan(modifiedBeforeDeleted));
        }

        [TestCase("1234567890")]
        [TestCase("")]
        [TestCase(null)]
        public void When_deleting_an_Custom_VP_Template_SimpleMvc_that_does_not_exist_in_database__error_will_be_returned(string itemId)
        {
            var serverResponse = _custom_VP_Template_SimpleMvcService.Delete(itemId, TenantTestId);
            
            Assert.That(serverResponse.Errors.Count, Is.EqualTo(1));
            Assert.That(serverResponse.Errors.First(), Is.EqualTo(StandardErrors.EntityNotFound));
        }

        [Test]
        public void When_inserting_an_Custom_VP_Template_Mvc_with_valid_values__entity_will_exist_in_database()
        {
            var id = "Custom_VP_Template_SimpleMvc_2";

            var addModel = new VP_Template_SimpleMvcAddModel()
            {
                Title = "Title",
                VP_Template_SimpleMvcId = id,
            };

            var getModel = _custom_VP_Template_SimpleMvcService.Add(addModel, TenantTestId);

            //Assert.IsNotNull(getModel.Title);
            //Assert.IsNotEmpty(getModel.Title);
            //Assert.IsNotNull(getModel.VP_Template_BasicId);
            //Assert.IsNotEmpty(getModel.VP_Template_BasicId);

            var item = Storage.VP_Template_SimpleMvcs.First(x => x.VP_Template_SimpleMvcId == id && x.TenantId == TenantTestId);

            Assert.That(item, Is.Not.Null);
            Assert.That(item.Title, Is.Not.Null.And.Not.Empty);
            Assert.That(item.VP_Template_SimpleMvcId, Is.Not.Null.And.Not.Empty);
            Assert.That(item.ModifiedUtc, Is.GreaterThan(new DateTime(2010, 1, 1)));
        }

        private static List<TestCaseData> Custom_VP_Template_SimpleMvcAddList = new List<TestCaseData>(){
            new TestCaseData(1, new VP_Template_SimpleMvcAddModel() {VP_Template_SimpleMvcId = "1", Title = ""}, new List<string> {"Title is required"}),
            new TestCaseData(2, new VP_Template_SimpleMvcAddModel() {VP_Template_SimpleMvcId = "1", Title = null}, new List<string> {"Title is required"}),
        };
        [Test, TestCaseSource("Custom_VP_Template_SimpleMvcAddList")]
        public void When_adding_Custom_VP_Template_SimpleMvc_to_database_with_invalid_data__error_will_be_returned(int testRow, VP_Template_SimpleMvcAddModel vpTemplateMvcAddModel, List<string> errors)
        {
            Console.WriteLine($"Executing row: {testRow}");

            var serverResponse = _custom_VP_Template_SimpleMvcService.Add(vpTemplateMvcAddModel, TenantTestId);
            var gotExpectedErrors = errors.All(x => serverResponse.Errors.Contains(x));

            if (gotExpectedErrors)
            {
                return;
            }

            Console.WriteLine("Expected errors and actual errors do not match.\n");
            Console.WriteLine("Expected errors:");
            foreach (var error in errors)
            {
                Console.WriteLine(error);
            }

            Console.WriteLine("\n--------------------------------\n");

            Console.WriteLine("Actual errors:");
            foreach (var error in serverResponse.Errors)
            {
                Console.WriteLine(error);
            }

            Assert.Fail();
        }

        [Test]
        public void When_updating_an_Custom_VP_Tamplate_Mvc_with_valid_values__entity_will_be_updated_in_database()
        {
            var addDbModel = _dbItemTemplate;
            addDbModel.VP_Template_SimpleMvcId += "1";
            Storage.Add(addDbModel);
            Storage.SaveChanges();

            var updateModel = new VP_Template_SimpleMvcUpdateModel()
            {
                Title = "Title Updated",
                VP_Template_SimpleMvcId = addDbModel.VP_Template_SimpleMvcId,
                ModifiedUtcDate = DateTimeProvider.Now()
            };

            var getModel = _custom_VP_Template_SimpleMvcService.Update(updateModel, TenantTestId);

            //Assert.IsNotNull(getModel.Title);
            //Assert.IsNotEmpty(getModel.Title);
            //Assert.AreEqual(updateModel.Title, getModel.Title);
            //Assert.IsNotNull(getModel.VP_Template_BasicId);
            //Assert.IsNotEmpty(getModel.VP_Template_BasicId);

            var item = Storage.VP_Template_SimpleMvcs.First(x => x.VP_Template_SimpleMvcId == addDbModel.VP_Template_SimpleMvcId && x.TenantId == TenantTestId);

            Assert.That(item, Is.Not.Null);
            Assert.That(item.Title, Is.Not.Null.And.Not.Empty);
            Assert.That(item.Title, Is.EqualTo(updateModel.Title));
            Assert.That(item.VP_Template_SimpleMvcId, Is.Not.Null.And.Not.Empty);
            Assert.That(item.ModifiedUtc, Is.GreaterThan(new DateTime(2010, 1, 1)));
        }

        private static List<TestCaseData> Custom_VP_Template_SimpleMvcUpdateList = new List<TestCaseData>(){
            new TestCaseData(1, new VP_Template_SimpleMvcUpdateModel() {VP_Template_SimpleMvcId = "1", Title = ""}, new List<string> { StandardErrors.EntityNotFound, "Title is required" }),
            new TestCaseData(2, new VP_Template_SimpleMvcUpdateModel() {VP_Template_SimpleMvcId = "1", Title = null}, new List<string> { StandardErrors.EntityNotFound, "Title is required" }),
            new TestCaseData(3, new VP_Template_SimpleMvcUpdateModel() {VP_Template_SimpleMvcId = "1", Title = "APA"}, new List<string> { StandardErrors.EntityNotFound}),
        };
        [Test, TestCaseSource("Custom_VP_Template_SimpleMvcUpdateList")]
        public void When_updating_Custom_VP_Template_SimpleMvc_to_database_with_invalid_data__error_keys_will_be_retured_with_exception(int testRow, VP_Template_SimpleMvcUpdateModel vpTemplateMvcUpdateModel, List<string> errors)
        {
            Console.WriteLine($"Executing row: {testRow}");

            var serverResponse = _custom_VP_Template_SimpleMvcService.Update(vpTemplateMvcUpdateModel, TenantTestId);
            var gotExpectedErrors = serverResponse.Errors.All(x => errors.Contains(x));

            if (gotExpectedErrors)
            {
                return;
            }

            Console.WriteLine("Expected errors and actual errors do not match.\n");
            Console.WriteLine("Expected errors:");
            foreach (var error in errors)
            {
                Console.WriteLine(error);
            }

            Console.WriteLine("\n--------------------------------\n");

            Console.WriteLine("Actual errors:");
            foreach (var error in serverResponse.Errors)
            {
                Console.WriteLine(error);
            }

            Assert.Fail();
        }
    }
}
