using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using VPBase.Base.Server.Areas.Admin.Interfaces;
using VPBase.Base.Server.Areas.Admin.Models.ViewModels.FileStorage;
using VPBase.Custom.Core.Definitions;
using VPBase.Shared.Core.Types;
using VPBase.Shared.Server.Helpers;

namespace VPBase.Custom.Server.Areas.Custom.Controllers
{
    [Area("Custom")]
    [Authorize]
    public class CustomSampleStorageController : Controller
    {
        private IFileStorageWebAppService _fileStorageWebAppService;
        private IFileStorageFetchWebAppService _fileStorageFetchWebAppService;
        private ILogger<CustomSampleStorageController> _logger;
        private HomePageHelper _homePageHelper;

        public CustomSampleStorageController(IFileStorageWebAppService fileStorageWebAppService,
            IFileStorageFetchWebAppService fileStorageFetchWebAppService,
            ILogger<CustomSampleStorageController> logger,
            HomePageHelper homePageHelper)
        {
            _fileStorageWebAppService = fileStorageWebAppService;
            _fileStorageFetchWebAppService = fileStorageFetchWebAppService;
            _logger = logger;
            _homePageHelper = homePageHelper;
        }

        /// <summary>
        /// Sample example with custom implementation of dropzone behaviour
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            // Here we use a custom sample view model but it must inherit base class FileStorageDropzoneViewModel!
            var testmodel = _fileStorageWebAppService.BuldDropzoneViewModel<FileStorageDropzoneViewModel>(HttpContext.Request, HttpContext.GetActiveTenantIdCookie());

            // Use the heading info and description to describe the scenario and what the user expect
            // to do and the purpose of uploading the files.
            // It is possible to leave them out.
            testmodel.HeadingInfo = "Custom Sample Heading Example";
            testmodel.Description = "Custom sample user profile images connected to user: " + HttpContext.User.DisplayName();

            // Here we specify using entity file transformation.
            // Leave all entity and file transformation out if raw upload is prefered! In that case instead of using EntityId, you can use ReferenceId.

            // Entity and File Transformation BEGIN

            testmodel.EntityId = HttpContext.User.UserId();                 // Here we connect the entity to the user identification
            var fileTransformation = _fileStorageFetchWebAppService.GetFileTransformationByName(CustomSampleFileStorageDefinition.Transformations.Name_UserSampleProfileImage); // This should be added in the foundation data injection
            testmodel.FileTransformationId = fileTransformation.FileTransformationId;

            testmodel.FileTransformationName = fileTransformation.Name;     // Optional: display file transformation name in gui/confirms -> bether UX experience for the user.

            testmodel.EntityFilesCleanupBeforeAdd = true;                   // Erase old connected entity files before add new ones. Otherwise we add new ones to the entity which is prefered in
                                                                            // some cases. For example photo album connected to a special entity
            
            testmodel.EntityName = HttpContext.User.DisplayName();          // Optional: display entity name in gui/confirms -> bether UX experience for the user.

            // Entity and File Transformation END

            testmodel.PreviewEnabled = true;                                // Preview of files

            testmodel.ReferenceId = "ReferenceId Example";                  // Optional: value to set to mark the files with some kind of reference id, for example if not entity filetransformation is used. 
                                                                            // Use this to connect the files to some kind of reference. You could use for example the UserId() here. 
                                                                            // It should be possible to use EntityId and ReferenceId in combination if needed.

            testmodel.ReferenceName = "Reference Name";                     // Optional: same as above but the 'Name' referenced. 

            testmodel.ExtraRelativePath = HttpContext.User.UserId();        // Extra folder for the file, system will make the path storage friendly

            testmodel.DropzoneSettings.MaxFiles = 2;                        // Here two files can be uploaded each time, normally we prefered ONE (in this case) since it should only be ONE profile image
                                                                            // with different sizes

            testmodel.DropzoneSettings.AcceptedFileTypes = _fileStorageWebAppService.GetValidExtensions(new List<FileCategoryType>() { FileCategoryType.StandardImage });

            testmodel.FileStorageType = FileStorageType.Public;

            testmodel.ReturnBtnUrl = _homePageHelper.GetStartPage().Replace("~", "");

            //testmodel.UseUniqueFilePrefix = false;                        // Default is true, no conflict with file names since the file name will be unique with a unique prefix

            //testmodel.UploadUrl = "/Custom/CustomSampleStorage/CustomSaveDropZoneFiles";      // Custom implementation of save drop zone files.

            _fileStorageWebAppService.RefreshDropzoneViewModel(testmodel);


            return View(ViewAdminHelper.BaseDropZonePath, testmodel);
        }
    }
}
