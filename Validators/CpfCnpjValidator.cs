using System.Text.RegularExpressions;

namespace CadastroPessoasApi.Validators
{
    public static class CpfCnpjValidator
    {
        public static bool ValidarCPF(string cpf)
        {
            cpf = SomenteNumeros(cpf);

            if (cpf.Length != 11 || TodosDigitosIguais(cpf))
                return false;

            var multiplicador1 = new[] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new[] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCpf = cpf[..9];
            int digito1 = CalcularDigitoVerificador(tempCpf, multiplicador1);

            tempCpf += digito1;
            int digito2 = CalcularDigitoVerificador(tempCpf, multiplicador2);

            return cpf.EndsWith($"{digito1}{digito2}");
        }

        public static bool ValidarCNPJ(string cnpj)
        {
            cnpj = SomenteNumeros(cnpj);

            if (cnpj.Length != 14 || TodosDigitosIguais(cnpj))
                return false;

            var multiplicador1 = new[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };

            string tempCnpj = cnpj[..12];
            int digito1 = CalcularDigitoVerificador(tempCnpj, multiplicador1);

            tempCnpj += digito1;
            int digito2 = CalcularDigitoVerificador(tempCnpj, multiplicador2);

            return cnpj.EndsWith($"{digito1}{digito2}");
        }

        private static int CalcularDigitoVerificador(string input, int[] multiplicadores)
        {
            int soma = 0;
            for (int i = 0; i < multiplicadores.Length; i++)
                soma += int.Parse(input[i].ToString()) * multiplicadores[i];

            int resto = soma % 11;
            return (resto < 2) ? 0 : 11 - resto;
        }

        private static bool TodosDigitosIguais(string valor)
            => valor.Distinct().Count() == 1;

        private static string SomenteNumeros(string valor)
            => Regex.Replace(valor ?? string.Empty, "[^0-9]", "");
    }
}
