using LED.Entities;
using LED.DAL;
using LED.Web.Models;
using EstruturaPadrao;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace LED.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly EstruturaIdentificacaoUsuario _estUsuario;


        public HomeController()
        {
            _estUsuario = new IdentificacaoUsuario().RecuperaUsuarioLogado();

        }

        

        public ActionResult Index()
        {
            return View();
        }
    }
}