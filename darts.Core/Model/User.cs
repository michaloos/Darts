using darts.Data.Interfaces;

namespace darts.Core.Model;

public class User : IEntity
{
    public required Guid Id { get; set; }
    public required string Name { get; set; }
}