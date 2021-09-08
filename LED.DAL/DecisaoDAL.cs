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
    public class DecisaoDAL : baseDAL
    {
        public DecisaoDAL(EstruturaIdentificacaoUsuario estUsuario) : base(estUsuario)
        {

        }
        public void ConfirmarAnalise(Decisao pDocumento)
        {
            try
            {
                sd.Open();

                sd.ExecutaProc("PKG_DECISAO.INCLUIR_DECISAO", pDocumento.processo, pDocumento.dataDecisao, pDocumento.codAutoridade,
                    pDocumento.matriculaMagistrado, pDocumento.disponibilizadoWeb, pDocumento.dataAtualizacao, pDocumento.dataPublicacao,
                    pDocumento.ConselheiroTCE, pDocumento.numeroProcessoTCE, pDocumento.decisaoComParecer, pDocumento.usuario, pDocumento.Sigiloso,
                    pDocumento.matriculaServidor, pDocumento.procConselhoMagistratura,pDocumento.integra, pDocumento.dataInclusao, pDocumento.idDocumento);

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

        public List<Decisao> ConsultaDecisaoPortal(string pNumeroProcesso, string pDataInicio, string pDataFim, string pAssunto, string pAutoridadeProlatora)
        {
            try
            {
                sd.Open();
                List<Decisao> listaDecisao = new List<Decisao>();

                DataTable dt = new DataTable();

                dt = sd.ExecutaProcDS("PKG_DECISAO.CONSULTAR_DECISAO",
                string.IsNullOrEmpty(pNumeroProcesso) ? DBNull.Value : (object)FuncoesBasicas.RemoveFormatacao(pNumeroProcesso),
                string.IsNullOrEmpty(pDataInicio) ? DBNull.Value : (object)pDataInicio,
                string.IsNullOrEmpty(pDataFim) ? DBNull.Value : (object)pDataFim,
                string.IsNullOrEmpty(pAutoridadeProlatora) ? DBNull.Value : (object)pAutoridadeProlatora,
                string.IsNullOrEmpty(pAssunto) ? DBNull.Value : (object)pAssunto,                
                sd.CriaRefCursor("C")).Tables[0];

                foreach (var linha in dt.Rows.Cast<DataRow>())
                {
                    var o = new Decisao();

                    o.processo = linha["processo"].ToString();

                    o.numeroDecisao = Convert.ToInt32(linha["numerodecisao"].ToString());

                    o.dataDecisao = Convert.ToDateTime(linha["data_Dcoumento"].ToString());

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

                    listaDecisao.Add(o);
                }

                return listaDecisao;
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
