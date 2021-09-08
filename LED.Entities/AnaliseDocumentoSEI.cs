using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LED.Entities
{
    public class AnaliseDocumentoSEI : Validador
    {
        public string numeroProcesso { get; set; }
        public int idDocumento { get; set; }
        public List<Interessado> parte { get; set; }
        public string tipoDocumento { get; set; }
        public string dataDocumento { get; set; }
        public string AnoDocumento { get; set; }
        public string dataPublicacao { get; set; }
        public string integraDocumento { get; set; }
        public string linkAcesso { get; set; } 
        public bool sigilosoNao { get; set; }  
        public bool sigilosoSim { get; set; }
        public bool disponibilizadoWebNao { get; set; }
        public bool disponibilizadoWebSim { get; set; }
        public string MensagemErroSei { get; set; }

        public bool ClassificarSigilo(int NivelAcesso)
        {
            if((int)Constantes.NivelAcesso.sigiloso == NivelAcesso)
            {
                return true;
            }

            return false;
        }
        public bool ClassificarDisponivelWeb(int NivelAcesso)
        {
            if ((int)Constantes.NivelAcesso.publico == NivelAcesso)
            {
                return true;
            }
            return false;
        }
    }



    
}
