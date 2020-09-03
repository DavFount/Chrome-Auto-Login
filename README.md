# Chrome Auto Logon with C# and Selenium
This application was created to solve allow for kiosk accounts to be automatically logged into for a company. This is a generic shell for automatic logins using a username and password.

## To configure custome logins
Once the application is compiled you can easily create custom logins for various sites using one compiled application.

* Create a file filename.config
	* Add your appSettings
	```xml
		<appSettings>
		  <add key="restrictAccess" value="true"/>
		  <add key="domainName" value="contoso.com"/>
		  <add key="searchBase" value="DC=contoso,DC=com"/>
		  <add key="securityGroup" value="Group1;Group2;Group3"/>
		  <add key="username" value=""/>
		  <add key="password" value=""/>
		  <add key ="url" value=""/>
		  <add key="usernameField" value=""/>
		  <add key="passwordField" value=""/>
		  <add key="submitField" value=""/>
		</appSettings>
	```
	* restrictAccess set to true if you want to restrict access based on AD Groups (Values: true or false)
	* domainName is the Domain Name of your organization
	* SearchBase is the base OU where your groups are held. You can set to the Root of the domain as done above and sub-directories will be searched also.
	* securityGroup is a semi-colon deliminated list of groups permitted to use that shortcut. I have found that Domain Admins doesn't seem to work in this spot.
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
