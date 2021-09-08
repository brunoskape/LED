using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LED.Entities
{
    public class PesquisaDocumentoViewModel
    {
        public string NumeroProcesso { get; set; }
        public string DataInicial { get; set; }
        public string DataFinal { get; set; }
        public string TipoDocumento { get; set; }
        public Boolean Load { get; set; }
    }
}
