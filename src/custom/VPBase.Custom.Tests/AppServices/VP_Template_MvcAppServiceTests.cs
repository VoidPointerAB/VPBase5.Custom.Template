using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using VPBase.Custom.Core.Data.Entities;
using VPBase.Custom.Server.Areas.Custom.Models.ViewModels.VP_Template_Mvc;
using VPBase.Custom.Server.Areas.Custom.WebAppServices;
using VPBase.Custom.Tests.Helpers;
using VPBase.Shared.Core.Helpers.Validation;
using VPBase.Shared.Core.Types;

namespace VPBase.Custom.Tests.AppServices
{
    public class VP_Template_MvcAppServiceTests : MemoryCustomDbTestBase
    {
        private VP_Template_MvcWebAppService _custom_VP_Template_MvcWebAppService;

        [SetUp]
        public new void SetUp()
        {
            _custom_VP_Template_MvcWebAppService = ServiceProvider.GetService<VP_Template_MvcWebAppService>();
        }

        [Test]
        public async Task When_getting_all_Custom_VP_Template_Mvc_items__all_except_the_deleted_ones_will_be_returned()
        {
            await AddTestDataAsync();

            var notDeletedCount = Storage.VP_Template_Mvcs.Count(x => x.TenantId == TenantTestId && x.DeletedUtc == null);

            var returnModel = await _custom_VP_Template_MvcWebAppService.GetListAsync(0, 100, SortType.CreatedUtc_Asc, TenantTestId);

            Assert.That(returnModel.Data, Is.Not.Null);
            Assert.That(returnModel.Data.Count, Is.EqualTo(notDeletedCount));
        }

        [TestCase(1, TenantTestId)]
        [TestCase(2, "ANOTHER_TENANT")]
        [TestCase(3, "NOT_EXISTING_TENANT")]
        public async Task When_getting_all_Custom_VP_Template_Mvc_items_for_tenant__all_items_for_that_tenant_is_returned(int testRow, string tenantId)
        {
            await AddTestDataAsync();

            Console.WriteLine($"Executing row: {testRow}");

            var dbList = Storage.VP_Template_Mvcs.Where(x => x.TenantId == tenantId && x.DeletedUtc == null);

            var returnModel = await _custom_VP_Template_MvcWebAppService.GetListAsync(0, 100, SortType.CreatedUtc_Asc, tenantId);

            foreach (var dbItem in dbList)
            {
                Assert.That(returnModel.Data.Any(x => x.VP_Template_MvcId == dbItem.VP_Template_MvcId), Is.True);
            }
        }

        [TestCase(1, 0, 1)]
        [TestCase(2, 0, 2)]
        [TestCase(3, 1, 2)]
        [TestCase(4, 2, 2)]
        [TestCase(5, 0, 4)]
        public async Task When_getting_all_Custom_VP_Template_Mvc_items_with_skip_and_take_parameters__all_except_the_deleted_ones_will_be_returned
            (int testRow, int skip, int take)
        {
            await AddTestDataAsync();

            Console.WriteLine($"Executing row: {testRow}");

            var returnModel = await _custom_VP_Template_MvcWebAppService.GetListAsync(skip, take, SortType.CreatedUtc_Asc, TenantTestId);

            Assert.That(returnModel.Data.Count(), Is.EqualTo(take));
        }

        [Test]
        public async Task Saving_Custom_VP_Template_Mvc_is_not_allowed_if_its_already_is_updated()
        {
            Storage.Add(new VP_Template_Mvc(TenantTestId)
            {
                VP_Template_MvcId = "1",
                Title = "Test 1",
                CreatedUtc = DateTimeProvider.Now(),
                ModifiedUtc = DateTimeProvider.Now()
            });

            await Storage.SaveChangesAsync();

            var serverResponse = await _custom_VP_Template_MvcWebAppService.EditAsync(new VP_Template_MvcAddOrEditViewModel 
            {
                VP_Template_MvcId = "1",
                Title = "Test 1",
                ModifiedUtcDate = DateTimeProvider.Now().AddSeconds(-35)
            }, TenantTestId);

            Assert.That(serverResponse.Errors.Count == 1);
            Assert.That(serverResponse.Errors.First(), Is.EqualTo(StandardErrors.EntityHasBeenModified));
        }

        [TestCase(1, SortType.CreatedUtc_Asc, "Custom_VP_Template_Mvc_4")]
        [TestCase(2, SortType.CreatedUtc_Desc, "Custom_VP_Template_Mvc_1")]
        public async Task When_getting_all_Custom_VP_Template_Mvc_items_with_sort_parameter__will_be_sorted_as_expected
            (int testRow, SortType sort, string expectedId)
        {
            await AddTestDataAsync();

            Console.WriteLine($"Executing row: {testRow}");

            var returnModel = await _custom_VP_Template_MvcWebAppService.GetListAsync(0, 100, sort, TenantTestId);

            Assert.That(returnModel.Data.First().VP_Template_MvcId, Is.EqualTo(expectedId));
        }

        [Test]
        public async Task When_getting_a_Custom_VP_Template_Mvc_item_with_valid_id__expected_item_will_be_returned()
        {
            await AddTestDataAsync();

            var testWithItem = await Storage.VP_Template_Mvcs.FirstAsync(x => x.DeletedUtc == null);

            var item = await _custom_VP_Template_MvcWebAppService.GetEditModelAsync(testWithItem.VP_Template_MvcId, testWithItem.TenantId);

            Assert.That(item, Is.Not.Null);
            Assert.That(item.VP_Template_MvcId, Is.EqualTo(testWithItem.VP_Template_MvcId));
        }

        [TestCase(1, "", TenantTestId)]
        [TestCase(1, "123123123123", TenantTestId)]
        [TestCase(1, null, TenantTestId)]
        public async Task When_getting_a_Custom_VP_Template_Mvc_item_with_invalid_id__null_will_be_returned(int testRow, string id, string tenantId)
        {
            await AddTestDataAsync();

            Console.WriteLine($"Executing row: {testRow}");

            var item = await _custom_VP_Template_MvcWebAppService.GetEditModelAsync(id, tenantId);

            Assert.That(item, Is.Null);
        }

        [Test]
        public async Task Saving_Custom_VP_Template_Mvc_is_successful_if_its_not_updated()
        {
            var modifiedDate = DateTimeProvider.Now();
            Storage.Add(new VP_Template_Mvc(TenantTestId)
            {
                VP_Template_MvcId = "1",
                Title = "Test 1",
                CreatedUtc = DateTimeProvider.Now(),
                ModifiedUtc = modifiedDate
            });

            await Storage.SaveChangesAsync();

            var serverResponse = await _custom_VP_Template_MvcWebAppService.EditAsync(new VP_Template_MvcAddOrEditViewModel
            {
                VP_Template_MvcId = "1",
                Title = "Test 1",
                ModifiedUtcDate = modifiedDate
            }, TenantTestId);

            Assert.That(serverResponse.IsValid);
        }

        [Test]
        public async Task Deleting_Custom_VP_Template_Mvc_is_successful()
        {
            Storage.Add(new VP_Template_Mvc(TenantTestId)
            {
                VP_Template_MvcId = "1",
                Title = "Test 1",
                CreatedUtc = DateTimeProvider.Now(),
                ModifiedUtc = DateTimeProvider.Now(),
            });

            Storage.SaveChanges();

            var serverResponse = await _custom_VP_Template_MvcWebAppService.DeleteAsync("1", TenantTestId);

            Assert.That(serverResponse.IsValid);
            Assert.That(Storage.VP_Template_Mvcs.First().DeletedUtc.HasValue, Is.True);
        }

        [Test]
        public async Task Deleting_Custom_VP_Template_Mvc_is_successful_if_item_already_deleted()
        {
            Storage.Add(new VP_Template_Mvc(TenantTestId)
            {
                VP_Template_MvcId = "1",
                Title = "Test 1",
                CreatedUtc = DateTimeProvider.Now(),
                ModifiedUtc = DateTimeProvider.Now(),
                DeletedUtc = DateTimeProvider.Now(),
            });

            Storage.SaveChanges();

            var serverResponse = await _custom_VP_Template_MvcWebAppService.DeleteAsync("1", TenantTestId);

            Assert.That(serverResponse.IsValid);
        }

        private async Task AddTestDataAsync(CancellationToken cancellationToken = default)
        {
            await Storage.AddAsync(new VP_Template_Mvc(TenantTestId)
            {
                VP_Template_MvcId = "Custom_VP_Template_Mvc_1",
                Title = "Test 1",
                CreatedUtc = DateTimeProvider.Now().AddDays(-1),
                ModifiedUtc = DateTimeProvider.Now().AddDays(-1)
            }, cancellationToken);
            await Storage.AddAsync(new VP_Template_Mvc(TenantTestId)
            {
                VP_Template_MvcId = "Custom_VP_Template_Mvc_2",
                Title = "Test 2",
                CreatedUtc = DateTimeProvider.Now().AddDays(-2),
                ModifiedUtc = DateTimeProvider.Now().AddDays(-2)
            }, cancellationToken);
            await Storage.AddAsync(new VP_Template_Mvc(TenantTestId)
            {
                VP_Template_MvcId = "Custom_VP_Template_Mvc_3",
                Title = "Test 3",
                CreatedUtc = DateTimeProvider.Now().AddDays(-3),
                ModifiedUtc = DateTimeProvider.Now().AddDays(-3)
            }, cancellationToken);
            await Storage.AddAsync(new VP_Template_Mvc(TenantTestId)
            {
                VP_Template_MvcId = "Custom_VP_Template_Mvc_4",
                Title = "Test 4",
                CreatedUtc = DateTimeProvider.Now().AddDays(-4),
                ModifiedUtc = DateTimeProvider.Now().AddDays(-4)
            }, cancellationToken);
            await Storage.AddAsync(new VP_Template_Mvc(TenantTestId)
            {
                VP_Template_MvcId = "Custom_VP_Template_Mvc_5",
                Title = "Test 5",
                CreatedUtc = DateTimeProvider.Now().AddDays(-5),
                ModifiedUtc = DateTimeProvider.Now().AddDays(-5),
                DeletedUtc = DateTimeProvider.Now()
            }, cancellationToken);
            await Storage.AddAsync(new VP_Template_Mvc(TenantTestId)
            {
                VP_Template_MvcId = "Custom_VP_Template_Mvc_6",
                Title = "Test 6",
                CreatedUtc = DateTimeProvider.Now().AddDays(-6),
                ModifiedUtc = DateTimeProvider.Now().AddDays(-6),
                DeletedUtc = DateTimeProvider.Now()
            }, cancellationToken);
            await Storage.AddAsync(new VP_Template_Mvc(TenantTestId)
            {
                VP_Template_MvcId = "Custom_VP_Template_Mvc_7",
                Title = "Test 7",
                CreatedUtc = DateTimeProvider.Now().AddDays(-7),
                ModifiedUtc = DateTimeProvider.Now().AddDays(-7),
                DeletedUtc = DateTimeProvider.Now()
            }, cancellationToken);
            await Storage.AddAsync(new VP_Template_Mvc("ANOTHER_TENANT")
            {
                VP_Template_MvcId = "Custom_VP_Template_Mvc_8",
                Title = "Test 8",
                CreatedUtc = DateTimeProvider.Now().AddDays(-8),
                ModifiedUtc = DateTimeProvider.Now().AddDays(-8)
            }, cancellationToken);

            await Storage.SaveChangesAsync(cancellationToken);
        }
    }
}
