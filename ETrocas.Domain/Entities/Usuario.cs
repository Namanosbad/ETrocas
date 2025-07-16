using ETrocas.Domain.Interfaces;

namespace ETrocas.Domain.Entities
{
    public class Usuario : IEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? SenhaHash { get; set; }

        public List<Produtos>? Produtos { get; set; }
        public List<Proposta>? PropostasFeitas { get; set; }
        public List<Proposta>? PropostasRecebidas {get; set;}
    }
}