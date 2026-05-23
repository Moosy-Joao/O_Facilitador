namespace facilitador_application.Application.Validators.Utils
{
    internal class ValidarSenha
    {
        public static bool Validar(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha))
            { return false; }

            bool temMaiuscula = senha.Any(char.IsUpper);
            bool temMinuscula = senha.Any(char.IsLower);
            bool temNumero = senha.Any(char.IsDigit);
            bool temEspecial = senha.Any(c => !char.IsLetterOrDigit(c));

            return temMaiuscula && temMinuscula && temNumero && temEspecial;
        }
    }
}
