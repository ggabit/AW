using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProiectDAW.Models
{
    [MetadataType(typeof(PersoanaMetadata))]
    public partial class Persoana
    {
    }

    public class PersoanaMetadata
    {
        [Required(ErrorMessage ="Numele persoanei este obligatoriu!")]
        public string Nume { get; set; }

        [Required(ErrorMessage = "Prenumele persoanei este obligatoriu!")]
        public string Prenume { get; set; }

        [Required(ErrorMessage = "CNP-ul este obligatoriu!"), MaxLength(13, ErrorMessage = "CNP-ul trebuie să aibe maxim 13 cifre!")]
        [ValidareCNP(ErrorMessage ="CNP invalid!")]
        public string CNP { get; set; }

        [Required(ErrorMessage = "Numărul de telefon este obligatoriu!"), MaxLength(10, ErrorMessage = "Numărul de telefon trebuie să aibe maxim 10 cifre!")]
        [Display(Name ="Număr de telefon")]
        public string NrTelefon { get; set; }

        [Required(ErrorMessage = "Data nașterii este obligatorie!")]
        [Display(Name = "Data nașterii"), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public System.DateTime DataNasterii { get; set; }

        [Display(Name ="Actualizat de")]
        public string UserId { get; set; }

        [Display(Name = "Actualizat la"), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        public System.DateTime UpdateDate { get; set; }

    }

    public class ValidareCNP: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            String cnp = (String)value;
            return Validare.CNP(cnp);
        }
    }

    [MetadataType(typeof(JudetMetadata))]
    public partial class Judet
    {
    }

    public class JudetMetadata
    {
        [Required(ErrorMessage = "Indicativul este obligatoriu!")]
        [Display(Name = "Indicativ"),MaxLength(2,ErrorMessage ="Introduceți maxim două caractere!")]
        public string JudetID { get; set; }

        [Required(ErrorMessage = "Denumirea este obligatorie!")]
        [Display(Name = "Județ")]
        public string Denumire { get; set; }
    }

    [MetadataType(typeof(LocalitateMetadata))]
    public partial class Localitate
    {
    }

    public class LocalitateMetadata
    {
        [Required(ErrorMessage = "Denumirea este obligatorie!")]
        [Display(Name = "Localitate")]
        public string Denumire { get; set; }

        [Display(Name = "Județ")]
        public string JudetID { get; set; }
    }

    [MetadataType(typeof(PersoanaDetaliuMetadata))]
    public partial class PersoanaDetaliu
    {
    }

    public class PersoanaDetaliuMetadata
    {
        [Display(Name ="Persoana")]
        [Required(ErrorMessage = "Persoana este obligatorie!")]
        public int PersoanaId { get; set; }

        [Required(ErrorMessage = "Adresa este obligatorie!")]
        public string Adresa { get; set; }

        [Display(Name ="Locuința are curte?")]
        public bool LocuintaCurte { get; set; }

        [Required(ErrorMessage = "Localitatea este obligatorie!")]
        [Display(Name = "Localitate")]
        public int LocalitateID { get; set; }

    }

    [MetadataType(typeof(CaineMetadata))]
    public partial class Caine
    {
    }

    public class CaineMetadata
    {
        [Required(ErrorMessage = "Numele este obligatoriu!")]
        public string Nume { get; set; }

        [Display(Name = "Cod")]
        [Required(ErrorMessage = "Codul este obligatoriu!")]
        [ValidareCod(ErrorMessage ="Codul trebuie să înceapă cu C!")]
        public string Cod { get; set; }

        [Required(ErrorMessage = "Selectați genul!")]
        public string Gen { get; set; }

        [Display(Name = "Data nașterii")]
        [Required(ErrorMessage = "Data nașterii este obligatorie!"), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public System.DateTime DataNasterii { get; set; }

        [Display(Name = "Disponibil?")]
        public bool Disponibil { get; set; }

        [Display(Name = "Actualizat la")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        public System.DateTime UpdateDate { get; set; }

        [Display(Name = "Actualizat de")]
        public string UserId { get; set; }

    }

    public class ValidareCod : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            String cod = (String)value;
            return Validare.Cod(cod);
        }
    }

    [MetadataType(typeof(AdoptieMetadata))]
    public partial class Adoptie
    {
    }
    public class AdoptieMetadata
    {
        [Display(Name = "Nr înregistrare")]
        [Required(ErrorMessage = "Numărul este obligatoriu!")]
        public string NrInregistrare { get; set; }

        [Display(Name = "Data adopției")]
        [Required(ErrorMessage = "Data adopției este obligatorie!"), DisplayFormat(DataFormatString = "{0:dd.MM.yyyy}")]
        public System.DateTime DataAdoptie { get; set; }

        [Display(Name = "Câinele")]
        public int CaineId { get; set; }

        [Display(Name = "Persoana")]
        public int PersoanaId { get; set; }

        [Display(Name = "Actualizat la")]
        [DisplayFormat(DataFormatString = "{0:dd.MM.yyyy HH:mm}")]
        public System.DateTime UpdateDate { get; set; }

        [Display(Name = "Actualizat de")]
        public string UserId { get; set; }
    }
}