using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventario
{
    class Numeros
    {
        private string entrada;
        private string salida;
        public Numeros(string numero)
        {
            entrada = numero;
        }
        public Numeros(int numero)
        {
            entrada = "" + numero;
        }
        public Numeros(double numero)
        {
            entrada = "" + numero;
        }
        public Numeros(long numero)
        {
            entrada = "" + numero;
        }
        public Numeros()
        {
            //constructor por default;
        }
        public string numeroSolo()
        {
            salida = "";
            foreach (char letra in entrada)
            {
                if (letra == '0' || letra == '1' || letra == '2' || letra == '3' || letra == '4' || letra == '5' || letra == '6' || letra == '7' || letra == '8' || letra == '9' || letra=='-' || letra==',')
                {
                    salida += letra;
                }
            }
            if (salida == "")
            {
                salida = "0";
            }
            return salida;
        }
        public string numeroMiles()
        {
            numeroSolo();
            try
            {
                salida = Convert.ToInt32(salida).ToString("N0");
            }
            catch
            {
                salida = "0";
            }
            return salida;
        }
        public string validarRut()
        {
            salida = numeroMiles() + "-" + obtenerValidador();
            return salida;
        }
        public string obtenerValidador()
        {
            salida = "";
            int numero = 0, indice = 2, suma = 0;
            try
            {
                numero = Convert.ToInt32(numeroSolo());
            }
            catch
            {
            }
            int resto, final;
            while (numero != 0)
            {
                if (indice == 8)
                {
                    indice = 2;
                }
                suma += (numero % 10) * indice;
                indice++;
                numero = numero / 10;
            }
            resto = suma / 11;
            final = suma - (11 * resto);
            resto = 11 - final;
            if (resto == 11)
            {
                return "0";
            }
            else if (resto == 10)
            {
                return "K";
            }
            else
            {
                return "" + resto;
            }
        }
    }
}