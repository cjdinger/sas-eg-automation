EGPSearch - a sample application for SAS Enterprise Guide automation
---------------------------------------------------------------------
Author/Support: Chris Hemedinger (http://blogs.sas.com/sasdummy)

Companion to this paper/presentation:
  https://communities.sas.com/t5/SAS-Communities-Library/Doing-More-with-SAS-Enterprise-Guide-Automation/ta-p/417832

Use this tool to search a collection of SAS Enterprise Guide project files
for ANY text within SAS programs, logs, notes, and item labels.

Note: this tool, as built, works only with SAS Enterprise Guide 4.3.  You can use the source
code to adapt to a later version.  If you simply need a search tool for SAS Enterprise Guide
projects, try this:
  https://blogs.sas.com/content/sasdummy/egp-search-tool2/   
  
To USE the example as a tool:
 - Copy EGPSearch43.exe to a PC with SAS Enterprise Guide 4.3 installed
 - Run EGPSearch43.exe. 

To open/build the source code for the example:
 - Expand EGPSearch_src.zip to a folder on your PC.
 - Open the EGPSearch43.csproj file with Microsoft Visual Studio 2010 (or Microsoft C# Express 2010, free edition)
 - Add a project reference to your SAS Enterprise Guide 4.3 installation directory, so that
   the SASEGScripting.dll can be resolved.

   