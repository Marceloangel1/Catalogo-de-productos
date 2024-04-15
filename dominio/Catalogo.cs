﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using dominio;



namespace dominio
{
    public class Catalogo
    {

        public int Id { get; set; }

        [DisplayName ("Código")]
        public string Codigo { get; set; }

        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public  decimal Precio { get; set; }
        public  string ImagenUrl { get; set; }
        public  Categoria Categoria  { get; set; }
        public Marca Marca { get; set; }
       

    }
}
