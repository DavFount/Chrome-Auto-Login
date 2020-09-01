using System;
using System.Configuration;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace ChromeAutoLogon
{
    class Program
    {
        static void Main(string[] args)
        {
            setConfigFileAtRunTime(args);

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

            Environment.Exit(0);
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
    }
}
