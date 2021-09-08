using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LED.Entities
{
    public class Documento
    {
        public string Tipo { get; set; }

        public int? IdProcedimento { get; set; }
        
        public string ProtocoloProcedimento { get; set; }

        public int IdSerie { get; set; }

        public int? Numero { get; set; }

        public DateTime? Data { get; set; }

        public String Descricao { get; set; }

        public int IdTipoConferencia { get; set; }

        public Remetente Remetente { get; set; }

        public List<Interessado> Interessados { get; set; }

        public List<Destinatario> Destinatarios { get; set; }
        
        public string Observacao { get; set; }

        public string NomeArquivo { get; set; }

        public int? NivelAcesso { get; set; }

        public int IdHipoteseLegal { get; set; }

        public string Conteudo { get; set; }

        public string ConteudoMTOM { get; set; }

        public int IdArquivo { get; set; } // Identificador do arquivo (ver serviço adicionarArquivo)

        public Campo Campos { get; set; }

        public string SinBloqueado { get; set; }
    }
}
