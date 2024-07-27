using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using WordProjetoVideo.Data;
using WordProjetoVideo.Models;

namespace WordProjetoVideo.Controllers
{
    public class HomeController : Controller
    {
        //propriedade privada --> vai ser usada somente dentro da classe
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var documentos = await _context.Documentos.ToListAsync();
            return View(documentos);
        }

        public IActionResult CriarDocumento()
        {
            return View();
        }

        //m�todo ass�crono que retornar um IActionResult (uma resposta HTTP)
        public async Task<IActionResult> EditarDocumento(int id)
        {   
            //procura na tabela "documentos" a primeira apari��o desse id recebido
            //se existir joga o valor desse documento na vari�vel
            var documento = await _context.Documentos.FirstOrDefaultAsync(d => d.Id == id);
            //retorna a p�gina(view) com as informa��es desse "documento"
            return View(documento);
        }

        //m�todo ass�crono que retorna um IActionResult
        public async Task<IActionResult> RemoverDocumento(int id)
        {   
            //verifica na tabela Documentos qual que tem o id correspondnete ao do par�metro e retorna as informa��es no compat�vel
            //nessa vari�vel
            var documento = await _context.Documentos.FirstOrDefaultAsync(d => d.Id == id);

            _context.Remove(documento);//remove esse documento do banco
            await _context.SaveChangesAsync();//salva a altera��o

            return RedirectToAction("Index");//retorna para a p�gina Index
        }

        //m�todo ass�crono que RESPONDE a uma requisi��o POST
        //ele s� roda quando uma requisi��o post ocorre
        //esse m�todo recebe um documento do tipo Documento
        [HttpPost]
        public async Task<IActionResult> EditarDocumento(Documento documentoEditado)
        {   
            //verifica se os atributos dessa model s�o v�lidos
            if(ModelState.IsValid)
            {   
                //procura se o id do documento existe na tabela Documentos
                //se existe as informa�oes desse documento da tabela vai para a vari�vel
                var documento = await _context.Documentos.FirstOrDefaultAsync(d => d.Id == documentoEditado.Id);

                //adicionando os valores dos atributos do documento do par�metro naquele achado na tabela
                documento.Titulo = documentoEditado.Titulo;
                documento.Conteudo = documentoEditado.Conteudo;
                documento.DataAlteracao = DateTime.Now;

                //atualiza a tabela com esse documento com as novas informa��es
                _context.Update(documento);
                await _context.SaveChangesAsync();//salva as informa��es

                return RedirectToAction("Index");//vai para a p�gina index
            }
            else
            {   
                //se tem algum problema volta essa mesma p�gina com as informa��es do documento inicial
                return View(documentoEditado);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CriarDocumento(Documento documentoRecebido)
        {
            if(ModelState.IsValid)
            {
                _context.Add(documentoRecebido);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            else
            {
                return View(documentoRecebido);
            }
        }
    }
}
