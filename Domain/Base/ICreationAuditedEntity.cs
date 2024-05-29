namespace Domain.Base
{
    public interface ICreationAuditedEntity
    {
        DateTime CreationTime { get; set; }

        string? CreatorUserId { get; set; }
    }
}