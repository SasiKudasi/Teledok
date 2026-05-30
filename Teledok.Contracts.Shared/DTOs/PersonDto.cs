using Teledok.Contracts.Shared.Enums;

namespace Teledok.Contracts.Shared.DTOs;

public class PersonDto
{
    public Guid Id { get; set; }
    public string Inn { get; set; }
    public string Name { get; set; }
    public ClientType Type { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
