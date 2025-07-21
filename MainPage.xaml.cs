
using KDP_EC.App.Infraestructure.Repository.Sincronizar;
using KDP_EC.App.Infraestructure.Repository.SQLite;
using KDP_EC.App.Views;

namespace KDP_EC.App
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        private readonly UserLoginRepository _userLoginRepository;
        private readonly CompaniesRepository _companiesRepository;
        private readonly CountriesRepository _countriesRepository;
        private readonly RolRepository _rolesRepository;
        private readonly PersonRepository _personRepository;
        private readonly FarmsRepository _farmsRepository;
        private readonly FarmsRepositoryAPI _farmsRepositoryAPI;
        private readonly Lots_TypeRepository _lots_TypeRepository;
        private readonly Lots_VarietysRepository _lots_VarietysRepository;
        private readonly LotsRepository _lotsRepository;
        private readonly LotsRepositoryAPI _lotsRepositoryAPI;
        private readonly Lots_TypeRepositoryAPI _lots_TypeRepositoryAPI;
        private readonly Lots_VarietysRepositoryAPI _lots_VarietysRepositoryAPI;
        private readonly Renewal_TypesRepository _renewal_TypesRepository;
        private readonly Renewal_TypesRepositoryAPI _renewal_TypesRepositoryAPI;

        private readonly ActivityTypeRepository _activityTypeRepository;
        private readonly ActivityTypeRepositoryAPI _activityTypeRepositoryAPI;

        private readonly StatesRepository _statesRepository;
        private readonly StatesRepositoryAPI _statesRepositoryAPI;
        private readonly CitiesRepository _citiesRepository;
        private readonly CitiesRepositoryAPI _citiesRepositoryAPI;
        private readonly VillagesRepository _villagesRepository;
        private readonly VillagesRepositoryAPI _villagesRepositoryAPI;


        private readonly ActivitiesRepositoryAPI _activitiesRepositoryAPI;
        private readonly ActivitiesRepository _activitiesRepository;
        private readonly CostCenterRepository _costCenterRepository;
        private readonly CostCenterRepositoryAPI _costCenterRepositoryAPI;
        private readonly StageOfCultRepository _stageOfCultRepository;
        private readonly StageOfCultRepositoryAPI _stageOfCultRepositoryAPI;
        private readonly ExpensesRepository _expensesRepository;
        private readonly ExpensesRepositoryAPI _expensesRepositoryAPI;
        private readonly IncomesTypesRepository _incomesTypesRepository;
        private readonly IncomesTypesRepositoryAPI _incomesTypesRepositoryAPI;
        private readonly IncomesRepository _incomesRepository;
        private readonly IncomesRepositoryAPI _incomesRepositoryAPI;
        private readonly FloweringRecordsRepository _floweringRecordsRepository;
        private readonly FloweringRecordsRepositoryAPI _floweringRecordsRepositoryAPI;
        private readonly CoffeeSalesRepository _coffeeSalesRepository;
        private readonly CoffeeSalesRepositoryAPI _coffeeSalesRepositoryAPI;
        private readonly ProductivityReportRepository _productivityReportRepository;
        private readonly ProductivityReportRepositoryAPI _productivityReportRepositoryAPI;
        private readonly BalanceCostCentersRepository _balanceCostCentersRepository;
        private readonly BalanceCostCentersRepositoryAPI _balanceCostCentersRepositoryAPI;







        public MainPage(UserLoginRepository userLoginRepository, 
            CompaniesRepository companiesRepository, 
            CountriesRepository countriesRepository, 
            RolRepository rolRepository,
            PersonRepository personRepository,
            FarmsRepository farmsRepository,
            FarmsRepositoryAPI farmsRepositoryAPI,
            Lots_TypeRepository lots_TypeRepository,
            Lots_VarietysRepository lots_VarietysRepository,
            Renewal_TypesRepository renewal_TypesRepository,
            Renewal_TypesRepositoryAPI renewal_TypesRepositoryAPI,
            LotsRepository lotsRepository,
            LotsRepositoryAPI lotsRepositoryAPI,
            Lots_TypeRepositoryAPI lots_TypeRepositoryAPI,
            Lots_VarietysRepositoryAPI lots_VarietysRepositoryAPI,
            FloweringRecordsRepository floweringRecordsRepository,
            FloweringRecordsRepositoryAPI floweringRecordsRepositoryAPI,
            StatesRepository statesRepository,
            StatesRepositoryAPI statesRepositoryAPI,
            CitiesRepository citiesRepository,
            CitiesRepositoryAPI citiesRepositoryAPI,
            VillagesRepository villagesRepository,
            VillagesRepositoryAPI villagesRepositoryAPI,
            CoffeeSalesRepository coffeeSalesRepository,
            CoffeeSalesRepositoryAPI coffeeSalesRepositoryAPI,
            ActivityTypeRepository activityTypeRepository,
            ActivityTypeRepositoryAPI activityTypeRepositoryAPI,
            ActivitiesRepository activitiesRepository,
            ActivitiesRepositoryAPI activitiesRepositoryAPI,
            CostCenterRepository costCenterRepository,
            CostCenterRepositoryAPI costCenterRepositoryAPI,
            StageOfCultRepository stageOfCultRepository,
            StageOfCultRepositoryAPI stageOfCultRepositoryAPI,
            ExpensesRepository expensesRepository,
            ExpensesRepositoryAPI expensesRepositoryAPI,
            IncomesTypesRepository incomesTypesRepository,
            IncomesTypesRepositoryAPI incomesTypesRepositoryAPI,
            IncomesRepository incomesRepository,
            IncomesRepositoryAPI incomesRepositoryAPI,
            ProductivityReportRepository productivityReportRepository,
            ProductivityReportRepositoryAPI productivityReportRepositoryAPI,
            BalanceCostCentersRepository balanceCostCentersRepository,
            BalanceCostCentersRepositoryAPI balanceCostCentersRepositoryAPI

            )
        {
            _userLoginRepository = userLoginRepository;
            _companiesRepository = companiesRepository;
            _countriesRepository = countriesRepository;
            _rolesRepository = rolRepository;
            _personRepository = personRepository;
            _farmsRepository = farmsRepository;
            _farmsRepositoryAPI = farmsRepositoryAPI;
            _lots_TypeRepository = lots_TypeRepository;
            _lots_VarietysRepository = lots_VarietysRepository;
            _renewal_TypesRepository = renewal_TypesRepository;
            _renewal_TypesRepositoryAPI = renewal_TypesRepositoryAPI;
            _lotsRepository = lotsRepository;
            _lotsRepositoryAPI = lotsRepositoryAPI;
            _lots_TypeRepositoryAPI = lots_TypeRepositoryAPI;
            _lots_VarietysRepositoryAPI = lots_VarietysRepositoryAPI;
            _activityTypeRepositoryAPI = activityTypeRepositoryAPI;
            _activityTypeRepository = activityTypeRepository;
            _activitiesRepositoryAPI = activitiesRepositoryAPI;
            _activitiesRepository = activitiesRepository;
            _costCenterRepository = costCenterRepository;
            _costCenterRepositoryAPI = costCenterRepositoryAPI;
            _stageOfCultRepository = stageOfCultRepository;
            _stageOfCultRepositoryAPI = stageOfCultRepositoryAPI;
            _expensesRepository = expensesRepository;
            _expensesRepositoryAPI = expensesRepositoryAPI;
            _incomesTypesRepository = incomesTypesRepository;
            _incomesTypesRepositoryAPI = incomesTypesRepositoryAPI;
            _incomesRepository = incomesRepository;
            _incomesRepositoryAPI = incomesRepositoryAPI;
            _floweringRecordsRepository = floweringRecordsRepository;
            _floweringRecordsRepositoryAPI = floweringRecordsRepositoryAPI;
            _statesRepository = statesRepository;
            _statesRepositoryAPI = statesRepositoryAPI;
            _citiesRepository = citiesRepository;
            _citiesRepositoryAPI = citiesRepositoryAPI;
            _villagesRepository = villagesRepository;
            _villagesRepositoryAPI = villagesRepositoryAPI;
            _coffeeSalesRepository = coffeeSalesRepository;
            _coffeeSalesRepositoryAPI = coffeeSalesRepositoryAPI;
            _productivityReportRepository = productivityReportRepository;
            _productivityReportRepositoryAPI = productivityReportRepositoryAPI;
            _balanceCostCentersRepository = balanceCostCentersRepository;
            _balanceCostCentersRepositoryAPI = balanceCostCentersRepositoryAPI;


            InitializeComponent();

        }


        private async void OnLoginClicked(object sender, EventArgs e)
        {
            string username = UsernameEntry.Text;
            string password = PasswordEntry.Text;

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                LoginResultLabel.Text = "Por favor ingresa usuario y contraseña.";
                return;
            }

            var loginResponse = await _userLoginRepository.LoginUsuario(username, password);
           

            if (loginResponse != null)
            {
                await DisplayAlert("Éxito", $"Token: {loginResponse.Token}", "OK");

                
                await Shell.Current.GoToAsync("index");

                var userinfo = await _userLoginRepository.GetUserInfoAsync(loginResponse.Token, loginResponse.UserId.ToString());
                // Llenado de tablas Locales

                var companiesResponse = await _companiesRepository.GetCompanies();
                await _companiesRepository.SaveCompaniesLocally(companiesResponse);

                var countriesResponse = await _countriesRepository.GetCountries();
                await _countriesRepository.SaveCoutriesLocally(countriesResponse);

                var RolesResponse = await _rolesRepository.GetRoles();
                await _rolesRepository.SaveRolesLocally(RolesResponse);

                var PersonResponse = await _personRepository.GetPersons();
                await _personRepository.SavePersonsLocally(PersonResponse);

                var lotsTypeResponse = await _lots_TypeRepositoryAPI.ObtenerTiposLotes();
                await _lots_TypeRepository.SaveLots_TypesLocally(lotsTypeResponse);

                var lotsVarietysResponse = await _lots_VarietysRepositoryAPI.ObtenerVariedadesLotes();
                await _lots_VarietysRepository.SaveLots_VarietysLocally(lotsVarietysResponse);

                var renewalTypesResponse = await _renewal_TypesRepositoryAPI.ObtenerTiposRenovacion();
                await _renewal_TypesRepository.SaveRenewal_TypesLocally(renewalTypesResponse);

                var ActivityTypeResponses = await _activityTypeRepositoryAPI.ObtenerTiposActividad();
                await _activityTypeRepository.SaveActivityTypesLocally(ActivityTypeResponses);

                var ActivitiesResponse = await _activitiesRepositoryAPI.ObtenerActividades();
                await _activitiesRepository.SaveActivitiesLocally(ActivitiesResponse);

                var CostCenterResponse = await _costCenterRepositoryAPI.ObtenerCentrosDeCosto();
                await _costCenterRepository.SaveCostCentersLocally(CostCenterResponse);

                var incomesTypesResponse = await _incomesTypesRepositoryAPI.ObtenerTiposIngreso();
                await _incomesTypesRepository.SaveIncomesTypesLocally(incomesTypesResponse);

                var StageOfCultResponse = await _stageOfCultRepositoryAPI.ObtenerEtapasDeCultivo();
                await _stageOfCultRepository.SaveStageOfCultsLocally(StageOfCultResponse);

                var StatesResponse = await _statesRepositoryAPI.ObtenerEstados();
                await _statesRepository.SaveStatesLocally(StatesResponse);

                var CitiesResponse = await _citiesRepositoryAPI.ObtenerCiudades();
                await _citiesRepository.SaveCitiesLocally(CitiesResponse);


                var user = userinfo?.FirstOrDefault();

                Preferences.Set("Identification", user?.Identification);
                Preferences.Set("Rol", user?.Id_Rol.ToString());

                var FarmResponses = await _farmsRepositoryAPI.ObtenerFincasProductor(user?.Identification);
                await _farmsRepository.SaveFarmsLocally(FarmResponses);

                var FloweringResponses = await _floweringRecordsRepositoryAPI.ObtenerFloracionesporUserId(user.Id);
                await _floweringRecordsRepository.SaveFloweringRecordsLocally(FloweringResponses);

                int cantidadFincas = FarmResponses?.Count ?? 0;

                await _lotsRepository.DeleteAllLots();

                foreach (var farm in FarmResponses)
                {
                    
                   var LotesResponse = await _lotsRepositoryAPI.ObtenerLotesporFarmId (farm.Id);
                   await _lotsRepository.SaveLotsLocally(LotesResponse);

                    var expenses = await _expensesRepositoryAPI.ObtenerGastosporFincaId(farm.Id);
                    await _expensesRepository.SaveExpensesLocally(expenses);

                    var incomes = await _incomesRepositoryAPI.ObtenerIngresosporFincaId(farm.Id);
                    await _incomesRepository.SaveIncomesLocally(incomes);

                    var CoffeeSalesResponse = await _coffeeSalesRepositoryAPI.ObtenerVentasCafe(farm.Id);
                    await _coffeeSalesRepository.SaveCoffeeSalesLocally(CoffeeSalesResponse);

                    var ProductivityReportResponse = await _productivityReportRepositoryAPI.ObtenerProductivityReports(farm.Id);
                    await _productivityReportRepository.SaveProductivityReportsLocally(ProductivityReportResponse);

                    var BasicCostCenters = await _balanceCostCentersRepositoryAPI.ObtenerBalanceCC(farm.Id);
                    await _balanceCostCentersRepository.SaveBalanceCostCentersLocally(BasicCostCenters);
                }
            }
            else
            {
                await DisplayAlert("Error", "Credenciales incorrectas", "OK");
            }
        }
    }

}
