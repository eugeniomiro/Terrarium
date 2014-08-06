Terrarium Project
=================

This is the README file trom the Terrarium Project hosted on 
[CodePlex][codeplex_project_site], updated for the current project.

Project Structure
-----------------

The project is layed out in a series of folders and supports Visual Studio 2012 (.NET 4.0 / 4.5) version. 

<pre>
C:.
+---Client							# old .NET 2.0 client
|   +---AsmCheck
|   +---Configuration
|   +---Controls
|   +---Controls2
|   +---ControlsWPF					# abandoned WPF client for .NET 2.0
|   +---DXVBLib
|   +---Game
|   +---Glass
|   +---Graphics
|   +---HttpListener
|   +---OrganismBase
|   +---Renderer
|   +---Services
|   +---Setup
|   +---Terrarium					# original WinForms 2.0 client
|   +---Terrarium2					# abandoned client project for .NET 4.0
|   +---TerrariumWPF				# abandoned WPF client for .NET 2.0
+---ClientWPF						# new .NET 4.0 clients
|   +---TerrariumClient				# net .NET 4.0 WPF client
+---Keys
+---Samples
+---SDK
+---SDK2
+---Server							# original WebForms server and services
+---ServerMVC						# new .NET 4.5 MVC 4.0 server
|   +---TerrariumServer
|   +---TerrariumServer.Tests
+---Tools
    +---ServerConfig
    +---StyleEditor
</pre>

Required Extensions
-------------------

[asp.net mvc 5][asp_net] for Visual Studio 2012

[Wix Toolkit][wix]


[codeplex_project_site]: http://terrarium2.codeplex.com/
[asp_net]: http://blogs.msdn.com/b/webdev/archive/2013/11/18/announcing-release-of-asp-net-and-web-tools-2013-1-for-visual-studio-2012.aspx
[wix]: http://wixtoolset.org/
