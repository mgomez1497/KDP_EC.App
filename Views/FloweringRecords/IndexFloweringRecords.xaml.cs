using KDP_EC.App.Infraestructure.Repository.Sincronizar;
using KDP_EC.App.Infraestructure.Repository.SQLite;
using FarmModel = KDP_EC.Core.Models.Farms;
namespace KDP_EC.App.Views.FloweringRecords;

public partial class IndexFloweringRecords : ContentPage
{
	private readonly FloweringRecordsRepository _floweringRepository;
    private readonly FloweringRecordsRepositoryAPI _floweringRecordsRepositoryAPI;

    
    public IndexFloweringRecords()
	{
		InitializeComponent();
		_floweringRepository = new FloweringRecordsRepository();
        _floweringRecordsRepositoryAPI = new FloweringRecordsRepositoryAPI();
        LoadLocalFlowerings();

    }

    private async void LoadLocalFlowerings()
    {
        try
        {
            var flowerings = await _floweringRepository.GetLocalFloweringRecords();

            if (flowerings != null && flowerings.Any())
            {
                BindingContext = new { FloweringList = flowerings };
            }
            else
            {
                await DisplayAlert("Aviso", "No hay registros de floración disponibles.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"No se pudieron cargar las floraciones: {ex.Message}", "OK");
        }
    }

    private async void OnCreateFloweringClicked(object sender, EventArgs e)
    {
        await DisplayAlert("Info", "¡Clic detectado!", "OK");
        await Navigation.PushAsync(new CreateFloweringRecords());
    }

    private async void OnCreateFlowClicked(object sender, EventArgs e)
    {
        
        await Navigation.PushAsync(new CreateFloweringRecords());
    }

    private async void OnSincronizarClicked(object sender, EventArgs e)
    {
        try
        {
            var registros = await _floweringRepository.GetLocalFloweringRecords();

            if (registros == null || !registros.Any())
            {
                await DisplayAlert("Aviso", "No hay floraciones locales para sincronizar.", "OK");
                return;
            }

            Console.WriteLine($"?? Intentando sincronizar {registros.Count} floraciones...");

            var response = await _floweringRecordsRepositoryAPI.SincronizarConRespuesta(registros);

            if (response.IsSuccessStatusCode)
            {
                await DisplayAlert("Éxito", "Floraciones sincronizadas correctamente con el servidor.", "OK");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"? Error al sincronizar: {response.StatusCode} - {errorContent}");
                await DisplayAlert("Error", $"Ocurrió un error al sincronizar las floraciones.\nCódigo: {response.StatusCode}", "OK");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"? Excepción: {ex.Message}");
            await DisplayAlert("Error", $"Fallo al sincronizar: {ex.Message}", "OK");
        }
    }

}