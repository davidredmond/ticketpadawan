namespace TP.Database.Models
{
    public interface IEntity
    {
        int Id { get; set; }
        DateTime Created { get; set; }
        DateTime Updated { get; set; }
    }
}
