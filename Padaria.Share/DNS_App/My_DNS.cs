using System;

namespace Padaria.Share.DNS_App;

public static class My_DNS
{
    public static string App_DNS {get; set;}
    static My_DNS()
    {
        App_DNS = "http://192.168.1.170:5091/";    
    }
}
