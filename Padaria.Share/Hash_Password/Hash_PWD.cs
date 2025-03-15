using System;
using System.Security.Cryptography;
using System.Text;

namespace Padaria.Share.Hash_Password;

public class Hash_PWD : IHash_PWD
{
    public string HashSenha(string senha)
    {
        var encodedValue = Encoding.UTF8.GetBytes(senha);
        var encryptedPassword = SHA512.HashData(encodedValue);

        var sb = new StringBuilder();
        foreach (var caracter in encryptedPassword)
        {
            sb.Append(caracter.ToString("X2"));
        }
        return sb.ToString();
    }
}
