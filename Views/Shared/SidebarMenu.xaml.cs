using KDP_EC.App.Infraestructure.Repository.Sincronizar;
using KDP_EC.App.Infraestructure.Repository.SQLite;
using KDP_EC.App.Views.Farms;
using KDP_EC.App.Views.FloweringRecords;
using KDP_EC.App.Views.InEx;
using KDP_EC.App.Views.Lots;
using KDP_EC.App.Views.Reports;

namespace KDP_EC.App.Views.Shared;

public partial class SidebarMenu : ContentView
{
    private bool isSidebarVisible = true;

    private readonly UserLoginRepository _userLoginRepository;

    

    public SidebarMenu()
    {
        InitializeComponent();

        var rol = Preferences.Get("Rol", "");

        //if (rol != "5fc8995f-191a-4956-ba16-50606261bc96")
        //{
        //    UserManagementGrid.IsVisible = false;
        //}

        if (rol != "48818893-94f0-4a52-afcc-2756480712bd")
        {
            FarmsManagementGrid.IsVisible = false;
            LotsManagementGrid.IsVisible = false;
        }

        _userLoginRepository = new UserLoginRepository();
    }



    private async void OnMenuTapped(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new IndexPage());
    }

    private void ToggleSidebarClicked(object sender, EventArgs e)
    {
        if (isSidebarVisible)
        {
            SidebarContent.TranslateTo(-250, 0, 250, Easing.CubicInOut);
        }
        else
        {
            SidebarContent.TranslateTo(0, 0, 250, Easing.CubicInOut);
        }

        isSidebarVisible = !isSidebarVisible;
    }

    private async void OnFarmsManagementTapped(object sender, EventArgs e)
    {
        var identification = Preferences.Get("Identification", null);

        if (string.IsNullOrEmpty(identification))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "No se encontró el identificador del usuario", "OK");
            return;
        }

 

        FarmsRepositoryAPI farmsRepositoryAPI = new FarmsRepositoryAPI();
       
        var farms = await farmsRepositoryAPI.ObtenerFincasporId(identification);

        foreach (var farm in farms)
        {
            Console.WriteLine($"✅ Finca: {farm.NombreFinca}");
        }

        await Application.Current.MainPage.Navigation.PushAsync(new IndexFarms());
    }


    private async void OnLotsManagementTapped(object sender, EventArgs e)
    {
        var identification = Preferences.Get("Identification", null);
        if (string.IsNullOrEmpty(identification))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "No se encontró el identificador del usuario", "OK");
            return;
        }
        FarmsRepositoryAPI farmsRepositoryAPI = new FarmsRepositoryAPI();

        var farms = await farmsRepositoryAPI.ObtenerFincasporId(identification);
        foreach (var farm in farms)
        {
            Console.WriteLine($"✅ Finca: {farm.NombreFinca}");
        }
        await Application.Current.MainPage.Navigation.PushAsync(new IndexLots());
    }

    private async void OnFloweringManagementTapped(object sender, EventArgs e)
    {
        var identification = Preferences.Get("Identification", null);
        if (string.IsNullOrEmpty(identification))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "No se encontró el identificador del usuario", "OK");
            return;
        }
        FarmsRepositoryAPI farmsRepositoryAPI = new FarmsRepositoryAPI();

        var farms = await farmsRepositoryAPI.ObtenerFincasporId(identification);
        foreach (var farm in farms)
        {
            Console.WriteLine($"✅ Finca: {farm.NombreFinca}");
        }
        await Application.Current.MainPage.Navigation.PushAsync(new IndexFloweringRecords());
    }

    private async void OnReportsManagmentTapped(object sender, EventArgs e)
    {
        var identification = Preferences.Get("Identification", null);
        if (string.IsNullOrEmpty(identification))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "No se encontró el identificador del usuario", "OK");
            return;
        }
        FarmsRepositoryAPI farmsRepositoryAPI = new FarmsRepositoryAPI();
        var farms = await farmsRepositoryAPI.ObtenerFincasporId(identification);
        foreach (var farm in farms)
        {
            Console.WriteLine($"✅ Finca: {farm.NombreFinca}");
        }
        await Application.Current.MainPage.Navigation.PushAsync(new IndexReports());
    }

    private async void OnRegisterManagmentTapped(object sender, EventArgs e)
    {
        var identification = Preferences.Get("Identification", null);
        if (string.IsNullOrEmpty(identification))
        {
            await Application.Current.MainPage.DisplayAlert("Error", "No se encontró el identificador del usuario", "OK");
            return;
        }
        FarmsRepositoryAPI farmsRepositoryAPI = new FarmsRepositoryAPI();

        var farms = await farmsRepositoryAPI.ObtenerFincasporId(identification);
        foreach (var farm in farms)
        {
            Console.WriteLine($"✅ Finca: {farm.NombreFinca}");
        }
        await Application.Current.MainPage.Navigation.PushAsync(new InExIndex());
    }
    private async void OnLogoutManagementTapped(object sender, EventArgs e)
    {
       await  _userLoginRepository.LogoutAsync();
    }






}

