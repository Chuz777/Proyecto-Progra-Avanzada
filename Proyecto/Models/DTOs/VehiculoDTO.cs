namespace Proyecto.Models.DTOs
{
    public class VehiculoDTO
    {
        public int Id { get; set; }
        public string Marca { get; set; }
        public string Modelo { get; set; }
        public int Anio { get; set; }
        public string VIN { get; set; }
        public decimal Precio { get; set; }
        public string ImagenUrl { get; set; }
        public string Descripcion { get; set; }
        public string Estado { get; set; }
        public int CategoriaId { get; set; }
        public int SucursalId { get; set; }
    }
}