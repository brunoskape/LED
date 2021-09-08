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
    public class AutoridadeDAL : baseDAL
    {
        public AutoridadeDAL(EstruturaIdentificacaoUsuario estUsuario) : base(estUsuario)
        {
        }

        public List<Autoridade> ListarAutoridade(String pCodAutoridade = "", String pResponsavel = "", String pTipoResponsavel = "")
        {
            try
            {
                sd.Open();
                List<Autoridade> listaAutoridade = new List<Autoridade>();

                DataTable dt = new DataTable();

                dt = sd.ExecutaProcDS("PKG_AUTORIDADE.CONSULTA_AUTORIDADE",
                string.IsNullOrEmpty(pCodAutoridade) ? DBNull.Value : (object)pCodAutoridade,
                string.IsNullOrEmpty(pResponsavel) ? DBNull.Value : (object)pResponsavel,
                string.IsNullOrEmpty(pTipoResponsavel) ? DBNull.Value : (object)pTipoResponsavel,
                sd.CriaRefCursor("C")).Tables[0];

                foreach (var linha in dt.Rows.Cast<DataRow>())
                {
                    var o = new Autoridade();

                    o.codAutoridade = Convert.ToInt32(linha["cod_autoridade"].ToString());                  
                    o.responsavel = linha["responsavel"].ToString();
                    o.tipoResponsavel = Convert.ToInt32(linha["tipo_responsavel"].ToString());


                    listaAutoridade.Add(o);
                }

                return listaAutoridade;
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

        public List<Funcionario> ListarMagistrados(string pMatricula = "", string pNome = "")
        {
            try
            {
                sd.Open();
                List<Funcionario> listaAutoridade = new List<Funcionario>();

                DataTable dt = new DataTable();

                dt = sd.ExecutaProcDS("PKG_AUTORIDADE.CONSULTA_MAGISTRADO",
                string.IsNullOrEmpty(pMatricula) ? DBNull.Value : (object)pMatricula,
                string.IsNullOrEmpty(pNome) ? DBNull.Value : (object)pNome.ToUpper().Trim(),
                sd.CriaRefCursor("C")).Tables[0];

                foreach (var linha in dt.Rows.Cast<DataRow>())
                {
                    var o = new Funcionario();

                    o.matricula = linha["matricula"].ToString();
                    o.Nome = linha["nome"].ToString();
                    
                    listaAutoridade.Add(o);
                }

                return listaAutoridade;
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
