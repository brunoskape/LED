using EstruturaPadrao;
using LED.DAL;
using LED.Entities;
using LED.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using App = System.Configuration.ConfigurationManager;

namespace LED.Web.Controllers
{
    public class PortalController : Controller
    {
        private readonly EstruturaIdentificacaoUsuario _identificacaoUsuario;
        private readonly ParecerDAL _dalParecer;
        private readonly DecisaoDAL _dalDecisao;
        private readonly InteressadoDAL _dalInteressado;

        public PortalController()
        {
            _identificacaoUsuario = new IdentificacaoUsuario().CriarVisitante();
            _dalParecer = new ParecerDAL(_identificacaoUsuario);
            _dalDecisao = new DecisaoDAL(_identificacaoUsuario);
            _dalInteressado = new InteressadoDAL(_identificacaoUsuario);

        }
        // GET: Portal
        public ActionResult Consultar()
        {
            return View();
        }
        public ActionResult ProcessoDocumento()
        {
            List<DocumentoPortal> listaDocumento = new List<DocumentoPortal>();

            if(TempData["documento"] == null)
            {
                TempData["documento"] = TempData["Lista"];
                foreach (var item in (IEnumerable<DocumentoPortal>)TempData["Lista"])
                {
                    listaDocumento.Add(item);
                }
            }
            else
            {
                TempData["Lista"] = TempData["documento"];

                foreach (var item in (IEnumerable<DocumentoPortal>)TempData["documento"])
                {
                    listaDocumento.Add(item);
                }
            }

            return View(listaDocumento);
        }

        public ActionResult Documento(string pProcesso, int pSequencial, string pTipoDocumento)       
        {
            DocumentoPortal.DocumentosProcesso documento = new DocumentoPortal.DocumentosProcesso();

            if (pTipoDocumento == Constantes.Parecer)
            {
                var listaParecer = _dalParecer.ConsultaParcerPortal(pProcesso, "", "", "", "");

                 var parecer = listaParecer.Where(x => x.numeroParecer == pSequencial).FirstOrDefault();

                documento.integra = parecer.integra;
            }

            if (pTipoDocumento == Constantes.Decisao)
            {
                var listaDecisao = _dalDecisao.ConsultaDecisaoPortal(pProcesso, "", "", "", "");

                var decisao = listaDecisao.Where(x => x.numeroDecisao == pSequencial).FirstOrDefault();

                documento.integra = decisao.integra;
            }

            return View(documento);
        }

        [HttpGet]
        public ActionResult CarregaDadosEntrada(string pNumeroProcesso, string pDataInicio, string pDataFim, string pAssunto, string pAutoridadeProlatora)
        {
            List<DocumentoPortal> listaDocumentoPortal = new List<DocumentoPortal>();

            var textoVerificacao = new Validador().ValidaConsultaPortal(pNumeroProcesso, pDataInicio, pDataFim, pAssunto, pAutoridadeProlatora);

            if (!String.IsNullOrEmpty(textoVerificacao))
            {
                return Json(new { data = false, msg = textoVerificacao }, JsonRequestBehavior.AllowGet);
            }

            var listaParecer = _dalParecer.ConsultaParcerPortal(pNumeroProcesso, pDataInicio, pDataFim, pAssunto, pAutoridadeProlatora);

            var listaDecisao = _dalDecisao.ConsultaDecisaoPortal(pNumeroProcesso, pDataInicio, pDataFim, pAssunto, pAutoridadeProlatora);
          

            if (listaParecer.Count == 0 && listaDecisao.Count == 0)
            {
                return Json(new { data = false, msg = "Nenhum processo foi encontrado." }, JsonRequestBehavior.AllowGet);
            }

            var listaDocumentoPortalTemp = CarregaDocumentoPortal(listaParecer, listaDecisao, pNumeroProcesso, pDataInicio, pDataFim, pAssunto, pAutoridadeProlatora);

            SeiServico.SeiService servicoSei = new SeiServico.SeiService(App.AppSettings["AMBIENTE"].ToString());

            var lstPessoaLegado = CarregaPorcessoInterssados(listaDocumentoPortalTemp);

            foreach (var processo in listaDocumentoPortalTemp)
            {
                //listaDocumentoPortal.Remove(processo);

                if (processo.numeroProcesso.Length == Constantes.tamanhoProcessoSei && processo.numeroProcesso.IndexOf(Constantes.processoSEI,4) > 0)
                {
                    var listaDcocumentos = processo.listDocumentosProcesso.ToList();

                    foreach (var documento in listaDcocumentos)
                    {
                        processo.listDocumentosProcesso.Remove(documento);

                        var retornoProcedimentoParecer = servicoSei.consultarProcedimento(Constantes.SiglaServico, Constantes.IdentificacaoServico, documento.idorgaoGerador, processo.numeroProcesso, "S", "S", "S", "S", "S", "S", "S", "S", "S");
                        var retornoDocumentoParecer = servicoSei.consultarDocumento(Constantes.SiglaServico, Constantes.IdentificacaoServico, documento.idorgaoGerador, documento.numeroDocumento, "S", "S", "S", "S");

                        documento.linkDocumento = retornoDocumentoParecer.LinkAcesso;

                        int seq = 1;
                        List<Interessado> lstPessoa = new List<Interessado>();

                        foreach (var parte in retornoProcedimentoParecer.Interessados.ToList())
                        {
                            Interessado pessoa = new Interessado();
                            pessoa.sequencia = seq;
                            pessoa.nome = parte.Nome;
                            pessoa.sigla = parte.Sigla;

                            lstPessoa.Add(pessoa);
                            seq++;
                        }

                        processo.listParte = lstPessoa;
                        processo.eProcessoSEI = true;
                        processo.listDocumentosProcesso.Add(documento);
                    }


                }
                else
                {
                    var lstPessoa = lstPessoaLegado.Where(x => x.sigla == processo.numeroProcesso).ToList();//_dalInteressado.ListarInterssado(processo.numeroProcesso);
                    processo.listParte = lstPessoa;
                    processo.eProcessoSEI = false;
                    processo.filtroNumeroProcesso = processo.filtroNumeroProcesso.Replace("_", string.Empty);
                }

                listaDocumentoPortal.Add(processo);
            }


            TempData["documento"] = listaDocumentoPortal;

            return Json(new { data = true}, JsonRequestBehavior.AllowGet);
        }

        public List<DocumentoPortal> CarregaDocumentoPortal(List<Parecer> listParecer, List<Decisao> listDecisao, string pNumeroProcesso, string pDataInicio, string pDataFim, string pAssunto, string pAutoridadeProlatora)
        {
            List<DocumentoPortal> listDocumentoPortal = new List<DocumentoPortal>();

            foreach(var parecer in listParecer)
            {
                var documentoDoPortal = listDocumentoPortal.Where(x => x.numeroProcesso == parecer.processo).FirstOrDefault();

                if (documentoDoPortal == null)
                {
                    DocumentoPortal doc = new DocumentoPortal();
                    doc.numeroProcesso = parecer.processo;                  
                    doc.filtroNumeroProcesso = pNumeroProcesso;
                    doc.filtroDataInicial = pDataInicio;
                    doc.filtroFinal = pDataFim;
                    doc.filtroAssunto = pAssunto;
                    doc.filtroAutoridade = pAutoridadeProlatora;

                    List<DocumentoPortal.DocumentosProcesso> lista = new List<DocumentoPortal.DocumentosProcesso>();
                    DocumentoPortal.DocumentosProcesso docProcesso = new DocumentoPortal.DocumentosProcesso();
                    docProcesso.autoridadeProlatora = parecer.NomeAutoridade;
                    docProcesso.dataDocumento =  parecer.dataParecer.ToString().Substring(0,10);
                    docProcesso.dataPublicacao = parecer.dataPublicacao;
                    docProcesso.disponibilizarWeb = parecer.disponibilizadoWeb == "S" ? true : false;
                    docProcesso.idorgaoGerador = parecer.idorgaoGerador;
                    docProcesso.integra = parecer.integra;
                    docProcesso.numeroDocumento = parecer.numeroDocumento;
                    docProcesso.sigiloso = parecer.Sigiloso == "S" ? true : false;
                    docProcesso.tipoDocumento = Constantes.Parecer;
                    docProcesso.seqDocumento = parecer.numeroParecer;
                    lista.Add(docProcesso);

                    doc.listDocumentosProcesso = lista;
                    listDocumentoPortal.Add(doc);
                }
                else
                {
                    listDocumentoPortal.Remove(documentoDoPortal);

                    DocumentoPortal.DocumentosProcesso docProcesso = new DocumentoPortal.DocumentosProcesso();
                    docProcesso.autoridadeProlatora = parecer.NomeAutoridade;
                    docProcesso.dataDocumento = parecer.dataParecer.ToString().Substring(0, 10);
                    docProcesso.dataPublicacao = parecer.dataPublicacao;
                    docProcesso.disponibilizarWeb = parecer.disponibilizadoWeb == "S" ? true : false;
                    docProcesso.idorgaoGerador = parecer.idorgaoGerador;
                    docProcesso.integra = parecer.integra;
                    docProcesso.numeroDocumento = parecer.numeroDocumento;
                    docProcesso.sigiloso = parecer.Sigiloso == "S" ? true : false;
                    docProcesso.tipoDocumento = Constantes.Parecer;
                    docProcesso.seqDocumento = parecer.numeroParecer;
                    documentoDoPortal.listDocumentosProcesso.Add(docProcesso);

                    listDocumentoPortal.Add(documentoDoPortal);
                }
            }

            foreach (var decisao in listDecisao)
            {
                var documentoDoPortal = listDocumentoPortal.Where(x => x.numeroProcesso == decisao.processo).FirstOrDefault();

                if (documentoDoPortal == null)
                {
                    DocumentoPortal doc = new DocumentoPortal();
                    doc.numeroProcesso = decisao.processo;
                    doc.filtroNumeroProcesso = pNumeroProcesso;
                    doc.filtroDataInicial = pDataInicio;
                    doc.filtroFinal = pDataFim;
                    doc.filtroAssunto = pAssunto;
                    doc.filtroAutoridade = pAutoridadeProlatora;

                    List<DocumentoPortal.DocumentosProcesso> lista = new List<DocumentoPortal.DocumentosProcesso>();
                    DocumentoPortal.DocumentosProcesso docProcesso = new DocumentoPortal.DocumentosProcesso();
                    docProcesso.autoridadeProlatora = decisao.NomeAutoridade;
                    docProcesso.dataDocumento = decisao.dataDecisao.ToString().Substring(0, 10);
                    docProcesso.dataPublicacao = decisao.dataPublicacao;
                    docProcesso.disponibilizarWeb = decisao.disponibilizadoWeb == "S" ? true : false;
                    docProcesso.idorgaoGerador = decisao.idorgaoGerador;
                    docProcesso.integra = decisao.integra;
                    docProcesso.numeroDocumento = decisao.numeroDocumento;
                    docProcesso.sigiloso = decisao.Sigiloso == "S" ? true : false;
                    docProcesso.tipoDocumento = Constantes.Decisao;
                    docProcesso.seqDocumento = decisao.numeroDecisao;

                    lista.Add(docProcesso);

                    doc.listDocumentosProcesso = lista;
                    listDocumentoPortal.Add(doc);
                }
                else
                {
                    listDocumentoPortal.Remove(documentoDoPortal);

                    DocumentoPortal.DocumentosProcesso docProcesso = new DocumentoPortal.DocumentosProcesso();
                    docProcesso.autoridadeProlatora = decisao.NomeAutoridade;
                    docProcesso.dataDocumento = decisao.dataDecisao.ToString().Substring(0, 10);
                    docProcesso.dataPublicacao = decisao.dataPublicacao;
                    docProcesso.disponibilizarWeb = decisao.disponibilizadoWeb == "S" ? true : false;
                    docProcesso.idorgaoGerador = decisao.idorgaoGerador;
                    docProcesso.integra = decisao.integra;
                    docProcesso.numeroDocumento = decisao.numeroDocumento;
                    docProcesso.sigiloso = decisao.Sigiloso == "S" ? true : false;
                    docProcesso.tipoDocumento = Constantes.Decisao;
                    docProcesso.seqDocumento = decisao.numeroDecisao;
                    documentoDoPortal.listDocumentosProcesso.Add(docProcesso);

                    listDocumentoPortal.Add(documentoDoPortal);
                }
            }


            return listDocumentoPortal;
        }

        public List<Interessado> CarregaPorcessoInterssados(List<DocumentoPortal> listaDocumentoPortalTemp)
        {
            List<Interessado> listInteressados = new List<Interessado>();
            string textoProcessosEncontrados = "";
            

            if (listaDocumentoPortalTemp.Where(x => x.numeroProcesso.Length < Constantes.tamanhoProcessoSei).Any())
            {
                int contador = 1;

                foreach(var processo in listaDocumentoPortalTemp.Where(x => x.numeroProcesso.Length < Constantes.tamanhoProcessoSei).Select(z => z.numeroProcesso).ToList())
                {


                    if ((textoProcessosEncontrados.Length + processo.Length) <= Constantes.limiteMaxCaraceteres && contador <= Constantes.limiteDeParametrosNoIn)
                    {
                        if(textoProcessosEncontrados.Length == 0)
                        {
                            textoProcessosEncontrados = processo;
                        }
                        else
                        {
                            textoProcessosEncontrados = String.Concat(textoProcessosEncontrados,",", processo);
                        }
                        contador++;
                    }
                    else
                    {
                        listInteressados.AddRange(_dalInteressado.ListarProcessosInterssado(textoProcessosEncontrados));

                        textoProcessosEncontrados = processo;
                        contador = 1;
                    }
                    
                }

                //textoProcessosEncontrados = String.Concat(textoProcessosEncontrados, ")");

                listInteressados.AddRange(_dalInteressado.ListarProcessosInterssado(textoProcessosEncontrados));
            }

            return listInteressados;



        }

    }
}