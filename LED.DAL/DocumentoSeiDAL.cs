using EstruturaPadrao;
using LED.Entities;
using ServicoDadosODPNETM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using App = System.Configuration.ConfigurationManager;

namespace LED.DAL
{
    public class DocumentoSeiDAL : baseDAL
    {
        public DocumentoSeiDAL(EstruturaIdentificacaoUsuario estUsuario) : base(estUsuario)
        {
        }

        public List<DocumentoSei> BuscaDocumentoSei(String pNumeroProcesso, String pDataInicial, String pDataFinal, String pTipoDocumento)
        {
            try
            {
                sd.Open();
                List<DocumentoSei> listaDocumentoSei = new List<DocumentoSei>();

                DataTable dt = new DataTable();

                dt = sd.ExecutaProcDS("PKG_DOCUMENTO_SEI.CONSULTA_ANALISE_DOCUMENTO_SEI",                
                string.IsNullOrEmpty(pNumeroProcesso) ? DBNull.Value : (object)pNumeroProcesso,
                string.IsNullOrEmpty(pDataInicial) ? DBNull.Value : (object)pDataInicial,
                string.IsNullOrEmpty(pDataFinal) ? DBNull.Value : (object)pDataFinal,
                string.IsNullOrEmpty(pTipoDocumento) ? DBNull.Value : (object)pTipoDocumento,
                sd.CriaRefCursor("C")).Tables[0];

                foreach (var linha in dt.Rows.Cast<DataRow>())
                {
                    var o = new DocumentoSei();

                    o.numeroProcesso = linha["numeroprocesso"].ToString();
                    //o.idTipoDocumento = Convert.ToInt32(linha["IDTIPODOCUMENTO"].ToString());
                    o.nomeTipoDocumento = linha["nometipodocumento"].ToString();
                    o.dataDocumento =  linha["datadocumento"].ToString().Substring(0,10);                   
                    o.idDocumento = Convert.ToInt32(linha["idDocumento"].ToString());
                    o.numeroDocumento = linha["NUMERODOCUMENTO"].ToString();
                    o.idNivelAcesso = Convert.ToInt32(linha["idNivelAcesso"].ToString());
                    o.DescricaoNivelAcesso = linha["DescricaoNivelAcesso"].ToString();
                    o.idOrgaoGerador = Convert.ToInt32(linha["IDORGAOGERADOR"].ToString());
                    o.descricaoOrgaoGerador = linha["DESCRICAOORGAOGERADOR"].ToString();
                    o.siglaTipoDocumento = linha["siglatipodocumento"].ToString();
                    o.idAnexo = Convert.ToInt32(linha["idanexo"].ToString());
                    listaDocumentoSei.Add(o);
                }
                
                return listaDocumentoSei;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sd.Close();
            }
            
        }

        public DocumentoSei BuscaDocumentoSei(int idDocumento)
        {
            try
            {
                sd.Open();
                DocumentoSei documentoSei = new DocumentoSei();

                DataTable dt = new DataTable();

                dt = sd.ExecutaProcDS("PKG_DOCUMENTO_SEI.CONSULTA_DOCUMENTO_SEI",idDocumento,
                sd.CriaRefCursor("C")).Tables[0];

                foreach (var linha in dt.Rows.Cast<DataRow>())
                {
                    var o = new DocumentoSei();

                    o.numeroProcesso = linha["numeroprocesso"].ToString();
                    o.idTipoDocumento = Convert.ToInt32(linha["IDTIPODOCUMENTO"].ToString());
                    o.nomeTipoDocumento = linha["nometipodocumento"].ToString();
                    o.dataDocumento = linha["datadocumento"].ToString().Substring(0, 10);
                    o.idDocumento = Convert.ToInt32(linha["idDocumento"].ToString());
                    o.numeroDocumento = linha["NUMERODOCUMENTO"].ToString();
                    o.idNivelAcesso = Convert.ToInt32(linha["idNivelAcesso"].ToString());
                    o.DescricaoNivelAcesso = linha["DescricaoNivelAcesso"].ToString();
                    o.idOrgaoGerador = Convert.ToInt32(linha["IDORGAOGERADOR"].ToString());
                    o.descricaoOrgaoGerador = linha["DESCRICAOORGAOGERADOR"].ToString();
                    o.siglaTipoDocumento = linha["siglatipodocumento"].ToString();

                    documentoSei = o;
                }

                return documentoSei;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sd.Close();
            }

        }


        public void Alterar_Satatus_DocumentoSei(int idDocumento, string status)
        {
            try
            {
                sd.Open();

                sd.ExecutaProc("PKG_DOCUMENTO_SEI.ALTERAR_STATUS_DOCUMENTO_SEI", idDocumento, status);
            }
            catch (ServicoDadosException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sd.Close();
            }
        }

        
    }
   

}
