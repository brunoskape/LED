using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LED.Entities
{
    public class Constantes
    {
        public static string codTelaConsultaDocumentoDisponivel = "DOCDISCON";

        //Nivel de Acesso vindo do sistema SEI
        public enum NivelAcesso
        {
            publico,
            restrito,
            sigiloso
        }

        //Status da Análise do Documento
        public static string Analisado = "A";
        public static string Dispensado = "D";
        public static string NaoAnalisado = "N";

        //Status da Tipo de Documento
        public static string Parecer = "P";
        public static string Decisao = "D";

        public static string AutoridadeComMagistradoParecer = "1, 2, 3, 4, 5, 6, 7, 8, 15";

        public static string AutoridadeComMagistradoDecisao = "1, 2, 3, 4, 5, 15";

        //Parametros Para ultizar no WebService do Sei
        public static string SiglaServico = "LEDWEB";
        public static string IdentificacaoServico = "integracao";

        //Processos do SEI formato - AAAA-06NNNNN
        public static string processoSEI = "06";
        public static int tamanhoProcessoSei = 11;

        ////Homologação
        //public static int idTipoDocumentoParecerHML_PRD = 452;
        //public static int idTipoDocumentoDescisaoHML = 71;
        //public static int idTipoDocumentoEmentaHML = 854;

        ////Limite do Oracle
        public static int limiteMaxCaraceteres = 29721;
        public static int limiteDeParametrosNoIn = 990;

        //Parametros Para ambientes
        public static string Desenvolvimento = "1";
        public static string Homologacao = "2";
        public static string Producao = "3";
    }
}
