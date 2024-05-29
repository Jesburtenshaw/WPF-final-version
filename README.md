**This project is extension of Windows Explorer. 
In this project following technology used.**
> **C++ and WPF using C#**

**STEPS FOR HOW TO BUILD?**

1. Build all projects from Solution
2. After successfully build all required dll files are available in "_bin\output\" folder. 

**Following DLL files are generated**
CDM.dll CDMWrapper.dll Microsoft.Expression.Interactions.dll System.Windows.Interactivity.dll CDM-shell-ext.dll

**STEPS FOR HOW TO Register extension**
1. Open Windows Command Prompt as Administrator
2. Type "regsvr32" and "{Location of all generated dll files which is ..\_bin\output\CDM-shell-ext.dll}"
   For Example:
   ![image](https://github.com/Jesburtenshaw/WPF-final-version/assets/152365305/5c6ba7f2-2e49-4c6c-8052-2e8989659483)

4. Press Enter and successfull message will appear
5. You can see CFM drive option left side of Windows Explorer if not found then open task manager and find WindowExplorer process and right click on that and find Restart option and click on it.
6. Repeat step 2 if WindowsExplorer service restart else click on CFM drive option

**STEPS FOR HOW TO UNINSTALL extension**
1. Run following command 
![image](https://github.com/Jesburtenshaw/WPF-final-version/assets/152365305/b354779d-544f-4a6f-a034-91a2da44bed6)
