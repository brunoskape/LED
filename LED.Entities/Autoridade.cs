using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LED.Entities
{
    public class Autoridade : Validador
    {
        public int codAutoridade { get; set; }
        public string responsavel { get; set; }
        public int tipoResponsavel { get; set; }
    }
}
