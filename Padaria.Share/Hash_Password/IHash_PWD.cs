using System;

namespace Padaria.Share.Hash_Password;

public interface IHash_PWD
{
    string HashSenha(string senha);
}
