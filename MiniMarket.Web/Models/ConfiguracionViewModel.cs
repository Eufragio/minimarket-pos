using System.ComponentModel.DataAnnotations;

namespace MiniMarket.Web.Models
{
    public class ConfiguracionViewModel
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [Display(Name = "Razón Social / Nombre")]
        public string NombreEmpresa { get; set; } = "MiniMarket Pro";

        [Required(ErrorMessage = "El RUC es obligatorio")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "El RUC debe tener 11 dígitos")]
        public string Ruc { get; set; } = "20600000001";

        [Required]
        [Display(Name = "Dirección Fiscal")]
        public string Direccion { get; set; } = "Av. Principal 123 - Centro";

        public string Telefono { get; set; } = "(01) 555-0100";

        [Required]
        [EmailAddress]
        [Display(Name = "Email de Contacto")]
        public string EmailContacto { get; set; } = "admin@minimarket.com";

        [Required]
        [Range(0, 100)]
        [Display(Name = "Porcentaje de IGV")]
        public decimal IgvPorcentaje { get; set; } = 18.00m;
    }
}