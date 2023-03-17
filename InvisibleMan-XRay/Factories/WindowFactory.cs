using System.Windows;

namespace InvisibleManXRay.Factories
{
    using Core;
    using Managers;
    using Handlers;

    public class WindowFactory
    {
        private InvisibleManXRayCore core;
        private HandlersManager handlersManager;

        public void Setup(InvisibleManXRayCore core, HandlersManager handlersManager)
        {
            this.core = core;
            this.handlersManager = handlersManager;
        }

        public MainWindow CreateMainWindow()
        {
            ConfigHandler configHandler = handlersManager.GetHandler<ConfigHandler>();
            UpdateHandler updateHandler = handlersManager.GetHandler<UpdateHandler>();
            LinkHandler linkHandler = handlersManager.GetHandler<LinkHandler>();

            MainWindow mainWindow = new MainWindow();
            mainWindow.Setup(
                getConfig: configHandler.GetCurrentConfig,
                loadConfig: core.LoadConfig,
                checkForUpdate: updateHandler.CheckForUpdate,
                openServerWindow: CreateServerWindow,
                openUpdateWindow: CreateUpdateWindow,
                openAboutWindow: CreateAboutWindow,
                onRunServer: core.Run,
                onStopServer: core.Stop,
                onEnableProxy: core.EnableProxy,
                onDisableProxy: core.DisableProxy,
                onGitHubClick: linkHandler.OpenGitHubRepositoryLink,
                onBugReportingClick: linkHandler.OpenBugReportingLink
            );
            
            return mainWindow;
        }

        public UpdateWindow CreateUpdateWindow()
        {
            UpdateHandler updateHandler = handlersManager.GetHandler<UpdateHandler>();
            LinkHandler linkHandler = handlersManager.GetHandler<LinkHandler>();

            UpdateWindow updateWindow = new UpdateWindow();
            updateWindow.Setup(
                checkForUpdate: updateHandler.CheckForUpdate,
                onUpdateClick: linkHandler.OpenLatestReleaseLink
            );

            return updateWindow;
        }

        public AboutWindow CreateAboutWindow()
        {
            LinkHandler linkHandler = handlersManager.GetHandler<LinkHandler>();

            AboutWindow aboutWindow = new AboutWindow();
            aboutWindow.Setup(
                onEmailClick: linkHandler.OpenEmailLink,
                onWebsiteClick: linkHandler.OpenWebsiteLink,
                onBugReportingClick: linkHandler.OpenBugReportingLink
            );

            return aboutWindow;
        }

        private ServerWindow CreateServerWindow()
        {
            ConfigHandler configHandler = handlersManager.GetHandler<ConfigHandler>();
            TemplateHandler templateHandler = handlersManager.GetHandler<TemplateHandler>();
            SettingsHandler settingsHandler = handlersManager.GetHandler<SettingsHandler>();
            MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
            
            ServerWindow serverWindow = new ServerWindow();
            serverWindow.Setup(
                getCurrentConfigIndex: settingsHandler.UserSettings.GetCurrentConfigIndex,
                getAllConfigs: configHandler.GetAllConfigs,
                convertConfigLinkToV2Ray: templateHandler.ConverLinkToV2Ray,
                loadConfig: core.LoadConfig,
                testConnection: core.Test,
                onCopyConfig: configHandler.CopyConfig,
                onCreateConfig: configHandler.CreateConfig,
                onDeleteConfig: configHandler.LoadConfigFiles,
                onUpdateConfig: UpdateConfig
            );
            
            return serverWindow;

            void UpdateConfig(int index)
            {
                settingsHandler.UpdateCurrentConfigIndex(index);
                mainWindow.UpdateUI();
                mainWindow.TryReconnect();
            }
        }
    }
}