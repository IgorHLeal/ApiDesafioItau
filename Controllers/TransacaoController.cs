using ApiDesafioItau.Models;
using ApiDesafioItau.Services;
using Microsoft.AspNetCore.Mvc;

namespace ApiDesafioItau.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransacaoController : ControllerBase
    {
        private readonly TransacaoService _service;

        public TransacaoController(TransacaoService service)
        {
            _service = service;
        }

        [HttpPost]
        public IActionResult Post([FromBody] Transacao transacao)
        {
            if (transacao == null)
                return BadRequest();

            if (!ModelState.IsValid)
                return UnprocessableEntity();

            if (transacao.valor < 0 || transacao.dataHora > DateTime.UtcNow)
                return UnprocessableEntity();

            _service.AdicionarTransacao(transacao);
            return StatusCode(201);
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            _service.ApagarTransacoes();
            return Ok();
        }

        [HttpGet("/estatistica")]
        public IActionResult GetEstatistica()
        {
            var transacoes = _service.OterTransacoesUltimoMinuto();

            var count = transacoes.Count();
            var sum = transacoes.Sum(x => x.valor);
            var avg = count > 0 ? transacoes.Average(x => x.valor) : 0;
            var min = count > 0 ? transacoes.Min(x => x.valor) : 0;
            var max = count > 0 ? transacoes.Max(x => x.valor) : 0;

            return Ok(new
            {
                count,
                sum = Math.Round(sum, 2),
                avg = Math.Round(avg, 3),
                min = Math.Round(min, 2),
                max = Math.Round(max, 2)
            });
        }
    }
}
