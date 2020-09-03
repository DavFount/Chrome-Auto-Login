using System;
using System.Configuration;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.DirectoryServices.AccountManagement;

namespace ChromeAutoLogon
{
    class Program
    {
        static void Main(string[] args)
        {
            setConfigFileAtRunTime(args);

            bool restrictAccess;
            bool.TryParse(ConfigurationManager.AppSettings.Get("restrictAccess"), out restrictAccess);

            if(restrictAccess && !isInGroup(ConfigurationManager.AppSettings.Get("securityGroup")))
            {
                Console.WriteLine("Insufficient Permissions. Please contact your IT Department.");
            }
            else
            {
                beginAutoLogin();
            }
        }

        protected static void beginAutoLogin()
        {
            ChromeDriverService service = ChromeDriverService.CreateDefaultService();
            service.HideCommandPromptWindow = true;
            service.SuppressInitialDiagnosticInformation = true;

            ChromeOptions options = new ChromeOptions();
            options.AddArgument("start-maximized");
            options.AddExcludedArguments("enable-automation");
            options.AddAdditionalCapability("useAutomationExtension", false);


            ChromeDriver driver = new ChromeDriver(service, options);

            string url = ConfigurationManager.AppSettings.Get("url");
            driver.Navigate().GoToUrl(url);

            string usernameFieldName = ConfigurationManager.AppSettings.Get("usernameField");
            string passwordFieldName = ConfigurationManager.AppSettings.Get("passwordField");
            string submitButtonName = ConfigurationManager.AppSettings.Get("submitField");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(30));
            wait.Until(drv => driver.FindElement(By.Id(usernameFieldName)));

            IWebElement usernameField = driver.FindElement(By.Id(usernameFieldName));
            IWebElement passwordField = driver.FindElement(By.Id(passwordFieldName));
            IWebElement submitButton = driver.FindElement(By.Id(submitButtonName));

            string username = ConfigurationManager.AppSettings.Get("username");
            string password = ConfigurationManager.AppSettings.Get("password");

            usernameField.SendKeys(username);
            passwordField.SendKeys(password);
            submitButton.Click();

            foreach (Process process in Process.GetProcessesByName("chromedriver"))
            {
                process.Kill();
            }

            //Environment.Exit(0);
        }
        protected static void setConfigFileAtRunTime(string[] args)
        {
            string runtimeConfigFile;

            if (args.Length == 0)
            {
                Console.WriteLine("Please specify a config file: ");
                Console.Write("> ");
                runtimeConfigFile = Console.ReadLine();
            }
            else
            {
                runtimeConfigFile = args[0];
            }

            Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            config.AppSettings.File = runtimeConfigFile;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        private static bool isInGroup(string groups)
        {
            string[] groupList = groups.Split(';');
            string username = Environment.UserName;
            string domainName = ConfigurationManager.AppSettings.Get("domainName");
            string searchBase = ConfigurationManager.AppSettings.Get("searchBase");
            PrincipalContext domainctx = new PrincipalContext(ContextType.Domain, domainName, searchBase);

            UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(domainctx, IdentityType.SamAccountName, username);

            foreach(string group in groupList)
            {
                if (userPrincipal.IsMemberOf(domainctx, IdentityType.Name, group))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
