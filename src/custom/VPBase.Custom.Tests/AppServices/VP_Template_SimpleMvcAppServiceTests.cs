using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using VPBase.Custom.Core.Data.Entities;
using VPBase.Custom.Server.Areas.Custom.Models.ViewModels.VP_Template_SimpleMvc;
using VPBase.Custom.Server.Areas.Custom.WebAppServices;
using VPBase.Custom.Tests.Helpers;
using VPBase.Shared.Core.Helpers.Validation;
using VPBase.Shared.Core.Types;

namespace VPBase.Custom.Tests.AppServices
{
    [TestFixture]
    public class VP_Template_SimpleMvcAppServiceTests : MemoryCustomDbTestBase
    {
        private VP_Template_SimpleMvcWebAppService _vpTemplateMvcWebVpTemplateMvcWebAppService;

        [SetUp]
        public new void SetUp()
        {
            _vpTemplateMvcWebVpTemplateMvcWebAppService = ServiceProvider.GetService<VP_Template_SimpleMvcWebAppService>();
        }

        [Test]
        public void When_getting_all_Custom_VP_Template_SimpleMvc_items__all_except_the_deleted_ones_will_be_returned()
        {
            AddTestData();

            var notDeletedCount = Storage.VP_Template_SimpleMvcs.Count(x => x.TenantId == TenantTestId && x.DeletedUtc == null);

            var returnModel = _vpTemplateMvcWebVpTemplateMvcWebAppService.GetList(null, null, true, null, null, 0, 100, SortType.CreatedUtc_Asc, TenantTestId);

            Assert.That(returnModel.Data, Is.Not.Null);
            Assert.That(returnModel.Data.Count(), Is.EqualTo(notDeletedCount));
        }

        [TestCase(1, TenantTestId)]
        [TestCase(2, "ANOTHER_TENANT")]
        [TestCase(3, "NOT_EXISTING_TENANT")]
        public void When_getting_all_Custom_VP_Template_SimpleMvc_items_for_tenant__all_items_for_that_tenant_is_returned(int testRow, string tenantId)
        {
            AddTestData();

            Console.WriteLine($"Executing row: {testRow}");

            var dbList = Storage.VP_Template_SimpleMvcs.Where(x => x.TenantId == tenantId && x.DeletedUtc == null);

            var returnModel = _vpTemplateMvcWebVpTemplateMvcWebAppService.GetList(null, null, true, null, null, 0, 100, SortType.CreatedUtc_Asc, tenantId);

            foreach (var dbItem in dbList)
            {
                Assert.That(returnModel.Data.Any(x => x.VP_Template_SimpleMvcId == dbItem.VP_Template_SimpleMvcId), Is.True);
            }
        }

        [TestCase(1, 0, 1)]
        [TestCase(2, 0, 2)]
        [TestCase(3, 1, 2)]
        [TestCase(4, 2, 2)]
        [TestCase(5, 0, 4)]
        public void When_getting_all_Custom_VP_Template_SimpleMvc_items_with_skip_and_take_parameters__all_except_the_deleted_ones_will_be_returned
            (int testRow, int skip, int take)
        {
            AddTestData();

            Console.WriteLine($"Executing row: {testRow}");

            var returnModel = _vpTemplateMvcWebVpTemplateMvcWebAppService.GetList(null, null, true, null, null, skip, take, SortType.CreatedUtc_Asc, TenantTestId);

            Assert.That(returnModel.Data.Count(), Is.EqualTo(take));
        }

        [TestCase(1, SortType.CreatedUtc_Asc, "Custom_VP_Template_SimpleMvc_4")]
        [TestCase(2, SortType.CreatedUtc_Desc, "Custom_VP_Template_SimpleMvc_1")]
        public void When_getting_all_Custom_VP_Template_SimpleMvc_items_with_sort_parameter__will_be_sorted_as_expected
            (int testRow, SortType sort, string expectedId)
        {
            AddTestData();

            Console.WriteLine($"Executing row: {testRow}");

            var returnModel = _vpTemplateMvcWebVpTemplateMvcWebAppService.GetList(null, null, true, null, null, 0, 100, sort, TenantTestId);

            Assert.That(returnModel.Data.First().VP_Template_SimpleMvcId, Is.EqualTo(expectedId));
        }

        [Test]
        public void When_getting_a_Custom_VP_Template_SimpleMvc_item_with_valid_id__expected_item_will_be_returned()
        {
            AddTestData();

            var testWithItem = Storage.VP_Template_SimpleMvcs.First(x => x.DeletedUtc == null);

            var item = _vpTemplateMvcWebVpTemplateMvcWebAppService.GetEditModel(testWithItem.VP_Template_SimpleMvcId, testWithItem.TenantId);

            Assert.That(item, Is.Not.Null);
            Assert.That(item.VP_Template_SimpleMvcId, Is.EqualTo(testWithItem.VP_Template_SimpleMvcId));
        }

        [TestCase(1, "", TenantTestId)]
        [TestCase(1, "123123123123", TenantTestId)]
        [TestCase(1, null, TenantTestId)]
        public void When_getting_a_Custom_VP_Template_SimpleMvc_item_with_invalid_id__null_will_be_returned(int testRow, string id, string tenantId)
        {
            AddTestData();

            Console.WriteLine($"Executing row: {testRow}");

            var item = _vpTemplateMvcWebVpTemplateMvcWebAppService.GetEditModel(id, tenantId);

            Assert.That(item, Is.Null);
        }

        [Test]
        public void Saving_Custom_VP_Template_SimpleMvc_is_not_allowed_if_its_already_is_updated()
        {
            Storage.Add(new VP_Template_SimpleMvc(TenantTestId)
            {
                VP_Template_SimpleMvcId = "1",
                Title = "Test 1",
                CreatedUtc = DateTimeProvider.Now(),
                ModifiedUtc = DateTimeProvider.Now()
            });

            Storage.SaveChanges();

            var returnModel = _vpTemplateMvcWebVpTemplateMvcWebAppService.Edit(new VP_Template_SimpleMvcAddOrEditViewModel
            {
                VP_Template_SimpleMvcId = "1",
                Title = "Test 1",
                ModifiedUtcDate = DateTimeProvider.Now().AddSeconds(-35)
            }, TenantTestId);

            Assert.That(returnModel.Errors.Any());
            Assert.That(returnModel.Errors.First(), Is.EqualTo(StandardErrors.EntityHasBeenModified));
        }

        [Test]
        public void Saving_Custom_VP_Template_SimpleMvc_is_successful_if_its_not_updated()
        {
            var modifiedDate = DateTimeProvider.Now();
            Storage.Add(new VP_Template_SimpleMvc(TenantTestId)
            {
                VP_Template_SimpleMvcId = "1",
                Title = "Test 1",
                CreatedUtc = DateTimeProvider.Now(),
                ModifiedUtc = modifiedDate
            });

            Storage.SaveChanges();

            var returnModel = _vpTemplateMvcWebVpTemplateMvcWebAppService.Edit(new VP_Template_SimpleMvcAddOrEditViewModel
            {
                VP_Template_SimpleMvcId = "1",
                Title = "Test 1",
                ModifiedUtcDate = modifiedDate
            }, TenantTestId);

            Assert.That(!returnModel.Errors.Any());
        }

        [Test]
        public void Deleting_Custom_VP_Template_SimpleMvc_is_successful_if_its_not_updated()
        {
            Storage.Add(new VP_Template_SimpleMvc(TenantTestId)
            {
                VP_Template_SimpleMvcId = "1",
                Title = "Test 1",
                CreatedUtc = DateTimeProvider.Now(),
                ModifiedUtc = DateTimeProvider.Now().AddSeconds(-10)
            });

            Storage.SaveChanges();

            var result = _vpTemplateMvcWebVpTemplateMvcWebAppService.Delete("1", TenantTestId);

            Assert.That(!result.Errors.Any());
        }

        private void AddTestData()
        {
            Storage.Add(new VP_Template_SimpleMvc(TenantTestId)
            {
                VP_Template_SimpleMvcId = "Custom_VP_Template_SimpleMvc_1",
                Title = "Test 1",
                IsActive = true,
                CreatedUtc = DateTimeProvider.Now().AddDays(-1),
                ModifiedUtc = DateTimeProvider.Now().AddDays(-1)
            });
            Storage.Add(new VP_Template_SimpleMvc(TenantTestId)
            {
                VP_Template_SimpleMvcId = "Custom_VP_Template_SimpleMvc_2",
                Title = "Test 2",
                IsActive = true,
                CreatedUtc = DateTimeProvider.Now().AddDays(-2),
                ModifiedUtc = DateTimeProvider.Now().AddDays(-2)
            });
            Storage.Add(new VP_Template_SimpleMvc(TenantTestId)
            {
                VP_Template_SimpleMvcId = "Custom_VP_Template_SimpleMvc_3",
                Title = "Test 3",
                IsActive = true,
                CreatedUtc = DateTimeProvider.Now().AddDays(-3),
                ModifiedUtc = DateTimeProvider.Now().AddDays(-3)
            });
            Storage.Add(new VP_Template_SimpleMvc(TenantTestId)
            {
                VP_Template_SimpleMvcId = "Custom_VP_Template_SimpleMvc_4",
                Title = "Test 4",
                IsActive = true,
                CreatedUtc = DateTimeProvider.Now().AddDays(-4),
                ModifiedUtc = DateTimeProvider.Now().AddDays(-4)
            });
            Storage.Add(new VP_Template_SimpleMvc(TenantTestId)
            {
                VP_Template_SimpleMvcId = "Custom_VP_Template_SimpleMvc_5",
                Title = "Test 5",
                IsActive = true,
                CreatedUtc = DateTimeProvider.Now().AddDays(-5),
                ModifiedUtc = DateTimeProvider.Now().AddDays(-5),
                DeletedUtc = DateTimeProvider.Now()
            });
            Storage.Add(new VP_Template_SimpleMvc(TenantTestId)
            {
                VP_Template_SimpleMvcId = "Custom_VP_Template_SimpleMvc_6",
                Title = "Test 6",
                IsActive = true,
                CreatedUtc = DateTimeProvider.Now().AddDays(-6),
                ModifiedUtc = DateTimeProvider.Now().AddDays(-6),
                DeletedUtc = DateTimeProvider.Now()
            });
            Storage.Add(new VP_Template_SimpleMvc(TenantTestId)
            {
                VP_Template_SimpleMvcId = "Custom_VP_Template_SimpleMvc_7",
                Title = "Test 7",
                IsActive = true,
                CreatedUtc = DateTimeProvider.Now().AddDays(-7),
                ModifiedUtc = DateTimeProvider.Now().AddDays(-7),
                DeletedUtc = DateTimeProvider.Now()
            });
            Storage.Add(new VP_Template_SimpleMvc("ANOTHER_TENANT")
            {
                VP_Template_SimpleMvcId = "Custom_VP_Template_SimpleMvc_8",
                Title = "Test 8",
                IsActive = true,
                CreatedUtc = DateTimeProvider.Now().AddDays(-8),
                ModifiedUtc = DateTimeProvider.Now().AddDays(-8)
            });

            Storage.SaveChanges();
        }
    }
}
