using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LED.Web.Models
{
    public class Menu
    {

        public List<ItemMenu> lstItemMenu = new List<ItemMenu>()
        {
            new ItemMenu() { ordCategoria = 1, ordPagina = 1, icone = "fa fa-home fa-fw", categoria = "Página Principal", pagina = "Página Principal", sigla = "MNP", controller = "Home", action = "Index" },
            //new ItemMenu() { ordCategoria = 2, ordPagina = 1, icone = "fa fa-edit fa-fw", categoria = "Cadastros Básicos", pagina = "Área de Manifestação", sigla = "CADTPARMANIF", controller = "TipoAreaManifestacao", action = "Cadastro" }
        };

    }

    public class ItemMenu
    {
        public int ordCategoria { get; set; }
        public int ordPagina { get; set; }
        public string icone { get; set; }
        public string categoria { get; set; }
        public string pagina { get; set; }
        public string sigla { get; set; }
        public string controller { get; set; }
        public string action { get; set; }
    }

}