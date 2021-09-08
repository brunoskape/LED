using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LED.Entities
{
    public class Validador
    {

        public string ValidarData(string pDataInicial, string pDataFinal)
        {
            if (String.IsNullOrWhiteSpace(pDataInicial))
                return "Favor informar o data inicial da consulta.";
            if (string.IsNullOrWhiteSpace(pDataFinal))
                return "Favor informar o data final da consulta.";


            DateTime dateValue;
            if (DateTime.TryParse(pDataInicial, out dateValue) == false)
                return "Favor informar o data inicial da consulta válida.";

            if (DateTime.TryParse(pDataFinal, out dateValue) == false)
                return "Favor informar o data final da consulta válida.";

            if (DateTime.Compare(Convert.ToDateTime(pDataInicial), Convert.ToDateTime(pDataFinal)) > 0)
                return "O data inicial não pode ser maior que a data final!";

            if (pDataInicial.Length < 10 || pDataFinal.Length < 10)
                return "A data informada não se encontra no formado dd/mm/yyyyy!";

            if (Convert.ToInt32(pDataInicial.Substring(6, 4)) < 1900)
                return "O Ano da data inicial não pode ser menor que o ano 1900!";

            if (Convert.ToInt32(pDataFinal.Substring(6, 4)) < 1900)
                return "O Ano da data Final não pode ser menor que o ano 1900!";

            if (Convert.ToInt32(pDataInicial.Substring(6, 4)) < 2002)
                return "O Ano da data Inicial só pode ser maior ou igual a 2002!";

            return "";
        }

        public string ValidaCampos(String pNumeroDocumento, String pDataInicial, String pDataFinal, String pTipoDocumento,string siglaTela)
        {
            string textoResposta = "";

            if (siglaTela == Constantes.codTelaConsultaDocumentoDisponivel)
            {
                if(String.IsNullOrEmpty(pNumeroDocumento) && String.IsNullOrEmpty(pTipoDocumento))
                {
                    textoResposta = ValidarData(pDataInicial, pDataFinal);
                    
                } 
                else if(!String.IsNullOrEmpty(pTipoDocumento))
                {
                    if(String.IsNullOrEmpty(pNumeroDocumento) && String.IsNullOrEmpty(pDataInicial) && String.IsNullOrEmpty(pDataFinal))
                    {
                        textoResposta = "Por favor informar o nº do processo ou um período que deseja consultar.";
                    }
                        else if(String.IsNullOrEmpty(pNumeroDocumento))
                    {
                        textoResposta = ValidarData(pDataInicial, pDataFinal);
                    }
                }

            }

            return textoResposta;
        }

        public string ValidaAnalise(string pTipoDocumento, string pAutoridade, string pMagistrado, string pDataDocumento, string pExisteDecisaoOuParecer, string pDisponivelWeb)
        {
            string textoResposta = "";

            if(pTipoDocumento == Constantes.Parecer)
            {
                if (String.IsNullOrEmpty(pDataDocumento))
                {
                    textoResposta = "Por favor solicitar o preenchimento da Data do Parecer para o sistema SEI";
                }

                if (String.IsNullOrEmpty(pExisteDecisaoOuParecer))
                {
                    textoResposta = "Por favor selecionar uma opção no campo 'Existe Decisão ?'  ";
                }

                if (String.IsNullOrEmpty(pDisponivelWeb))
                {
                    textoResposta = "Por favor selecionar uma opção no campo 'Disponibilizar na Web'  ";
                }

                if (Constantes.AutoridadeComMagistradoParecer.IndexOf(pAutoridade) > 0 )
                {
                    if(String.IsNullOrEmpty(pMagistrado))
                    textoResposta = "Por favor informar o magistrado.";
                }

            }

            if (pTipoDocumento == Constantes.Decisao)
            {
                if (String.IsNullOrEmpty(pDataDocumento))
                {
                    textoResposta = "Por favor solicitar o preenchimento da Data da Decisao para o sistema SEI";
                }

                if (String.IsNullOrEmpty(pExisteDecisaoOuParecer))
                {
                    textoResposta = "Por favor selecionar uma opção no campo 'Decisão com parecer ?'  ";
                }

                if (String.IsNullOrEmpty(pDisponivelWeb))
                {
                    textoResposta = "Por favor selecionar uma opção no campo 'Disponibilizar na Web'  ";
                }

                if (Constantes.AutoridadeComMagistradoDecisao.IndexOf(pAutoridade) > 0)
                {
                    if (String.IsNullOrEmpty(pMagistrado))
                        textoResposta = "Por favor informar o magistrado.";
                }

            }

            return textoResposta;

        }


        public string ValidaConsultaPortal(string pNumeroProcesso, string pDataInicio, string pDataFim, string pAssunto, string pAutoridadeProlatora)
        {
            string textoResposta = "";

            if(String.IsNullOrEmpty(pNumeroProcesso) && String.IsNullOrEmpty(pDataInicio) && String.IsNullOrEmpty(pDataFim) && String.IsNullOrEmpty(pAssunto) && String.IsNullOrEmpty(pAutoridadeProlatora))
            {
                textoResposta = "Nenhuma informação foi preenchida na aba selecionada.";
            }
            else if (String.IsNullOrEmpty(pNumeroProcesso))
            {
                textoResposta = ValidarData(pDataInicio, pDataFim);

                if(String.IsNullOrEmpty(textoResposta))
                {
                    if (!String.IsNullOrEmpty(pAutoridadeProlatora) && String.IsNullOrEmpty(pAssunto))
                    {
                        textoResposta = "Por favor preencher o Campo Assunto quando selecionar a Autoridade Prolatora.";
                    }
                }
               
            }

            

            return textoResposta;

        }
    }
}
