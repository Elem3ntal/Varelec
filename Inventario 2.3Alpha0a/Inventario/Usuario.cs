using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Inventario
{
    public class Usuario
    {
        private string nombre;
        private int nivel;
        public Usuario(string name, int nivel) 
        {
            this.nombre = name;
            this.nivel = nivel;
        }
        public Usuario() { } //constructor por defecto

        public string obtenerNombre()
        {
            return nombre;
        }
        public int obtenerNivel()
        {
            return nivel;
        }
    }
}
