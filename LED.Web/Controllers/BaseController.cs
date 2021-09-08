using LED.Web.Models;
using EstruturaPadrao;
using System.Web.Mvc;
using App = System.Configuration.ConfigurationManager;

namespace LED.Web.Controllers
{
	public class BaseController : Controller
    {

        private EstruturaIdentificacaoUsuario estUsuario;

        public BaseController()
        {
            var objIdentUsu = new IdentificacaoUsuario();
            estUsuario = objIdentUsu.RecuperaUsuarioLogado();
		}

		[HttpGet]
		public string Logout()
		{
			Session.Abandon();
			Session.Clear();
			string Ambiente = App.AppSettings["AMBIENTE"].ToString();
			string key = "SEGWEB." + Ambiente;
			return App.AppSettings[key];
		}

	}
}