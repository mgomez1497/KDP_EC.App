using KDP_EC.App.Infraestructure.Repository.Sincronizar;
using KDP_EC.App.Infraestructure.Repository.SQLite;
using Microcharts.Maui;
using Microsoft.Extensions.Logging;

namespace KDP_EC.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("fa-solid-900.ttf", "FASolid");
                });

#if DEBUG
    		builder.Logging.AddDebug();

#endif

            builder.UseMicrocharts();


            //Inyeccion de dependencias
            builder.Services.AddSingleton<UserLoginRepository>();
            builder.Services.AddSingleton<CompaniesRepository>();
            builder.Services.AddSingleton<CountriesRepository>();
            builder.Services.AddSingleton<RolRepository>();
            builder.Services.AddSingleton<PersonRepository>();
            builder.Services.AddSingleton<FarmsRepository>();
            builder.Services.AddSingleton<Lots_TypeRepository>();
            builder.Services.AddSingleton<Lots_VarietysRepository>();
            builder.Services.AddSingleton<Renewal_TypesRepository>();
            builder.Services.AddSingleton<Renewal_TypesRepositoryAPI>();
            builder.Services.AddSingleton<LotsRepository>();
            builder.Services.AddSingleton<LotsRepositoryAPI>();
            builder.Services.AddSingleton<ActivityTypeRepository>();
            builder.Services.AddSingleton<ActivityTypeRepositoryAPI>();
            builder.Services.AddSingleton<ActivitiesRepository>();
            builder.Services.AddSingleton<ActivitiesRepositoryAPI>();
            builder.Services.AddSingleton<CostCenterRepository>();
            builder.Services.AddSingleton<CostCenterRepositoryAPI>();
            builder.Services.AddSingleton<StageOfCultRepository>();
            builder.Services.AddSingleton<StageOfCultRepositoryAPI>();
            builder.Services.AddSingleton<ExpensesRepository>();
            builder.Services.AddSingleton<ExpensesRepositoryAPI>();
            builder.Services.AddSingleton<IncomesRepository>();
            builder.Services.AddSingleton<IncomesRepositoryAPI>();
            builder.Services.AddSingleton<IncomesTypesRepository>();
            builder.Services.AddSingleton<IncomesTypesRepositoryAPI>();
            builder.Services.AddSingleton<FloweringRecordsRepository>();
            builder.Services.AddSingleton<FloweringRecordsRepositoryAPI>();
            builder.Services.AddSingleton<StatesRepository>();
            builder.Services.AddSingleton<StatesRepositoryAPI>();
            builder.Services.AddSingleton<CitiesRepository>();
            builder.Services.AddSingleton<CitiesRepositoryAPI>();
            builder.Services.AddSingleton<VillagesRepository>();
            builder.Services.AddSingleton<VillagesRepositoryAPI>();
            builder.Services.AddSingleton<CoffeeSalesRepository>();
            builder.Services.AddSingleton<CoffeeSalesRepositoryAPI>();
            builder.Services.AddSingleton<FarmsRepositoryAPI>();
            builder.Services.AddSingleton<Lots_TypeRepositoryAPI>();
            builder.Services.AddSingleton<Lots_VarietysRepositoryAPI>();
            builder.Services.AddSingleton<ProductivityReportRepositoryAPI>();
            builder.Services.AddSingleton<ProductivityReportRepository>();


            return builder.Build();
        }
    }
}
