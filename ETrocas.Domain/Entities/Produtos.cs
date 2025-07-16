using ETrocas.Domain.Interfaces;

namespace ETrocas.Domain.Entities
{
    public class Produtos : IEntity
    {
        public Guid Id { get; set; }
        public string? Produto{ get; set; }
        public string? Tipo { get; set; }
        public decimal Valor { get; set; }
        public string? Descricao { get; set; }
        public bool Disponivel { get; set; }
        public DateTime DataCadastro { get; set; } =DateTime.Now;
        public string? ImageUrl { get; set; }
        
        public Guid UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
    }
}