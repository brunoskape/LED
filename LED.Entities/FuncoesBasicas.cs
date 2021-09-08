using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LED.Entities
{
    public class FuncoesBasicas
    {
        public static string PegaConteudoEntreTags(string html, string TagIncial, string TagFinal,bool pegarEntreTag = true)
        {
            int inicio = 0;
            int Fim = 0;

            if (pegarEntreTag)
            {
                inicio = html.IndexOf(TagIncial) + TagIncial.Length;
                Fim = html.IndexOf(TagFinal);
            }
            else
            {
                inicio = html.IndexOf(TagIncial);
                Fim = html.IndexOf(TagFinal) + TagFinal.Length;
            }

           
            return html.Substring(inicio, Fim - inicio);
          
             
        }
        public static string RemoveTodasTags(string html)
        {

            return Regex.Replace(html, "<.*?>", string.Empty);
        }

        public static string RemoveFormatacao(string texto)
        {
            return texto.Replace(".", string.Empty).Replace("-", string.Empty).Replace("/", string.Empty).Replace("\\", string.Empty).Replace("_",string.Empty);//Regex.Replace(texto, @"[^\w\.@-]", string.Empty);
        }


        public static int ContaQuantidadeTagEncontrada(string html,string tagParaEncontrar)
        {           
            var palavras = html.Split(' ');

            //
            string[] palavrasContador = new string[palavras.Length];
            int totalPalavrasUsadas = 0;

            //
            for (int i = 0; i < palavras.Length; i++)
            {
                if(palavras[i].IndexOf(tagParaEncontrar) > 0)
                {
                    totalPalavrasUsadas++;
                }
 
            }

            return totalPalavrasUsadas;

        }

    }
}
