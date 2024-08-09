using Microsoft.AspNetCore.Mvc;
using VPBase.Custom.Server.Areas.Custom.Models.ApiModels;
using VPBase.Shared.Server.Code.Api;

namespace VPBase.Custom.Server.Areas.Custom.Controllers
{
    [Route("api/custom/[controller]")]
    [ApiExplorerSettings(GroupName = "custom")] // Check group in: StartupCustomSampleConfigureInstruction
    [ApiController]
    public class CustomSampleApiController : ControllerBase
    {
        private static List<CustomSampleItem> _customSampleItems = new List<CustomSampleItem>();
        private const int MaxCountItems = 100;

        public CustomSampleApiController()
        {

        }

        [HttpGet, Route("Echo")]
        public string Echo()
        {
            return ApiResultHelper.GetStandardEchoResult("CustomSample");
        }

        /// <summary>
        /// Gets a list of custom sample items
        /// </summary>
        /// <returns></returns>
        [HttpGet]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<ActionResult<IEnumerable<CustomSampleItem>>> GetCustomSampleItems()
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            return _customSampleItems.ToList();
        }

       /// <summary>
       /// 
       /// </summary>
       /// <param name="id"></param>
       /// <returns></returns>
        [HttpGet("{id}")]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<ActionResult<CustomSampleItem>> GetCustomSampleItem(long id)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var item = _customSampleItems.FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                return NotFound("Request is incorrect! Custom sample item with id: " + id + " not found!");
            }

            return item;
        }

        [HttpPost]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<ActionResult<CustomSampleItem>> PostCustomSampleItem(CustomSampleItem customSampleItem)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var validateResult = ValidateAddSampleItem(customSampleItem);

            if (!validateResult.IsSuccess)
            {
                return BadRequest("Request is incorrect! " + validateResult.GetErrors());
            }

            var item = _customSampleItems.FirstOrDefault(x => x.Id == customSampleItem.Id);

            if (item != null)
            {
                return BadRequest("Request is incorrect! Custom sample item with id: " + customSampleItem.Id + " already exists!");
            }
            else
            {
                if (_customSampleItems.Count < MaxCountItems)   // Avoid memory hacks
                {
                    _customSampleItems.Add(customSampleItem);
                }
            }

            return CreatedAtAction(nameof(GetCustomSampleItem), new { id = customSampleItem.Id }, customSampleItem);
        }


        [HttpPut("{id}")]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IActionResult> PutCustomSampleItem(long id, CustomSampleItem customSampleItem)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            if (id != customSampleItem.Id)
            {
                return BadRequest();
            }

            var sampleItem = _customSampleItems.FirstOrDefault(x => x.Id == customSampleItem.Id);

            if (sampleItem == null)
            {
                return NotFound("Request incorrect! Custom sample item not found with id: " + id);
            }
            else
            {
                var validateResult = ValidateAddSampleItem(customSampleItem);

                if (!validateResult.IsSuccess)
                {
                    return BadRequest("Request is incorrect! " + validateResult.GetErrors());
                }

                if (_customSampleItems.Count < MaxCountItems)         // Avoid memory hacks
                {
                    _customSampleItems.Remove(sampleItem);            // Important. Must be sampleItem not customSampleItem

                    _customSampleItems.Add(customSampleItem);
                }
            }

            return Ok();
        }

        /// <summary>
        /// Deletes a custom sample item identified by "id"
        /// </summary>
        /// <param name="id">Unique id of the custom sample item</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
        public async Task<IActionResult> DeleteCustomSampleItem(long id)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
        {
            var item = _customSampleItems.FirstOrDefault(x => x.Id == id);

            if (item == null)
            {
                return NotFound("Request is incorrect! Custom sample item with id: " + id + " not found to delete!");
            }

            _customSampleItems.Remove(item);

            return Ok();
        }

        private ApiValidateResult ValidateAddSampleItem(CustomSampleItem customSampleItem)
        {
            var validateResult = new ApiValidateResult();

            if (customSampleItem == null)
            {
                validateResult.Errors.Add("CustomSampleItem 'object' is empty!");
            }
            else
            {
                if (customSampleItem.Id <= 0)
                {
                    validateResult.Errors.Add("CustomSampleItem 'Id' is zero or less!");
                }
                if (string.IsNullOrEmpty(customSampleItem.Name))
                {
                    validateResult.Errors.Add("CustomSampleItem 'Name' is empty!");
                }

                // Note: Description and CustomProperty are not required and therefore no validation, 
                // IsComplete is a boolean and not nullable so validation is not required.
            }

            return validateResult;
        }
    }
}
