using ApiDesafioItau.Models;

namespace ApiDesafioItau.Services
{
    public class TransacaoService
    {
        private readonly List<Transacao> _transacoes = new();

        public void AdicionarTransacao (Transacao transacao)
        {
            //lock(_transacoes)
            //{
                _transacoes.Add(transacao);
            //}
        }

        public void ApagarTransacoes()
        {
            _transacoes.Clear();
        }

        public List<Transacao> OterTransacoesUltimoMinuto()
        {
            var date = DateTime.UtcNow;

            return _transacoes
                .Where(x => x.dataHora >= date.AddSeconds(-60) && x.dataHora <= date)
                .ToList();
        }
    }
}
