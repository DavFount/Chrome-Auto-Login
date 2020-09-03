# Chrome Auto Logon with C# and Selenium
This application was created to solve allow for kiosk accounts to be automatically logged into for a company. This is a generic shell for automatic logins using a username and password.

## To configure custome logins
Once the application is compiled you can easily create custom logins for various sites using one compiled application.

* Create a file filename.config
	* Add your appSettings
	```xml
		<appSettings>
		  <add key="username" value=""/>
		  <add key="password" value=""/>
		  <add key ="url" value=""/>
		  <add key="usernameField" value=""/>
		  <add key="passwordField" value=""/>
		  <add key="submitField" value=""/>
		</appSettings>
	```
	* Username is the login username
	* Password is the login password
	* Url is the path to the login form
	* usernameField is the HTML ID of the username field
	* passwordField is the HTML ID of the password field
	* submitField is the button to submit the login form.
* Create a Shortcut of ChromeAutoLogon.exe
	* Right-Click Select Properties
	* On the Target line add "path\to\config.config" to the end of the line. It is good practice to wrap the path in quotes
* Click the shortcut and the config file specified will be used to perform the autologin.

You can create as many of these configs as you'd like. Remember all passwords stored in this file are plain text and can be read if the enduser knows where to look for them.

Note: Paths to the config are relative unless a fully qualified path is provided. (C:\somedir\anotherdir\file.config)
