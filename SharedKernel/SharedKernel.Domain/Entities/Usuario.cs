namespace SharedKernel.Domain.Entities
{
    public class Usuario : EntityBase, IAggregateRoot
    {               
        public string Nome   { get; set; }
        public string Login  { get; set; }
        public string Senha  { get; set; }
        public string Perfil { get; set; }
        public string Email  { get; set; }
        public byte[] Foto   { get; set; }
    }
}