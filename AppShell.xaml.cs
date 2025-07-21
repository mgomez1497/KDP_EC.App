namespace KDP_EC.App
{
    public partial class AppShell : Shell
    {

        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute("index", typeof(KDP_EC.App.Views.IndexPage));
        }
    }
}
