using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProiectDAW.Models
{
    public class JudetViewModel
    {
        public Judet judet { get; set; }
        public List<Localitate> localitati { get; set; }
        public Localitate localitate { get; set; }
    }

    public class PersoanaViewModel
    {
        public Persoana persoana { get; set; }
        public PersoanaDetaliu detaliu { get; set; }
        public List<Adoptie> adoptii { get; set; }
        public Adoptie adoptie { get; set; }
    }

    public class PersoanaAdoptiiViewModel
    {
        public Persoana persoana { get; set; }
        public List<Persoana> persoane { get; set; }
        public List<Adoptie> adoptii { get; set; }
        public Adoptie adoptie { get; set; }
    }

    public class CaineViewModel
    {
        public Caine caine { get; set; }
        public Adoptie adoptie { get; set; }
    }

    public class AdoptieViewModel
    {
        public Adoptie adoptie { get; set; }
        public PersoanaDetaliu persoanaDetaliu { get; set; }
    }
}