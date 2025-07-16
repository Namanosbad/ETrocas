using ETrocas.Domain.Interfaces;

namespace ETrocas.Domain.Entities
{
    public class Entity : IEntity
    {
        public Guid Id { get; set; }
    }
}