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

        //método assícrono que retornar um IActionResult (uma resposta HTTP)
        public async Task<IActionResult> EditarDocumento(int id)
        {   
            //procura na tabela "documentos" a primeira aparição desse id recebido
            //se existir joga o valor desse documento na variável
            var documento = await _context.Documentos.FirstOrDefaultAsync(d => d.Id == id);
            //retorna a página(view) com as informações desse "documento"
            return View(documento);
        }

        //método assícrono que retorna um IActionResult
        public async Task<IActionResult> RemoverDocumento(int id)
        {   
            //verifica na tabela Documentos qual que tem o id correspondnete ao do parâmetro e retorna as informações no compatível
            //nessa variável
            var documento = await _context.Documentos.FirstOrDefaultAsync(d => d.Id == id);

            _context.Remove(documento);//remove esse documento do banco
            await _context.SaveChangesAsync();//salva a alteração

            return RedirectToAction("Index");//retorna para a página Index
        }

        //método assícrono que RESPONDE a uma requisição POST
        //ele só roda quando uma requisição post ocorre
        //esse método recebe um documento do tipo Documento
        [HttpPost]
        public async Task<IActionResult> EditarDocumento(Documento documentoEditado)
        {   
            //verifica se os atributos dessa model são válidos
            if(ModelState.IsValid)
            {   
                //procura se o id do documento existe na tabela Documentos
                //se existe as informaçoes desse documento da tabela vai para a variável
                var documento = await _context.Documentos.FirstOrDefaultAsync(d => d.Id == documentoEditado.Id);

                //adicionando os valores dos atributos do documento do parâmetro naquele achado na tabela
                documento.Titulo = documentoEditado.Titulo;
                documento.Conteudo = documentoEditado.Conteudo;
                documento.DataAlteracao = DateTime.Now;

                //atualiza a tabela com esse documento com as novas informações
                _context.Update(documento);
                await _context.SaveChangesAsync();//salva as informações

                return RedirectToAction("Index");//vai para a página index
            }
            else
            {   
                //se tem algum problema volta essa mesma página com as informações do documento inicial
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
