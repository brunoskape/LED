using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LED.Entities
{
    public class DocumentoPortal: Validador
    {
        public string numeroProcesso { get; set; }
        public List<Interessado> listParte { get; set; }
        public List<DocumentosProcesso> listDocumentosProcesso { get; set; }
        public string filtroNumeroProcesso { get; set; }
        public string filtroDataInicial { get; set; }
        public string filtroFinal { get; set; }
        public string filtroAssunto { get; set; }
        public string filtroAutoridade { get; set; }
        public bool eProcessoSEI { get; set; }
        public class  DocumentosProcesso
        {
            public string tipoDocumento { get; set; }
            public int seqDocumento { get; set; }
            public string dataDocumento { get; set; }
            public string dataPublicacao { get; set; }
            public string autoridadeProlatora { get; set; }
            public string integra { get; set; }
            public Boolean sigiloso { get; set; }
            public Boolean disponibilizarWeb { get; set; }
            public string idorgaoGerador { get; set; }
            public string numeroDocumento { get; set; }
            public string linkDocumento { get; set; }
        }
    }
}
