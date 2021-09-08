using EstruturaPadrao;
using LED.DAL;
using LED.Entities;
using LED.Web.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using App = System.Configuration.ConfigurationManager;

namespace LED.Web.Controllers
{
    public class DocumentoDisponivelController : Controller
    {
        private readonly EstruturaIdentificacaoUsuario _identificacaoUsuario;
        private readonly DocumentoSeiDAL _dalDocumentoSei;
        private readonly AutoridadeDAL _dalAutoridade;
        private readonly ParecerDAL _dalParecer;
        private readonly DecisaoDAL _dalDecisao;
       
        public DocumentoDisponivelController()
        {
            _identificacaoUsuario = new IdentificacaoUsuario().RecuperaUsuarioLogado();
            _dalDocumentoSei = new DocumentoSeiDAL(_identificacaoUsuario);
            _dalAutoridade = new AutoridadeDAL(_identificacaoUsuario);
            _dalParecer = new ParecerDAL(_identificacaoUsuario);
            _dalDecisao = new DecisaoDAL(_identificacaoUsuario);
            

        }
        // GET: DocumentoDisponivel
        public ActionResult Consultar()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Listar(PesquisaDocumentoViewModel parametros)
        {

            if (parametros.Load && String.IsNullOrEmpty(parametros.DataFinal) && String.IsNullOrEmpty(parametros.DataInicial) && String.IsNullOrEmpty(parametros.NumeroProcesso) && String.IsNullOrEmpty(parametros.TipoDocumento))
                throw new Exception();

            DocumentoSei documento = new DocumentoSei();
            var textoResposta = documento.ValidaCampos(parametros.NumeroProcesso, parametros.DataInicial, parametros.DataFinal, parametros.TipoDocumento, "DOCDISCON");

            if (!String.IsNullOrEmpty(textoResposta))
            {
                throw new Exception(textoResposta);
            }

            var lstDocumento = _dalDocumentoSei.BuscaDocumentoSei(parametros.NumeroProcesso, parametros.DataInicial, parametros.DataFinal, parametros.TipoDocumento);

            return Json(new { data = lstDocumento }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CarregaDadosEntrada(string numeroProcesso, string nomeTipoDocumento, string dataDocumento)
        {

            var documento = _dalDocumentoSei.BuscaDocumentoSei(numeroProcesso, dataDocumento, "", nomeTipoDocumento).FirstOrDefault();

            if(documento.idAnexo > 0)
            {
                return Json(new { data = false, msg = "O documento esta anexado e o sistema por enquanto não tem tratamento para esse documento" }, JsonRequestBehavior.AllowGet);
            }


            var documentoParaAnalise = CarregaDocumentoParaAnalise(documento);

            if(!String.IsNullOrEmpty(documentoParaAnalise.MensagemErroSei))
            {
                return Json(new { data = false, msg = documentoParaAnalise.MensagemErroSei }, JsonRequestBehavior.AllowGet);
            }
            

            TempData["documento"] = documentoParaAnalise;

            return Json(new { data = true, tipoDocumento = documento.siglaTipoDocumento }, JsonRequestBehavior.AllowGet);//View(documentoParaAnalise);
        }

        public AnaliseDocumentoSEI CarregaDocumentoParaAnalise(DocumentoSei documentoEmAnalise)
        {
            AnaliseDocumentoSEI documento = new AnaliseDocumentoSEI();
            SeiServico.SeiService servicoSei = new SeiServico.SeiService(App.AppSettings["AMBIENTE"].ToString());

            var retornoProcedimento = servicoSei.consultarProcedimento(Constantes.SiglaServico, Constantes.IdentificacaoServico, documentoEmAnalise.idOrgaoGerador.ToString(), documentoEmAnalise.numeroProcesso, "S", "S", "S", "S", "S", "S", "S", "S", "S");
            var retornoDocumento = servicoSei.consultarDocumento(Constantes.SiglaServico, Constantes.IdentificacaoServico, documentoEmAnalise.idOrgaoGerador.ToString(), documentoEmAnalise.numeroDocumento, "S", "S", "S", "S");


            documento.integraDocumento = AbrirDocumentoIntegra(retornoDocumento.LinkAcesso);

            //Pegar o necessário
            documento.integraDocumento = FuncoesBasicas.PegaConteudoEntreTags(documento.integraDocumento, "<body>", "</body>");

            //Em caso de Erro no SEI
            if(documento.integraDocumento.IndexOf("infraExcecao") > 0)
            {
                documento.MensagemErroSei = String.Concat("Documento não pode ser carregado  - ", FuncoesBasicas.PegaConteudoEntreTags(documento.integraDocumento,String.Concat( "<span class=","\"","infraExcecao","\"",">"), "</span>"));
                return documento;
            }

            documento.numeroProcesso = retornoProcedimento.ProcedimentoFormatado;
            documento.idDocumento = documentoEmAnalise.idDocumento;
            documento.linkAcesso = retornoDocumento.LinkAcesso;
            documento.tipoDocumento = documentoEmAnalise.siglaTipoDocumento;
            documento.dataDocumento = retornoDocumento.Data;
            //documento.AnoDocumento = retornoDocumento.Data.Substring(6,4);
            //documento.dataPublicacao = retornoDocumento.Publicacao == null ? "" : retornoDocumento.Publicacao.DataPublicacao;

            int seq = 1;
            List<Interessado> listaInteressados = new List<Interessado>();

            foreach (var interessado in retornoProcedimento.Interessados)
            {
                Interessado item = new Interessado();
                item.sequencia = seq;
                item.nome = interessado.Nome;
                item.sigla = interessado.Sigla;
                listaInteressados.Add(item);
                seq++;
            }

            documento.parte = listaInteressados;
           
            documento.sigilosoNao = documento.ClassificarSigilo(Convert.ToInt32(retornoDocumento.NivelAcessoLocal));
            documento.sigilosoSim = documento.ClassificarSigilo(Convert.ToInt32(retornoDocumento.NivelAcessoLocal));
            //documento.disponibilizadoWebNao = documento.ClassificarDisponivelWeb(Convert.ToInt32(retornoDocumento.NivelAcessoGlobal));
            //documento.disponibilizadoWebSim = documento.ClassificarDisponivelWeb(Convert.ToInt32(retornoDocumento.NivelAcessoGlobal));



            return documento;


        }

        public string AbrirDocumentoIntegra(string Link)
        {
            Uri link = new Uri(Link);
            string documento = "";

            using (WebClient cliente = new WebClient())
            {
                try
                {
                    documento = cliente.DownloadString(link);
                }
                catch (WebException we)
                {
                    return "Não Foi possivel Carregar o Documento do SEI";
                }

            }

            return documento;
        }

        [HttpGet]
        public ActionResult AnalisarParecer()
        {
            if(TempData["documento"] == null)
            {
                TempData["documento"] = TempData["Parecer"];
                var documentoAnalisado = (AnaliseDocumentoSEI)TempData["Parecer"];
                return View(documentoAnalisado);
            }
            else
            {
                TempData["Parecer"] = TempData["documento"];
                var documentoAnalisado = (AnaliseDocumentoSEI)TempData["documento"];
                return View(documentoAnalisado);

            }
                                 
        }

        [HttpGet]
        public ActionResult AnalisarDecisao(string pIdDocumento = "")
        {

            if (TempData["documento"] == null)
            {
                TempData["documento"] = TempData["Decisao"];
                var documentoAnalisado = (AnaliseDocumentoSEI)TempData["Decisao"];
                return View(documentoAnalisado);
            }
            else
            {
                TempData["Decisao"] = TempData["documento"];
                var documentoAnalisado = (AnaliseDocumentoSEI)TempData["documento"];
                return View(documentoAnalisado);
            }

        }

        [HttpGet]
        public ActionResult CarregaComboAutoridadeProla(bool pCarregaDecisao = false)
        {
            var lstAutoridade = _dalAutoridade.ListarAutoridade();

            if(pCarregaDecisao)
            {
                var itemRemover = lstAutoridade.Where(x => x.codAutoridade == 9 || x.codAutoridade == 10 || x.codAutoridade == 12 || x.codAutoridade == 13).ToList();

                foreach(var item in itemRemover)
                {
                    lstAutoridade.Remove(item);
                }
                
            }
            return Json(new { data = lstAutoridade }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CarregaComboMagistrado(string pMatricula,  string pNome)
        {            
            var lstAutoridade = _dalAutoridade.ListarMagistrados(pMatricula == "0" ? "" : pMatricula, pNome);
            return Json(new { data = lstAutoridade }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
       public ActionResult Dispensar(int idDocumento)
       {

            try
            {
                _dalDocumentoSei.Alterar_Satatus_DocumentoSei(idDocumento,"D");
                return Json(new { data = true }, JsonRequestBehavior.AllowGet);
            }
             
            catch (Exception ex)
            {
                throw ex;
            }
                
       }

       [HttpPost]
       public ActionResult ConfirmarAnalise(int pIdDocumento, string pAutoridade, string pMagistrado, string pDataDocumento,string pDataPublicacao,string pAutoridadeTCE, string pNProcessoTCE, string pExisteDecisaoOuParecer, string pSigiloso, string pDisponivelWeb, string pAnoParecer = "",string pNProcessConsMagistratura = "")
        {
            

            var documentoSei = _dalDocumentoSei.BuscaDocumentoSei(pIdDocumento);

            var documentoAnalisado = CarregaDocumentoParaAnalise(documentoSei);

            if (documentoAnalisado.tipoDocumento != Constantes.Decisao && documentoAnalisado.tipoDocumento != Constantes.Parecer)
            {
                return Json(new { data = false, msg = "Esse documento não é uma Decisão ou Parecer. Por favor entrar em contato com o suporte do sistema." }, JsonRequestBehavior.AllowGet);
            }

            var texto = documentoAnalisado.ValidaAnalise(documentoAnalisado.tipoDocumento, pAutoridade, pMagistrado, pDataDocumento, pExisteDecisaoOuParecer, pDisponivelWeb);

            if (!String.IsNullOrEmpty(texto))
            {
                return Json(new { data = false, msg = texto }, JsonRequestBehavior.AllowGet);
            }

            try
            {

                if (documentoAnalisado.tipoDocumento == Constantes.Parecer)
                {
                    Parecer documento = new Parecer();
          
                    documento.autoridadeTCE = pAutoridadeTCE;
                    if(!string.IsNullOrEmpty(pAutoridade))
                        documento.codAutoridade = Convert.ToInt32(pAutoridade);                    
                    documento.dataAtualizacao = System.DateTime.Now;
                    documento.dataInclusao = System.DateTime.Now;
                    if (!string.IsNullOrEmpty(pAnoParecer))
                        documento.anoParecer = Convert.ToInt32(pAnoParecer);
                    documento.dataParecer = Convert.ToDateTime(pDataDocumento);
                    if (!string.IsNullOrEmpty(pDataPublicacao))
                        documento.dataPublicacao = pDataPublicacao;
                    documento.disponibilizadoWeb = pDisponivelWeb;
                    documento.Sigiloso = pSigiloso;
                    documento.existeDecisao = pExisteDecisaoOuParecer;
                    documento.integra = PegarIntegra(documentoAnalisado.integraDocumento);
                    if (!string.IsNullOrEmpty(pMagistrado))
                        documento.matriculaMagistrado = Convert.ToInt32(pMagistrado);
                    documento.matriculaServidor = _identificacaoUsuario.Matricula;
                    documento.usuario = _identificacaoUsuario.Login; 
                    //documento.numeroParecer =                   
                    documento.numeroProcessoTCE = pNProcessoTCE;
                    documento.processo = FuncoesBasicas.RemoveFormatacao(documentoAnalisado.numeroProcesso);
                    documento.idDocumento = documentoAnalisado.idDocumento;

                    _dalParecer.ConfirmarAnalise(documento);
                    
                }

                if (documentoAnalisado.tipoDocumento == Constantes.Decisao)
                {
                    Decisao documento = new Decisao();
                  
                    documento.ConselheiroTCE = pAutoridadeTCE;
                    if (!string.IsNullOrEmpty(pAutoridade))
                        documento.codAutoridade = Convert.ToInt32(pAutoridade);
                    documento.dataAtualizacao = System.DateTime.Now;
                    documento.dataInclusao = System.DateTime.Now;
                    documento.dataDecisao = Convert.ToDateTime(pDataDocumento);
                    if (!string.IsNullOrEmpty(pDataPublicacao))
                        documento.dataPublicacao = pDataPublicacao;
                    documento.disponibilizadoWeb = pDisponivelWeb;
                    documento.Sigiloso = pSigiloso;
                    documento.decisaoComParecer = pExisteDecisaoOuParecer;
                    documento.integra = PegarIntegra(documentoAnalisado.integraDocumento);
                    if (!string.IsNullOrEmpty(pMagistrado))
                        documento.matriculaMagistrado = Convert.ToInt32(pMagistrado);
                    documento.matriculaServidor = _identificacaoUsuario.Matricula;
                    documento.usuario = _identificacaoUsuario.Login;
                    //documento.numeroParecer =                   
                    documento.numeroProcessoTCE = pNProcessoTCE;
                    documento.processo = FuncoesBasicas.RemoveFormatacao(documentoAnalisado.numeroProcesso);
                    documento.idDocumento = documentoAnalisado.idDocumento;
                    if (!string.IsNullOrEmpty(pNProcessConsMagistratura))
                        documento.procConselhoMagistratura = Convert.ToInt32(FuncoesBasicas.RemoveFormatacao(pNProcessConsMagistratura));
                    _dalDecisao.ConfirmarAnalise(documento);
                }

                _dalDocumentoSei.Alterar_Satatus_DocumentoSei(documentoAnalisado.idDocumento, "A");
                return Json(new { data = true }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                throw ex;
            }

        }

        public string PegarIntegra(string texto)
        {
            var totalPalavrasUsadas = FuncoesBasicas.ContaQuantidadeTagEncontrada(texto, "table");

            for (int i = 0; i < totalPalavrasUsadas/2; i++)
            {

                var textoConteudo = FuncoesBasicas.PegaConteudoEntreTags(texto, "<table", "</table>", false);
                texto = texto.Replace(textoConteudo, string.Empty);

            }

            texto = FuncoesBasicas.RemoveTodasTags(texto);

            return texto;
        }

    }
}