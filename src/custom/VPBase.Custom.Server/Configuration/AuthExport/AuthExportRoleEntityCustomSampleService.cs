using System;
using System.Collections.Generic;
using System.Linq;
using VPBase.Base.Core.Data;
using VPBase.Custom.Core.Data;
using VPBase.Custom.Core.Definitions;
using VPBase.Shared.Server.Code.Exporting;

namespace VPBase.Custom.Server.Configuration.AuthExport
{
    public class AuthExportRoleEntityCustomSampleService : IAuthExportRoleEntityService
    {
        private readonly ICustomStorage _storage;

        public AuthExportRoleEntityCustomSampleService(ICustomStorage storage)
        {
            _storage = storage;
        }

        /// <summary>
        /// Method to access those entities that needs to be synced to Auth client
		/// NOTE: If this implementation is made inside Auth-client, you will have to add the rows to the database yourself
        /// </summary>
        /// <returns>List of models that needs to be sent to Auth client - A complete list that you want to be added in Auth. Auth will remove those not provided, add those who are new and update the rest.</returns>
        public List<RoleEntityModel> GetRoleEntitiesToExport()
        {
            var list = _storage.VP_Template_Mvcs.Where(x => x.DeletedUtc == null)
                .Select(x => new RoleEntityModel()
                {
                    EntityId = x.VP_Template_MvcId,
                    EntityName = x.Title,
                    EntityType = "VP_Template_Mvc",
                    ModifiedUtc = x.ModifiedUtc,
                    RoleId = RoleIdToConnectTheEntitiesTo
                }).ToList();

            return list;
        }

        // The role in Auth client that the entities is gonna be connected to
        public string RoleIdToConnectTheEntitiesTo
        {
            get { return CustomSampleAppConfigDefinition.RoleCustomSampleUser.RoleId; }
        }
    }

}