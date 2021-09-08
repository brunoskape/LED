using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LED.Entities
{
    public class Decisao
    {
        public string processo { get; set; }
        public int numeroDecisao { get; set; }
        public DateTime dataDecisao { get; set; }
        public int codAutoridade { get; set; }
        public int matriculaMagistrado { get; set; }        
        public string disponibilizadoWeb { get; set; }
        public string dataPublicacao { get; set; }
        public string ConselheiroTCE { get; set; }
        public string numeroProcessoTCE { get; set; }
        public string decisaoComParecer { get; set; }
        public string usuario { get; set; }
        public DateTime dataAtualizacao { get; set; }
        public string Sigiloso { get; set; }
        public string matriculaServidor { get; set; }
        //public string nomeServidor { get; set; }
        public int procConselhoMagistratura { get; set; }
        public string integra { get; set; }
        public DateTime dataInclusao { get; set; }
        public int idDocumento { get; set; }
        //
        public string NomeAutoridade { get; set; }
        public string idorgaoGerador { get; set; }
        public string numeroDocumento { get; set; }
    }
   
}
