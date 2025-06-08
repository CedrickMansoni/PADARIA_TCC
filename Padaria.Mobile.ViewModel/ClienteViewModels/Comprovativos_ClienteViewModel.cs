using System;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Windows.Input;
using Padaria.Share.Comprovativos.DTO;
using Padaria.Share.DNS_App;

namespace Padaria.Mobile.ViewModel.ClienteViewModels;

public class Comprovativos_ClienteViewModel : BindableObject
{
    readonly HttpClient client;
    readonly JsonSerializerOptions options;

    public Comprovativos_ClienteViewModel()
    {
        client = new HttpClient() { BaseAddress = new Uri($"{My_DNS.App_DNS}") };
        options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
    }

    private string img = "upload";
    public string Img
    {
        get => img;
        set
        {
            img = value;
            OnPropertyChanged(nameof(Img));
        }
    }

    private ComprovativoDTO comprovativo = new();
    public ComprovativoDTO Comprovativo
    {
        get => comprovativo;
        set
        {
            comprovativo = value;
            OnPropertyChanged(nameof(Comprovativo));
        }
    }

    private string caminhoComprovativo = string.Empty;
    public string CaminhoComprovativo
    {
        get => caminhoComprovativo;
        set
        {
            caminhoComprovativo = value;
            OnPropertyChanged(nameof(CaminhoComprovativo));
        }
    }

    public ICommand AbrirCameraCommand => new Command(async () =>
   {
       bool response = await Shell.Current.DisplayAlert("...", "Como pretende obter a imagem do produto?", "Camera", "Galeria");
       var doc = response ?
           await MediaPicker.CapturePhotoAsync() :
           await MediaPicker.PickPhotoAsync();

       if (doc is null) return;

       CaminhoComprovativo = doc.FullPath;
       Comprovativo.Comprovativo = doc.FileName;
       if (string.IsNullOrEmpty(CaminhoComprovativo)) { Img = "upload"; }
       if (CaminhoComprovativo.Contains(".pdf")) { Img = "image_pdf"; return; } else { Img = "image_png"; }
   });

    public ICommand EnviarComprovativoCommand => new Command(async () =>
     {
         //ActivityCommand.Execute(null);
         Comprovativo.IdCliente = Convert.ToInt32(await SecureStorage.Default.GetAsync("IdUsuario"));
         if (string.IsNullOrEmpty(Comprovativo.Comprovativo))
         {
             //ActivityCommand.Execute(null);
             await Shell.Current.DisplayAlert("Erro", "Selecione o comprovativo!", "OK");
             return;
         }
         if (string.IsNullOrEmpty(CaminhoComprovativo))
         {
             //ActivityCommand.Execute(null);
             await Shell.Current.DisplayAlert("Erro", "Selecione o comprovativo!", "OK");
             return;
         }
         var telefoneCliente = await SecureStorage.Default.GetAsync("telefoneUsuario");
         var formData = new MultipartFormDataContent
         {
            { new StringContent(Comprovativo.IdCliente.ToString()), "idCliente" },
            { new StringContent(telefoneCliente!.ToString()), "telefone" },
            { new StringContent(Comprovativo.Comprovativo), "comprovativo" }
         };

         AdicionarArquivoAoFormData(formData, CaminhoComprovativo, "comprovativo");

         var response = await client.PostAsync("salvar/comprovativo", formData);
         if (response.IsSuccessStatusCode)
         {
             ActivityCommand.Execute(null);
             Img = "upload";
             CaminhoComprovativo = string.Empty;
             await Shell.Current.DisplayAlert("Sucesso", "Comprovativo enviado com sucesso!", "OK");
         }
         else
         {
             ActivityCommand.Execute(null);
             await Shell.Current.DisplayAlert("Erro", "Erro ao enviar o comprovativo!", "OK");
         }
     });

    // Método para adicionar arquivos ao formData
    private void AdicionarArquivoAoFormData(MultipartFormDataContent formData, string caminho, string nomeCampo)
    {
        if (!string.IsNullOrEmpty(caminho) && File.Exists(caminho))
        {
            var mimeType = ObterTipoMime(caminho);
            var imagemStream = new StreamContent(File.OpenRead(caminho));
            imagemStream.Headers.ContentType = new MediaTypeHeaderValue(mimeType);
            formData.Add(imagemStream, nomeCampo, Path.GetFileName(caminho));
        }
    }

    // Método auxiliar para determinar o tipo MIME com base na extensão do arquivo
    private string ObterTipoMime(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();

        return extension switch
        {
            ".jpg" or ".jpeg" => "image/jpeg",
            ".png" => "image/png",
            ".gif" => "image/gif",
            ".bmp" => "image/bmp",
            ".tiff" => "image/tiff",
            ".pdf" => "application/pdf",
            _ => "application/octet-stream", // Padrão para tipos desconhecidos
        };
    }

    private bool activity = false;
    public bool Activity
    {
        get => activity;
        set
        {
            activity = value;
            OnPropertyChanged(nameof(Activity));
        }
    }

    private bool enablePage = true;
    public bool EnablePage
    {
        get => enablePage;
        set
        {
            enablePage = value;
            OnPropertyChanged(nameof(EnablePage));
        }
    }

    public ICommand ActivityCommand => new Command(() =>
    {
        EnablePage = !EnablePage;
        Activity = !Activity;
    });


}
