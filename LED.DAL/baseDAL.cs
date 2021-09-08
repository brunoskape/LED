using DataProtection;
using EstruturaPadrao;
using ServicoDadosODPNETM;
using App = System.Configuration.ConfigurationManager;

namespace LED.DAL
{
    public class baseDAL
    {

        private string strConexao;
        private EstruturaIdentificacaoUsuario usu;
        protected ServicoDadosOracle sd;

        public baseDAL(EstruturaIdentificacaoUsuario estrUsuario)
        {
            if (sd is null)
            {
                this.sd = new ServicoDadosOracle();
                this.sd.ConnectionString = this.StringConexao();
                this.sd.Connection.TnsAdmin = App.AppSettings["TNSADMIN"].ToString() + string.Empty;
            }
            this.sd.Usuario = estrUsuario;
        }

        protected string StringConexao()
        {

            OracleConnectionStringProtector dp = new OracleConnectionStringProtector();
            dp.SecureKey = App.AppSettings["Chave"];
            this.strConexao = (App.AppSettings[App.AppSettings["AMBIENTE"] + ".StringConexao"]).Replace("#SENHA#", dp.Decrypt(App.AppSettings[App.AppSettings["AMBIENTE"] + ".SenhaConexao"]));

            return this.strConexao;
        }
        
    }
}
