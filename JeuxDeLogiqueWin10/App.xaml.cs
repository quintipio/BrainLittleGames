using System;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Store;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using JeuxDeLogiqueWin10.Business;
using JeuxDeLogiqueWin10.Context;
using JeuxDeLogiqueWin10.Views;

namespace JeuxDeLogiqueWin10
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App
    {

        /// <summary>
        /// Indique si le jeu est en mode Trial ou non
        /// </summary>
        public static bool IsFull { get; set; }

        /// <summary>
        /// pour la lecture de la license
        /// </summary>
        private static LicenseInformation _licenseInfo;

        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            Microsoft.ApplicationInsights.WindowsAppInitializer.InitializeAsync(
                Microsoft.ApplicationInsights.WindowsCollectors.Metadata |
                Microsoft.ApplicationInsights.WindowsCollectors.Session);
            InitializeComponent();
            Suspending += OnSuspending;

            _licenseInfo = CurrentApp.LicenseInformation;
            _licenseInfo.LicenseChanged += LicenseInfoOnLicenseChanged;
        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        protected async override void OnLaunched(LaunchActivatedEventArgs e)
        {

#if DEBUG
            if (System.Diagnostics.Debugger.IsAttached)
            {
                DebugSettings.EnableFrameRateCounter = true;
            }
#endif

            //initialisation des données de l'appli
            ContextAppli.Init();
            await CheckLicense();

            var rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null)
            {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame();

                rootFrame.NavigationFailed += OnNavigationFailed;
                rootFrame.Navigated += OnNavigated;
                
                // Place the frame in the current Window
                Window.Current.Content = rootFrame;

                // Register a handler for BackRequested events and set the
                // visibility of the Back button
                SystemNavigationManager.GetForCurrentView().BackRequested += OnBackRequested;

                SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                    rootFrame.CanGoBack ?
                    AppViewBackButtonVisibility.Visible :
                    AppViewBackButtonVisibility.Collapsed;
            }

            if (rootFrame.Content == null)
            {
                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                rootFrame.Navigate(typeof(AcceuilView), e.Arguments);
            }
            // Ensure the current window is active
            Window.Current.Activate();
        }

        /// <summary>
        /// Invoked when Navigation to a certain page fails
        /// </summary>
        /// <param name="sender">The Frame which failed navigation</param>
        /// <param name="e">Details about the navigation failure</param>
        void OnNavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            throw new Exception("Failed to load Page " + e.SourcePageType.FullName);
        }

        /// <summary>
        /// Invoked when application execution is being suspended.  Application state is saved
        /// without knowing whether the application will be terminated or resumed with the contents
        /// of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            deferral.Complete();
        }

        private void OnNavigated(object sender, NavigationEventArgs e)
        {
            // Each time a navigation event occurs, update the Back button's visibility
            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                ((Frame)sender).CanGoBack ?
                AppViewBackButtonVisibility.Visible :
                AppViewBackButtonVisibility.Collapsed;
        }

        private void OnBackRequested(object sender, BackRequestedEventArgs e)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            if (rootFrame.CanGoBack)
            {
                e.Handled = true;
                rootFrame.GoBack();
            }
        }


        /// <summary>
        /// Vérifie la license en cours sur l'application et passe la valeur en base à true si elle est achetée
        /// </summary>
        public async Task CheckLicense()
        {
            if (_licenseInfo == null)
            {
                _licenseInfo = CurrentApp.LicenseInformation;
                _licenseInfo.LicenseChanged += LicenseInfoOnLicenseChanged;
            }
            var applicationBusiness = new ApplicationBusiness();
            await applicationBusiness.Initialization;

            var licenseTrial = _licenseInfo.IsTrial;
            var licenseBase = false;
            if (licenseTrial)
            {
                licenseBase = await applicationBusiness.GetFullMode();
            }

            //si je vois qu'en base la valeur est false alors que la license est activée, je corrige
            if (!licenseBase && !licenseTrial)
            {
                await applicationBusiness.ActiverFullMode();
            }
            IsFull = !licenseTrial || licenseBase;

            #if DEBUG
            //IsFull = true;
            #endif
        }

        /// <summary>
        /// 2vènement pour le changement de la license
        /// </summary>
        private async void LicenseInfoOnLicenseChanged()
        {
            await CheckLicense();
        }
    }
}
