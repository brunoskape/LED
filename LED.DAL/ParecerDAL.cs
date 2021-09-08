using EstruturaPadrao;
using LED.Entities;
using ServicoDadosODPNETM;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LED.DAL
{
    public class ParecerDAL : baseDAL
    {
        public ParecerDAL(EstruturaIdentificacaoUsuario estUsuario) : base(estUsuario)
        {
        }

        public void ConfirmarAnalise(Parecer pDocumento)
        {
            try
            {
                sd.Open();

                sd.ExecutaProc("PKG_PARECER.INCLUIR_PARACER", pDocumento.processo, pDocumento.dataParecer, pDocumento.dataPublicacao,
                    pDocumento.codAutoridade, pDocumento.matriculaMagistrado, pDocumento.disponibilizadoWeb, pDocumento.numeroProcessoTCE,
                    pDocumento.usuario, pDocumento.dataAtualizacao, pDocumento.matriculaServidor, pDocumento.existeDecisao, pDocumento.Sigiloso,
                    pDocumento.integra, pDocumento.dataInclusao, pDocumento.idDocumento, pDocumento.autoridadeTCE, pDocumento.anoParecer );
                               
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

        public List<Parecer> ConsultaParcerPortal(string pNumeroProcesso, string pDataInicio, string pDataFim, string pAssunto, string pAutoridadeProlatora)
        {
            try
            {
                sd.Open();
                List<Parecer> listaDocumentoParecer = new List<Parecer>();

                DataTable dt = new DataTable();

                dt = sd.ExecutaProcDS("PKG_PARECER.CONSULTAR_PARACER",
                string.IsNullOrEmpty(pNumeroProcesso) ? DBNull.Value : (object) FuncoesBasicas.RemoveFormatacao(pNumeroProcesso),
                string.IsNullOrEmpty(pDataInicio) ? DBNull.Value : (object)pDataInicio,
                string.IsNullOrEmpty(pDataFim) ? DBNull.Value : (object)pDataFim,
                string.IsNullOrEmpty(pAutoridadeProlatora) ? DBNull.Value : (object)pAutoridadeProlatora,
                string.IsNullOrEmpty(pAssunto) ? DBNull.Value : (object)pAssunto,                
                sd.CriaRefCursor("C")).Tables[0];

                foreach (var linha in dt.Rows.Cast<DataRow>())
                {
                    var o = new Parecer();

                    o.processo = linha["processo"].ToString();
                    
                    o.numeroParecer = Convert.ToInt32(linha["numeroparecer"].ToString());

                    o.dataParecer = Convert.ToDateTime(linha["data_Dcoumento"].ToString());

                    if (!String.IsNullOrEmpty(linha["datapublicacao"].ToString()))
                    o.dataPublicacao = linha["datapublicacao"].ToString();

                    if (!String.IsNullOrEmpty(linha["AutoridadeProlatora"].ToString()))
                    o.NomeAutoridade = linha["AutoridadeProlatora"].ToString();

                    o.integra = linha["integra"].ToString();
                    o.Sigiloso = linha["sigiloso"].ToString();
                    o.disponibilizadoWeb = linha["disponibilizarweb"].ToString();

                    if (!String.IsNullOrEmpty(linha["Idorgaogerador"].ToString()))
                        o.idorgaoGerador = linha["Idorgaogerador"].ToString();

                    if (!String.IsNullOrEmpty(linha["Numerodocumento"].ToString()))
                        o.numeroDocumento = linha["Numerodocumento"].ToString();

                    listaDocumentoParecer.Add(o);
                }

                return listaDocumentoParecer;
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
