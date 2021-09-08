using EstruturaPadrao;
using System;
using System.Diagnostics;
using System.Web;
using System.Web.Mvc;
using App = System.Configuration.ConfigurationManager;

namespace LED.Web.Models
{
    public class IdentificacaoUsuario
	{

		private EstruturaIdentificacaoUsuario estUsuario;

		[OutputCache(Duration = 0)]
		public EstruturaIdentificacaoUsuario RecuperaUsuarioLogado()
		{
			SegurancaWeb.ServicoCofre.EstUsuario objEstUsuSeg = new SegurancaWeb.ServicoCofre.EstUsuario();

			objEstUsuSeg = (SegurancaWeb.ServicoCofre.EstUsuario)HttpContext.Current.Session["LEDWEB"];

			Trace.WriteLineIf(objEstUsuSeg == null, "objEstUsuSeg == null > " + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

			if (objEstUsuSeg is null)
			{
				string Ambiente = App.AppSettings["AMBIENTE"].ToString();
				string key = "SEGWEB." + Ambiente;
				HttpContext.Current.Response.Redirect(App.AppSettings[key], false);
			}
			else
			{
				estUsuario.IDPessoa = objEstUsuSeg.idPessoa;
				estUsuario.IDUsuario = objEstUsuSeg.idUsu;
				estUsuario.Login = objEstUsuSeg.codUsu;
				estUsuario.LoginAutent = objEstUsuSeg.codUsu;
				estUsuario.Matricula = objEstUsuSeg.matricula;
				estUsuario.NomeMaquina = Environment.MachineName;
				estUsuario.Senha = "";
				estUsuario.SenhaAutent = "";
				estUsuario.SiglaSist = App.AppSettings["siglaSistema"];
				estUsuario.UsuarioSO = objEstUsuSeg.codUsu;

				//Adiciona novamente a estrutura usuario do servico de cofre, na sessao.
				HttpContext.Current.Session.Add("LEDWEB", objEstUsuSeg);
			}

			return estUsuario;
		}


		public EstruturaIdentificacaoUsuario CriarVisitante()
		{
			var usuario = new EstruturaIdentificacaoUsuario()
			{
				CodOrg = 1,
				Login = "OUVIDORIA",
				LoginAutent = "PORTAL",
				NomeMaquina = "SERVIDORWEBDOTNET",
				Senha = "OUVIDORIA",
				SenhaAutent = "OUVIDORIA",
				SiglaSist = "OUVIDORIA",
				UsuarioSO = "WINSERVER"
			};

			return usuario;
		}

	}
}