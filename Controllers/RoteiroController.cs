using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Polimedica.Data;
using Polimedica.Models;
using Polimedica.ViewModel;
using PagedList;
using Polimedica.Data.Enum;
using Microsoft.AspNetCore.Authorization;

namespace Polimedica.Controllers
{
    public class RoteiroController : Controller
    {
        private readonly PolimedicaDbContetxt _context;

        public RoteiroController(PolimedicaDbContetxt context)
        {
            _context = context;
        }

        public IActionResult Index(int? page,DateTime? filtro)
        {
            IQueryable<Roteiro> roteiros = _context.Roteiros;
            //Aqui verica se o filtro está vazio - Ternário
            DateTime? tempo = filtro.HasValue ? filtro : DateTime.Today;
            ViewBag.temp = tempo;
            //Aqui conta quantas entregas tem no dia para colocar na paginação - Ternário
            int quant = roteiros.Count(d => d.Data.Equals(tempo)) == 0 ?
                1 :
                roteiros.Count(d => d.Data.Equals(tempo));
            

            //Aqui monta o IPagedList - Ternário
            IPagedList<Roteiro> model = DateTime.Today.Equals(tempo) ?
                roteiros.Where(d => d.Data.Equals(DateTime.Today)).ToPagedList(page ?? 1, quant) :
                roteiros.Where(d => d.Data.Equals(tempo)).ToPagedList(page ?? 1, quant);

            return View(model);
        }
        [Authorize]
        public IActionResult Criar()
        {
            var response = new CriarVM();
            return View(response);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Criar(CriarVM criarVM)
        {
            if (!ModelState.IsValid)
            {
                return View(criarVM);
            }

            string cookie = _context.GetType().Name;

            var roteiro = new Roteiro
            {
                Checado = "false",
                Data = criarVM.Data < DateTime.Today ?
                    DateTime.Today :
                    criarVM.Data,
                Cliente = criarVM.Cliente,
                Cidade = criarVM.Cidade,
                Observacao = criarVM.Observacao,
                Cartao = criarVM.Cartao,
                LojaPolimedica = criarVM.LojaPolimedica,
                PedidoNF = criarVM.PedidoNF,
                DinheiroCheque = criarVM.DinheiroCheque,
                Troco = criarVM.Troco,
                Periodo = criarVM.Periodo,
                Criou = cookie
            };

            _context.Roteiros.Add(roteiro);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index", "Roteiro");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id)
        {
            var roteiro = await _context.Roteiros.FirstOrDefaultAsync(i => i.Id == id);
            if (roteiro == null) return View("Error");

            string cookie = _context.GetType().Name;

            var editVM = new EditRoteiroVM
            {
                Data = roteiro.Data,
                Cliente = roteiro.Cliente,
                Cidade = roteiro.Cidade,
                Observacao = roteiro.Observacao,
                Cartao = roteiro.Cartao,
                LojaPolimedica = roteiro.LojaPolimedica,
                PedidoNF = roteiro.PedidoNF,
                DinheiroCheque = roteiro.DinheiroCheque,
                Troco = roteiro.Troco,
                Periodo = roteiro.Periodo,
                Editou = cookie
            };
            return View(editVM);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditRoteiroVM editRVM)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("", "Fail to edit");
                return View("Edit",editRVM);
            }

            //procurar no gpt o significado do asnotracking
            var rotaRoteiro = await _context.Roteiros.AsNoTracking().FirstOrDefaultAsync(i => i.Id == id);

            if(rotaRoteiro != null)
            {
                var trocaRoteiro = new Roteiro
                {
                    Id = editRVM.Id,
                    Data = editRVM.Data,
                    Cliente = editRVM.Cliente,
                    Cidade = editRVM.Cidade,
                    Observacao = editRVM.Observacao,
                    Cartao = editRVM.Cartao,
                    LojaPolimedica = editRVM.LojaPolimedica,
                    PedidoNF = editRVM.PedidoNF,
                    DinheiroCheque = editRVM.DinheiroCheque,
                    Troco = editRVM.Troco,
                    Periodo = editRVM.Periodo
                };
                _context.Roteiros.Update(trocaRoteiro);
                _context.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View(editRVM);
            }
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            if(id == null)
            {
                return View("Error");
            }

            var rota = await _context.Roteiros.FindAsync(id);
            if(rota != null)
            {
                _context.Roteiros.Remove(rota);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
