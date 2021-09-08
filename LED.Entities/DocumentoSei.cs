using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LED.Entities
{
    public class DocumentoSei : Validador
    {
        public string numeroProcesso { get; set; }
        public string descricaoTipoDocumento { get; set; }
        public string tipoDocumento { get; set; }
        public int idAnexo { get; set; }
        public int idTipoDocumento { get; set; }
        public string nomeTipoDocumento { get; set; }
        public int idDocumento { get; set; }
        public string dataDocumento { get; set; }
        public string numeroDocumento { get; set; }
        public string siglaOrgaoGerador { get; set; }
        public int idOrgaoGerador { get; set; }
        public string descricaoOrgaoGerador { get; set; }
        public string idUsuarioGerador { get; set; }
        public string codigoProt { get; set; }
        public string siglaTipoDocumento { get; set; }
        public string analisado { get; set; }
        public string assunto { get; set; }
        public int idNivelAcesso { get; set; }
        public string DescricaoNivelAcesso { get; set; }
    }
}
