using System;

namespace Padaria.Share.DNS_App;

public static class My_DNS
{
    public static string App_DNS {get; set;}
    static My_DNS()
    {
        App_DNS = "http://172.20.10.2:5091/";    
    }
}
