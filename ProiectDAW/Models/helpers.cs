using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProiectDAW.Models
{
    public class Validare
    {
        public static bool CNP(String cnp)
        {
            // aici pun validarea 
            int control;
            if (cnp.Length != 13) return false;
            //279146358279
            int suma = Convert.ToInt32(cnp[0].ToString()) * 2 +
                Convert.ToInt32(cnp[1].ToString()) * 7 +
                Convert.ToInt32(cnp[2].ToString()) * 9 +
                Convert.ToInt32(cnp[3].ToString()) * 1 +
                Convert.ToInt32(cnp[4].ToString()) * 4 +
                Convert.ToInt32(cnp[5].ToString()) * 6 +
                Convert.ToInt32(cnp[6].ToString()) * 3 +
                Convert.ToInt32(cnp[7].ToString()) * 5 +
                Convert.ToInt32(cnp[8].ToString()) * 8 +
                Convert.ToInt32(cnp[9].ToString()) * 2 +
                Convert.ToInt32(cnp[10].ToString()) * 7 +
                Convert.ToInt32(cnp[11].ToString()) * 9;
            int rest = suma % 11;
            if (rest < 10) control = rest;
            else control = 1;
            if (Convert.ToInt32(cnp[12].ToString()) != control) return false;
            return true;
        }

        public static bool Cod(String cod)
        {
            return cod.StartsWith("C");
        }
    }
}