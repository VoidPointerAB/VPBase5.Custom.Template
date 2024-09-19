using VPBase.Shared.Core.Models.TenantMigration;

public class CustomTenantMigrationSpecification : TenantMigrationSpecificationBaseModel
{
    public CustomTenantMigrationSpecification()
    {
        Mvcs = new List<VP_Template_MvcMigrationModel>();
        SimpleMvcs = new List<VP_Template_SimpleMvcMigrationModel>();
    }

    public ICollection<VP_Template_MvcMigrationModel> Mvcs { get; set; }

    public ICollection<VP_Template_SimpleMvcMigrationModel> SimpleMvcs { get; set; }
}

public class VP_Template_MvcMigrationModel : TenantMigrationCustomFieldValuesEntityModel 
{
    private string _id;

    public string Id 
    { 
        get { return _id; }
        set { _id = value; CustomFieldEntityId = value; }
    }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
}

public class VP_Template_SimpleMvcMigrationModel
{
    public string Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
}


