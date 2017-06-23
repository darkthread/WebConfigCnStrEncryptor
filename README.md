Project Description
====
A handy tool to encrypt and decrypt connectionStrings section of web.config instead of typing complex aspnet_regiis arguments. It also supports RSA key container creating, deleting, exporting and importing and makes sharing RSA key among servers easier.

Introduction
====
Since ASP.NET 2.0, web.cofig adds connectionStrings section to store database connection string and provides encryption function to secure the sensitive information (like database account and password).  For example:

````xml
<connectionStrings>
<add name="PlaygroundConnectionString" connectionString="Data Source=(local);Initial Catalog=Playground;Integrated Security=True" providerName="System.Data.SqlClient" />
</connectionStrings>
````

Will be encrypted as:

````xml
<EncryptedData Type="http://www.w3.org/2001/04/xmlenc#Element"
  xmlns="http://www.w3.org/2001/04/xmlenc#">
  <EncryptionMethod Algorithm="http://www.w3.org/2001/04/xmlenc#tripledes-cbc" />
  <KeyInfo xmlns="http://www.w3.org/2000/09/xmldsig#">
   <EncryptedKey xmlns="http://www.w3.org/2001/04/xmlenc#">
    <EncryptionMethod Algorithm="http://www.w3.org/2001/04/xmlenc#rsa-1_5" />
    <KeyInfo xmlns="http://www.w3.org/2000/09/xmldsig#">
     <KeyName>Rsa Key</KeyName>
    </KeyInfo>
    <CipherData>
<CipherValue>a44a3giX...(Ignored)...o+VMsXS8os=</CipherValue>
    </CipherData>
   </EncryptedKey>
  </KeyInfo>
  <CipherData>
<CipherValue>TX5qKv+s...(Ignored)...3YgrV5wcA==</CipherValue>
  </CipherData>
</EncryptedData>
</connectionStrings>
````

The RSA encryption can protect the sensitive information from cracking, even when web.config is stolen.  You can find more information about web.config encryption from MSDN:

* [Encrypting and Decrypting Configuration Sections](http://msdn.microsoft.com/en-us/library/zhhddkxy.aspx)
* [Importing and Exporting Protected Configuration RSA Key Containers](http://msdn.microsoft.com/en-us/library/yxw286t2.aspx)

But the only way to encrypt and decrypt web.config is to use aspnet_regiis.exe command line utility, it means you have to know the arguments and type them manually, not very convenient.  For example, when I want to encrypt a web.config and deploy it to 3 web farm servers, here is what I have to do:

1. Use **aspnet_regiis -pc "SharedKeys" –exp** to create a shared RSA key container.
2. Use **aspnet_regiis -px "SharedKeys" keys.xml -pri** to export the RSA key container to a XML file.
3. Copy **keys.xml** to 3 web farm servers.
4. Execute **aspnet_regiis -pi "SharedKeys" keys.xml** to import RSA key container on 3 web farm servers.
5. Execute **aspnet_regiis -pa "SharedKeys" "NT AUTHORITY\NETWORK SERVICE"** to grant access permission to ASP.NET web application.  (Note: IIS 7.5 uses IIS APPPOOL\YourAppPoolName as identity, please replace NT AUTHORITY\NETWORK SERVICE to appropriate account. [reference])
In web.config, add:
````xml
<configProtectedData>
<providers>
  <add keyContainerName="SharedKeys" useMachineContainer="true"
   name="SharedKeys" type="System.Configuration.RsaProtectedConfigurationProvider,System.Configuration, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a" />
</providers>
</configProtectedData>
````
7. Use **aspnet_regiis -pe "connectionStrings" -app "/WebApplication" -prov "SharedKeys"** to encrypt connectionStrings section.
8. Copy encrypted web.config to 3 web farm servers.

I always think there should be a more convenient tools to finish these complex steps and cover the utility arguments detail, so I wrote the tool -- Web Config ConnectionString Encryptor!

It's a handy tool to provide GUI to cover the operations that aspnet_regiis.exe used to do, including web.config encyption and decryption and RSA key containers management.

The UI is straight forward ( I hope so ;P ), you can choose the web application from the dropdownlist, and then the web.config will show in the viewer, the [Edit] button can startup Notepad to edit the web.config.  If the web.config connectionStrings section is not encrypted, you can click [Encrypt] to encrypt it;  if it's encrypted, the [Encrypt] button will becomes [Decrypt], you can decrypt it by one click, too.

![Fig1](http://www.darkthread.net/photos/0972-7132-o.gif)

![Fig2](http://www.darkthread.net/photos/0973-967e-o.gif)

The RSA Key Container Management UI can be used to create, delete, export and import key containers.

![Fig3](http://www.darkthread.net/photos/0974-b009-o.gif)

The tool provides English and Traditional-Chinese (Taiwan) multi-language support so far.

![Fig4](http://www.darkthread.net/photos/0975-3613-o.gif)


Here are operation samples for some typical scenarios:

+ For single web server (using default key container)
  1. Choose web application from drowdownlist
  2. Click [Encrypt] button
+ For single web server (using specific key container)
  1. Choose web application from drowdownlist
  2. Input key container name
  3. Click [Encrypt] button
(If the key container doesn't exist, a new key container will be created automatically.  But the auto-created key container is not exportable so it can't be used for web farm severs, please ref the next case for web farm scenarios.  When using specific key container, a RsaProtectedConfigurationProvider named as the key container name will be appended in the web.config <configProtectedData><providers> node.)
  4. Use [Manage Key Container] function
  5. Input key container name and click [Grant] button
+ Sharing encrypted web.config among web farm servers
  1. Use [Manage Key Container] function
  2. Input key container name, click [Create] button
  3. Click [Grant] button
  4. Click [Export] and save as XML file
  5. Copy the XML file to web farm servers, run Web Config ConnectionString Encryptor on those servers, use [Manage Key Container], input key container name, click [Import] then [Grant].  Remember to delete the XML file from the server for security issue.
  6. Choose web application from drowdownlist
  7. Input key container name
  8. Click [Encrypt] button
  9. Cop the encrypted web.config to web farm servers.

Althogh I think this tool is simple and safe, however, use it at your own risk and remember to backup your web.config to be on the safe side.  Any feedback is welcome.

Reference:
----

* [V0.9 Release note in my blog](http://blog.darkthread.net/post-2010-08-29-web-config-connstr-encryptor-v09.aspx)
* [Introduction post in my blog (in Chinese)](http://blog.darkthread.net/post-2010-08-29-web-config-connstr-encryptor-v09-cht.aspx)

**Updated @ 2010-09-20**

Add web.config customErrors warning in ver 0.95, please check [ASP.NET security vulnerability issue](https://weblogs.asp.net/scottgu/important-asp-net-security-vulnerability) for detail.

Here is the rules for red color warning:

For ASP.NET 2.0 
    * customErrors mode Off, defaultRedirect undefined, or <error> sub element exists
For ASP.NET 3.5 / 4.0 
    * customErrors mode Off, defaultRedirect undefined, redirectMode not assgined as ResponseRewrite, or <error> sub element exists

