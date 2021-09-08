using EstruturaPadrao;
using LED.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LED.DAL
{
    public class InteressadoDAL : baseDAL
    {
        public InteressadoDAL(EstruturaIdentificacaoUsuario estUsuario) : base(estUsuario)
        {

        }

        public List<Interessado> ListarInterssado(string pProcesso = "")
        {
            try
            {
                sd.Open();
                List<Interessado> listaInteressado = new List<Interessado>();

                DataTable dt = new DataTable();

                dt = sd.ExecutaProcDS("PKG_PERSONAGEM.CONSULTA_PERSONAGEM",
                string.IsNullOrEmpty(pProcesso) ? DBNull.Value : (object)pProcesso,
                sd.CriaRefCursor("C")).Tables[0];

                foreach (var linha in dt.Rows.Cast<DataRow>())
                {
                    var o = new Interessado();

                    o.nome = linha["personagem"].ToString();
                    o.sequencia = Convert.ToInt32(linha["sequencial"].ToString());
                    o.sigla = linha["processo"].ToString();


                    listaInteressado.Add(o);
                }

                return listaInteressado;
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

        public List<Interessado> ListarProcessosInterssado(string listaProcessos)
        {
            try
            {
                sd.Open();
                List<Interessado> listaInteressado = new List<Interessado>();

                DataTable dt = new DataTable();

                dt = sd.ExecutaProcDS("PKG_PERSONAGEM.CONSULTA_PROCESSOS_PERSONAGENS",
                string.IsNullOrEmpty(listaProcessos) ? DBNull.Value : (object)listaProcessos,
                sd.CriaRefCursor("C")).Tables[0];

                foreach (var linha in dt.Rows.Cast<DataRow>())
                {
                    var o = new Interessado();

                    o.nome = linha["personagem"].ToString();
                    o.sequencia = Convert.ToInt32(linha["sequencial"].ToString());
                    o.sigla = linha["processo"].ToString();


                    listaInteressado.Add(o);
                }

                return listaInteressado;
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
